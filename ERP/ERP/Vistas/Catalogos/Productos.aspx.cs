using ERP.Complementos.ProcesoBehind;
using ERP.Models;
using HTTPupt;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ERP.Vistas.Catalogos
{
    public partial class Productos : System.Web.UI.Page
    {
        PeticionHTTP peticion = new PeticionHTTP(Url.ObtenerUrlServidor());

        //Variables de la clase
        private String Json, CodigoProducto, Estado;
        int ProductoID;
        private float PrecioProducto, CostoProducto;

        private ImagenOp imagen;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["jwt"] == null)
            {
                Response.RedirectToRoute("BorrarJWT");
                return;
            }

            //Por primera vez
            if (!IsPostBack)
            {
                if (!MostrarCategorias() || !MostrarProductos()) { Url.CerrarSesion(this); return; }
                else
                {
                    Session["jwt"] = Url.ActualizarJWT((String)Session["jwt"]);
                }
            }
        }

        protected void btnSubirProducto_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                // Proceso en la inserción a la base de datos - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
                PrecioProducto = Convert.ToSingle(txtPrecioProducto.Text, CultureInfo.CreateSpecificCulture("es-MX"));  //Conversión a flotante
                lblErrorPrecio.Text = "";

                CostoProducto = Convert.ToSingle(txtCostoProducto.Text, CultureInfo.CreateSpecificCulture("es-MX"));    //Conversión a flotante
                lblErrorCosto.Text = "";
            }
            catch
            {
                lblErrorPrecio.Text = "ESTE CAMPO SOLO ACEPTA CANTIDADES NUMERICAS";
                lblErrorCosto.Text = "ESTE CAMPO SOLO ACEPTA CANTIDADES NUMERICAS";
                return;
            }

            if (!flUpImagen.HasFile)
            {
                ClientScript.RegisterStartupScript(GetType(), "script", "SinImagen()", true);
                return;
            }

            // Creación del Codigo - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            CrearCodigo crearCodigo = new CrearCodigo();
            Dictionary<string, string> datosCodigo = crearCodigo.ProcesarDatosProductos(ddlCategoriaProducto, (String)Session["jwt"]);

            // VALIDAR SESIÓN - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
            if (datosCodigo == null) { Url.CerrarSesion(this); return; }
            else
            {
                Session["jwt"] = Url.ActualizarJWT((String)Session["jwt"]);
            }
            // VALIDAR SESIÓN - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 

            if (datosCodigo["Codigo anterior"] != "null")
            {
                string codigo = crearCodigo.GenerarCodigo(datosCodigo["Prefijo"], datosCodigo["Codigo anterior"]);
                CodigoProducto = codigo;
            }
            else
            {
                string codigo = crearCodigo.GenerarCodigo(datosCodigo["Prefijo"], 7);
                CodigoProducto = codigo;
            }

            // Se define la ruta de la carpeta Multimedia - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            imagen = new ImagenOp(flUpImagen, Server.MapPath("~/Multimedia/"));
            string mensaje = imagen.ValidarImagen(3000000);

            if (mensaje != "")
            {
                string funcion = string.Format($"ImagenIncompatible('{mensaje}');");
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alertaJS", funcion, true);
                return;
            }

            clsProducto producto = new clsProducto()
            {
                Codigo = CodigoProducto,
                Nombre = txtNombreProducto.Text.Trim(),
                CategoriaProductoID = int.Parse(ddlCategoriaProducto.SelectedValue),
                Precio = PrecioProducto,
                Costo = CostoProducto,
                Descripcion = txtDescripcionProducto.Text.Trim(),
                Imagen = $"{CodigoProducto}.jpg"
            };

            Json = JsonConvertidor.Objeto_Json(producto);
            peticion.PedirComunicacion("Productos/AgregarProducto", MetodoHTTP.POST, TipoContenido.JSON, (String)Session["jwt"]);
            peticion.enviarDatos(Json);

            Estado = peticion.ObtenerJson(); // Respuesta de la petición al servidor
            // VALIDAR SESIÓN - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
            if (Estado == null) { Url.CerrarSesion(this); return; }
            else
            {
                Session["jwt"] = Url.ActualizarJWT((String)Session["jwt"]);
            }
            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
            PeticionEstado respuesta = JsonConvertidor.Json_Objeto<PeticionEstado>(Estado);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertaJS", respuesta.AlertaJS, true);

            // Se guarda la imagen con el metodo que convierte a PNG 
            if (respuesta.Estado)// Estado de la petición
            {
                imagen.SavePNG(CodigoProducto);

                BorrarCampos();
                MostrarProductos(CodigoProducto); // No se valida la sesión dado que la ejecución del algoritmo la valida antes
            }
        }

        protected void btnBuscarProducto_Click(object sender, ImageClickEventArgs e)
        {
            if (!ValidarCaracteres.Letras(txtBusquedaProducto.Text)) { return; };

            if (ddlFiltroOrden.SelectedValue == "Todos" && txtBusquedaProducto.Text == "")
            {
                if (!MostrarProductos()) { Url.CerrarSesion(this); return; }
                else
                {
                    Session["jwt"] = Url.ActualizarJWT((String)Session["jwt"]);
                }
                BorrarCampos();
                return;
            }

            if (ddlFiltroOrden.SelectedValue == "Todos" && txtBusquedaProducto.Text != "")
            {
                if (!MostrarProductos(null, txtBusquedaProducto.Text)) { Url.CerrarSesion(this); return; }
                else
                {
                    Session["jwt"] = Url.ActualizarJWT((String)Session["jwt"]);
                }
                BorrarCampos();
                return;
            }

            if (ddlFiltroOrden.SelectedValue != "Todos" && txtBusquedaProducto.Text == "")
            {
                if (!MostrarProductos(int.Parse(ddlFiltroOrden.SelectedValue))) { Url.CerrarSesion(this); return; }
                else
                {
                    Session["jwt"] = Url.ActualizarJWT((String)Session["jwt"]);
                }
                BorrarCampos();
                return;
            }
            // Construir la URL del controlador
            StringBuilder UrlController = new StringBuilder("Productos/BusquedaCombinada/");
            UrlController.Append($"?CategoriaID={ddlFiltroOrden.SelectedValue}&Busqueda={txtBusquedaProducto.Text}");

            peticion.PedirComunicacion(UrlController.ToString(), MetodoHTTP.GET, TipoContenido.JSON, (String)Session["jwt"]);
            Estado = peticion.ObtenerJson();

            // VALIDAR SESIÓN - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
            if (Estado == null) { Url.CerrarSesion(this); return; }
            else
            {
                Session["jwt"] = Url.ActualizarJWT((String)Session["jwt"]);
            }
            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
            List<ProductoDTO> ProductoEncontrado = JsonConvertidor.Json_ListaObjeto<ProductoDTO>(Estado);

            MostrarRepetidor(ProductoEncontrado);
            BorrarCampos();
            return;
        }
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        protected void btnActualizar_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            CodigoProducto = btn.CommandArgument; // Hacer algo con el ID del producto

            peticion.PedirComunicacion($"Productos/ObtenerProducto/{CodigoProducto}", MetodoHTTP.GET, TipoContenido.JSON, (String)Session["jwt"]);
            Estado = peticion.ObtenerJson();

            // VALIDAR SESIÓN - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
            if (Estado == null) { Url.CerrarSesion(this); return; }
            else
            {
                Session["jwt"] = Url.ActualizarJWT((String)Session["jwt"]);
            }
            // VALIDAR SESIÓN - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
            List<ProductoDTO> productoEnLista = JsonConvertidor.Json_ListaObjeto<ProductoDTO>(Estado);

            txtNombreProducto.Text = productoEnLista.FirstOrDefault().Nombre;
            txtPrecioProducto.Text = productoEnLista.FirstOrDefault().Precio.ToString();
            txtCostoProducto.Text = productoEnLista.FirstOrDefault().Costo.ToString();
            txtDescripcionProducto.Text = productoEnLista.FirstOrDefault().Descripcion;
            ddlCategoriaProducto.SelectedValue = productoEnLista.FirstOrDefault().CategoriaID.ToString();

            ViewState["ProductoID"] = productoEnLista.FirstOrDefault().ID;
            ViewState["CodigoProducto"] = productoEnLista.FirstOrDefault().Codigo;
            ViewState["NombreImagen"] = productoEnLista.FirstOrDefault().Imagen;

            lblEncontrados.Text = productoEnLista.Count.ToString();

            MostrarRepetidor(productoEnLista);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "actualizarBTNs", "actualizarBTNs()", true);
        }

        protected void CancelarActuBTN_Click(object sender, ImageClickEventArgs e)
        {
            ClientScript.RegisterStartupScript(GetType(), "ReiniciarBTNs", "ReiniciarBTNs()", true);
            BorrarCampos();

            if (! MostrarProductos()) { Url.CerrarSesion(this); return; }
            else
            {
                Session["jwt"] = Url.ActualizarJWT((String)Session["jwt"]);
            }
        }

        protected void btnActualizarProducto_Click(object sender, ImageClickEventArgs e)
        {
            ProductoID = (int)ViewState["ProductoID"];
            CodigoProducto = (string)ViewState["CodigoProducto"];
            string imagenBD = (String)ViewState["NombreImagen"];

            string mensaje = string.Empty;
            bool file = false;

            // Se define la clase de las operaciones de la imagen - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            string ruta = Server.MapPath("~/Multimedia/");
            imagen = new ImagenOp(flUpImagen, ruta);

            try
            {
                // Proceso en la inserción a la base de datos - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
                PrecioProducto = Convert.ToSingle(txtPrecioProducto.Text, CultureInfo.CreateSpecificCulture("en-US"));  //Conversión a flotante
                lblErrorPrecio.Text = "";

                CostoProducto = Convert.ToSingle(txtCostoProducto.Text, CultureInfo.CreateSpecificCulture("en-US"));    //Conversión a flotante
                lblErrorCosto.Text = "";
            }
            catch
            {
                lblErrorPrecio.Text = "ESTE CAMPO SOLO ACEPTA CANTIDADES NUMERICAS";
                lblErrorCosto.Text = "ESTE CAMPO SOLO ACEPTA CANTIDADES NUMERICAS";
                return;
            }

            if (flUpImagen.HasFile)
            {
                file = true;
                mensaje = imagen.ValidarImagen(3000000);
            }

            if (mensaje != "")
            {
                string funcion = string.Format($"ImagenIncompatible('{mensaje}');");
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alertaJS", funcion, true);
                return;
            }

            clsProducto producto = new clsProducto()
            {
                ID = ProductoID,
                Codigo = CodigoProducto,
                Nombre = txtNombreProducto.Text.Trim(),
                CategoriaProductoID = int.Parse(ddlCategoriaProducto.SelectedValue),
                Precio = PrecioProducto,
                Costo = CostoProducto,
                Descripcion = txtDescripcionProducto.Text.Trim(),
                Imagen = $"{CodigoProducto}.jpg"
            };

            Json = JsonConvertidor.Objeto_Json(producto);
            peticion.PedirComunicacion("Productos/ActualizarProducto", MetodoHTTP.PUT, TipoContenido.JSON, (String)Session["jwt"]);
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
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alertaJS", respuesta.AlertaJS, true);
                MostrarProductos();

                BorrarCampos();
                return;
            }

            // Se borra la imagen pasada y se actualiza - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            if (file)
            {
                File.Delete(ruta + imagenBD);
                imagen.SavePNG(CodigoProducto);
            }

            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertaJS", respuesta.AlertaJS, true);
            MostrarProductos(CodigoProducto);
            BorrarCampos();
        }

        private bool MostrarProductos(string Codigo = null, string Busqueda = null)
        {
            if (Codigo != null)
            {
                peticion.PedirComunicacion("Productos/ObtenerProducto/" + Codigo, MetodoHTTP.GET, TipoContenido.JSON, (String)Session["jwt"]);
                Estado = peticion.ObtenerJson();

                if (Estado == null) { return false; }

                List<ProductoDTO> productoEnLista = JsonConvertidor.Json_ListaObjeto<ProductoDTO>(Estado);

                MostrarRepetidor(productoEnLista);
                return true;
            }

            if (Busqueda != null)
            {
                peticion.PedirComunicacion($"Productos/Buscartxt/{Busqueda}", MetodoHTTP.GET, TipoContenido.JSON, (String)Session["jwt"]);
                Estado = peticion.ObtenerJson();

                if (Estado == null) { return false; }

                List<ProductoDTO> productoEnLista = JsonConvertidor.Json_ListaObjeto<ProductoDTO>(Estado);

                MostrarRepetidor(productoEnLista);
                return true;
            }

            peticion.PedirComunicacion("Productos/MostrarProductos/", MetodoHTTP.GET, TipoContenido.JSON, (String)Session["jwt"]);
            Estado = peticion.ObtenerJson();

            if (Estado == null) { return false; }

            List<ProductoDTO> productosEnLista = JsonConvertidor.Json_ListaObjeto<ProductoDTO>(Estado);

            MostrarRepetidor(productosEnLista);
            return true;
        }

        private bool MostrarProductos(int CategoriaID)
        {
            peticion.PedirComunicacion($"Productos/Buscarddl/{CategoriaID}", MetodoHTTP.GET, TipoContenido.JSON, (String)Session["jwt"]);
            Estado = peticion.ObtenerJson();

            if (Estado == null) { return false; }

            List<ProductoDTO> productoEnLista = JsonConvertidor.Json_ListaObjeto<ProductoDTO>(Estado);

            MostrarRepetidor(productoEnLista);
            return true;
        }
        private void MostrarRepetidor(List<ProductoDTO> lista)
        {
            repProductos.DataSource = lista;
            repProductos.DataBind();

            lblEncontrados.Text = lista.Count.ToString();
        }

        private bool MostrarCategorias()
        {
            ddlCategoriaProducto.DataValueField = "ID";
            ddlCategoriaProducto.DataTextField = "Nombre";

            ddlFiltroOrden.DataValueField = "ID";
            ddlFiltroOrden.DataTextField = "Nombre";

            peticion.PedirComunicacion("CategoriaProductos/MostrarCategorias", MetodoHTTP.GET, TipoContenido.JSON, (String)Session["jwt"]);
            Estado = peticion.ObtenerJson();

            if (Estado == null) { return false; }

            List<CategoriaProductoDTO> Categorias = JsonConvertidor.Json_ListaObjeto<CategoriaProductoDTO>(Estado);

            ddlCategoriaProducto.DataSource = Categorias;
            ddlCategoriaProducto.DataBind();

            ddlFiltroOrden.DataSource = Categorias;
            ddlFiltroOrden.DataBind();

            return true;
        }

        private void BorrarCampos()
        {
            ddlCategoriaProducto.SelectedValue = "";
            txtPrecioProducto.Text = String.Empty;
            txtCostoProducto.Text = String.Empty;
            txtDescripcionProducto.Text = String.Empty;
            txtNombreProducto.Text = String.Empty;
        }


    }
}