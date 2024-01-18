using ERP.Complementos.ProcesoBehind;
using ERP.Models;
using HTTPupt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ERP.Vistas.AdminSistema
{
    public partial class Usuarios : System.Web.UI.Page
    {
        PeticionHTTP peticion = new PeticionHTTP(Url.ObtenerUrlServidor());
        private string Json, Estado;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["jwt"] == null)
            {
                Response.RedirectToRoute("BorrarJWT");
                return;
            }

            if (!IsPostBack)
            {
                if (!CargarPerfiles() || !MostrarGV()) { Url.CerrarSesion(this); return; }
                else
                {
                    Session["jwt"] = Url.ActualizarJWT((String)Session["jwt"]);
                }
            }
        }
        // Operaciones Usuarios - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        protected void btnSubirUsuario_Click(object sender, ImageClickEventArgs e)
        {
            clsUsuario usuario = new clsUsuario()
            {
                Nombre = txtNombre.Text.Trim(),
                Correo = txtCorreo.Text.Trim(),
                Contraseña = txtContraseña.Text,
                PerfilID = int.Parse(ddlPerfiles.SelectedValue)
            };

            Json = JsonConvertidor.Objeto_Json(usuario);
            peticion.PedirComunicacion("Usuarios/AgregarUsuario", MetodoHTTP.POST, TipoContenido.JSON, (String)Session["jwt"]);
            peticion.enviarDatos(Json);

            Estado = peticion.ObtenerJson();

            //VALIDAR SESIÓN - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            if (Estado == null) { Url.CerrarSesion(this); return; }
            else
            {
                Session["jwt"] = Url.ActualizarJWT((String)Session["jwt"]);
            }
            //VALIDAR SESIÓN - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            PeticionEstado respuesta = JsonConvertidor.Json_Objeto<PeticionEstado>(Estado);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertaJS", respuesta.AlertaJS, true);

            BorrarCampos();
            MostrarGV();
        }

        protected void btnActualizarUsuario_Click(object sender, ImageClickEventArgs e)
        {
            clsUsuario usuario = new clsUsuario()
            {
                ID = (int)ViewState["UsuarioID"],
                Nombre = txtUpdateNombre.Text.Trim(),
                Correo = txtUpdateCorreo.Text.Trim(),
                Contraseña = txtUpdateContraseña.Text,
                PerfilID = int.Parse(ddlUpdatePerfiles.SelectedValue)
            };

            Json = JsonConvertidor.Objeto_Json(usuario);
            peticion.PedirComunicacion("Usuarios/ActualizarUsuario", MetodoHTTP.PUT, TipoContenido.JSON, (String)Session["jwt"]);
            peticion.enviarDatos(Json);

            Estado = peticion.ObtenerJson(); // Respuesta de la petición al servidor

            //VALIDAR SESIÓN - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            if (Estado == null) { Url.CerrarSesion(this); return; }
            else
            {
                Session["jwt"] = Url.ActualizarJWT((String)Session["jwt"]);
            }
            //VALIDAR SESIÓN - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            PeticionEstado respuesta = JsonConvertidor.Json_Objeto<PeticionEstado>(Estado);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertaJS", respuesta.AlertaJS, true);

            BorrarCampos();
            MostrarGV();
        }

        protected void gvUsuarios_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            ViewState["UsuarioID"] = int.Parse(gvUsuarios.Rows[e.RowIndex].Cells[0].Text);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Peticion", "alertConfirmar() ", true);
        }

        protected void btnEliminarUsuario_Click(object sender, EventArgs e)
        {
            int usuarioID = (int)ViewState["UsuarioID"];
            peticion.PedirComunicacion("Usuarios/BorrarUsuario/" + usuarioID, MetodoHTTP.DELETE, TipoContenido.JSON, (String)Session["jwt"]);
            peticion.enviarDatos(Json);

            Estado = peticion.ObtenerJson();

            //VALIDAR SESIÓN - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            if (Estado == null) { Url.CerrarSesion(this); return; }
            else
            {
                Session["jwt"] = Url.ActualizarJWT((String)Session["jwt"]);
            }
            //VALIDAR SESIÓN - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            PeticionEstado respuesta = JsonConvertidor.Json_Objeto<PeticionEstado>(Estado);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertaJS", respuesta.AlertaJS, true);
            MostrarGV();
        }
        protected void gvUsuarios_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int usuarioID = int.Parse(gvUsuarios.Rows[e.RowIndex].Cells[0].Text);

            peticion.PedirComunicacion("Usuarios/ObtenerUsuario/" + usuarioID, MetodoHTTP.GET, TipoContenido.JSON, (String)Session["jwt"]);
            Estado = peticion.ObtenerJson();

            //VALIDAR SESIÓN - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            if (Estado == null) { Url.CerrarSesion(this); return; }
            else
            {
                Session["jwt"] = Url.ActualizarJWT((String)Session["jwt"]);
            }
            //VALIDAR SESIÓN - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            UsuarioDTO usuario = JsonConvertidor.Json_ListaObjeto<UsuarioDTO>(Estado).FirstOrDefault();

            ViewState["UsuarioID"] = usuario.ID;

            txtUpdateNombre.Text = usuario.Nombre;
            txtUpdateCorreo.Text = usuario.Correo;
            txtUpdateContraseña.Text = usuario.Contraseña;
            ddlUpdatePerfiles.SelectedValue = usuario.PerfilID.ToString();

            ClientScript.RegisterStartupScript(GetType(), "script", "UpdateViewUsuario()", true);
        }

        // Operaciones perfiles - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        protected void btnPerfil_Click(object sender, ImageClickEventArgs e) // Se sube el perfil
        {
            Dictionary<int, CheckBox> accesos = new Dictionary<int, CheckBox>()
            {
                {1, ChBoxCompras},
                {2, ChBoxVentas},
                {3, chBoxCatalogos},
                {4, ChBoxSistema},
            };

            string CadenaAccesos = string.Empty;
            for (int i = 0; i < accesos.Count; i++)
            {
                CadenaAccesos += Convert.ToInt32(accesos[i + 1].Checked);
            }

            clsPerfil perfil = new clsPerfil()
            {
                Nombre = txtPerfil.Text.Trim(),
                Accesos = CadenaAccesos,
            };

            Json = JsonConvertidor.Objeto_Json(perfil);
            peticion.PedirComunicacion("Perfiles/AgregarPerfil", MetodoHTTP.POST, TipoContenido.JSON, (String)Session["jwt"]);
            peticion.enviarDatos(Json);

            Estado = peticion.ObtenerJson();

            //VALIDAR SESIÓN - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            if (Estado == null) { Url.CerrarSesion(this); return; }
            else
            {
                Session["jwt"] = Url.ActualizarJWT((String)Session["jwt"]);
            }
            //VALIDAR SESIÓN - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            PeticionEstado respuesta = JsonConvertidor.Json_Objeto<PeticionEstado>(Estado);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertaJS", respuesta.AlertaJS, true);

            BorrarCampos();
            CargarPerfiles();
            MostrarGV();
        }

        protected void btnActualizarPerfil_Click(object sender, ImageClickEventArgs e) // Se actualizan los datos
        {
            Dictionary<int, CheckBox> accesos = new Dictionary<int, CheckBox>()
            {
                {1, ChBoxUpdateCompras},
                {2, ChBoxUpdateVentas},
                {3, ChBoxUpdateCatalogos},
                {4, ChBoxUpdateSistema},
            };

            string CadenaAccesos = string.Empty;
            for (int i = 0; i < accesos.Count; i++)
            {
                CadenaAccesos += Convert.ToInt32(accesos[i + 1].Checked);
            }

            clsPerfil perfil = new clsPerfil()
            {
                ID = (int)ViewState["PerfilID"],
                Nombre = txtUpdatePerfil.Text.Trim(),
                Accesos = CadenaAccesos,
            };

            Json = JsonConvertidor.Objeto_Json(perfil);
            peticion.PedirComunicacion("Perfiles/ActualizarPerfil/", MetodoHTTP.PUT, TipoContenido.JSON, (String)Session["jwt"]);
            peticion.enviarDatos(Json);

            Estado = peticion.ObtenerJson();

            //VALIDAR SESIÓN - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            if (Estado == null) { Url.CerrarSesion(this); return; }
            else
            {
                Session["jwt"] = Url.ActualizarJWT((String)Session["jwt"]);
            }
            //VALIDAR SESIÓN - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            PeticionEstado respuesta = JsonConvertidor.Json_Objeto<PeticionEstado>(Estado);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertaJS", respuesta.AlertaJS, true);

            MostrarGV();
            BorrarCampos();
            CargarPerfiles();
        }

        protected void gvPerfiles_RowUpdating(object sender, GridViewUpdateEventArgs e) // Obtengo datos para el formulario
        {
            int perfilID = int.Parse(gvPerfiles.Rows[e.RowIndex].Cells[0].Text);
            string NombrePerfil = HttpUtility.HtmlDecode(gvPerfiles.Rows[e.RowIndex].Cells[1].Text);

            ViewState["PerfilID"] = perfilID;

            //Obtener valores de acceso - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
            peticion.PedirComunicacion("Perfiles/ObtenerAccesos/" + perfilID, MetodoHTTP.GET, TipoContenido.JSON, (String)Session["jwt"]);
            Estado = peticion.ObtenerJson();

            //VALIDAR SESIÓN - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            if (Estado == null) { Url.CerrarSesion(this); return; }
            else
            {
                Session["jwt"] = Url.ActualizarJWT((String)Session["jwt"]);
            }
            //VALIDAR SESIÓN - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            string CadenaAccesos = Estado.Substring(1, (Estado.Length) - 2);

            Dictionary<int, CheckBox> accesos = new Dictionary<int, CheckBox>()
            {
                {1, ChBoxUpdateCompras},
                {2, ChBoxUpdateVentas},
                {3, ChBoxUpdateCatalogos},
                {4, ChBoxUpdateSistema},
            };

            for (int i = 0; i < accesos.Count; i++)
            {
                char caracter = CadenaAccesos[i];
                bool accesoBool = (caracter == '1');
                accesos[i + 1].Checked = accesoBool;
            }

            txtUpdatePerfil.Text = NombrePerfil;
            ClientScript.RegisterStartupScript(GetType(), "script", "UpdateViewNivel()", true);
        }

        protected void gvPerfiles_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            ViewState["PerfilID"] = int.Parse(gvPerfiles.Rows[e.RowIndex].Cells[0].Text);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Peticion", "alertConfirmar2() ", true);
        }

        protected void btnEliminarPerfil_Click(object sender, EventArgs e)
        {
            int perfilID = (int)ViewState["PerfilID"];

            peticion.PedirComunicacion("Perfiles/BorrarPerfil/" + perfilID, MetodoHTTP.DELETE, TipoContenido.JSON, (String)Session["jwt"]);
            peticion.enviarDatos(Json);

            Estado = peticion.ObtenerJson();

            //VALIDAR SESIÓN - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            if (Estado == null) { Url.CerrarSesion(this); return; }
            else
            {
                Session["jwt"] = Url.ActualizarJWT((String)Session["jwt"]);
            }
            //VALIDAR SESIÓN - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            PeticionEstado respuesta = JsonConvertidor.Json_Objeto<PeticionEstado>(Estado);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertaJS", respuesta.AlertaJS, true);

            MostrarGV();
            CargarPerfiles();
        }

        public bool MostrarGV()
        {
            peticion.PedirComunicacion("Usuarios/MostrarUsuarios", MetodoHTTP.GET, TipoContenido.JSON, (String)Session["jwt"]);
            Estado = peticion.ObtenerJson();

            if (Estado == null) { return false; }

            List<UsuarioDTO> usuarios = JsonConvertidor.Json_ListaObjeto<UsuarioDTO>(Estado);

            gvUsuarios.DataSource = usuarios;
            gvUsuarios.DataBind();

            peticion.PedirComunicacion("Perfiles/MostrarPerfiles", MetodoHTTP.GET, TipoContenido.JSON, (String)Session["jwt"]);
            Estado = peticion.ObtenerJson();

            if (Estado == null) { return false; }

            List<PerfilDTO> perfiles = JsonConvertidor.Json_ListaObjeto<PerfilDTO>(Estado);

            gvPerfiles.DataSource = perfiles;
            gvPerfiles.DataBind();

            return true;
        }

        public bool CargarPerfiles()
        {
            while (ddlPerfiles.Items.Count > 1 && ddlUpdatePerfiles.Items.Count > 1)
            {
                ddlPerfiles.Items.RemoveAt(1);
                ddlUpdatePerfiles.Items.RemoveAt(1);
            }

            ddlPerfiles.DataValueField = "ID";
            ddlPerfiles.DataTextField = "Nombre";

            ddlUpdatePerfiles.DataValueField = "ID";
            ddlUpdatePerfiles.DataTextField = "Nombre";

            peticion.PedirComunicacion("Perfiles/MostrarPerfiles", MetodoHTTP.GET, TipoContenido.JSON, (String)Session["jwt"]);
            Estado = peticion.ObtenerJson();

            if (Estado == null) { return false; }

            List<PerfilDTO> perfiles = JsonConvertidor.Json_ListaObjeto<PerfilDTO>(Estado);

            ddlPerfiles.DataSource = perfiles;
            ddlPerfiles.DataBind();

            ddlUpdatePerfiles.DataSource = perfiles;
            ddlUpdatePerfiles.DataBind();

            return true;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        protected void imbtnNuevoUsuario_Click(object sender, ImageClickEventArgs e)
        {
            ClientScript.RegisterStartupScript(GetType(), "script", "ViewUsuario()", true);
        }

        protected void imbtnClose_Click(object sender, ImageClickEventArgs e)
        {
            ClientScript.RegisterStartupScript(GetType(), "script", "CloseUsuario()", true);
        }

        protected void imbtnNuevoNivel_Click(object sender, ImageClickEventArgs e)
        {
            ClientScript.RegisterStartupScript(GetType(), "script", "ViewNivel()", true);
        }

        protected void imgbtnCloseNivel_Click(object sender, ImageClickEventArgs e)
        {
            ClientScript.RegisterStartupScript(GetType(), "script", "CloseNivel()", true);
        }

        private void BorrarCampos()
        {
            txtUpdateNombre.Text = string.Empty;
            txtUpdateCorreo.Text = string.Empty;
            txtUpdateContraseña.Text = string.Empty;

            txtNombre.Text = string.Empty;
            txtCorreo.Text = string.Empty;
            txtContraseña.Text = string.Empty;

            ddlPerfiles.SelectedValue = "";
            ddlUpdatePerfiles.SelectedValue = "";

            txtPerfil.Text = string.Empty;
            txtUpdatePerfil.Text = string.Empty;

            ChBoxCompras.Checked = false;
            ChBoxVentas.Checked = false;
            chBoxCatalogos.Checked = false;
            ChBoxSistema.Checked = false;
            
            ChBoxUpdateCompras.Checked = false;
            ChBoxUpdateVentas.Checked = false;
            ChBoxUpdateCatalogos.Checked = false;
            ChBoxUpdateSistema.Checked = false;
        }

    }
}