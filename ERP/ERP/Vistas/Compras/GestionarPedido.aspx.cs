using ERP.Complementos.PDF;
using ERP.Complementos.ProcesoBehind;
using ERP.Models;
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
    public partial class GestionarPedido : System.Web.UI.Page
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

            //En está petición llamo la petición desde el controlador de la cotización xq requiero todas las partidas sin importar su estado
            //PDTE a mejorar
            peticion.PedirComunicacion($"Pedidos/ObtenerPedidoCompleto/{folio}", MetodoHTTP.GET, TipoContenido.JSON, (String)Session["jwt"]);
            Estado = peticion.ObtenerJson();

            if (Estado == null) { return false; }

            PedidoDTO pedido = JsonConvert.DeserializeObject<PedidoDTO>(Estado);
            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
            ViewState["Partidas"] = pedido;
            ViewState["PartidasID"] = new List<int>();
            ViewState["PartidasMinimas"] = (pedido.Partidas.Count - Convert.ToInt32(Math.Floor(pedido.Partidas.Count / 2.0)));

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
            pedido.Partidas.ForEach(partida => partida.CostoFormato = partida.Costo.ToString("C2", new CultureInfo("es-MX")));
            pedido.Partidas.ForEach(partida => partida.TotalFormato = partida.Total.ToString("C2", new CultureInfo("es-MX")));

            txtCondiciones.Text = pedido.Condiciones;
            //txtCondiciones.Enabled = false;
            // Labels  - - - - - - - - - - - - - - - - - - - - - - - - -  
            lblFolio.Text = pedido.Folio; lblProveedor.Text = pedido.Proveedor;
            CalculosIVA(pedido);

            gvPartidas.DataSource = pedido.Partidas;
            gvPartidas.DataBind();

            return true;
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

        protected void BtnReutilizar_Click(object sender, ImageClickEventArgs e)
        {
            Response.RedirectToRoute("PedidoPlantilla", new { id = lblFolio.Text });
            return;
        }

        protected void BtnFormularioPDF_Click(object sender, ImageClickEventArgs e)
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

            ClientScript.RegisterStartupScript(GetType(), "MostrarDateConfig", "MostrarDateConfig()", true);
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
            PedidoActual.Condiciones = txtCondiciones.Text;

            PedidoPDF pdf = new PedidoPDF(PedidoActual, Server.MapPath("/Media/Resources/FondoPedidoPDF.jpg"), Server.MapPath("~/Multimedia/"));

            Estructura estructura = pdf.CrearPDFpedido();

            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", $"attachment;filename={PedidoActual.Folio}.pdf");
            HttpContext.Current.Response.Write(estructura.doc);
            Response.Flush();
            Response.End();
            estructura.writer.Close();
        }


        protected void imgbtnAtrasPedido_Click(object sender, ImageClickEventArgs e)
        {
            Response.RedirectToRoute("Historial de Pedidos");
        }

        protected void txtCondiciones_TextChanged(object sender, EventArgs e)
        {
            PedidoDTO PedidoActual = (PedidoDTO)ViewState["Partidas"];
            string parametros = $"?id={PedidoActual.Folio}&condiciones={txtCondiciones.Text.Trim()}";

            // Hacer la solicitud HTTP
            peticion.PedirComunicacion($"Pedidos/ActualizarCondiciones/{parametros}", MetodoHTTP.GET, TipoContenido.JSON, (String)Session["jwt"]);
            Estado = peticion.ObtenerJson();
        }

    }
}