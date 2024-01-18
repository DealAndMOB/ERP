using ERP.Complementos.ProcesoBehind;
using ERP.Migrations;
using ERP.Models;
using HTTPupt;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.ModelBinding;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ERP.Vistas.Ventas
{
    public partial class CrearRemision : System.Web.UI.Page
    {
        PeticionHTTP peticion = new PeticionHTTP(Url.ObtenerUrlServidor());
        string Json = string.Empty, FolioRemision = string.Empty, Estado = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["jwt"] == null)
            {
                Response.RedirectToRoute("BorrarJWT");
                return;
            }

            if (!IsPostBack)
            {
                ViewState["FolioRemision"] = null;

                if (!ObtenerCotizacion()) { Url.CerrarSesion(this); return; }
                else
                {
                    Session["jwt"] = Url.ActualizarJWT((String)Session["jwt"]);
                }
            }
        }

        private bool ObtenerCotizacion()
        {
            string folioVenta = Page.RouteData.Values["id"] as string;
            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
            peticion.PedirComunicacion($"Cotizaciones/ObtenerCotizacionCompleta/{folioVenta}", MetodoHTTP.GET, TipoContenido.JSON, (String)Session["jwt"]);
            Estado = peticion.ObtenerJson();
            // VALIDAR SESIÓN
            if (Estado == null) { return false; }

            CotizacionDTO cotizacion = JsonConvert.DeserializeObject<CotizacionDTO>(Estado);
            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
            peticion.PedirComunicacion($"PartidaRemisiones/TotalesPorRemision/{folioVenta}", MetodoHTTP.GET, TipoContenido.JSON, (String)Session["jwt"]);
            Json = peticion.ObtenerJson();

            List<PartidaRemisionDTO> partidasRemision = JsonConvertidor.Json_ListaObjeto<PartidaRemisionDTO>(Json);
            ViewState["PartidasCotizacionVenta"] = partidasRemision;
            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
            lblCliente.Text = cotizacion.Cliente;

            gvPartidas.DataSource = partidasRemision;
            gvPartidas.DataBind();

            return true;
        }

        protected void GenerarFolio_Click(object sender, EventArgs e)
        {
            string folioVenta = Page.RouteData.Values["id"] as string;
            ViewState["FolioRemision"] = CrearFolio(folioVenta);

            lblFolio.Text = (String)ViewState["FolioRemision"];
        }

        //protected void BtnConfirmar_Click(object sender, ImageClickEventArgs e)
        protected void BtnConfirmar_Click(object sender, EventArgs e)
        {
            string folioVenta = Page.RouteData.Values["id"] as string;

            // Validar Fecha - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
            (DateTime FechaDeEntrega, bool estadoFecha) = validarFecha();
            if (!estadoFecha)
                return;

            if (ViewState["FolioRemision"] == null)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alertaJS", "RemisionSinFolio()", true);
                return;
            }

            peticion.PedirComunicacion($"Remisiones/ComprobarFolio/{(String)ViewState["FolioRemision"]}", MetodoHTTP.GET, TipoContenido.JSON, (String)Session["jwt"]);
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
            if (!respuesta.Estado && respuesta.AlertaJS.StartsWith("D"))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alertaJS", respuesta.AlertaJS, true);
                return;
            }
            // VALIDAR SESIÓN - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 

            List<PartidaRemisionDTO> PartidasVenta = (List<PartidaRemisionDTO>)ViewState["PartidasCotizacionVenta"];
            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 

            List<clsPartidaRemision> partidas = new List<clsPartidaRemision>();
            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
            foreach (GridViewRow fila in gvPartidas.Rows)
            {
                (CheckBox estado, TextBox txtCantidad, Label lblErrorUnidades) = ObtenerControlesDeFila(fila);

                if (!estado.Checked)
                    continue; // Saltar a la siguiente iteración

                if (string.IsNullOrEmpty(txtCantidad.Text))
                {
                    lblErrorUnidades.Text = "CAMPO OBLIGATORIO";
                    return;
                }

                TableCell disponibles = fila.Cells[5];
                if (int.Parse(txtCantidad.Text) > int.Parse(disponibles.Text))
                {
                    lblErrorUnidades.Text = "¡Cantidad Excedida!";
                    return;
                }
                lblErrorUnidades.Text = string.Empty;

                clsPartidaRemision partida = new clsPartidaRemision()
                {
                    Folio = (String)ViewState["FolioRemision"],
                    PartidaCotizacionVentaID = (PartidasVenta[int.Parse(fila.Cells[1].Text) - 1].PartidaVentaID),
                    CantidadRemitida = int.Parse(txtCantidad.Text)
                };
                partidas.Add(partida);
            }

            if (partidas.Count == 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alertaJS", "SinPartidasSeleccionadas()", true);
                return;
            }

            clsRemision remision = new clsRemision()
            {
                Folio = (String)ViewState["FolioRemision"],
                FolioVenta = folioVenta,
                FechaEntrega = FechaDeEntrega
            };
            Json = JsonConvertidor.Objeto_Json(remision);
            peticion.PedirComunicacion("Remisiones/AgregarRemision", MetodoHTTP.POST, TipoContenido.JSON, (String)Session["jwt"]);
            peticion.enviarDatos(Json);

            Json = peticion.ObtenerJson();

            if (!Boolean.Parse(Json))
            {
                lblCliente.Text = "No se pudo subir la remisión";
                return;
            }

            Json = JsonConvertidor.Objeto_Json(partidas);
            peticion.PedirComunicacion("PartidaRemisiones/AgregarPartidas", MetodoHTTP.POST, TipoContenido.JSON, (String)Session["jwt"]);
            peticion.enviarDatos(Json);

            Json = peticion.ObtenerJson();
            Response.RedirectToRoute("ConsultarRemision", new { id = (String)ViewState["FolioRemision"] }); // --> Redirección con todo y ID
            return;
        }

        private (DateTime, bool) validarFecha()
        {
            DateTime FechaDeEntrega;
            if (!DateTime.TryParseExact(txtFechaEntrega.Text, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out FechaDeEntrega))
            {
                //lblCliente.Text = "";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alertaJS", "SinFecha()", true);
                return (FechaDeEntrega, false);
            }

            DateTime fechaActual = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
            int comparacion = FechaDeEntrega.CompareTo(fechaActual);

            if (comparacion == -1)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alertaJS", "FechaInferior()", true);
                return (FechaDeEntrega, false);
            }
            else
            {
                return (FechaDeEntrega, true);
            }
        }

        private string CrearFolio(string folioVenta)
        {
            //// - - - - - Creación del folio - - - - - //// 
            CrearCodigo crearFolio = new CrearCodigo();

            peticion.PedirComunicacion("Remisiones/ObtenerFolioFinal/", MetodoHTTP.GET, TipoContenido.JSON, (String)Session["jwt"]);
            Estado = peticion.ObtenerJson();

            //SE VALIDA EL ESTADO DE LA SESIÓN EN EL EVENTO PRINCIPAL
            //if(Estado == null) { return null; }

            string folioAnterior = Estado.Substring(1, (Estado.Length) - 2);
            string prefijo = folioVenta.Substring(0, 3) + "REM";

            if (folioAnterior != "null")
            {
                string folio = crearFolio.GenerarCodigo(prefijo, folioAnterior);
                FolioRemision = folio;
            }
            else
            {
                string folio = crearFolio.GenerarCodigo(prefijo, 7);
                FolioRemision = folio;
            }

            return FolioRemision;
        }

        protected void btnAlertaConfirmacion_Click(object sender, ImageClickEventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Peticion", "alertConfirmar() ", true);
        }

        protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
        {
            string folio = Page.RouteData.Values["id"] as string;

            Response.RedirectToRoute("Remitidos", new { id = folio });
            return;
        }

        private (CheckBox, TextBox, Label) ObtenerControlesDeFila(GridViewRow fila)
        {
            CheckBox estado = (CheckBox)fila.FindControl("chbxStatus");
            TextBox txtCantidad = (TextBox)fila.FindControl("txtCantidad");
            Label lblErrorUnidades = (Label)fila.FindControl("lblErrorUnidades");

            return (estado, txtCantidad, lblErrorUnidades);
        }


    }
}