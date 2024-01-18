using ERP.Complementos.PDF;
using ERP.Complementos.ProcesoBehind;
using ERP.Models;
using HTTPupt;
using iTextSharp.text.pdf.codec.wmf;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using static ERP.Complementos.PDF.CreatePDF;

namespace ERP.Vistas.Ventas
{
    public partial class Cotizacion : System.Web.UI.Page
    {
        PeticionHTTP peticion = new PeticionHTTP(Url.ObtenerUrlServidor());
        string Json = string.Empty, Estado = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["jwt"] == null)
            {
                Response.RedirectToRoute("BorrarJWT");
                return;
            }

            if (!IsPostBack)
            {
                if (!ObtenerCotizacion()) { Url.CerrarSesion(this); return; }
                else
                {
                    Session["jwt"] = Url.ActualizarJWT((String)Session["jwt"]);
                }
            }
        }

        private bool ObtenerCotizacion()
        {
            string folio = Page.RouteData.Values["id"] as string;

            peticion.PedirComunicacion($"Cotizaciones/ObtenerCotizacionCompleta/{folio}", MetodoHTTP.GET, TipoContenido.JSON, (String)Session["jwt"]);
            Estado = peticion.ObtenerJson();

            if (Estado == null) { return false; }

            CotizacionDTO cotizacion = JsonConvert.DeserializeObject<CotizacionDTO>(Estado);
            if (cotizacion.Estatus) { return false; }

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
            ViewState["Partidas"] = cotizacion;
            ViewState["PartidasID"] = new List<int>();
            ViewState["PartidasMinimas"] = (cotizacion.Partidas.Count - Convert.ToInt32(Math.Floor(cotizacion.Partidas.Count / 2.0)));

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
            cotizacion.Partidas.ForEach(partida => partida.PrecioFormato = partida.Precio.ToString("C2", new CultureInfo("es-MX")));
            cotizacion.Partidas.ForEach(partida => partida.TotalFormato = partida.Total.ToString("C2", new CultureInfo("es-MX")));

            txtCondiciones.Text = cotizacion.Condiciones;
            // Labels  - - - - - - - - - - - - - - - - - - - - - - - - -  
            lblFolio.Text = cotizacion.Folio; lblCliente.Text = cotizacion.Cliente;
            CalculosIVA(cotizacion);
            // Labels  - - - - - - - - - - - - - - - - - - - - - - - - -  
            chbxStatus.Checked = cotizacion.Estatus;

            gvPartidas.DataSource = cotizacion.Partidas;
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
            CotizacionDTO Cotizacion = (CotizacionDTO)ViewState["Partidas"];
            ActualizarUnidades(Cotizacion);
        }

        protected void gvPartidas_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            CotizacionDTO Cotizacion = (CotizacionDTO)ViewState["Partidas"];
            int PartidasMinimas = (int)ViewState["PartidasMinimas"];

            if (Cotizacion.Partidas.Count == PartidasMinimas)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "LimitePartidas", "LimitePartidas()", true);
                return;
            }
            List<int> partidasID = (List<int>)ViewState["PartidasID"];
            int Indice = e.RowIndex;

            GridViewRow fila = gvPartidas.Rows[e.RowIndex];

            partidasID.Add(Cotizacion.Partidas[Indice].ID);
            Cotizacion.Partidas.RemoveAt(Indice);

            Cotizacion.Partidas.ForEach(p => p.Partida = Cotizacion.Partidas.IndexOf(p) + 1);

            CalculosIVA(Cotizacion);
            // - - - - - - - - - - - - - - - - - - - - - - - - - 
            gvPartidas.DataSource = Cotizacion.Partidas;
            gvPartidas.DataBind();
        }

        //Se comparan los cambios de la plantilla y se actualizan o eliminan según sea el caso 
        protected void BTNConfirmar_Click(object sender, EventArgs e)
        {
            string Folio = Page.RouteData.Values["id"] as string;

            peticion.PedirComunicacion($"Cotizaciones/ComprobarFolio/{Folio}", MetodoHTTP.GET, TipoContenido.JSON, (String)Session["jwt"]);
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

            CotizacionDTO CotizacionActualizada = (CotizacionDTO)ViewState["Partidas"];
            List<clsPartidaCotizacionVenta> partidasCotizacion = new List<clsPartidaCotizacionVenta>();

            //Validar entrada de unidas
            if (!ActualizarUnidades(CotizacionActualizada))
                return;

            List<int> partidasID = (List<int>)ViewState["PartidasID"];

            Json = JsonConvertidor.Objeto_Json(partidasID);
            peticion.PedirComunicacion($"PartidaCotizaciones/BorrarPartidas", MetodoHTTP.DELETE, TipoContenido.JSON, (String)Session["jwt"]);
            peticion.enviarDatos(Json);
            Json = peticion.ObtenerJson();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            clsCotizacionVenta cotizacion = new clsCotizacionVenta()
            {
                Folio = CotizacionActualizada.Folio,
                ClienteID = CotizacionActualizada.ClienteID,
                Condiciones = txtCondiciones.Text,
                Total = CotizacionActualizada.Total,
                Estado = chbxStatus.Checked,
                FechaCotizacion = CotizacionActualizada.FechaCotizacion,
            };

            // - - - - - - - - - - - - - - - - - - - - - - - - - 
            if (chbxStatus.Checked)
            {
                cotizacion.FechaVenta = DateTime.Now;
                Response.RedirectToRoute("Orden");
            }
            // - - - - - - - - - - - - - - - - - - - - - - - - -
            Json = JsonConvertidor.Objeto_Json(cotizacion);
            peticion.PedirComunicacion("Cotizaciones/ActualizarCotizacion", MetodoHTTP.PUT, TipoContenido.JSON, (String)Session["jwt"]);
            peticion.enviarDatos(Json);

            Estado = peticion.ObtenerJson();

            foreach (var partidas in CotizacionActualizada.Partidas)
            {
                clsPartidaCotizacionVenta partida = new clsPartidaCotizacionVenta()
                {
                    ID = partidas.ID,
                    Folio = partidas.Folio,
                    ProductoID = partidas.ProductoID,
                    Unidades = partidas.Cantidad,
                    CostoCapturado = partidas.Costo,
                    PrecioUnitario = partidas.Precio,
                    TotalPartida = partidas.Total,
                    CriterioAumento = partidas.CriterioAumento,
                    PorcentajeAumento = partidas.PorcentajeAumento,
                    Estado = partidas.Estado,
                };
                partidasCotizacion.Add(partida);
            }

            Json = JsonConvertidor.Objeto_Json(partidasCotizacion);
            peticion.PedirComunicacion("PartidaCotizaciones/ActualizarPartidas", MetodoHTTP.PUT, TipoContenido.JSON, (String)Session["jwt"]);
            peticion.enviarDatos(Json);

            Json = peticion.ObtenerJson();
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Subido", "Subido()", true);
        }

        private bool ActualizarUnidades(CotizacionDTO Cotizacion)
        {
            for (int i = 0; i < gvPartidas.Rows.Count; i++) //bucle que recorre todas las celdas de unidades del GridView
            {
                GridViewRow fila = gvPartidas.Rows[i]; //Obtengo la fila con el indice
                TextBox txtCantidad = ValidarUnidades(fila);

                if (txtCantidad == null)
                    return false;

                //Actualizo parametros del objeto y la página en general
                Cotizacion.Partidas[i].Cantidad = int.Parse(txtCantidad.Text);
                float total = (Cotizacion.Partidas[i].Cantidad * Cotizacion.Partidas[i].Precio);

                // Actualizar partidas y cotización con formato númerico
                Cotizacion.Partidas[i].TotalFormato = total.ToString("C2", new CultureInfo("es-MX"));
                Cotizacion.Partidas[i].Total = total;
            }
            //Calculos de totales mas IVA
            CalculosIVA(Cotizacion);

            // - - - - - - - - - - - - - - - - - - - - - - - - - 
            gvPartidas.DataSource = Cotizacion.Partidas;
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
            Response.RedirectToRoute("CotizarPlantilla", new { id = lblFolio.Text });
            return;
        }

        private void CalculosIVA(CotizacionDTO cotizacion = null)
        {
            float SubTotal = 0, DiferenciaIVA = 0, Total = 0;

            foreach (var calcular in cotizacion.Partidas)
            {
                SubTotal += Convert.ToSingle(calcular.TotalFormato.Substring(1));
            }

            DiferenciaIVA = Convert.ToSingle((SubTotal * .16));
            Total = (SubTotal + DiferenciaIVA);
            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            lblTotal.Text = Total.ToString("N2", new CultureInfo("es-MX"));
            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            cotizacion.SubTotal = SubTotal;
            cotizacion.DiferenciaIVA = DiferenciaIVA;
            cotizacion.Total = Total;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        protected void BtnDescargarPDF_Click(object sender, ImageClickEventArgs e)
        {
            string Folio = Page.RouteData.Values["id"] as string;

            peticion.PedirComunicacion($"Cotizaciones/ComprobarFolio/{Folio}", MetodoHTTP.GET, TipoContenido.JSON, (String)Session["jwt"]);
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
            CotizacionDTO CotizacionActual = (CotizacionDTO)ViewState["Partidas"];
            CotizacionActual.FechaAlterada = string.Empty;

            DescargarPDF();
        }

        protected void btnFechaAlterada_Click(object sender, EventArgs e)
        {
            CotizacionDTO CotizacionActual = (CotizacionDTO)ViewState["Partidas"];

            DateTime FechaDeEntrega = DateTime.ParseExact(txtFechaEntrega.Text, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None);
            CotizacionActual.FechaAlterada = FechaDeEntrega.ToLongDateString();

            DescargarPDF();
        }

        private void DescargarPDF()
        {
            CotizacionDTO CotizacionActual = (CotizacionDTO)ViewState["Partidas"];
            // Se comprueba si el folio actual se ha encontrado en la bd, para saber si ya se ha confirmado el folio actual

            CreatePDF pdf = new CreatePDF(CotizacionActual, Server.MapPath("/Media/Resources/fondo-1.png"), Server.MapPath("~/Multimedia/"), "COTIZACIÓN");

            CotizacionActual.Condiciones = txtCondiciones.Text;

            Estructura estructura = pdf.Create();

            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", $"attachment;filename={CotizacionActual.Folio}.pdf");
            HttpContext.Current.Response.Write(estructura.doc);
            Response.Flush();
            Response.End();

            estructura.writer.Close();
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


        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        protected void imgbtnAtras_Click(object sender, ImageClickEventArgs e)
        {
            Response.RedirectToRoute("Orden");
        }
    }
}