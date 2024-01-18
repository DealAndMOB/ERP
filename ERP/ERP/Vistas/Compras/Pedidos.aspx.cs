using ERP.Complementos.PDF;
using ERP.Complementos.ProcesoBehind;
using ERP.Models;
using ERP.Vistas.Catalogos;
using ERP.Vistas.Ventas;
using HTTPupt;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using static ERP.Complementos.PDF.PedidoPDF;

namespace ERP.Vistas.Compras
{
    public partial class Pedidos : System.Web.UI.Page
    {
        PeticionHTTP peticion = new PeticionHTTP(Url.ObtenerUrlServidor());

        //Variables de la clase
        private String Json, Folio, RFC, Proveedor, Estado;
        private PedidoDTO PedidoActual;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["jwt"] == null)
            {
                Response.RedirectToRoute("BorrarJWT");
                return;
            }

            if (!IsPostBack)
            {
                if (!Plantilla()) { Url.CerrarSesion(this); return; }
                else
                {
                    Session["jwt"] = Url.ActualizarJWT((String)Session["jwt"]);
                }
            }

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
            if (ViewState["Pedido"] == null)
            {
                ViewState["Pedido"] = new PedidoDTO();
            }

            if (ViewState["Partidas"] == null)
            {
                ViewState["Partidas"] = new List<PartidaPedidoDTO>();
            }
            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        }

        private bool Plantilla()
        {
            string folio = Page.RouteData.Values["id"] as string;

            if (folio == null)
                return true;

            peticion.PedirComunicacion($"Pedidos/ObtenerPedidoCompleto/{folio}", MetodoHTTP.GET, TipoContenido.JSON, (String)Session["jwt"]);
            Estado = peticion.ObtenerJson();

            if (Estado == null) { return false; }

            PedidoDTO pedido = JsonConvert.DeserializeObject<PedidoDTO>(Estado);
            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
            pedido.Partidas.ForEach(partida => partida.CostoFormato = partida.Costo.ToString("C2", new CultureInfo("es-MX")));
            pedido.Partidas.ForEach(partida => partida.CostoBase = Convert.ToSingle(partida.CostoBase).ToString("C2", new CultureInfo("es-MX")));
            pedido.Partidas.ForEach(partida => partida.TotalFormato = partida.Total.ToString("C2", new CultureInfo("es-MX")));
            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
            CalcularTotales(pedido);
            txtCondicion.Text = pedido.Condiciones;
            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
            ViewState["Pedido"] = pedido;
            ViewState["Partidas"] = pedido.Partidas;
            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
            gvPedido.DataSource = pedido.Partidas;
            gvPedido.DataBind();
            return true;
        }

        protected void btnBuscarProveedor_Click(object sender, ImageClickEventArgs e)
        {
            string Busqueda = txtBusquedaProveedor.Text;
            if (!ValidarCaracteres.NumerosYLetras(Busqueda.Trim())) { return; };

            if (txtBusquedaProveedor.Text != "")
            {
                peticion.PedirComunicacion("Proveedores/BuscarProveedor/" + Busqueda.Trim(), MetodoHTTP.GET, TipoContenido.JSON, (String)Session["jwt"]);
                Estado = peticion.ObtenerJson();

                // VALIDAR SESIÓN - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
                if (Estado == null) { Url.CerrarSesion(this); return; }
                else
                {
                    Session["jwt"] = Url.ActualizarJWT((String)Session["jwt"]);
                }
                // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 

                List<ProveedorDTO> proveedor = JsonConvertidor.Json_ListaObjeto<ProveedorDTO>(Estado);

                gvBusquedaProveedor.DataSource = proveedor;
                gvBusquedaProveedor.DataBind();

                lblErrorBusqueda.Text = "";
            }
            else
            {
                lblErrorBusqueda.Text = "INGRESE UN RFC BUSCAR";

                gvBusquedaProveedor.DataSource = null;
                gvBusquedaProveedor.DataBind();
            }
        }



        protected void btnSeleccionarProveedor_Click(object sender, EventArgs e)
        {
            PedidoActual = (PedidoDTO)ViewState["Pedido"];

            RFC = GridViewButon(sender, 0);
            Proveedor = GridViewButon(sender, 1);

            lblProveedor.Text = Proveedor;
            lblProveedor.ForeColor = System.Drawing.Color.Black; // Puedes cambiar el color a tu preferencia

            gvBusquedaProveedor.DataSource = null;
            gvBusquedaProveedor.DataBind();

            //// - - - - - Creación del folio - - - - - //// 
            CrearCodigo crearFolio = new CrearCodigo();

            peticion.PedirComunicacion("Pedidos/ObtenerFolioFinal/", MetodoHTTP.GET, TipoContenido.JSON, (String)Session["jwt"]);
            Estado = peticion.ObtenerJson();
            // VALIDAR SESIÓN - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
            if(Estado == null) { Url.CerrarSesion(this);return; }
            else
            {
                Session["jwt"] = Url.ActualizarJWT((string)Session["jwt"]);
            }
            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
            string folioAnterior = Estado.Substring(1, (Estado.Length) - 2);
            string prefijo = RFC.Substring(0, 3) + "PED";

            if (folioAnterior != "null")
            {
                string folio = crearFolio.GenerarCodigo(prefijo, folioAnterior);
                Folio = folio;
            }
            else
            {
                string folio = crearFolio.GenerarCodigo(prefijo, 7);
                Folio = folio;
            }
            lblFolio.Text = Folio;

            peticion.PedirComunicacion($"Proveedores/BuscarProveedor/{RFC}", MetodoHTTP.GET, TipoContenido.JSON, (String)Session["jwt"]);
            Estado = peticion.ObtenerJson();

            List<ProveedorDTO> proveedor = JsonConvertidor.Json_ListaObjeto<ProveedorDTO>(Estado);

            // Recolección de datos de la variable de estado - - - - - - - - - - - - - - - - - - 
            this.PedidoActual.Folio = Folio;
            this.PedidoActual.RFC = RFC;
            this.PedidoActual.Proveedor = Proveedor;
            this.PedidoActual.ProveedorID = proveedor.FirstOrDefault().ID;
            this.PedidoActual.RazonSocial = proveedor.FirstOrDefault().RazonSocial;
            this.PedidoActual.Telefono = proveedor.FirstOrDefault().Telefono;
            this.PedidoActual.Direccion = proveedor.FirstOrDefault().Direccion;
            this.PedidoActual.Email = proveedor.FirstOrDefault().CorreoPagina;

            this.PedidoActual.Fecha = DateTime.Now;
            BorrarCampos(true);
        }

        protected void btnBusquedaProducto_Click(object sender, ImageClickEventArgs e)
        {
            string Busqueda = txtBusquedaProducto.Text;
            if (!ValidarCaracteres.NumerosYLetras(Busqueda)){ return; };

            if (txtBusquedaProducto.Text != "")
            {
                Busqueda = txtBusquedaProducto.Text;
                peticion.PedirComunicacion("Productos/Buscartxt/" + Busqueda, MetodoHTTP.GET, TipoContenido.JSON, (String)Session["jwt"]);
                Estado = peticion.ObtenerJson();
                // VALIDAR SESIÓN - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
                if (Estado == null) { Url.CerrarSesion(this); return; }
                else
                {
                    Session["jwt"] = Url.ActualizarJWT((String)Session["jwt"]);
                }
                // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 

                List<ProductoDTO> ProductosEncontrado = JsonConvertidor.Json_ListaObjeto<ProductoDTO>(Estado);

                gvProducto.DataSource = ProductosEncontrado;
                gvProducto.DataBind();

                lblErrorBusquedaProducto.Text = "";
            }
            else
            {
                lblErrorBusquedaProducto.Text = "INGRESE UN PRODUCTO A BUSCAR";

                gvBusquedaProveedor.DataSource = null;
                gvBusquedaProveedor.DataBind();
            }
        }

        private string validarFolio(string Folio)
        {
            peticion.PedirComunicacion($"Pedidos/ComprobarFolio/{Folio}", MetodoHTTP.GET, TipoContenido.JSON, (String)Session["jwt"]);
            string folioEncontrado = peticion.ObtenerJson();

            if (folioEncontrado == null) { return "null"; }

            PeticionEstado respuesta = JsonConvertidor.Json_Objeto<PeticionEstado>(folioEncontrado);
            if (!respuesta.Estado)
            {
                return respuesta.AlertaJS;
            }
            return "";
        }

        protected void btnCotizarProducto_Click(object sender, EventArgs e)
        {
            PedidoActual = (PedidoDTO)ViewState["Pedido"];
            PedidoActual.Partidas = (List<PartidaPedidoDTO>)ViewState["Partidas"];

            if (!string.IsNullOrEmpty(PedidoActual.Folio))
            {
                string AlertaJS = validarFolio(PedidoActual.Folio);
                switch (AlertaJS)
                {
                    case "null":
                        // VALIDAR SESIÓN - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
                        Url.CerrarSesion(this);
                        return;
                    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
                    case "":
                        Session["jwt"] = Url.ActualizarJWT((String)Session["jwt"]);
                        break;
                    default:
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "AlertaJS", AlertaJS, true);
                        Session["jwt"] = Url.ActualizarJWT((String)Session["jwt"]);
                        return;
                }
            }
            // Datos productos - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            string CodigoProducto = $"{GridViewButon(sender, 0)}";

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            peticion.PedirComunicacion($"Productos/ObtenerProductoID/{CodigoProducto}", MetodoHTTP.GET, TipoContenido.JSON, (String)Session["jwt"]);
            string productoID = peticion.ObtenerJson();
            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            string Imagen = $"{CodigoProducto}.jpg";
            float CostoUnitario = Convert.ToSingle(GridViewButon(sender, 1).Substring(1));
            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
            peticion.PedirComunicacion($"Productos/ObtenerDescripcion/{CodigoProducto}", MetodoHTTP.GET, TipoContenido.JSON, (String)Session["jwt"]);
            Estado = peticion.ObtenerJson();
            // VALIDAR SESIÓN - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
            if (Estado == null) { Url.CerrarSesion(this); return; }
            else
            {
                Session["jwt"] = Url.ActualizarJWT((String)Session["jwt"]);
            }
            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            string Descripcion = Estado.Substring(1, (Estado.Length - 2));

            try
            {
                if (int.Parse(txtCantidad.Text) <= 0)
                {
                    lblCantidad.Text = "INGRESE UNA CANTIDAD VALIDA";
                    return;
                }
            }
            catch
            {
                lblCantidad.Text = "INGRESE LA CANTIDAD DE PRODUCTOS POR COTIZAR";
                return;
            }
            lblCantidad.Text = "";

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            foreach (var partida in PedidoActual.Partidas)
            {
                if (CodigoProducto == partida.CodigoProducto)
                {
                    partida.Cantidad += int.Parse(txtCantidad.Text);
                    partida.TotalFormato = (partida.Cantidad * Convert.ToSingle(partida.CostoFormato.Substring(1))).ToString("C2", new CultureInfo("es-MX"));
                    CalcularTotales(PedidoActual);

                    gvPedido.DataSource = PedidoActual.Partidas;
                    gvPedido.DataBind();
                    return;
                }
            }
            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
            for (int i = 0; i < PedidoActual.Partidas.Count - 1; i++)
            {
                PedidoActual.Partidas[i].Partida = i + 1;
            }

            int contador = PedidoActual.Partidas.Count + 1;
            int Partida = contador;
            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            PartidaPedidoDTO NuevaPartida = new PartidaPedidoDTO()
            {
                // Campos de interfaz - - - - - - - - - - - - - -
                Partida = Partida,
                CodigoProducto = CodigoProducto,
                Imagen = Imagen,
                DescripProducto = Descripcion,
                CostoBase = CostoUnitario.ToString("C2", new CultureInfo("es-MX")),

                // Campos BD - - - - - - - - - - - - - - - - - - -
                ProductoID = int.Parse(productoID),
                Cantidad = int.Parse(txtCantidad.Text),
                CostoFormato = CostoUnitario.ToString("C2", new CultureInfo("es-MX")),
                TotalFormato = (int.Parse(txtCantidad.Text) * CostoUnitario).ToString("C2", new CultureInfo("es-MX"))
            };
            PedidoActual.Partidas.Add(NuevaPartida);

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
            CalcularTotales(PedidoActual);
            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 

            gvPedido.DataSource = PedidoActual.Partidas;
            gvPedido.DataBind();
        }

        protected void btnEliminarProducto_Click(object sender, EventArgs e)
        {
            PedidoActual = (PedidoDTO)ViewState["Pedido"];
            PedidoActual.Partidas = (List<PartidaPedidoDTO>)ViewState["Partidas"];

            int numPartida = int.Parse(GridViewButon(sender, 0));
            PedidoActual.Partidas.RemoveAt(numPartida - 1);
            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
            int contador = 0;
            foreach (var cotizacion in PedidoActual.Partidas)
            {
                contador += 1;
                int Partida = contador;

                cotizacion.Partida = Partida;
            }
            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            CalcularTotales(PedidoActual);

            gvPedido.DataSource = PedidoActual.Partidas;
            gvPedido.DataBind();
        }

        protected void btnNuevaPedido_Click(object sender, ImageClickEventArgs e)
        {
            ViewState.Clear();

            lblProveedor.Text = "Seleccionar Proveedor";
            lblFolio.Text = "";

            gvPedido.DataSource = null;
            gvPedido.DataBind();

            gvPedido.DataSource = null;
            gvPedido.DataBind();

            gvProducto.DataSource = null;
            gvProducto.DataBind();

            CalcularTotales();
            BorrarCampos(false);

            ScriptManager.RegisterStartupScript(this, this.GetType(), "VaciarCorizacion", "VaciarCorizacion()", true);
        }

        protected void btnConfirmarPedido_Click(object sender, EventArgs e)
        {
            if (lblProveedor.Text.Equals("Seleccionar Proveedor"))
            {
                lblProveedor.ForeColor = System.Drawing.Color.Red;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "SinCliente", "SinCliente()", true);
                return;
            }
            if (txtCondicion.Text.Equals(string.Empty))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "SinCondicionesCompra", "SinCondicionesCompra()", true);
                return;
            }
            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            PedidoActual = (PedidoDTO)ViewState["Pedido"];
            PedidoActual.Partidas = (List<PartidaPedidoDTO>)ViewState["Partidas"];
            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            // Se comprueba si el folio actual se ha encontrado en la bd, para saber si ya se ha confirmado el folio actual
            peticion.PedirComunicacion($"Pedidos/ComprobarFolio/{PedidoActual.Folio}", MetodoHTTP.GET, TipoContenido.JSON, (String)Session["jwt"]);
            string folioEncontrado = peticion.ObtenerJson();
            PeticionEstado respuesta;

            // VALIDAR SESIÓN - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
            if (folioEncontrado == null) { Url.CerrarSesion(this); return; }
            else
            {
                respuesta = JsonConvertidor.Json_Objeto<PeticionEstado>(folioEncontrado);
                Session["jwt"] = Url.ActualizarJWT((String)Session["jwt"]);
            }
            // SE VERIFICA EL ACCESO DE USUARIO Y EL FOLIO- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
            if (!respuesta.Estado)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alertaJS", respuesta.AlertaJS, true);
                return;
            }
            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            if (PedidoActual.Partidas.Count == 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "RegistroCotizacion", "RegistroCotizacion()", true);
                return;
            }

            clsPedido pedido = new clsPedido()
            {
                Folio = PedidoActual.Folio,
                ProveedorID = PedidoActual.ProveedorID,
                Condiciones = txtCondicion.Text,
                Total = PedidoActual.Total,
                Estado = false,
                Fecha = DateTime.Now
            };

            Json = JsonConvertidor.Objeto_Json(pedido);
            peticion.PedirComunicacion("Pedidos/AgregarPedido", MetodoHTTP.POST, TipoContenido.JSON, (String)Session["jwt"]);
            peticion.enviarDatos(Json);
            Json = peticion.ObtenerJson();

            List<clsPartidaPedido> partidasPedido = new List<clsPartidaPedido>();

            foreach (var partidas in PedidoActual.Partidas)
            {
                clsPartidaPedido partida = new clsPartidaPedido()
                {
                    Folio = PedidoActual.Folio,
                    ProductoID = partidas.ProductoID,
                    Unidades = partidas.Cantidad,
                    CostoUnitario = Convert.ToSingle(partidas.CostoFormato.Substring(1)),
                    TotalPartida = Convert.ToSingle(partidas.TotalFormato.Substring(1)),
                };
                partidasPedido.Add(partida);
            }

            Json = JsonConvertidor.Objeto_Json(partidasPedido);
            peticion.PedirComunicacion("PartidaPedidos/AgregarPartidas", MetodoHTTP.POST, TipoContenido.JSON, (String)Session["jwt"]);
            peticion.enviarDatos(Json);
            Json = peticion.ObtenerJson();

            ScriptManager.RegisterStartupScript(this, this.GetType(), "CotConfirmada", "Subido()", true);
        }

        protected void btnDescargarPDF_Click(object sender, ImageClickEventArgs e)
        {
            if (lblProveedor.Text.Equals("Seleccionar Proveedor"))
            {
                lblProveedor.ForeColor = System.Drawing.Color.Red;
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "MiScript", "SinProveedor()", true);
                return;
            }

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            PedidoActual = (PedidoDTO)ViewState["Pedido"];
            PedidoActual.Partidas = (List<PartidaPedidoDTO>)ViewState["Partidas"];
            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            if (PedidoActual.Partidas.Count() == 0)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "NoHayPartidas", "RegistroCotizacion()", true);
                return;
            }
            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            // Se comprueba si el folio actual se ha encontrado en la bd, para saber si ya se ha confirmado el folio actual
            peticion.PedirComunicacion($"Pedidos/ComprobarFolio/{PedidoActual.Folio}", MetodoHTTP.GET, TipoContenido.JSON, (String)Session["jwt"]);
            string folioEncontrado = peticion.ObtenerJson();
            PeticionEstado respuesta;

            // VALIDAR SESIÓN - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
            if (folioEncontrado == null) { Url.CerrarSesion(this); return; }
            else
            {
                Session["jwt"] = Url.ActualizarJWT((String)Session["jwt"]);
                respuesta = JsonConvertidor.Json_Objeto<PeticionEstado>(folioEncontrado);
            }

            // SE VERIFICA EL ACCESO DE USUARIO Y EL FOLIO- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
            if (!respuesta.Estado && respuesta.AlertaJS.StartsWith("P"))
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertaJS", respuesta.AlertaJS, true);
                return;
            }
            //SE BUSCA EL FOLIO EN LA BD, SI EL FOLIO NO SE ENCUENTRA NO SE IMPRIME
            if (respuesta.Estado)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "PDF", "PDF()", true);
                return;
            }
            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            PedidoActual = (PedidoDTO)ViewState["Pedido"];

            PedidoPDF pdf = new PedidoPDF(PedidoActual, Server.MapPath("/Media/Resources/FondoPedidoPDF.jpg"), Server.MapPath("~/Multimedia/"));

            PedidoActual.Condiciones = txtCondicion.Text;

            Estructura estructura = pdf.CrearPDFpedido();

            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", $"attachment;filename={PedidoActual.Folio}.pdf");
            HttpContext.Current.Response.Write(estructura.doc);
            Response.Flush();
            Response.End();
            estructura.writer.Close();
        }

        private void CalcularTotales(PedidoDTO pedido = null)
        {
            if (pedido == null)
            {
                lblSubtotal.Text = "0.00";
                lblIVA.Text = "0.00";
                lblTotal.Text = "0.00";
                return;
            }

            float SubTotal = 0, DiferenciaIVA = 0, Total = 0;

            foreach (var calcular in pedido.Partidas)
            {
                SubTotal += Convert.ToSingle(calcular.TotalFormato.Substring(1));
            }

            DiferenciaIVA = Convert.ToSingle((SubTotal * .16));
            Total = (SubTotal + DiferenciaIVA);
            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            lblSubtotal.Text = SubTotal.ToString("N2", new CultureInfo("es-MX"));
            lblIVA.Text = DiferenciaIVA.ToString("N2", new CultureInfo("es-MX"));
            lblTotal.Text = Total.ToString("N2", new CultureInfo("es-MX"));
            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            pedido.SubTotal = SubTotal;
            pedido.DiferenciaIVA = DiferenciaIVA;
            pedido.Total = Total;
        }


        private string GridViewButon(object sender, int numCelda)
        {
            Button btn = (Button)sender;
            GridViewRow Fila = (GridViewRow)btn.NamingContainer;
            string dato = Fila.Cells[numCelda].Text;

            return dato;
        }

        private void BorrarCampos(bool filtro)
        {
            if (filtro)
            {
                txtCantidad.Text = String.Empty;
                txtBusquedaProveedor.Text = String.Empty;
            }
            else
            {
                txtCantidad.Text = String.Empty;
                txtBusquedaProveedor.Text = String.Empty;

                txtCantidad.Text = String.Empty;
                txtCondicion.Text = String.Empty;
                txtBusquedaProducto.Text = String.Empty;
            }
        }

        protected void btnAlertaConfirmacion_Click(object sender, ImageClickEventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Peticion", "alertConfirmar() ", true);
        }
    }
}