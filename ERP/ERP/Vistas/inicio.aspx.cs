using ERP.Complementos.ProcesoBehind;
using HTTPupt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ERP.Views
{
    public partial class inicio : System.Web.UI.Page
    {
        PeticionHTTP peticion = new PeticionHTTP(Url.ObtenerUrlServidor());
        String Json;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["jwt"] == null)
            {
                Response.RedirectToRoute("BorrarJWT");
                return;
            }

            if ((bool)Session["Inicio"])
            {
                string FuncionAlerta = string.Format($"InicioSesion('{(string)Session["Usuario"]}');");

                ScriptManager.RegisterStartupScript(this, this.GetType(), "InicioSesion", FuncionAlerta, true);
                Session["Inicio"] = false;
            }

            string accesos = (string)Session["Accesos"];

            string funcion = string.Format($"validarAccesos('{accesos}');");
            ScriptManager.RegisterStartupScript(this, this.GetType(), "validar", funcion, true);
        }
        protected void btnInicio_Click(object sender, ImageClickEventArgs e)
        {
            peticion.PedirComunicacion("Usuarios/Expirar", MetodoHTTP.GET, TipoContenido.JSON, Session["jwt"].ToString());
            Json = peticion.ObtenerJson();

            if (Json == null) { Url.CerrarSesion(this); return; }
        }

        protected void btnLogout_Click(object sender, ImageClickEventArgs e)
        {
            Response.RedirectToRoute("BorrarJWT");
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
       

        protected void btnCotizar_Click(object sender, EventArgs e)
        {
            Response.RedirectToRoute("Cotizar");
        }

        protected void btnOrden_Click(object sender, EventArgs e)
        {
            Response.RedirectToRoute("Orden");
        }

        protected void btnProductos_Click(object sender, EventArgs e)
        {
            Response.RedirectToRoute("Productos");
        }

        protected void btnClientes_Click(object sender, EventArgs e)
        {
            Response.RedirectToRoute("Clientes");
        }

        protected void btnProveedor_Click(object sender, EventArgs e)
        {
            Response.RedirectToRoute("Proveedores");
        }

        protected void btnHistorial_Click(object sender, EventArgs e)
        {
            Response.RedirectToRoute("Historial de Pedidos");
        }

        protected void btnPedido_Click(object sender, EventArgs e)
        {
            Response.RedirectToRoute("Crear Pedido");
        }

        protected void btnUsuarios_Click(object sender, EventArgs e)
        {
            Response.RedirectToRoute("Usuarios");
        }

        protected void btnDependencias_Click(object sender, EventArgs e)
        {
            Response.RedirectToRoute("Dependencias");
        }

        protected void btnLogout_Click1(object sender, ImageClickEventArgs e)
        {
            Response.RedirectToRoute("BorrarJWT");
        }
    }
}