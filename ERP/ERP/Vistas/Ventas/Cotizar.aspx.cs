using ERP.Complementos.PDF;
using ERP.Complementos.ProcesoBehind;
using ERP.Models;
using ERP.Vistas.Catalogos;
using HTTPupt;
using Humanizer;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using static ERP.Complementos.PDF.CreatePDF;
using iTextSharp.text.pdf.codec.wmf;
using System.Web.Services;
using Newtonsoft.Json;

namespace ERP.Vistas.Ventas
{
    public partial class Cotizar : System.Web.UI.Page
    {
        PeticionHTTP peticion = new PeticionHTTP(Url.ObtenerUrlServidor());

        //Variables de la clase
        private String Json, Folio, RFC, Cliente, Direccion, Estado;

        private CotizacionDTO CotizacionActual;
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
            if (ViewState["Cotizacion"] == null)
            {
                ViewState["Cotizacion"] = new CotizacionDTO();
            }

            if (ViewState["Partidas"] == null)
            {
                ViewState["Partidas"] = new List<PartidaCotizacionDTO>();
            }
            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        }

        private bool Plantilla()
        {
            string folio = Page.RouteData.Values["id"] as string;

            if (folio == null)
                return true;

            peticion.PedirComunicacion($"Cotizaciones/ObtenerCotizacionCompleta/{folio}", MetodoHTTP.GET, TipoContenido.JSON, (String)Session["jwt"]);
            Estado = peticion.ObtenerJson();

            if (Estado == null) { return false; }

            CotizacionDTO cotizacion = JsonConvert.DeserializeObject<CotizacionDTO>(Estado);
            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
            cotizacion.Partidas.ForEach(partida => partida.PrecioFormato     = partida.Precio.ToString("C2", new CultureInfo("es-MX")));
            cotizacion.Partidas.ForEach(partida => partida.PrecioBaseFormato = Convert.ToSingle(partida.PrecioBase).ToString("C2", new CultureInfo("es-MX")));
            cotizacion.Partidas.ForEach(partida => partida.TotalFormato      = partida.Total.ToString("C2", new CultureInfo("es-MX")));
            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
            CalcularTotales(cotizacion);
            txtCondicion.Text = cotizacion.Condiciones;
            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
            ViewState["Cotizacion"] = cotizacion;
            ViewState["Partidas"] = cotizacion.Partidas;
            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
            gvCotizado.DataSource = cotizacion.Partidas;
            gvCotizado.DataBind();
            return true;
        }

        // Procesos Cliente - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // Función que trabaja con el buscador para mostrar en GridView
        protected void btnBuscarCliente_Click(object sender, ImageClickEventArgs e)
        {
            string Busqueda = txtBusquedaCliente.Text;
            if (!ValidarCaracteres.NumerosYLetras(Busqueda)) { return; };

            if (txtBusquedaCliente.Text != "")
            {
                peticion.PedirComunicacion("Clientes/BuscarCliente/" + Busqueda.Trim(), MetodoHTTP.GET, TipoContenido.JSON, (String)Session["jwt"]);
                Estado = peticion.ObtenerJson();

                // VALIDAR SESIÓN - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
                if (Estado == null) { Url.CerrarSesion(this); return; }
                else
                {
                    Session["jwt"] = Url.ActualizarJWT((String)Session["jwt"]);
                }
                // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 

                List<ClienteDTO> cliente = JsonConvertidor.Json_ListaObjeto<ClienteDTO>(Estado);

                gvBusquedaCliente.DataSource = cliente;
                gvBusquedaCliente.DataBind();

                lblErrorBusqueda.Text = "";
            }
            else
            {
                lblErrorBusqueda.Text = "INGRESE UN RFC BUSCAR";

                gvBusquedaCliente.DataSource = null;
                gvBusquedaCliente.DataBind();
            }
        }
        // Función que recolecta los datos necesarios del gvCliente para ser mostrados en la vista
        protected void btnSeleccionarCliente_Click(object sender, EventArgs e)
        {
            CotizacionActual = (CotizacionDTO)ViewState["Cotizacion"];

            RFC = GridViewButon(sender, 0);
            Cliente = GridViewButon(sender, 1);
            Direccion = GridViewButon(sender, 2);

            lblCliente.Text = Cliente;
            lblCliente.ForeColor = System.Drawing.Color.Black; // Puedes cambiar el color a tu preferencia

            gvBusquedaCliente.DataSource = null;
            gvBusquedaCliente.DataBind();

            //// - - - - - Creación del folio - - - - - //// 
            CrearCodigo crearFolio = new CrearCodigo();

            peticion.PedirComunicacion("Cotizaciones/ObtenerFolioFinal/", MetodoHTTP.GET, TipoContenido.JSON, (String)Session["jwt"]);
            Estado = peticion.ObtenerJson();
            // VALIDAR SESIÓN - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
            if (Estado == null) { Url.CerrarSesion(this); return; }
            else
            {
                Session["jwt"] = Url.ActualizarJWT((String)Session["jwt"]);
            }
            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
            string folioAnterior = Estado.Substring(1, (Estado.Length) - 2);
            string prefijo = RFC.Substring(0, 3) + "COT";

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

            peticion.PedirComunicacion($"Clientes/ObtenerClienteID/{RFC}", MetodoHTTP.GET, TipoContenido.JSON, (String)Session["jwt"]);
            string ClienteID = peticion.ObtenerJson();

            // Recolección de datos de la variable de estado 
            this.CotizacionActual.ClienteID = int.Parse(ClienteID);

            this.CotizacionActual.Folio = Folio;
            this.CotizacionActual.RFC = RFC;
            this.CotizacionActual.Cliente = Cliente;
            this.CotizacionActual.Direccion = Direccion;

            this.CotizacionActual.FechaCotizacion = DateTime.Now;
            BorrarCampos(true);
        }

