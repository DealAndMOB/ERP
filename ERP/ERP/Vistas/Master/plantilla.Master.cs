using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ERP.Views.Master
{
    public partial class plantilla : System.Web.UI.MasterPage
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            string accesos = (string)Session["Accesos"];

            string funcion = string.Format($"validarAccesos('{accesos}');");
            ScriptManager.RegisterStartupScript(this, this.GetType(), "validar", funcion, true);
        }
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
    }
}