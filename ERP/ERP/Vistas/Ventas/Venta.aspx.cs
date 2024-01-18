using ERP.Complementos.PDF;
using ERP.Complementos.ProcesoBehind;
using ERP.Models;
using HTTPupt;
using iTextSharp.text.pdf.codec.wmf;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using static ERP.Complementos.PDF.CreatePDF;

namespace ERP.Vistas.Ventas
{
    public partial class Venta : System.Web.UI.Page
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

            //En está petición llamo la petición desde el controlador de la cotización xq requiero todas las partidas sin importar su estado
            peticion.PedirComunicacion($"Cotizaciones/ObtenerCotizacionCompleta/{folio}", MetodoHTTP.GET, TipoContenido.JSON, (String)Session["jwt"]);
            Estado = peticion.ObtenerJson();

            if (Estado == null) { return false; }

            CotizacionDTO cotizacion = JsonConvert.DeserializeObject<CotizacionDTO>(Estado);
            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
            ViewState["cotizacion"] = cotizacion;
            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
            cotizacion.Partidas.ForEach(partida => partida.PrecioFormato = partida.Precio.ToString("C2", new CultureInfo("es-MX")));
            cotizacion.Partidas.ForEach(partida => partida.TotalFormato = partida.Total.ToString("C2", new CultureInfo("es-MX")));

            txtCondicion.Text = cotizacion.Condiciones;
            //txtCondiciones.Enabled = false;
            // Labels  - - - - - - - - - - - - - - - - - - - - - - - - -  
            lblFolio.Text = cotizacion.Folio; lblCliente.Text = cotizacion.Cliente;
            CalculosIVA(cotizacion);

            gvPartidas.DataSource = cotizacion.Partidas;
            gvPartidas.DataBind();

            return true;
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
            //lblSubtotal.Text = SubTotal.ToString("N2", new CultureInfo("es-MX"));
            //lblIVA.Text = DiferenciaIVA.ToString("N2", new CultureInfo("es-MX"));
            lblTotal.Text = Total.ToString("N2", new CultureInfo("es-MX"));
            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            cotizacion.SubTotal = SubTotal;
            cotizacion.DiferenciaIVA = DiferenciaIVA;
            cotizacion.Total = Total;
        }
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        protected void BtnFormularioPDF_Click(object sender, ImageClickEventArgs e)
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

            ClientScript.RegisterStartupScript(GetType(), "MostrarDateConfig", "MostrarDateConfig()", true);
        }
        protected void btnFechaCreación_Click(object sender, EventArgs e)
        {
            CotizacionDTO CotizacionActual = (CotizacionDTO)ViewState["cotizacion"];
            CotizacionActual.FechaAlterada = string.Empty;

            DescargarPDF();
        }

        protected void btnFechaAlterada_Click(object sender, EventArgs e)
        {
            CotizacionDTO CotizacionActual = (CotizacionDTO)ViewState["cotizacion"];

            DateTime FechaDeEntrega = DateTime.ParseExact(txtFechaEntrega.Text, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None);
            CotizacionActual.FechaAlterada = FechaDeEntrega.ToLongDateString();

            DescargarPDF();
        }

        private void DescargarPDF()
        {
            CotizacionDTO CotizacionActual = (CotizacionDTO)ViewState["cotizacion"];
            CotizacionActual.Condiciones = txtCondicion.Text;

            CreatePDF pdf = new CreatePDF(CotizacionActual, Server.MapPath("/Media/Resources/fondo-1.png"), Server.MapPath("~/Multimedia/"), "VENTA");

            Estructura estructura = pdf.Create();

            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", $"attachment;filename={CotizacionActual.Folio}.pdf");
            HttpContext.Current.Response.Write(estructura.doc);
            Response.Flush();
            Response.End();

            estructura.writer.Close();
        }

        protected void txtCondicion_TextChanged(object sender, EventArgs e)
        {
            CotizacionDTO CotizacionActual = (CotizacionDTO)ViewState["cotizacion"];
            string parametros = $"?id={CotizacionActual.Folio}&condiciones={txtCondicion.Text.Trim()}";

            // Hacer la solicitud HTTP
            peticion.PedirComunicacion($"Cotizaciones/ActualizarCondiciones/{parametros}", MetodoHTTP.GET, TipoContenido.JSON, (String)Session["jwt"]);
            Estado = peticion.ObtenerJson();
        }
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        protected void btnRemision_Click(object sender, ImageClickEventArgs e)
        {
            string folio = Page.RouteData.Values["id"] as string;

            Response.RedirectToRoute("Remitidos", new { id = folio });
            return;
        }

        protected void BtnReutilizar_Click(object sender, ImageClickEventArgs e)
        {
            Response.RedirectToRoute("CotizarPlantilla", new { id = lblFolio.Text });
            return;
        }

        protected void imgbtnReporte_Click(object sender, ImageClickEventArgs e)
        {
            string FolioActual = Page.RouteData.Values["id"] as string;

            peticion.PedirComunicacion($"Cotizaciones/ComprobarFolio/{FolioActual}", MetodoHTTP.GET, TipoContenido.JSON, (String)Session["jwt"]);
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

            string Folio = Page.RouteData.Values["id"] as string;
            Response.RedirectToRoute("Reporte", new { id = Folio });
        }

        protected void atras_Click(object sender, ImageClickEventArgs e)
        {
            Response.RedirectToRoute("Orden");
        }
    }
}