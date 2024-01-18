using ERP.Complementos.PDF;
using ERP.Complementos.ProcesoBehind;
using ERP.Models;
using HTTPupt;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using static ERP.Complementos.PDF.PDFReporte;

namespace ERP.Vistas.Ventas
{
    public partial class ReporteVenta : System.Web.UI.Page
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
                if (!obtenerPartidas()) { Url.CerrarSesion(this); return; }
                else
                {
                    Session["jwt"] = Url.ActualizarJWT((String)Session["jwt"]);
                }
            }

            string Folio = Page.RouteData.Values["id"] as string;

            peticion.PedirComunicacion($"Cotizaciones/ComprobarFolio/{Folio}", MetodoHTTP.GET, TipoContenido.JSON, (String)Session["jwt"]);
            string folioEncontrado = peticion.ObtenerJson();

            PeticionEstado respuesta = JsonConvertidor.Json_Objeto<PeticionEstado>(folioEncontrado);

            if (!respuesta.Estado && respuesta.AlertaJS.StartsWith("P"))
            {
                Response.RedirectToRoute("BorrarJWT");
                return;
            }
        }

        private bool obtenerPartidas()
        {
            string folio = Page.RouteData.Values["id"] as string;

            peticion.PedirComunicacion($"Cotizaciones/ObtenerPartidas/{folio}", MetodoHTTP.GET, TipoContenido.JSON, (String)Session["jwt"]);
            Estado = peticion.ObtenerJson();

            if(Estado == null) { return false; }

            List<DatosReportePartidas> partidas = JsonConvertidor.Json_ListaObjeto<DatosReportePartidas>(Estado);
            ViewState["Partidas"] = partidas;

            float sumaCosto = partidas.Sum(p => p.TotalPartidaCosto);
            float C_IVA = Convert.ToSingle(sumaCosto * 0.16);
            float C_Total = sumaCosto + C_IVA;

            float sumaPrecio = partidas.Sum(p => float.Parse(p.TotalPartidaVenta.ToString()));
            float P_IVA = Convert.ToSingle(sumaPrecio * 0.16);
            float P_Total = sumaPrecio + P_IVA;

            foreach (var partida in partidas)
            {
                partida.CostoFormato = partida.Costo.ToString("C", CultureInfo.CreateSpecificCulture("es-MX"));
                partida.PrecioFormato = partida.PrecioFinal.ToString("C", CultureInfo.CreateSpecificCulture("es-MX"));
            }

            partidas.ForEach(p => p.Porcentaje = (p.PorcentajeAumento).ToString("N2") + "%");

            List<DatosReportePartidas> Extras = new List<DatosReportePartidas>();
            foreach (var criterio in partidas)
            {
                if (criterio.CriterioAumento != null)
                {
                    criterio.BaseFormato = (criterio.precioBase).ToString("C2", new CultureInfo("es-MX"));
                    criterio.PrecioFormato = (criterio.PrecioFinal).ToString("C2", new CultureInfo("es-MX"));
                    Extras.Add(criterio);
                }
            }

            GvDatosCotizacion.DataSource = partidas;
            GvDatosCotizacion.DataBind();

            GvGastosExtra.DataSource = Extras;
            GvGastosExtra.DataBind();

            lblSubGastos.Text = sumaCosto.ToString("C2", new CultureInfo("es-MX")).Substring(1);
            lblValorIVA.Text = C_IVA.ToString("C2", new CultureInfo("es-MX")).Substring(1);
            TotalGastos.Text = C_Total.ToString("C2", new CultureInfo("es-MX")).Substring(1);

            lblSubtotal.Text = sumaPrecio.ToString("C2", new CultureInfo("es-MX")).Substring(1);
            lblIVA2.Text = P_IVA.ToString("C2", new CultureInfo("es-MX")).Substring(1);
            lblTotal.Text = P_Total.ToString("C2", new CultureInfo("es-MX")).Substring(1);

            MargenSinIVAtotal.Text = (sumaPrecio - sumaCosto).ToString("C2", new CultureInfo("es-MX"));
            MargenConIVAtotal.Text = (P_Total - C_Total).ToString("C2", new CultureInfo("es-MX"));
            return true;
        }

        protected void imgbtnAtrasVenta_Click(object sender, ImageClickEventArgs e)
        {
            Response.RedirectToRoute("Venta");
        }

        protected void ImgDescargarPDF_Click(object sender, ImageClickEventArgs e)
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

            string folio = Page.RouteData.Values["id"] as string;
            List<DatosReportePartidas> partidas = (List<DatosReportePartidas>)ViewState["Partidas"];

            PDFReporte pdf = new PDFReporte(partidas, Server.MapPath("/Media/Resources/fondo-1.png"));

            Estructura estructura = pdf.CrearPDF();

            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", $"attachment;filename=Reporte {folio}.pdf");
            HttpContext.Current.Response.Write(estructura.doc);
            Response.Flush();
            Response.End();

            estructura.writer.Close();
        }
    }
}