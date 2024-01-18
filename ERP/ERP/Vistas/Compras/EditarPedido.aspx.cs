using ERP.Complementos.PDF;
using ERP.Complementos.ProcesoBehind;
using ERP.Migrations;
using ERP.Models;
using ERP.Vistas.Ventas;
using HTTPupt;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using static ERP.Complementos.PDF.PedidoPDF;

namespace ERP.Vistas.Compras
{
    public partial class EditarPedido : System.Web.UI.Page
    {
        PeticionHTTP peticion = new PeticionHTTP(Url.ObtenerUrlServidor());
        string Json = string.Empty, Estado;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["jwt"] == null)
            {
                Response.RedirectToRoute("BorrarJWT");
                return;
            }

            if (!IsPostBack)
            {
                if (!ObtenerPedido()) { Url.CerrarSesion(this); return; }
                else
                {
                    Session["jwt"] = Url.ActualizarJWT((String)Session["jwt"]);
                }
            }
        }

        private bool ObtenerPedido()
        {
            string folio = Page.RouteData.Values["id"] as string;

            peticion.PedirComunicacion($"Pedidos/ObtenerPedidoCompleto/{folio}", MetodoHTTP.GET, TipoContenido.JSON, (String)Session["jwt"]);
            Estado = peticion.ObtenerJson();

            if(Estado == null) { return false; }

            PedidoDTO pedido = JsonConvert.DeserializeObject<PedidoDTO>(Estado);
            if (pedido.Estatus) { return false; }

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
            ViewState["Partidas"] = pedido;
            ViewState["PartidasID"] = new List<int>();
            ViewState["PartidasMinimas"] = (pedido.Partidas.Count - Convert.ToInt32(Math.Floor(pedido.Partidas.Count / 2.0)));
            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
            pedido.Partidas.ForEach(partida => partida.CostoFormato = partida.Costo.ToString("C2", new CultureInfo("es-MX")));
            pedido.Partidas.ForEach(partida => partida.TotalFormato = partida.Total.ToString("C2", new CultureInfo("es-MX")));

            txtCondiciones.Text = pedido.Condiciones;
            // Labels  - - - - - - - - - - - - - - - - - - - - - - - - -  
            lblFolio.Text = pedido.Folio; lblProveedor.Text = pedido.Proveedor;
            CalculosIVA(pedido);
            // Labels  - - - - - - - - - - - - - - - - - - - - - - - - -  
            chbxStatus.Checked = pedido.Estatus;

            gvPartidas.DataSource = pedido.Partidas;
            gvPartidas.DataBind();

            return true;
        }

        //Se obtiene el numero de Unidades de la partida, y se asigna a cada TextBox de partida
        protected void gvPartidas_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            // Verificar que la fila sea de tipo DataRow para que no incluya los encabezados ni el pie de página
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Encontrar los controles TextBox en la columnas "Unidades"
                TextBox txtCantidad = e.Row.FindControl("txtCantidad") as TextBox;
                // Obtener los valores de las propiedades "Cantidad"
                int cantidad = (int)DataBinder.Eval(e.Row.DataItem, "Cantidad");
                // Establecer los valores de los TextBox con las cantidades
                txtCantidad.Text = cantidad.ToString();
            }
        }
        protected void gvPartidas_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            PedidoDTO Pedido = (PedidoDTO)ViewState["Partidas"];
            ActualizarUnidades(Pedido);
        }

        protected void gvPartidas_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            PedidoDTO Pedido = (PedidoDTO)ViewState["Partidas"];
            int PartidasMinimas = (int)ViewState["PartidasMinimas"];

            if (Pedido.Partidas.Count == PartidasMinimas)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "LimitePartidas", "LimitePartidas()", true);
                return;
            }
            List<int> partidasID = (List<int>)ViewState["PartidasID"];
            int Indice = e.RowIndex;

            GridViewRow fila = gvPartidas.Rows[e.RowIndex];

            partidasID.Add(Pedido.Partidas[Indice].ID);
            Pedido.Partidas.RemoveAt(Indice);

            Pedido.Partidas.ForEach(p => p.Partida = Pedido.Partidas.IndexOf(p) + 1);

            CalculosIVA(Pedido);
            // - - - - - - - - - - - - - - - - - - - - - - - - - 
            gvPartidas.DataSource = Pedido.Partidas;
            gvPartidas.DataBind();
        }

        protected void BTNConfirmar_Click(object sender, EventArgs e)
        {
            string Folio = Page.RouteData.Values["id"] as string;

            peticion.PedirComunicacion($"Pedidos/ComprobarFolio/{Folio}", MetodoHTTP.GET, TipoContenido.JSON, (String)Session["jwt"]);
            string folioEncontrado = peticion.ObtenerJson();
            PeticionEstado respuesta;

            // VALIDAR SESIÓN - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
            if (folioEncontrado == null) { Url.CerrarSesion(this); return; }
            else
            {
                Session["jwt"] = Url.ActualizarJWT((String)Session["jwt"]);
                respuesta = JsonConvertidor.Json_Objeto<PeticionEstado>(folioEncontrado);
            }
            if (!respuesta.Estado && respuesta.AlertaJS.StartsWith("P"))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alertaJS", respuesta.AlertaJS, true);
                return;
            }
            // VALIDAR SESIÓN - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            PedidoDTO PedidoActualizado = (PedidoDTO)ViewState["Partidas"];
            List<clsPartidaPedido> partidasPedido = new List<clsPartidaPedido>();

            //Validar entrada de unidas
            if (!ActualizarUnidades(PedidoActualizado))
                return;

            List<int> partidasID = (List<int>)ViewState["PartidasID"];

            Json = JsonConvertidor.Objeto_Json(partidasID);
            peticion.PedirComunicacion($"PartidaPedidos/BorrarPartidas", MetodoHTTP.DELETE, TipoContenido.JSON, (String)Session["jwt"]);
            peticion.enviarDatos(Json);

            Estado = peticion.ObtenerJson();

            clsPedido pedido = new clsPedido()
            {
                Folio = PedidoActualizado.Folio,
                ProveedorID = PedidoActualizado.ProveedorID,
                Condiciones = txtCondiciones.Text,
                Total = PedidoActualizado.Total,
                Estado = chbxStatus.Checked,
                Fecha = PedidoActualizado.Fecha,
            };

            // - - - - - - - - - - - - - - - - - - - - - - - - - 
            if (chbxStatus.Checked)
            {
                Response.RedirectToRoute("Historial de Pedidos");
            }
            // - - - - - - - - - - - - - - - - - - - - - - - - -

            Json = JsonConvertidor.Objeto_Json(pedido);
            peticion.PedirComunicacion("Pedidos/ActualizarPedido", MetodoHTTP.PUT, TipoContenido.JSON, (String)Session["jwt"]);
            peticion.enviarDatos(Json);

            Json = peticion.ObtenerJson();

            foreach (var partidas in PedidoActualizado.Partidas)
            {
                clsPartidaPedido partida = new clsPartidaPedido()
                {
                    ID = partidas.ID,
                    Folio = partidas.Folio,
                    ProductoID = partidas.ProductoID,
                    Unidades = partidas.Cantidad,
                    CostoUnitario = partidas.Costo,
                    TotalPartida = partidas.Total
                };
                partidasPedido.Add(partida);
            }

            Json = JsonConvertidor.Objeto_Json(partidasPedido);
            peticion.PedirComunicacion("PartidaPedidos/ActualizarPartidas", MetodoHTTP.PUT, TipoContenido.JSON, (String)Session["jwt"]);
            peticion.enviarDatos(Json);

            Json = peticion.ObtenerJson();
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Subido", "Subido()", true);
        }

        //Se redirije a la pagina de cotizar con el folio de las partidas a reutilizar

        private bool ActualizarUnidades(PedidoDTO Pedido)
        {
            for (int i = 0; i < gvPartidas.Rows.Count; i++) //bucle que recorre todas las celdas de unidades del GridView
            {
                GridViewRow fila = gvPartidas.Rows[i]; //Obtengo la fila con el indice
                TextBox txtCantidad = ValidarUnidades(fila);

                if (txtCantidad == null)
                    return false;

                //Actualizo parametros del objeto y la página en general
                Pedido.Partidas[i].Cantidad = int.Parse(txtCantidad.Text);
                float total = (Pedido.Partidas[i].Cantidad * Pedido.Partidas[i].Costo);

                // Actualizar partidas y cotización con formato númerico
                Pedido.Partidas[i].TotalFormato = total.ToString("C2", new CultureInfo("es-MX"));
                Pedido.Partidas[i].Total = total;
            }
            //Calculos de totales mas IVA
            CalculosIVA(Pedido);

            // - - - - - - - - - - - - - - - - - - - - - - - - - 
            gvPartidas.DataSource = Pedido.Partidas;
            gvPartidas.DataBind();

            return true;
        }
        private TextBox ValidarUnidades(GridViewRow fila)
        {
            TableCell celda = fila.Cells[4];
            TextBox txtCantidad = (TextBox)celda.FindControl("txtCantidad");
            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
            if (!int.TryParse(txtCantidad.Text, out int unidades) || unidades < 1 || string.IsNullOrEmpty(txtCantidad.Text))
            {
                var REV1 = fila.FindControl("REV1") as RegularExpressionValidator;
                REV1.Enabled = true;
                REV1.IsValid = false;

                txtCantidad = null;
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "UnidadesVacias", "UnidadesVacias()", true);
                return txtCantidad;
            }
            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
            return txtCantidad;
        }
        protected void BtnReutilizar_Click(object sender, ImageClickEventArgs e)
        {
            Response.RedirectToRoute("PedidoPlantilla", new { id = lblFolio.Text });
            return;
        }

        protected void BtnDescargarPDF_Click(object sender, ImageClickEventArgs e)
        {
            string Folio = Page.RouteData.Values["id"] as string;

            peticion.PedirComunicacion($"Pedidos/ComprobarFolio/{Folio}", MetodoHTTP.GET, TipoContenido.JSON, (String)Session["jwt"]);
            string folioEncontrado = peticion.ObtenerJson();
            PeticionEstado respuesta;

            // VALIDAR SESIÓN - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
            if (folioEncontrado == null) { Url.CerrarSesion(this); return; }
            else
            {
                Session["jwt"] = Url.ActualizarJWT((String)Session["jwt"]);
                respuesta = JsonConvertidor.Json_Objeto<PeticionEstado>(folioEncontrado);
            }
            if (!respuesta.Estado && respuesta.AlertaJS.StartsWith("P"))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alertaJS", respuesta.AlertaJS, true);
                return;
            }
            // VALIDAR SESIÓN - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "MostrarDateConfig", "MostrarDateConfig()", true);
        }

        protected void btnFechaCreación_Click(object sender, EventArgs e)
        {
            PedidoDTO PedidoActual = (PedidoDTO)ViewState["Partidas"];
            PedidoActual.FechaAlterada = string.Empty;

            DescargarPDF();
        }

        protected void btnFechaAlterada_Click(object sender, EventArgs e)
        {
            PedidoDTO PedidoActual = (PedidoDTO)ViewState["Partidas"];

            DateTime FechaDeEntrega = DateTime.ParseExact(txtFechaEntrega.Text, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None);
            PedidoActual.FechaAlterada = FechaDeEntrega.ToLongDateString();

            DescargarPDF();
        }

        private void DescargarPDF()
        {
            PedidoDTO PedidoActual = (PedidoDTO)ViewState["Partidas"];

            PedidoPDF pdf = new PedidoPDF(PedidoActual, Server.MapPath("/Media/Resources/FondoPedidoPDF.jpg"), Server.MapPath("~/Multimedia/"));

            PedidoActual.Condiciones = txtCondiciones.Text;

            Estructura estructura = pdf.CrearPDFpedido();

            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", $"attachment;filename={PedidoActual.Folio}.pdf");
            HttpContext.Current.Response.Write(estructura.doc);
            Response.Flush();
            Response.End();
            estructura.writer.Close();
        }

        protected void imgbtnAtras_Click(object sender, ImageClickEventArgs e)
        {
            Response.RedirectToRoute("Historial de Pedidos");
        }

        private void CalculosIVA(PedidoDTO pedido = null)
        {
            float SubTotal = 0, DiferenciaIVA = 0, Total = 0;

            foreach (var calcular in pedido.Partidas)
            {
                SubTotal += Convert.ToSingle(calcular.TotalFormato.Substring(1));
            }

            DiferenciaIVA = Convert.ToSingle((SubTotal * .16));
            Total = (SubTotal + DiferenciaIVA);
            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            //lblSubtotal.Text = SubTotal.ToString("N2", new CultureInfo("es-MX"));
            //lblIVA.Text = DiferenciaIVA.ToString("N2", new CultureInfo("es-MX"));
            lblTotal.Text = Total.ToString("N2", new CultureInfo("es-MX"));
            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            pedido.SubTotal = SubTotal;
            pedido.DiferenciaIVA = DiferenciaIVA;
            pedido.Total = Total;
        }
        protected void btnAlertaConfirmacion_Click(object sender, ImageClickEventArgs e)
        {
            if (chbxStatus.Checked)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "AlertaPreventaVenta", "alertConfirmar2() ", true);
                return;
            }

            ScriptManager.RegisterStartupScript(this, this.GetType(), "Peticion", "alertConfirmar() ", true);
        }
    }
}