        // Procesos Producto - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // Función que trabaja con el buscador para mostrar en GridView
        protected void btnBuscarProducto_Click(object sender, ImageClickEventArgs e)
        {
            string Busqueda = txtBusquedaProducto.Text;
            if (!ValidarCaracteres.NumerosYLetras(Busqueda)) { return; };

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

                gvProductos.DataSource = ProductosEncontrado;
                gvProductos.DataBind();

                lblErrorBusquedaProducto.Text = "";
            }
            else
            {
                lblErrorBusquedaProducto.Text = "INGRESE UN PRODUCTO A BUSCAR";

                gvBusquedaCliente.DataSource = null;
                gvBusquedaCliente.DataBind();
            }
        }

        private string validarFolio(string Folio)
        {
            peticion.PedirComunicacion($"Cotizaciones/ComprobarFolio/{Folio}", MetodoHTTP.GET, TipoContenido.JSON, (String)Session["jwt"]);
            string folioEncontrado = peticion.ObtenerJson();

            if (folioEncontrado == null) { return "null"; }

            PeticionEstado respuesta = JsonConvertidor.Json_Objeto<PeticionEstado>(folioEncontrado);
            if (!respuesta.Estado)
            {
                return respuesta.AlertaJS;
            }
            return "";
        }
        // Procesos Cotización - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        protected void btnCotizar_Click(object sender, EventArgs e)
        {
            CotizacionActual = (CotizacionDTO)ViewState["Cotizacion"];
            CotizacionActual.Partidas = (List<PartidaCotizacionDTO>)ViewState["Partidas"];

            if (!string.IsNullOrEmpty(CotizacionActual.Folio))
            {
                string AlertaJS = validarFolio(CotizacionActual.Folio);
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

            peticion.PedirComunicacion($"Productos/ObtenerProducto/{CodigoProducto}", MetodoHTTP.GET, TipoContenido.JSON, (String)Session["jwt"]);
            string productoJson = peticion.ObtenerJson();

            // VALIDAR SESIÓN - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
            if (productoJson == null) { Url.CerrarSesion(this); return; }
            else
            {
                Session["jwt"] = Url.ActualizarJWT((String)Session["jwt"]);
            }
            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            List<ProductoDTO> ProductoCotizado = JsonConvertidor.Json_ListaObjeto<ProductoDTO>(productoJson);
            //float PrecioUnitario = Convert.ToSingle(GridViewButon(sender, 1).Substring(1));
            int productoID       = ProductoCotizado.FirstOrDefault().ID;
            float PrecioUnitario = ProductoCotizado.FirstOrDefault().Precio;
            float CostoCapturado = ProductoCotizado.FirstOrDefault().Costo;

            //string Imagen = $"{ProductoCotizado.Codigo}.jpg";
            string Imagen        = ProductoCotizado.FirstOrDefault().Imagen;
            string Descripcion   = ProductoCotizado.FirstOrDefault().Descripcion;

            //peticion.PedirComunicacion($"Productos/ObtenerDescripcion/{CodigoProducto}", MetodoHTTP.GET, TipoContenido.JSON, (String)Session["jwt"]);
            //Json = peticion.ObtenerJson();
            //string Descripcion = Json.Substring(1, (Json.Length - 2));
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
            foreach (var partida in CotizacionActual.Partidas)
            {
                if (CodigoProducto == partida.CodigoProducto)
                {
                    partida.Cantidad += int.Parse(txtCantidad.Text);
                    partida.TotalFormato = (partida.Cantidad * Convert.ToSingle(partida.PrecioFormato.Substring(1))).ToString("C2", new CultureInfo("es-MX"));
                    CalcularTotales(CotizacionActual);

                    gvCotizado.DataSource = CotizacionActual.Partidas;
                    gvCotizado.DataBind();
                    return;
                }
            }
            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
            for (int i = 0; i < CotizacionActual.Partidas.Count - 1; i++)
            {
                CotizacionActual.Partidas[i].Partida = i + 1;
            }

            int contador = CotizacionActual.Partidas.Count + 1;
            int Partida = contador;
            // - - - - - - - - - - - - - - - - - - - - - - - - - -
            PartidaCotizacionDTO NuevaPartida = new PartidaCotizacionDTO()
            {
                // Campos de interfaz - - - - - - - - - - - - - -
                Partida = Partida,
                CodigoProducto = CodigoProducto,
                Imagen = Imagen,
                DescripProducto = Descripcion,
                PrecioBaseFormato = PrecioUnitario.ToString("C2", new CultureInfo("es-MX")),

                // Campos BD - - - - - - - - - - - - - - - - - - -
                // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
                Costo = CostoCapturado, // Costo en el momento de la cotización (ES CAMBIANTE)
                // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
                ProductoID = productoID,
                Cantidad = int.Parse(txtCantidad.Text),
                PrecioFormato = PrecioUnitario.ToString("C2", new CultureInfo("es-MX")),
                TotalFormato = (int.Parse(txtCantidad.Text) * PrecioUnitario).ToString("C2", new CultureInfo("es-MX"))
            };
            CotizacionActual.Partidas.Add(NuevaPartida);

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
            CalcularTotales(CotizacionActual);
            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 

            gvCotizado.DataSource = CotizacionActual.Partidas;
            gvCotizado.DataBind();
        }

        protected void btnAumentar_Click(object sender, EventArgs e)
        {
            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            CotizacionActual = (CotizacionDTO)ViewState["Cotizacion"];
            CotizacionActual.Partidas = (List<PartidaCotizacionDTO>)ViewState["Partidas"];
            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            int numPartida = int.Parse(GridViewButon(sender, 0));
            float diferencia, precioBase, precioUnitario, porcentaje;

            ViewState["numPartida"] = numPartida;

            if (CotizacionActual.Partidas[numPartida - 1].CriterioAumento != null)
            {
                //precioBase = Convert.ToSingle(CotizacionActual.Partidas[numPartida - 1].PrecioBaseFormato.Substring(1));
                //precioUnitario = Convert.ToSingle(CotizacionActual.Partidas[numPartida - 1].PrecioFormato.Substring(1));

                //diferencia = (precioUnitario - precioBase);
                //porcentaje = (100 * diferencia) / precioBase;

                txtNombreAumento.Text = CotizacionActual.Partidas[numPartida - 1].CriterioAumento;
                txtPorcentaje.Text = CotizacionActual.Partidas[numPartida - 1].PorcentajeAumento.ToString("N2");
            }
            else
            {
                txtNombreAumento.Text = "";
                txtPorcentaje.Text = "";
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), "MiScript", "Viewformulario()", true);
        }

        protected void btnRegistrarAumento_Click(object sender, ImageClickEventArgs e)
        {
            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            CotizacionActual = (CotizacionDTO)ViewState["Cotizacion"];
            CotizacionActual.Partidas = (List<PartidaCotizacionDTO>)ViewState["Partidas"];
            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            int numPartida = (int)ViewState["numPartida"], Cantidad = 0;
            float aumento, precioUnitario, precioBase;

            //El aumento de precio se genera siempre al precio base del producto
            precioBase = Convert.ToSingle(CotizacionActual.Partidas[numPartida - 1].PrecioBaseFormato.Substring(1));
            Cantidad = CotizacionActual.Partidas[numPartida - 1].Cantidad;

            // Criterios del Aumento
            CotizacionActual.Partidas[numPartida - 1].CriterioAumento = txtNombreAumento.Text;
            CotizacionActual.Partidas[numPartida - 1].PorcentajeAumento = Convert.ToSingle(txtPorcentaje.Text);

            aumento = (precioBase * Convert.ToSingle(txtPorcentaje.Text)) / 100;
            precioUnitario = precioBase + aumento;

            CotizacionActual.Partidas[numPartida - 1].PrecioFormato = precioUnitario.ToString("C2", new CultureInfo("es-MX"));
            CotizacionActual.Partidas[numPartida - 1].TotalFormato = (precioUnitario * Cantidad).ToString("C2", new CultureInfo("es-MX"));

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            CalcularTotales(CotizacionActual);
            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            gvCotizado.DataSource = CotizacionActual.Partidas;
            gvCotizado.DataBind();
        }

        protected void btnEliminar_Click(object sender, EventArgs e)
        {
            CotizacionActual = (CotizacionDTO)ViewState["Cotizacion"];
            CotizacionActual.Partidas = (List<PartidaCotizacionDTO>)ViewState["Partidas"];

            int numPartida = int.Parse(GridViewButon(sender, 0));
            CotizacionActual.Partidas.RemoveAt(numPartida - 1);
            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
            int contador = 0;
            foreach (var cotizacion in CotizacionActual.Partidas)
            {
                contador += 1;
                int Partida = contador;

                cotizacion.Partida = Partida;
            }
            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            CalcularTotales(CotizacionActual);

            gvCotizado.DataSource = CotizacionActual.Partidas;
            gvCotizado.DataBind();
        }

        protected void btnNuevaCotizacion_Click(object sender, ImageClickEventArgs e)
        {
            ViewState.Clear();

            lblCliente.Text = "Seleccionar Cliente";
            lblFolio.Text = "";

            gvCotizado.DataSource = null;
            gvCotizado.DataBind();

            gvProductos.DataSource = null;
            gvProductos.DataBind();

            CalcularTotales();
            BorrarCampos(false);

            ScriptManager.RegisterStartupScript(this, this.GetType(), "VaciarCorizacion", "VaciarCorizacion()", true);
        }

        protected void btnConfirmar_Click(object sender, EventArgs e)
        {
            if (lblCliente.Text.Equals("Seleccionar Cliente"))
            {
                lblCliente.ForeColor = System.Drawing.Color.Red;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "SinCliente", "SinCliente()", true);
                return;
            }

            if (txtCondicion.Text.Equals(string.Empty))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "SinCondicionesVenta", "SinCondicionesVenta()", true);
                return;
            }

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            CotizacionActual = (CotizacionDTO)ViewState["Cotizacion"];
            CotizacionActual.Partidas = (List<PartidaCotizacionDTO>)ViewState["Partidas"];
            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            // Se comprueba si el folio actual se ha encontrado en la bd, para saber si ya se ha confirmado el folio actual
            peticion.PedirComunicacion($"Cotizaciones/ComprobarFolio/{CotizacionActual.Folio}", MetodoHTTP.GET, TipoContenido.JSON, (String)Session["jwt"]);
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
            if (CotizacionActual.Partidas.Count == 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "RegistroCotizacion", "RegistroCotizacion()", true);
                return;
            }

            clsCotizacionVenta cotizacion = new clsCotizacionVenta()
            {
                Folio = CotizacionActual.Folio,
                ClienteID = CotizacionActual.ClienteID,
                Condiciones = txtCondicion.Text,
                Total = CotizacionActual.Total,
                Estado = false,
                FechaCotizacion = DateTime.Now,
            };

            Json = JsonConvertidor.Objeto_Json(cotizacion);
            peticion.PedirComunicacion("Cotizaciones/AgregarCotizacion", MetodoHTTP.POST, TipoContenido.JSON, (String)Session["jwt"]);
            peticion.enviarDatos(Json);

            Estado = peticion.ObtenerJson();
            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            List<clsPartidaCotizacionVenta> partidasCotizacion = new List<clsPartidaCotizacionVenta>();

            foreach (var partidas in CotizacionActual.Partidas)
            {
                clsPartidaCotizacionVenta partida = new clsPartidaCotizacionVenta()
                {
                    Folio = CotizacionActual.Folio,
                    ProductoID = partidas.ProductoID,
                    Unidades = partidas.Cantidad,
                    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
                    CostoCapturado = partidas.Costo,
                    PorcentajeAumento = partidas.PorcentajeAumento,
                    PrecioUnitario = Convert.ToSingle(partidas.PrecioFormato.Substring(1)),
                    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
                    TotalPartida = Convert.ToSingle(partidas.TotalFormato.Substring(1)),
                    CriterioAumento = partidas.CriterioAumento,
                    Estado = false,
                };
                partidasCotizacion.Add(partida);
            }

            Json = JsonConvertidor.Objeto_Json(partidasCotizacion);
            peticion.PedirComunicacion("PartidaCotizaciones/AgregarPartidas", MetodoHTTP.POST, TipoContenido.JSON, (String)Session["jwt"]);
            peticion.enviarDatos(Json);

            Estado = peticion.ObtenerJson();

            ScriptManager.RegisterStartupScript(this, this.GetType(), "CotConfirmada", "Subido()", true);
        }

        protected void btnDescargarPDF_Click(object sender, ImageClickEventArgs e)
        {
            if (lblCliente.Text.Equals("Seleccionar Cliente"))
            {
                lblCliente.ForeColor = System.Drawing.Color.Red;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "MiScript", "SinCliente()", true);
                return;
            }

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            CotizacionActual = (CotizacionDTO)ViewState["Cotizacion"];
            CotizacionActual.Partidas = (List<PartidaCotizacionDTO>)ViewState["Partidas"];
            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            if (CotizacionActual.Partidas.Count() == 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "NoHayPartidas", "RegistroCotizacion()", true);
                return;
            }
            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            // Se comprueba si el folio actual se ha encontrado en la bd, para saber si ya se ha confirmado el folio actual
            peticion.PedirComunicacion($"Cotizaciones/ComprobarFolio/{CotizacionActual.Folio}", MetodoHTTP.GET, TipoContenido.JSON, (String)Session["jwt"]);
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
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alertaJS", respuesta.AlertaJS, true);
                return;
            }
            //SE BUSCA EL FOLIO EN LA BD, SI EL FOLIO NO SE ENCUENTRA NO SE IMPRIME
            if (respuesta.Estado)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "PDF", "PDF()", true);
                return;
            }
            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            CotizacionActual = (CotizacionDTO)ViewState["Cotizacion"];
            CreatePDF pdf = new CreatePDF(CotizacionActual, Server.MapPath("/Media/Resources/fondo-1.png"), Server.MapPath("~/Multimedia/"), "COTIZACIÓN");

            CotizacionActual.Condiciones = txtCondicion.Text;

            Estructura estructura = pdf.Create();

            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", $"attachment;filename={CotizacionActual.Folio}.pdf");
            HttpContext.Current.Response.Write(estructura.doc);
            Response.Flush();
            Response.End();

            estructura.writer.Close();
        }

        // Funciones de los eventos - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // Función que adquiere el dato del objeto sender que proviene del botón que esta en un GridView  
        private string GridViewButon(object sender, int numCelda)
        {
            Button btn = (Button)sender;
            GridViewRow Fila = (GridViewRow)btn.NamingContainer;
            string dato = Fila.Cells[numCelda].Text;

            return dato;
        }

        private void CalcularTotales(CotizacionDTO cotizacion = null)
        {
            if (cotizacion == null)
            {
                lblSubtotal.Text = "0.00";
                lblIVA.Text = "0.00";
                lblTotal.Text = "0.00";
                return;
            }

            float SubTotal = 0, DiferenciaIVA = 0, Total = 0;

            foreach (var calcular in cotizacion.Partidas)
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
            cotizacion.SubTotal = SubTotal;
            cotizacion.DiferenciaIVA = DiferenciaIVA;
            cotizacion.Total = Total;
        }

        private void BorrarCampos(bool filtro)
        {
            if (filtro)
            {
                txtCantidad.Text = String.Empty;
                txtBusquedaCliente.Text = String.Empty;
            }
            else
            {
                txtCantidad.Text = String.Empty;
                txtBusquedaCliente.Text = String.Empty;

                txtCantidad.Text = String.Empty;
                txtCondicion.Text = String.Empty;
                txtPorcentaje.Text = String.Empty;
                txtNombreAumento.Text = String.Empty;
                txtBusquedaProducto.Text = String.Empty;
            }
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        protected void btnAlertaConfirmacion_Click(object sender, ImageClickEventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Peticion", "alertConfirmar() ", true);
        }

    }
}