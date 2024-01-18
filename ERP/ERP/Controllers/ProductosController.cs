using ERP.Complementos;
using ERP.Complementos.jwt;
using ERP.Complementos.ProcesoBehind;
using ERP.Models;
using ERP.Vistas.AdminSistema;
using ERP.Vistas.Catalogos;
using ERP.Vistas.Ventas;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ERP.Controllers
{
    public class ProductosController : ApiController
    {
        private Modelo bd = new Modelo();
        private AccesoPerfil accesoPerfil = new AccesoPerfil(Modulo.Catalogos);
        private PeticionEstado respuesta = new PeticionEstado();
        //-------Controlador para agregar productos a la base de datos-----------//

        [JWT]
        [ActionName("AgregarProducto")]
        [HttpPost]
        public PeticionEstado AgregarProducto(clsProducto producto)
        {
            JSONwt usuarioJwt = JsonConvert.DeserializeObject<JSONwt>(Request.Properties["payload"].ToString());
            bool coincidencias = bd.Producto.Any(p => p.Nombre == producto.Nombre);
            bool CodigoDuplicado = bd.Producto.Any(p => p.Codigo == producto.Codigo);

            try
            {
                if (!accesoPerfil.ComprobarAcceso(usuarioJwt.uap))
                {
                    respuesta.AlertaJS = "PerfilDenegado()";
                    respuesta.Estado = false;
                    return respuesta;
                }

                if (CodigoDuplicado)
                {
                    respuesta.AlertaJS = "MismoCodigoProducto()";
                    respuesta.Estado = false;
                    return respuesta;
                }

                if (coincidencias)
                {
                    respuesta.AlertaJS = "NombreDuplicado()";
                    respuesta.Estado = false;
                    return respuesta;
                }

                if (producto.Costo >= producto.Precio)
                {
                    respuesta.AlertaJS = "ErrorCostoPrecio()";
                    respuesta.Estado = false;
                    return respuesta;
                }

                // - - - - - - - - - - - - - - -
                bd.Producto.Add(producto);
                bd.SaveChanges();

                respuesta.AlertaJS = "Subido()";
                respuesta.Estado = true;

                return respuesta;
            }
            catch
            {
                respuesta.AlertaJS = "ErrorDatos()";
                respuesta.Estado = false;
                return respuesta;
            }
        }

        //- - - - - - - Controlador que muestra solo un producto que coinsida con el ID del producto - - - - - - -
        [JWT]
        [ActionName("ObtenerProducto")]
        [HttpGet]
        public IQueryable<ProductoDTO> Get(string id)
        {
            IQueryable<ProductoDTO> productosDTOs = from productos in bd.Producto
                                                    where productos.Codigo == id
                                                    select new ProductoDTO
                                                    {
                                                        ID = productos.ID,
                                                        Codigo= productos.Codigo,
                                                        Nombre = productos.Nombre,
                                                        Descripcion = productos.Descripcion,
                                                        Precio = productos.Precio,
                                                        Costo = productos.Costo,
                                                        NombreCategoria = productos.CategoriaProducto.Nombre,
                                                        CategoriaID = productos.CategoriaProducto.ID,
                                                        Imagen = productos.Imagen
                                                    };
            return productosDTOs;
        }
        // - - - - - - - Controlador para mostrar las coinsidencias de palabras en el nombre del producto - - - - - - -

        [JWT]
        [ActionName("Buscartxt")]
        [HttpGet]
        public List<ProductoDTO> Buscartxt(String id)
        {
            List<ProductoDTO> productosQuery = (from productos in bd.Producto
                                                     where productos.Nombre.Contains(id)
                                                     select new ProductoDTO
                                                     {
                                                         ID = productos.ID,
                                                         Codigo = productos.Codigo,
                                                         Nombre = productos.Nombre,
                                                         Descripcion = productos.Descripcion,
                                                         Precio = productos.Precio,
                                                         Costo = productos.Costo,
                                                         NombreCategoria = productos.CategoriaProducto.Nombre,
                                                         Imagen = productos.Imagen
                                                     }).ToList();

            productosQuery.ForEach(p => p.PrecioFormato = p.Precio.ToString("C2", new CultureInfo("es-MX")));
            productosQuery.ForEach(p => p.CostoFormato = p.Costo.ToString("C2", new CultureInfo("es-MX")));
            return productosQuery;
        } 
        
        [JWT]
        [ActionName("Buscarddl")]
        [HttpGet]
        public List<ProductoDTO> Buscarddl(int id)
        {
            List<ProductoDTO> productosQuery = (from productos in bd.Producto
                                                     where productos.CategoriaProductoID == id
                                                     select new ProductoDTO
                                                     {
                                                         ID = productos.ID,
                                                         Codigo = productos.Codigo,
                                                         Nombre = productos.Nombre,
                                                         Descripcion = productos.Descripcion,
                                                         Precio = productos.Precio,
                                                         Costo = productos.Costo,
                                                         NombreCategoria = productos.CategoriaProducto.Nombre,
                                                         Imagen = productos.Imagen
                                                     }).ToList();

            productosQuery.ForEach(p => p.PrecioFormato = p.Precio.ToString("C2", new CultureInfo("es-MX")));
            productosQuery.ForEach(p => p.CostoFormato = p.Costo.ToString("C2", new CultureInfo("es-MX")));
            return productosQuery;
        }

        // - - - - - - - Controlador para mostrar las coinsidencias de ID con el ID de la categoria que se posee - - - - - - -

        [JWT]
        [ActionName("BusquedaCombinada")]
        [HttpGet]
        public IQueryable<ProductoDTO> BusquedaCombinada(int CategoriaID, String Busqueda)
        {

            IQueryable<ProductoDTO> ProductosDTOs = from productos in bd.Producto
                                                    where productos.CategoriaProductoID == CategoriaID && productos.Nombre.Contains(Busqueda)
                                                    select new ProductoDTO
                                                    {
                                                        ID = productos.ID,
                                                        Codigo = productos.Codigo,
                                                        Nombre = productos.Nombre,
                                                        Descripcion = productos.Descripcion,
                                                        Precio = productos.Precio,
                                                        Costo = productos.Costo,
                                                        NombreCategoria = productos.CategoriaProducto.Nombre,
                                                        Imagen = productos.Imagen
                                                    };

            return ProductosDTOs;
        }

        //-------Controlador para mostrar un producto de la base de datos-----------//

        [JWT]
        [ActionName("MostrarProductos")]
        [HttpGet]

        public IQueryable<ProductoDTO> Get()
        {
            IQueryable<ProductoDTO> ProductosDTOs = from productos in bd.Producto
                                                    select new ProductoDTO
                                                    {
                                                        ID = productos.ID,
                                                        Codigo = productos.Codigo,
                                                        Nombre = productos.Nombre,
                                                        Descripcion = productos.Descripcion,
                                                        Precio = productos.Precio,
                                                        Costo = productos.Costo,
                                                        NombreCategoria = productos.CategoriaProducto.Nombre,
                                                        Imagen = productos.Imagen
                                                    };
            return ProductosDTOs;
        }

        //-------Controlador para Actualizar producto de la base de datos-----------//

        [JWT]
        [ActionName("ActualizarProducto")]
        [HttpPut]
        public PeticionEstado ActualizarProducto(clsProducto producto)
        {
            JSONwt usuarioJwt = JsonConvert.DeserializeObject<JSONwt>(Request.Properties["payload"].ToString());

            try
            {
                if (!accesoPerfil.ComprobarAcceso(usuarioJwt.uap))
                {
                    respuesta.AlertaJS = "PerfilDenegado()";
                    respuesta.Estado = false;
                    return respuesta;
                }

                // Verificar coincidencias solo si el nombre se modifica
                var productoExistente = bd.Producto.FirstOrDefault(p => p.ID == producto.ID);
                if (productoExistente != null && producto.Nombre != productoExistente.Nombre)
                {
                    bool coincidencias = bd.Producto.Any(p => p.Nombre == producto.Nombre);
                    if (coincidencias)
                    {
                        respuesta.AlertaJS = "NombreDuplicado()";
                        respuesta.Estado = false;
                        return respuesta;
                    }
                }
                  
                if (producto.Costo >= producto.Precio)
                {
                    respuesta.AlertaJS = "ErrorCostoPrecio()";
                    respuesta.Estado = false;
                    return respuesta;
                }

                // Actualizar las propiedades del producto existente con los valores del producto actualizado
                bd.Entry(productoExistente).CurrentValues.SetValues(producto);
                bd.SaveChanges();

                respuesta.AlertaJS = "Subido()";
                respuesta.Estado = true;
                return respuesta;
            }
            catch (Exception ex)
            {
                respuesta.AlertaJS = "ErrorDatos()";
                respuesta.Estado = false;
                return respuesta;
            }
        }

        //-------Controlador para eliminar producto de la base de datos-----------//
        //[JWT]
        //[ActionName("BorrarProducto")]
        //[HttpDelete]

        //public bool BorrarProducto(string id)
        //{
        //    try
        //    {
        //        clsProducto productos = bd.Producto.Find(id);
        //        bd.Entry(productos).State = System.Data.Entity.EntityState.Deleted;
        //        bd.SaveChanges();
        //        return true;
        //    }
        //    catch
        //    {
        //        return false;
        //    }
        //}

        [JWT]
        [ActionName("obtenerCodigoFinal")]
        [HttpGet]
        public string obtenerCodigo(string id)
        {
            var CodigosDistintos = (from producto in bd.Producto
                                    where producto.Codigo.Contains(id)
                                    select producto.Codigo).ToList();

            if (!CodigosDistintos.Any())
            {
                return "null";
            }

            var CodigosYnumeros = CodigosDistintos
                .Select(codigo => (codigo, int.Parse(new string(codigo.Where(char.IsDigit).ToArray()))))
                .ToList();

            var CodigoFinal = CodigosYnumeros
                .OrderByDescending(x => x.Item2)
                .First()
                .codigo;

            return CodigoFinal;
        }

        [JWT]
        [ActionName("ObtenerDescripcion")]
        [HttpGet]
        public string ObtenerCondiciones(string id)
        {
            string descripcion = bd.Producto.Where(c => c.Codigo == id).Select(c => c.Descripcion).FirstOrDefault();
            descripcion = descripcion.Replace("\r", "").Replace("\n", " ");

            return descripcion;
        }

        [JWT]
        [ActionName("ObtenerProductoID")]
        [HttpGet]
        public int ObtenerProductoID(string id)
        {
            int ID = bd.Producto.Where(c => c.Codigo == id).Select(c => c.ID).FirstOrDefault();

            return ID;
        }

    }
}
