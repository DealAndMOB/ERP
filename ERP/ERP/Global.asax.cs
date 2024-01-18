using ERP.Complementos;
using ERP.Complementos.ProcesoBehind;
using ERP.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Routing;

namespace ERP
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            InitializeDatabase.IniciarBD();
            // Inicio
            RouteTable.Routes.MapPageRoute("Inicio", "inicio", "~/Vistas/inicio.aspx");
            // Login
			RouteTable.Routes.MapPageRoute("CerrarSesion", "login", "~/Index.aspx");
            // Logout
			RouteTable.Routes.MapPageRoute("BorrarJWT", "BorrarJWT", "~/Vistas/Logout.aspx");

            // Ruta modulos
			RouteTable.Routes.MapPageRoute("Clientes",         "clientes",                  "~/Vistas/Catalogos/Clientes.aspx");
			RouteTable.Routes.MapPageRoute("Productos",        "productos",                 "~/Vistas/Catalogos/Productos.aspx");
			RouteTable.Routes.MapPageRoute("productoAgregado", "productoAgregado/{id}",     "~/Vistas/Catalogos/Productos.aspx");
			RouteTable.Routes.MapPageRoute("Proveedores",      "proveedores",               "~/Vistas/Catalogos/Proveedores.aspx");
															  				  		     
			RouteTable.Routes.MapPageRoute("Cotizar",          "Cotizar",                   "~/Vistas/Ventas/Cotizar.aspx");
			RouteTable.Routes.MapPageRoute("CotizarPlantilla", "CotizarPlantilla/{id}",     "~/Vistas/Ventas/Cotizar.aspx");
            RouteTable.Routes.MapPageRoute("Reporte",          "Reporte/{id}",              "~/Vistas/Ventas/ReporteVenta.aspx");

            RouteTable.Routes.MapPageRoute("Cotizacion",       "Cotizacion/{id}",           "~/Vistas/Ventas/Cotizacion.aspx");
			RouteTable.Routes.MapPageRoute("Orden",            "orden",                     "~/Vistas/Ventas/OrdenDeVenta.aspx");

			RouteTable.Routes.MapPageRoute("Venta",              "Venta/{id}",              "~/Vistas/Ventas/Venta.aspx");
			RouteTable.Routes.MapPageRoute("Remitidos",          "Remitidos/{id}",          "~/Vistas/Ventas/Remisiones.aspx");
			RouteTable.Routes.MapPageRoute("CrearRemision",      "CrearRemision/{id}",      "~/Vistas/Ventas/CrearRemision.aspx");
			RouteTable.Routes.MapPageRoute("ConsultarRemision",  "ConsultarRemision/{id}",  "~/Vistas/Ventas/Remision.aspx");
																			  	         
			RouteTable.Routes.MapPageRoute("Usuarios",     "usuarios",                      "~/Vistas/AdminSistema/Usuarios.aspx");
			RouteTable.Routes.MapPageRoute("Dependencias", "dependencias",                  "~/Vistas/AdminSistema/Dependencias.aspx");


            RouteTable.Routes.MapPageRoute("Historial de Pedidos", "Historial",				"~/Vistas/Compras/HistorialPedidos.aspx");
            RouteTable.Routes.MapPageRoute("Crear Pedido",		   "Pedido",				"~/Vistas/Compras/Pedidos.aspx");
            RouteTable.Routes.MapPageRoute("EditarPedido",		   "EditarPedido/{id}",		"~/Vistas/Compras/EditarPedido.aspx");
            RouteTable.Routes.MapPageRoute("GestionarPedido",	   "GestionarPedido/{id}",  "~/Vistas/Compras/GestionarPedido.aspx");
            RouteTable.Routes.MapPageRoute("PedidoPlantilla",	   "PedidoPlantilla/{id}",  "~/Vistas/Compras/Pedidos.aspx");

            GlobalConfiguration.Configure(WebApiConfig.Register);
        } 
    }
}
