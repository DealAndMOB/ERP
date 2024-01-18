using ERP.Complementos.ProcesoBehind;
using ERP.Models;
using HTTPupt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ERP.Vistas.Catalogos
{
    public partial class Proveedores : System.Web.UI.Page
    {
        PeticionHTTP peticion = new PeticionHTTP(Url.ObtenerUrlServidor());
        Modelo bd = new Modelo();

        //variables utilizadas
        private String Json, Estado;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["jwt"] == null)
            {
                Response.RedirectToRoute("BorrarJWT");
            }

            if (!IsPostBack)
            {
                if (!(MostrarProveedores() && MostrarCategoriasProv() && MostrarZonas() && MostrarEstados()))
                {
                    Url.CerrarSesion(this);
                    return;
                }
                else
                {
                    Session["jwt"] = Url.ActualizarJWT((String)Session["jwt"]);
                }
            }
        }

        protected void btnAgregarProveedor_Click(object sender, ImageClickEventArgs e)
        {
            clsProveedor proveedor = new clsProveedor()
            {
                RFC = txtRFCProveedor.Text.Trim(),
                CategoriaProveedorID = int.Parse(ddlCategoriaProveedor.SelectedValue),
                EstadoID = int.Parse(ddlEstados.SelectedValue),
                Empresa = txtEmpresaProveedor.Text.Trim(),
                RazonSocial = txtRazonSocialProveedor.Text.Trim(),
                NombreContacto = txtContactoProveedor.Text.Trim(),
                Descripcion = txtDescripcionProveedor.Text.Trim(),
                CorreoPagina = txtCorreoPaginaProveedor.Text.Trim(),
                Telefono = txtTelefonoProveedor.Text.Trim(),
                Direccion = txtDireccionProveedor.Text.Trim()
            };

            Json = JsonConvertidor.Objeto_Json(proveedor);
            peticion.PedirComunicacion("Proveedores/AgregarProveedor", MetodoHTTP.POST, TipoContenido.JSON, (String)Session["jwt"]);
            peticion.enviarDatos(Json);

            Estado = peticion.ObtenerJson();

            // VALIDAR SESIÓN - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
            if (Estado == null) { Url.CerrarSesion(this); return; }
            else
            {
                Session["jwt"] = Url.ActualizarJWT((String)Session["jwt"]);
            }
            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 

            PeticionEstado respuesta = JsonConvertidor.Json_Objeto<PeticionEstado>(Estado);
            if (!respuesta.Estado)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alertaJS", respuesta.AlertaJS, true);
                return;
            }

            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertaJS", respuesta.AlertaJS, true);

            BorrarCampos();
            MostrarProveedores();
        }

        protected void gvProveedores_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            // Obtener el valor de la primera celda (columna) de la fila seleccionada
            string RFC = gvProveedores.Rows[e.RowIndex].Cells[0].Text;

            peticion.PedirComunicacion("Proveedores/BuscarProveedor/" + RFC, MetodoHTTP.GET, TipoContenido.JSON, (String)Session["jwt"]);
            Estado = peticion.ObtenerJson();

            // VALIDAR SESIÓN - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
            if (Estado == null) { Url.CerrarSesion(this); return; }
            else
            {
                Session["jwt"] = Url.ActualizarJWT((String)Session["jwt"]);
            }
            // VALIDAR SESIÓN - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
            List<ProveedorDTO> proveedor = JsonConvertidor.Json_ListaObjeto<ProveedorDTO>(Estado);

            txtRFCProveedor.Text = proveedor.FirstOrDefault().RFC;
            ddlCategoriaProveedor.SelectedValue = proveedor.FirstOrDefault().CategoriaID.ToString();
            ddlEstados.SelectedValue = proveedor.FirstOrDefault().EstadoID.ToString();
            txtEmpresaProveedor.Text = proveedor.FirstOrDefault().Empresa;
            txtRazonSocialProveedor.Text = proveedor.FirstOrDefault().RazonSocial;
            txtContactoProveedor.Text = proveedor.FirstOrDefault().NombreContacto;
            txtDescripcionProveedor.Text = proveedor.FirstOrDefault().Descripcion;
            txtCorreoPaginaProveedor.Text = proveedor.FirstOrDefault().CorreoPagina;
            txtTelefonoProveedor.Text = proveedor.FirstOrDefault().Telefono;
            txtDireccionProveedor.Text = proveedor.FirstOrDefault().Descripcion;

            ViewState["ProveedorID"] = proveedor.FirstOrDefault().ID;

            gvProveedores.DataSource = proveedor;
            gvProveedores.DataBind();

            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertaJS", "actualizarBTNs()", true);
        }

        protected void CancelarActuBTN_Click(object sender, ImageClickEventArgs e)
        {
            ClientScript.RegisterStartupScript(GetType(), "ReiniciarBTNs", "ReiniciarBTNs()", true);
            BorrarCampos();

            if (!MostrarProveedores()) { Url.CerrarSesion(this); return; }
            else
            {
                Session["jwt"] = Url.ActualizarJWT((String)Session["jwt"]);
            }
        }
        protected void btnActualizarProveedor_Click(object sender, ImageClickEventArgs e)
        {
            clsProveedor proveedor = new clsProveedor()
            {
                ID = (int)ViewState["ProveedorID"],
                RFC = txtRFCProveedor.Text.Trim(),
                CategoriaProveedorID = int.Parse(ddlCategoriaProveedor.SelectedValue),
                EstadoID = int.Parse(ddlEstados.SelectedValue),
                Empresa = txtEmpresaProveedor.Text.Trim(),
                RazonSocial = txtRazonSocialProveedor.Text.Trim(),
                NombreContacto = txtContactoProveedor.Text.Trim(),
                Descripcion = txtDescripcionProveedor.Text.Trim(),
                CorreoPagina = txtCorreoPaginaProveedor.Text.Trim(),
                Telefono = txtTelefonoProveedor.Text.Trim(),
                Direccion = txtDireccionProveedor.Text.Trim()
            };

            Json = JsonConvertidor.Objeto_Json(proveedor);
            peticion.PedirComunicacion("Proveedores/ActualizarProveedor", MetodoHTTP.PUT, TipoContenido.JSON, (String)Session["jwt"]);
            peticion.enviarDatos(Json);

            Estado = peticion.ObtenerJson();

            // VALIDAR SESIÓN - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
            if (Estado == null) { Url.CerrarSesion(this); return; }
            else
            {
                Session["jwt"] = Url.ActualizarJWT((String)Session["jwt"]);
            }
            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 

            PeticionEstado respuesta = JsonConvertidor.Json_Objeto<PeticionEstado>(Estado);

            if (!respuesta.Estado)
            {
                BorrarCampos();
                MostrarProveedores();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alertaJS", respuesta.AlertaJS, true);
                return;
            }

            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertaJS", respuesta.AlertaJS, true);

            BorrarCampos();
            MostrarProveedores();
        }

        protected void btnBuscar_Click(object sender, ImageClickEventArgs e)
        {
            int FiltroZonas = int.Parse(ddlFiltroZonas.SelectedValue);
            int FiltroCategoria = int.Parse(ddlFiltroCategorias.SelectedValue);

            if (FiltroZonas.Equals(0) && FiltroCategoria.Equals(0))
            {
                if (!MostrarProveedores()) { Url.CerrarSesion(this); return; }
                else
                {
                    Session["jwt"] = Url.ActualizarJWT((String)Session["jwt"]);
                }
                BorrarCampos();
                return;
            }

            if (FiltroZonas.Equals(0) || FiltroCategoria.Equals(0))
            {
                if (!MostrarProveedores(FiltroZonas, FiltroCategoria)) { Url.CerrarSesion(this); return; }
                else
                {
                    Session["jwt"] = Url.ActualizarJWT((String)Session["jwt"]);
                }
                BorrarCampos();
                return;
            }

            StringBuilder Controlador = new StringBuilder("Proveedores/BusquedaCombinada/");
            Controlador.Append($"?ZonaID={FiltroZonas}&CategoriaID={FiltroCategoria}").ToString();

            peticion.PedirComunicacion(Controlador.ToString(), MetodoHTTP.GET, TipoContenido.JSON, (String)Session["jwt"]);
            Estado = peticion.ObtenerJson();

            // VALIDAR SESIÓN - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
            if (Estado == null) { Url.CerrarSesion(this); return; }
            else
            {
                Session["jwt"] = Url.ActualizarJWT((String)Session["jwt"]);
            }
            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 

            List<ProveedorDTO> proveedor = JsonConvertidor.Json_ListaObjeto<ProveedorDTO>(Estado);

            gvProveedores.DataSource = proveedor;
            gvProveedores.DataBind();

            BorrarCampos();
            return;
        }

        private bool MostrarProveedores(int? ZonaID = null, int? CategoriaID = null)
        {
            if (CategoriaID.HasValue || ZonaID.HasValue)
            {
                peticion.PedirComunicacion($"Proveedores/BusquedaIndividual/?ZonaID={ZonaID}&CategoriaID={CategoriaID}", MetodoHTTP.GET, TipoContenido.JSON, (String)Session["jwt"]);
                Estado = peticion.ObtenerJson();

                // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
                if (Estado == null) { return false; }
                // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

                List<ProveedorDTO> resultados = JsonConvertidor.Json_ListaObjeto<ProveedorDTO>(Estado);

                gvProveedores.DataSource = resultados;
                gvProveedores.DataBind();
                return true;
            }

            peticion.PedirComunicacion("Proveedores/MostrarProveedores/", MetodoHTTP.GET, TipoContenido.JSON, (String)Session["jwt"]);
            Estado = peticion.ObtenerJson();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
            if (Estado == null) { return false; }
            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            List<ProveedorDTO> todos = JsonConvertidor.Json_ListaObjeto<ProveedorDTO>(Estado);

            gvProveedores.DataSource = todos;
            gvProveedores.DataBind();
            return true;
        }

        private bool MostrarCategoriasProv()
        {
            ddlCategoriaProveedor.DataValueField = "ID";
            ddlCategoriaProveedor.DataTextField = "Nombre";

            ddlFiltroCategorias.DataValueField = "ID";
            ddlFiltroCategorias.DataTextField = "Nombre";

            peticion.PedirComunicacion("CategoriaProveedores/MostrarCategoriasProveedor", MetodoHTTP.GET, TipoContenido.JSON, (String)Session["jwt"]);
            Estado = peticion.ObtenerJson();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
            if (Estado == null) { return false; }
            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            List<CategoriaProveedorDTO> Categorias = JsonConvertidor.Json_ListaObjeto<CategoriaProveedorDTO>(Estado);

            ddlCategoriaProveedor.DataSource = Categorias;
            ddlCategoriaProveedor.DataBind();

            ddlFiltroCategorias.DataSource = Categorias;
            ddlFiltroCategorias.DataBind();
            return true;
        }


        private bool MostrarZonas()
        {
            ddlFiltroZonas.DataValueField = "ID";
            ddlFiltroZonas.DataTextField = "Zona";

            peticion.PedirComunicacion("Zonas/MostrarZonas", MetodoHTTP.GET, TipoContenido.JSON, (String)Session["jwt"]);
            Estado = peticion.ObtenerJson();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
            if (Estado == null) { return false; }
            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            List<ZonaDTO> Zonas = JsonConvertidor.Json_ListaObjeto<ZonaDTO>(Estado);

            ddlFiltroZonas.DataSource = Zonas;
            ddlFiltroZonas.DataBind();

            return true;
        }
        private bool MostrarEstados()
        {
            ddlEstados.DataValueField = "ID";
            ddlEstados.DataTextField = "Nombre";

            peticion.PedirComunicacion("Estados/MostrarEstados", MetodoHTTP.GET, TipoContenido.JSON, (String)Session["jwt"]);
            Estado = peticion.ObtenerJson();

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
            if (Estado == null) { return false; }
            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            List<EstadoDTO> estados = JsonConvertidor.Json_ListaObjeto<EstadoDTO>(Estado);

            ddlEstados.DataSource = estados;
            ddlEstados.DataBind();

            return true;
        }

        private void BorrarCampos()
        {
            ddlEstados.SelectedValue = "";
            ddlCategoriaProveedor.SelectedValue = "";

            txtRFCProveedor.Text = String.Empty;
            txtEmpresaProveedor.Text = String.Empty;
            txtRazonSocialProveedor.Text = String.Empty;
            txtContactoProveedor.Text = String.Empty;
            txtDescripcionProveedor.Text = String.Empty;
            txtCorreoPaginaProveedor.Text = String.Empty;
            txtTelefonoProveedor.Text = String.Empty;
            txtDireccionProveedor.Text = String.Empty;
        }
    }
}