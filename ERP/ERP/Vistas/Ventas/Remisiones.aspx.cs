using ERP.Complementos.ProcesoBehind;
using ERP.Models;
using HTTPupt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ERP.Vistas.Ventas
{
    public partial class Remisiones : System.Web.UI.Page
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
                if (!ObtenerRemisiones()) { Url.CerrarSesion(this); return; }
                else
                {
                    Session["jwt"] = Url.ActualizarJWT((String)Session["jwt"]);
                }
            }
        }

        private bool ObtenerRemisiones()
        {
            string folio = Page.RouteData.Values["id"] as string;
            peticion.PedirComunicacion($"Remisiones/MostrarRemisiones/{folio}", MetodoHTTP.GET, TipoContenido.JSON, (String)Session["jwt"]);
            Estado = peticion.ObtenerJson();

            if(Estado == null) { return false; }

            List<RemisionDTO> remisiones = JsonConvertidor.Json_ListaObjeto<RemisionDTO>(Estado);
            remisiones.ForEach(c => c.FechaFormato = c.FechaEntrega.ToShortDateString());

            gvRemisiones.DataSource = remisiones;
            gvRemisiones.DataBind();

            return true;
        }

        protected void btnCrearRemision_Click(object sender, ImageClickEventArgs e)
        {
            string folio = Page.RouteData.Values["id"] as string;

            Response.RedirectToRoute("CrearRemision", new { id = folio });
            return;
        }

        private string GridViewButon(object sender, int numCelda)
        {
            ImageButton btn = (ImageButton)sender;
            GridViewRow Fila = (GridViewRow)btn.NamingContainer;
            string dato = Fila.Cells[numCelda].Text;

            return dato;
        }

        protected void btnRemision_Click(object sender, ImageClickEventArgs e)
        {
            string Folio = GridViewButon(sender, 0);

            Response.RedirectToRoute("ConsultarRemision", new { id = Folio });
        }

        protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
        {
            string folio = Page.RouteData.Values["id"] as string;

            Response.RedirectToRoute("Venta", new { id = folio });
            return;
        }

        protected void btnBuscarFolio_Click(object sender, ImageClickEventArgs e)
        {
            string folioVenta = Page.RouteData.Values["id"] as string;
            string parametros = $"?id={txtBusquedaFolio.Text.Trim()}&folioVenta={folioVenta}";

            if (!ValidarCaracteres.NumerosYLetras(txtBusquedaFolio.Text)) { return; }

            peticion.PedirComunicacion($"Remisiones/BuscarRemision/{parametros}", MetodoHTTP.GET, TipoContenido.JSON, (String)Session["jwt"]);
            Estado = peticion.ObtenerJson();

            // VALIDAR SESIÓN - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
            if (Estado == null) { Url.CerrarSesion(this); return; }
            else
            {
                Session["jwt"] = Url.ActualizarJWT((String)Session["jwt"]);
            }
            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
            List<RemisionDTO> remisiones = JsonConvertidor.Json_ListaObjeto<RemisionDTO>(Estado);
            remisiones.ForEach(c => c.FechaFormato = c.FechaEntrega.ToShortDateString());

            gvRemisiones.DataSource = remisiones;
            gvRemisiones.DataBind();
        }
    }
}