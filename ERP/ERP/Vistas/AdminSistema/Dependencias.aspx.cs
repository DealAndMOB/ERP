using ERP.Complementos.ProcesoBehind;
using ERP.Models;
using HTTPupt;
using iTextSharp.text.pdf.codec.wmf;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ERP.Vistas.AdminSistema
{
    public partial class Dependencias : System.Web.UI.Page
    {
        PeticionHTTP peticion = new PeticionHTTP(Url.ObtenerUrlServidor());

        String Json, Estado = String.Empty;
        int ID;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["jwt"] == null)
            {
                Response.RedirectToRoute("BorrarJWT");
                return;
            }

            if (!IsPostBack)
            {
                if (!MostrarZonasddl() || !MostrarCategoriaProductos() || !MostrarCategoriaProveedores() 
                    || !MostrarZonas() || !MostrarEstados()) { Url.CerrarSesion(this); return; }
                else
                {
                    Session["jwt"] = Url.ActualizarJWT((String)Session["jwt"]);
                }
            }

            if (Session["AlertaJS"] != null)
            {
                string alertaJS = (string)Session["AlertaJS"];
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "PDF", alertaJS, true);
                Session.Remove("AlertaJS");
            }
        }
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        protected void btnSubirCatProducto_Click(object sender, EventArgs e)
        {
            if (!ValidarCaracteres.NumerosYLetras(txtAddCatProducto.Text.Trim()))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alertaJS", "ErrorCaracteres()", true);
                BorrarDatos();
                return;
            }

            clsCategoriaProducto categoria = new clsCategoriaProducto()
            {
                Nombre = txtAddCatProducto.Text.Trim(),
            };

            Json = JsonConvertidor.Objeto_Json(categoria);
            peticion.PedirComunicacion("CategoriaProductos/AgregarCategoria", MetodoHTTP.POST, TipoContenido.JSON, (String)Session["jwt"]);
            peticion.enviarDatos(Json);

            Estado = peticion.ObtenerJson();

            if (Estado == null) { Url.CerrarSesion(this); return; }
            else
            {
                Session["jwt"] = Url.ActualizarJWT((String)Session["jwt"]);
            }

            PeticionEstado respuesta = JsonConvertidor.Json_Objeto<PeticionEstado>(Estado);
            Session["AlertaJS"] = respuesta.AlertaJS;

            Response.Redirect(Request.RawUrl);
        }

        protected void btnActualizarCatProducto_Click(object sender, EventArgs e)
        {
            if (!ValidarCaracteres.NumerosYLetras(txtUpdateCatProducto.Text.Trim()))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alertaJS", "ErrorCaracteres()", true);
                BorrarDatos();
                return;
            }

            clsCategoriaProducto categoria = new clsCategoriaProducto()
            {
                ID = (int)ViewState["CatProductoID"],
                Nombre = txtUpdateCatProducto.Text.Trim(),
            };

            Json = JsonConvertidor.Objeto_Json(categoria);
            peticion.PedirComunicacion("CategoriaProductos/ActualizarCategoria", MetodoHTTP.PUT, TipoContenido.JSON, (String)Session["jwt"]);
            peticion.enviarDatos(Json);

            Estado = peticion.ObtenerJson();

            if (Estado == null) { Url.CerrarSesion(this); return; }
            else
            {
                Session["jwt"] = Url.ActualizarJWT((String)Session["jwt"]);
            }

            PeticionEstado respuesta = JsonConvertidor.Json_Objeto<PeticionEstado>(Estado);
            Session["AlertaJS"] = respuesta.AlertaJS;

            Response.Redirect(Request.RawUrl);
        }

        //Desencadena la animación y carga de datos
        protected void gvCatProducto_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            string categoria = gvCatProducto.Rows[e.RowIndex].Cells[0].Text;

            peticion.PedirComunicacion("CategoriaProductos/ObtenerCategoriaID/" + HttpUtility.HtmlDecode(categoria), MetodoHTTP.GET, TipoContenido.JSON, (String)Session["jwt"]);
            Estado = peticion.ObtenerJson();

            if (Estado == null) { Url.CerrarSesion(this); return; }
            else
            {
                Session["jwt"] = Url.ActualizarJWT((String)Session["jwt"]);
            }

            ViewState["CatProductoID"] = int.Parse(Estado);

            txtUpdateCatProducto.Text = HttpUtility.HtmlDecode(categoria);
            ClientScript.RegisterStartupScript(GetType(), "script", "Updateproductos()", true);
        }

        protected void btnProductos_Click(object sender, ImageClickEventArgs e)
        {
            ClientScript.RegisterStartupScript(GetType(), "script", "productos()", true);
        }


        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        protected void btnSubirCatProveedor_Click(object sender, EventArgs e)
        {
            if (!ValidarCaracteres.NumerosYLetras(txtAddCatProveedor.Text.Trim()))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alertaJS", "ErrorCaracteres()", true);
                BorrarDatos();
                return;
            }

            clsCategoriaProveedor categoria = new clsCategoriaProveedor()
            {
                Nombre = txtAddCatProveedor.Text.Trim(),
            };

            Json = JsonConvertidor.Objeto_Json(categoria);
            peticion.PedirComunicacion("CategoriaProveedores/AgregarCategoriaProveedor", MetodoHTTP.POST, TipoContenido.JSON, (String)Session["jwt"]);
            peticion.enviarDatos(Json);

            Estado = peticion.ObtenerJson();

            if (Estado == null) { Url.CerrarSesion(this); return; }
            else
            {
                Session["jwt"] = Url.ActualizarJWT((String)Session["jwt"]);
            }

            PeticionEstado respuesta = JsonConvertidor.Json_Objeto<PeticionEstado>(Estado);
            Session["AlertaJS"] = respuesta.AlertaJS;

            Response.Redirect(Request.RawUrl);
        }

        protected void btnActualizarCatProveedor_Click(object sender, EventArgs e)
        {
            if (!ValidarCaracteres.NumerosYLetras(txtUpdateeCatProveedor.Text.Trim()))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alertaJS", "ErrorCaracteres()", true);
                BorrarDatos();
                return;
            }

            clsCategoriaProveedor categoria = new clsCategoriaProveedor()
            {
                ID = (int)ViewState["CatProveedorID"],
                Nombre = txtUpdateeCatProveedor.Text.Trim(),
            };

            Json = JsonConvertidor.Objeto_Json(categoria);
            peticion.PedirComunicacion("CategoriaProveedores/ActualizarCategoria", MetodoHTTP.PUT, TipoContenido.JSON, (String)Session["jwt"]);
            peticion.enviarDatos(Json);

            Estado = peticion.ObtenerJson();

            if (Estado == null) { Url.CerrarSesion(this); return; }
            else
            {
                Session["jwt"] = Url.ActualizarJWT((String)Session["jwt"]);
            }

            PeticionEstado respuesta = JsonConvertidor.Json_Objeto<PeticionEstado>(Estado);
            Session["AlertaJS"] = respuesta.AlertaJS;

            Response.Redirect(Request.RawUrl);
        }

        protected void gvCatProveedor_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            string categoria = gvCatProveedor.Rows[e.RowIndex].Cells[0].Text;

            peticion.PedirComunicacion("CategoriaProveedores/ObtenerCategoriaID/" + HttpUtility.HtmlDecode(categoria), MetodoHTTP.GET, TipoContenido.JSON, (String)Session["jwt"]);
            Estado = peticion.ObtenerJson();

            if (Estado == null) { Url.CerrarSesion(this); return; }
            else
            {
                Session["jwt"] = Url.ActualizarJWT((String)Session["jwt"]);
            }

            ViewState["CatProveedorID"] = int.Parse(Estado);

            txtUpdateeCatProveedor.Text = HttpUtility.HtmlDecode(categoria);
            ClientScript.RegisterStartupScript(GetType(), "script", "Updateproveedores()", true);
        }

        protected void btnProveedores_Click(object sender, ImageClickEventArgs e)
        {
            ClientScript.RegisterStartupScript(GetType(), "script", "proveedores()", true);
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        protected void btnSubirZona_Click(object sender, EventArgs e)
        {
            if (!ValidarCaracteres.NumerosYLetras(txtAddZona.Text.Trim()))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alertaJS", "ErrorCaracteres()", true);
                BorrarDatos();
                return;
            }

            clsZona zona = new clsZona()
            {
                Zona = txtAddZona.Text.Trim()
            };

            Json = JsonConvertidor.Objeto_Json(zona);
            peticion.PedirComunicacion("Zonas/AgregarZona", MetodoHTTP.POST, TipoContenido.JSON, (String)Session["jwt"]);
            peticion.enviarDatos(Json);

            Estado = peticion.ObtenerJson();

            if (Estado == null) { Url.CerrarSesion(this); return; }
            else
            {
                Session["jwt"] = Url.ActualizarJWT((String)Session["jwt"]);
            }

            PeticionEstado respuesta = JsonConvertidor.Json_Objeto<PeticionEstado>(Estado);
            Session["AlertaJS"] = respuesta.AlertaJS;

            Response.Redirect(Request.RawUrl);
        }

        protected void btnActualizarZona_Click(object sender, EventArgs e)
        {
            if (!ValidarCaracteres.NumerosYLetras(txtUpdateZona.Text.Trim()))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alertaJS", "ErrorCaracteres()", true);
                BorrarDatos();
                return;
            }

            clsZona categoria = new clsZona()
            {
                ID = (int)ViewState["ZonaID"],
                Zona = txtUpdateZona.Text.Trim(),
            };

            Json = JsonConvertidor.Objeto_Json(categoria);
            peticion.PedirComunicacion("Zonas/ActualizarZona", MetodoHTTP.PUT, TipoContenido.JSON, (String)Session["jwt"]);
            peticion.enviarDatos(Json);

            Estado = peticion.ObtenerJson();

            if (Estado == null) { Url.CerrarSesion(this); return; }
            else
            {
                Session["jwt"] = Url.ActualizarJWT((String)Session["jwt"]);
            }

            PeticionEstado respuesta = JsonConvertidor.Json_Objeto<PeticionEstado>(Estado);
            Session["AlertaJS"] = respuesta.AlertaJS;

            Response.Redirect(Request.RawUrl);
        }

        protected void gvZonas_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            string zona = gvZonas.Rows[e.RowIndex].Cells[0].Text;

            peticion.PedirComunicacion("Zonas/ObtenerCategoriaID/" + HttpUtility.HtmlDecode(zona), MetodoHTTP.GET, TipoContenido.JSON, (String)Session["jwt"]);
            Estado = peticion.ObtenerJson();

            if (Estado == null) { Url.CerrarSesion(this); return; }
            else
            {
                Session["jwt"] = Url.ActualizarJWT((String)Session["jwt"]);
            }

            ViewState["ZonaID"] = int.Parse(Estado);

            txtUpdateZona.Text = zona;
            txtUpdateZona.Text = HttpUtility.HtmlDecode(zona);
            ClientScript.RegisterStartupScript(GetType(), "script", "UpdateZona()", true);
        }

        protected void btnZonas_Click(object sender, ImageClickEventArgs e)
        {
            ClientScript.RegisterStartupScript(GetType(), "script", "Zona()", true);
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        protected void btnSubirEstado_Click(object sender, EventArgs e)
        {
            if (!ValidarCaracteres.NumerosYLetras(txtAddEstado.Text.Trim()))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alertaJS", "ErrorCaracteres()", true);
                BorrarDatos();
                return;
            }

            clsEstado estado = new clsEstado()
            {
                Nombre = txtAddEstado.Text.Trim(),
                ZonaID = int.Parse(ddlAddZona.SelectedValue)
            };

            Json = JsonConvertidor.Objeto_Json(estado);
            peticion.PedirComunicacion("Estados/AgregarEstado", MetodoHTTP.POST, TipoContenido.JSON, (String)Session["jwt"]);
            peticion.enviarDatos(Json);

            Estado = peticion.ObtenerJson();

            if (Estado == null) { Url.CerrarSesion(this); return; }
            else
            {
                Session["jwt"] = Url.ActualizarJWT((String)Session["jwt"]);
            }

            PeticionEstado respuesta = JsonConvertidor.Json_Objeto<PeticionEstado>(Estado);
            Session["AlertaJS"] = respuesta.AlertaJS;

            Response.Redirect(Request.RawUrl);
        }

        protected void btnActualizarEstado_Click(object sender, EventArgs e)
        {
            if (!ValidarCaracteres.NumerosYLetras(txtUpdateEstado.Text.Trim()))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alertaJS", "ErrorCaracteres()", true);
                BorrarDatos();
                return;
            }

            clsEstado estado = new clsEstado()
            {
                ID = (int)ViewState["EstadoID"],
                Nombre = txtUpdateEstado.Text.Trim(),
                ZonaID = int.Parse(ddlUpdateZona.SelectedValue)
            };

            Json = JsonConvertidor.Objeto_Json(estado);
            peticion.PedirComunicacion("Estados/ActualizarEstado", MetodoHTTP.PUT, TipoContenido.JSON, (String)Session["jwt"]);
            peticion.enviarDatos(Json);

            Estado = peticion.ObtenerJson();

            if (Estado == null) { Url.CerrarSesion(this); return; }
            else
            {
                Session["jwt"] = Url.ActualizarJWT((String)Session["jwt"]);
            }

            PeticionEstado respuesta = JsonConvertidor.Json_Objeto<PeticionEstado>(Estado);
            Session["AlertaJS"] = respuesta.AlertaJS;

            Response.Redirect(Request.RawUrl);
        }

        protected void gvEstados_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            string estado = gvEstados.Rows[e.RowIndex].Cells[0].Text;

            peticion.PedirComunicacion("Estados/ObtenerEstado/" + HttpUtility.HtmlDecode(estado), MetodoHTTP.GET, TipoContenido.JSON, (String)Session["jwt"]);
            Estado = peticion.ObtenerJson();
            EstadoDTO RegistroEstado = JsonConvertidor.Json_ListaObjeto<EstadoDTO>(Estado).FirstOrDefault();

            if (Estado == null) { Url.CerrarSesion(this); return; }
            else
            {
                Session["jwt"] = Url.ActualizarJWT((String)Session["jwt"]);
            }

            ViewState["EstadoID"] = RegistroEstado.ID;

            txtUpdateEstado.Text = HttpUtility.HtmlDecode(estado);
            ddlUpdateZona.SelectedValue = RegistroEstado.ZonaID.ToString();

            ClientScript.RegisterStartupScript(GetType(), "script", "UpdateEstado()", true);
        }

        protected void imgbtnEstados_Click(object sender, ImageClickEventArgs e)
        {
            ClientScript.RegisterStartupScript(GetType(), "script", "Estado()", true);
        }
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        private bool MostrarCategoriaProductos()
        {
            peticion.PedirComunicacion("CategoriaProductos/MostrarCategorias/", MetodoHTTP.GET, TipoContenido.JSON, (String)Session["jwt"]);
            Estado = peticion.ObtenerJson();

            if (Estado == null) { return false; }

            List<CategoriaProductoDTO> catProductos = JsonConvertidor.Json_ListaObjeto<CategoriaProductoDTO>(Estado);

            gvCatProducto.DataSource = catProductos;
            gvCatProducto.DataBind();

            return true;
        }
        
        private bool MostrarCategoriaProveedores()
        {
            peticion.PedirComunicacion("CategoriaProveedores/MostrarCategoriasProveedor/", MetodoHTTP.GET, TipoContenido.JSON, (String)Session["jwt"]);
            Estado = peticion.ObtenerJson();

            if (Estado == null) { return false; }

            List<CategoriaProveedorDTO> catProveedores = JsonConvertidor.Json_ListaObjeto<CategoriaProveedorDTO>(Estado);

            gvCatProveedor.DataSource = catProveedores;
            gvCatProveedor.DataBind();

            return true;
        }
        
        private bool MostrarZonas()
        {
            peticion.PedirComunicacion("Zonas/MostrarZonas/", MetodoHTTP.GET, TipoContenido.JSON, (String)Session["jwt"]);
            Estado = peticion.ObtenerJson();

            if (Estado == null) { return false; }

            List<ZonaDTO> zonas = JsonConvertidor.Json_ListaObjeto<ZonaDTO>(Estado);

            gvZonas.DataSource = zonas;
            gvZonas.DataBind();

            return true;
        }
        
        private bool MostrarEstados()
        {
            peticion.PedirComunicacion("Estados/MostrarEstados/", MetodoHTTP.GET, TipoContenido.JSON, (String)Session["jwt"]);
            Estado = peticion.ObtenerJson();

            if (Estado == null) { return false; }

            List<EstadoDTO> estado = JsonConvertidor.Json_ListaObjeto<EstadoDTO>(Estado);

            gvEstados.DataSource = estado;
            gvEstados.DataBind();

            return true;
        }

        private bool MostrarZonasddl()
        {
            // Remover todos los elementos excepto el primero
            while (ddlAddZona.Items.Count > 1 && ddlUpdateZona.Items.Count > 1)
            {
                ddlAddZona.Items.RemoveAt(1);
                ddlUpdateZona.Items.RemoveAt(1);
            }

            ddlAddZona.DataValueField = "ID";
            ddlAddZona.DataTextField = "Zona";

            ddlUpdateZona.DataValueField = "ID";
            ddlUpdateZona.DataTextField = "Zona";

            peticion.PedirComunicacion("Zonas/MostrarZonas", MetodoHTTP.GET, TipoContenido.JSON, (String)Session["jwt"]);
            Estado = peticion.ObtenerJson();

            if (Estado == null) { return false; }
            else
            {
                Session["jwt"] = Url.ActualizarJWT((String)Session["jwt"]);
            }

            List<ZonaDTO> Zonas = JsonConvertidor.Json_ListaObjeto<ZonaDTO>(Estado);

            ddlAddZona.DataSource = Zonas;
            ddlAddZona.DataBind();

            ddlUpdateZona.DataSource = Zonas;
            ddlUpdateZona.DataBind();

            return true;
        }

        private void BorrarDatos()
        {
            ddlAddZona.SelectedValue = "";
            txtAddZona.Text = String.Empty;
            txtUpdateEstado.Text = String.Empty;
            txtAddCatProducto.Text = String.Empty;
            txtAddEstado.Text = String.Empty;
            txtAddCatProveedor.Text = String.Empty;
        }
    }
}