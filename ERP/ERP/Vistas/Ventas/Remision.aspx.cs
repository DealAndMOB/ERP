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
using static ERP.Complementos.PDF.PDFremision;

namespace ERP.Vistas.Ventas
{
    public partial class Remision : System.Web.UI.Page
    {
        PeticionHTTP peticion = new PeticionHTTP(Url.ObtenerUrlServidor());
        string Json = string.Empty, Estado = string.Empty;

        private RemisionDTO remisionDTO;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["jwt"] == null)
            {
                Response.RedirectToRoute("BorrarJWT");
                return;
            }

            if (!ObtenerRemision()) { Url.CerrarSesion(this); return; }
            else
            {
                Session["jwt"] = Url.ActualizarJWT((String)Session["jwt"]);
            }
        }

        private bool ObtenerRemision()
        {
            string folioRemision = Page.RouteData.Values["id"] as string;

            //En está petición llamo la petición desde el controlador de la cotización xq requiero todas las partidas sin importar su estado
            peticion.PedirComunicacion($"Remisiones/ObtenerRemisionCompleta/{folioRemision}", MetodoHTTP.GET, TipoContenido.JSON, (String)Session["jwt"]);
            Estado = peticion.ObtenerJson();

            if(Estado == null) { return false; }

            RemisionDTO remision = JsonConvert.DeserializeObject<RemisionDTO>(Estado);
            ViewState["Remision"] = remision;
            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
            lblCliente.Text = remision.Cliente;
            lblFecha.Text = remision.FechaEntrega.ToShortDateString();
            lblFolio.Text = remision.Folio;

            gvPartidas.DataSource = remision.partidasRemision;
            gvPartidas.DataBind();

            return true;
        }
        protected void BtnFormularioPDF_Click(object sender, ImageClickEventArgs e)
        {
            string Folio = Page.RouteData.Values["id"] as string;

            peticion.PedirComunicacion($"Remisiones/ComprobarFolio/{Folio}", MetodoHTTP.GET, TipoContenido.JSON, (String)Session["jwt"]);
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
            RemisionDTO PedidoActual = (RemisionDTO)ViewState["Remision"];
            PedidoActual.FechaAlterada = string.Empty;

            DescargarPDF();
        }

        protected void btnFechaAlterada_Click(object sender, EventArgs e)
        {
            RemisionDTO PedidoActual = (RemisionDTO)ViewState["Remision"];

            DateTime FechaDeEntrega = DateTime.ParseExact(txtFechaEntrega.Text, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None);
            PedidoActual.FechaAlterada = FechaDeEntrega.ToLongDateString();

            DescargarPDF();
        }

        private void DescargarPDF()
        {
            remisionDTO = (RemisionDTO)ViewState["Remision"];
            PDFremision pdf = new PDFremision(remisionDTO, Server.MapPath("/Media/Resources/fondo-1.png"), Server.MapPath("~/Multimedia/"));
            Estructura estructura = pdf.CrearPDFremision();

            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", $"attachment;filename=Reporte {remisionDTO.Folio}.pdf");
            HttpContext.Current.Response.Write(estructura.doc);
            Response.Flush();
            Response.End();
            estructura.writer.Close();
        }

        protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
        {
            remisionDTO = (RemisionDTO)ViewState["Remision"];
            Response.RedirectToRoute("Remitidos", new { id = remisionDTO.FolioVenta });
            return;
        }
    }

}