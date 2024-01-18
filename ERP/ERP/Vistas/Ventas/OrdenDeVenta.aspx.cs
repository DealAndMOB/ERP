using ERP.Complementos.ProcesoBehind;
using ERP.Migrations;
using ERP.Models;
using ERP.Vistas.Catalogos;
using ERP.Vistas.Compras;
using HTTPupt;
using iTextSharp.text.pdf.codec.wmf;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ERP.Vistas.Ventas
{
    public partial class OrdenDeVenta : System.Web.UI.Page
    {
        PeticionHTTP peticion = new PeticionHTTP(Url.ObtenerUrlServidor());
        string Estado;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["jwt"] == null)
            {
                Response.RedirectToRoute("BorrarJWT");
                return;
            }
        }
        protected void ddlEstado_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool estado = Convert.ToBoolean(Convert.ToInt32(ddlEstado.SelectedItem.Value));

            var url = $"Cotizaciones/MostrarCotizaciones/{estado}";

            peticion.PedirComunicacion(url, MetodoHTTP.GET, TipoContenido.JSON, (String)Session["jwt"]);
            Estado = peticion.ObtenerJson();
            // VALIDAR SESIÓN - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
            if (Estado == null) { Url.CerrarSesion(this); return; }
            else
            {
                Session["jwt"] = Url.ActualizarJWT((String)Session["jwt"]);
            }
            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
            List<CotizacionDTO> cotizaciones = JsonConvertidor.Json_ListaObjeto<CotizacionDTO>(Estado);

            var formatoFecha = estado ? "FechaVenta" : "FechaCotizacion";
            var formatoTotal = "C2";

            cotizaciones.ForEach(c =>
            {
                c.FechaCorta = (DateTime.Parse(c.GetType().GetProperty(formatoFecha).GetValue(c).ToString())).ToShortDateString();
                c.TotalFormato = c.Total.ToString(formatoTotal, new CultureInfo("es-MX"));
            });

            gvCotizacionVenta.DataSource = cotizaciones;
            gvCotizacionVenta.DataBind();
        }

        protected void btnEditCotizacion_Click(object sender, ImageClickEventArgs e)
        {
            string Folio = GridViewButon(sender, 0);
            string estado = GridViewButon(sender, 4);

            if (!Boolean.Parse(estado))
            {
                Response.RedirectToRoute("Cotizacion", new { id = Folio });
                return;
            }
            else
            {
                Response.RedirectToRoute("Venta", new { id = Folio });
                return;
            }
        }

        private string GridViewButon(object sender, int numCelda)
        {
            ImageButton btn = (ImageButton)sender;
            GridViewRow Fila = (GridViewRow)btn.NamingContainer;
            string dato = Fila.Cells[numCelda].Text;

            return dato;
        }

        protected void btnBuscarFolio_Click(object sender, ImageClickEventArgs e)
        {
            string Busqueda = txtBusquedaFolio.Text;
            if (!ValidarCaracteres.NumerosYLetras(Busqueda)) { return; };

            var url = $"Cotizaciones/BuscarFolioCotizacion/{Busqueda}";

            peticion.PedirComunicacion(url, MetodoHTTP.GET, TipoContenido.JSON, (String)Session["jwt"]);
            Estado = peticion.ObtenerJson();
            // VALIDAR SESIÓN - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
            if (Estado == null) { Url.CerrarSesion(this); return; }
            else
            {
                Session["jwt"] = Url.ActualizarJWT((String)Session["jwt"]);
            }
            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
            List<CotizacionDTO> cotizaciones = JsonConvertidor.Json_ListaObjeto<CotizacionDTO>(Estado);

            if(cotizaciones.Count == 0) { return; }

            var formatoFecha = cotizaciones.FirstOrDefault().Estatus ? "FechaVenta" : "FechaCotizacion";
            var formatoTotal = "C2";

            cotizaciones.ForEach(c =>
            {
                c.FechaCorta = (DateTime.Parse(c.GetType().GetProperty(formatoFecha).GetValue(c).ToString())).ToShortDateString();
                c.TotalFormato = c.Total.ToString(formatoTotal, new CultureInfo("es-MX"));
            });

            gvCotizacionVenta.DataSource = cotizaciones;
            gvCotizacionVenta.DataBind();
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 

    }
}