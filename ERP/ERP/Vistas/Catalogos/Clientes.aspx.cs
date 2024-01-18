using ERP.Complementos.ProcesoBehind;
using ERP.Models;
using HTTPupt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
//using System.Net.Http;

namespace ERP.Vistas.Catalogos
{
    public partial class Clientes : System.Web.UI.Page
    {
        PeticionHTTP peticion = new PeticionHTTP(Url.ObtenerUrlServidor());

        String Json = String.Empty;
        String Estado = String.Empty;
        String RFC = String.Empty;
        String Busqueda = String.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["jwt"] == null)
            {
                Response.RedirectToRoute("BorrarJWT");
                return;
            }

            if (!IsPostBack)
            {
                if (!MostrarClientes()) { Url.CerrarSesion(this); return; }
                else
                {
                    Session["jwt"] = Url.ActualizarJWT((String)Session["jwt"]);
                }
            }
        }
        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 

        protected void gvClientes_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            // Obtener el valor de la primera celda (columna) de la fila seleccionada
            string RFC = gvClientes.Rows[e.RowIndex].Cells[0].Text;

            peticion.PedirComunicacion("Clientes/BuscarCliente/" + RFC, MetodoHTTP.GET, TipoContenido.JSON, (String)Session["jwt"]);
            Estado = peticion.ObtenerJson();

            // VALIDAR SESIÓN - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
            if (Estado == null) { Url.CerrarSesion(this); return; }
            else
            {
                Session["jwt"] = Url.ActualizarJWT((String)Session["jwt"]);
            }
            // VALIDAR SESIÓN - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
            List<ClienteDTO> cliente = JsonConvertidor.Json_ListaObjeto<ClienteDTO>(Estado);

            txtRFCCliente.Text       = cliente.FirstOrDefault().RFC;
            txtNombreCliente.Text    = cliente.FirstOrDefault().Nombre;
            txtDireccionCliente.Text = cliente.FirstOrDefault().Direccion;
            txtTelefonoCliente.Text  = cliente.FirstOrDefault().Telefono;
            txtCorreo.Text           = cliente.FirstOrDefault().Correo;

            ViewState["ClienteID"] = cliente.FirstOrDefault().ID;

            gvClientes.DataSource = cliente;
            gvClientes.DataBind();

            //txtRFCCliente.Enabled= false;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertaJS", "actualizarBTNs()", true);
        }

        protected void CancelarActuBTN_Click(object sender, ImageClickEventArgs e)
        {
            ClientScript.RegisterStartupScript(GetType(), "ReiniciarBTNs", "ReiniciarBTNs()", true);
            txtRFCCliente.Enabled = true;

            BorrarCampos();
            MostrarClientes();
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        protected void btnSubirCliente_Click(object sender, ImageClickEventArgs e)
        {
            clsCliente cliente = new clsCliente()
            {
                RFC = txtRFCCliente.Text,
                Nombre = txtNombreCliente.Text,
                Direccion = txtDireccionCliente.Text,
                Telefono = txtTelefonoCliente.Text,
                Correo = txtCorreo.Text,
            };

            Json = JsonConvertidor.Objeto_Json(cliente);
            peticion.PedirComunicacion("Clientes/AgregarCliente", MetodoHTTP.POST, TipoContenido.JSON, (String)Session["jwt"]);
            peticion.enviarDatos(Json);

            Estado = peticion.ObtenerJson();
            // VALIDAR SESIÓN - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
            if (Estado == null) { Url.CerrarSesion(this); return; }
            else
            {
                Session["jwt"] = Url.ActualizarJWT((String)Session["jwt"]);
            }
            // VALIDAR SESIÓN - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 

            PeticionEstado respuesta = JsonConvertidor.Json_Objeto<PeticionEstado>(Estado);

            if (!respuesta.Estado)
            {
                //ScriptManager.RegisterStartupScript(this, typeof(Page), "alertaJS", respuesta.AlertaJS, false);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alertaJS", respuesta.AlertaJS, true);
                return;
            }

            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertaJS", respuesta.AlertaJS, true);

            BorrarCampos();
            MostrarClientes();
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        protected void btnBuscarCliente_Click(object sender, ImageClickEventArgs e)
        {
            if (!ValidarCaracteres.NumerosYLetras(txtBusquedaCliente.Text)) { return; };

            if (txtBusquedaCliente.Text != "")
            {
                Busqueda = txtBusquedaCliente.Text;
                peticion.PedirComunicacion("Clientes/BuscarCliente/" + Busqueda.Trim(), MetodoHTTP.GET, TipoContenido.JSON, (String)Session["jwt"]);
                Estado = peticion.ObtenerJson();

                // VALIDAR SESIÓN - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
                if (Estado == null) { Url.CerrarSesion(this); return; }
                else
                {
                    Session["jwt"] = Url.ActualizarJWT((String)Session["jwt"]);
                }
                // VALIDAR SESIÓN - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

                List<ClienteDTO> cliente = JsonConvertidor.Json_ListaObjeto<ClienteDTO>(Estado);

                gvClientes.DataSource = cliente;
                gvClientes.DataBind();

                lblErrorBusqueda.Text = "";
            }
            else
            {
                lblErrorBusqueda.Text = "INGRESE UN RFC A BUSCAR";

                gvClientes.DataSource = null;
                gvClientes.DataBind();
            }
            BorrarCampos();
        }

        //Metodos y más eventos
        private bool MostrarClientes()
        {
            peticion.PedirComunicacion("Clientes/MostrarClientes/", MetodoHTTP.GET, TipoContenido.JSON, (String)Session["jwt"]);
            Estado = peticion.ObtenerJson();

            if(Estado == null) { return false; }

            List<ClienteDTO> cliente = JsonConvertidor.Json_ListaObjeto<ClienteDTO>(Estado);

            gvClientes.DataSource = cliente;
            gvClientes.DataBind();

            return true;
        }

        private void BorrarCampos()
        {
            txtRFCCliente.Text = String.Empty;
            txtNombreCliente.Text = String.Empty;
            txtDireccionCliente.Text = String.Empty;
            txtTelefonoCliente.Text = String.Empty;
            txtCorreo.Text = String.Empty;

            //txtRFCCliente.Enabled = true;
        }

        protected void btnActualizarCliente_Click(object sender, ImageClickEventArgs e)
        {
            clsCliente cliente = new clsCliente()
            {
                ID = (int)ViewState["ClienteID"],
                RFC = txtRFCCliente.Text,
                Nombre = txtNombreCliente.Text,
                Direccion = txtDireccionCliente.Text,
                Telefono = txtTelefonoCliente.Text,
                Correo = txtCorreo.Text,
            };

            Json = JsonConvertidor.Objeto_Json(cliente);
            peticion.PedirComunicacion("Clientes/ActualizarCliente", MetodoHTTP.PUT, TipoContenido.JSON, (String)Session["jwt"]);
            peticion.enviarDatos(Json);

            Estado = peticion.ObtenerJson();
            // VALIDAR SESIÓN - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
            if (Estado == null) { Url.CerrarSesion(this); return; }
            else
            {
                Session["jwt"] = Url.ActualizarJWT((String)Session["jwt"]);
            }
            // VALIDAR SESIÓN - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 

            PeticionEstado respuesta = JsonConvertidor.Json_Objeto<PeticionEstado>(Estado);

            if (!respuesta.Estado)
            {
                BorrarCampos();
                MostrarClientes();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alertaJS", respuesta.AlertaJS, true);
                return;
            }

            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertaJS", respuesta.AlertaJS, true);

            BorrarCampos();
            MostrarClientes();
        }
    }
}