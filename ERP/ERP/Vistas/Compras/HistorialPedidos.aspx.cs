using ERP.Complementos.ProcesoBehind;
using ERP.Migrations;
using ERP.Models;
using HTTPupt;
using iTextSharp.text.pdf.codec.wmf;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ERP.Vistas.Compras
{
    public partial class HistorialPedidos : System.Web.UI.Page
    {
        PeticionHTTP peticion = new PeticionHTTP(Url.ObtenerUrlServidor());
        string Json = string.Empty, Estado;
        String Busqueda = String.Empty;

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
            peticion.PedirComunicacion($"Pedidos/MostrarPedido/{estado}", MetodoHTTP.GET, TipoContenido.JSON, (String)Session["jwt"]);
            Estado = peticion.ObtenerJson();
            // VALIDAR SESIÓN - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
            if (Estado == null) { Url.CerrarSesion(this); return; }
            else
            {
                Session["jwt"] = Url.ActualizarJWT((String)Session["jwt"]);
            }
            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 

            List<PedidoDTO> pedidos = JsonConvertidor.Json_ListaObjeto<PedidoDTO>(Estado);

            List<PedidoDTO> pedidosVista = new List<PedidoDTO>();
            foreach (var pedido in pedidos)
            {
                PedidoDTO Pedido = new PedidoDTO()
                {
                    Folio = pedido.Folio,
                    Proveedor = pedido.Proveedor,
                    TotalFormato = pedido.Total.ToString("C2", new CultureInfo("es-MX")),
                    FechaCorta = pedido.Fecha.ToShortDateString(),
                    Estatus = pedido.Estatus
                };
                pedidosVista.Add(Pedido);
            }
            gvPedidos.DataSource = pedidosVista;
            gvPedidos.DataBind();
        }


        protected void btnBuscarFolio_Click(object sender, ImageClickEventArgs e)
        {
            Busqueda = txtBusquedaFolio.Text;
            if (!ValidarCaracteres.NumerosYLetras(Busqueda)) { return; };

            peticion.PedirComunicacion($"Pedidos/BuscarPedido/{Busqueda.Trim()}", MetodoHTTP.GET, TipoContenido.JSON, (String)Session["jwt"]);
            Estado = peticion.ObtenerJson();
            // VALIDAR SESIÓN - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
            if (Estado == null) { Url.CerrarSesion(this); return; }
            else
            {
                Session["jwt"] = Url.ActualizarJWT((String)Session["jwt"]);
            }
            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 

            List<PedidoDTO> pedidos = JsonConvertidor.Json_ListaObjeto<PedidoDTO>(Estado);
            List<PedidoDTO> pedidosVista = new List<PedidoDTO>();

            foreach (var pedido in pedidos)
            {
                PedidoDTO compra = new PedidoDTO()
                {
                    Folio = pedido.Folio,
                    Proveedor = pedido.Proveedor,
                    TotalFormato = pedido.Total.ToString("C2", new CultureInfo("es-MX")),
                    FechaCorta = pedido.Fecha.ToShortDateString(),
                    Estatus= pedido.Estatus,
                };
                pedidosVista.Add(compra);
            }

            gvPedidos.DataSource = pedidosVista;
            gvPedidos.DataBind();
        }

        protected void btnEditPedido_Click(object sender, ImageClickEventArgs e)
        {
            string Folio = GridViewButon(sender, 0);
            string estado = GridViewButon(sender, 4);

            if (!Boolean.Parse(estado))
            {
                Response.RedirectToRoute("EditarPedido", new { id = Folio });
                return;
            }
            else
            {
                Response.RedirectToRoute("GestionarPedido", new { id = Folio });
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
    }
}