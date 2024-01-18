using ERP.Complementos.jwt;
using ERP.Complementos.ProcesoBehind;
using ERP.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ERP.Controllers
{
    public class CategoriaProductosController : ApiController
    {
        private Modelo bd = new Modelo();
        private AccesoPerfil accesoPerfil = new AccesoPerfil(Modulo.Sistema);
        private PeticionEstado respuesta = new PeticionEstado();

        [JWT]
        [ActionName("AgregarCategoria")]
        [HttpPost]

        public PeticionEstado AgregarCategoria(clsCategoriaProducto categoria)
        {
            JSONwt usuarioJwt = JsonConvert.DeserializeObject<JSONwt>(Request.Properties["payload"].ToString());
            bool coincidencias = bd.CategoriaProducto.Any(p => p.Nombre == categoria.Nombre);

            //string NombrePerfil = bd.Usuario.Where(c => c.ID == usuarioJwt.iss).Select(c => c.perfil.Nombre).FirstOrDefault(); {COMPARO CON SUPER ADMIN}

            try
            {
                if (!accesoPerfil.ComprobarAcceso(usuarioJwt.uap))
                {
                    respuesta.AlertaJS = "PerfilDenegado()";
                    respuesta.Estado = false;
                    return respuesta;
                }

                if (coincidencias)
                {
                    respuesta.AlertaJS = "CategoriaDuplicada()";
                    respuesta.Estado = false;
                    return respuesta;
                }

                bd.CategoriaProducto.Add(categoria);
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

        [JWT]
        [ActionName("MostrarCategorias")]
        [HttpGet]
        public IQueryable<CategoriaProductoDTO> Get()
        {
            IQueryable<CategoriaProductoDTO> CategoriasDTOs = from categorias in bd.CategoriaProducto
                                                              select new CategoriaProductoDTO
                                                              {
                                                                  ID = categorias.ID,
                                                                  Nombre = categorias.Nombre,
                                                              };
            return CategoriasDTOs;
        }

        //Proceso conjunto con la clase CrearCodigo
        [JWT]
        [ActionName("ObtenerCategoria")]
        [HttpGet]
        public string Get(int id)
        {
            var categoria = bd.CategoriaProducto
                .Where(c => c.ID == id)
                .Select(c => c.Nombre).FirstOrDefault() ?? string.Empty;

            return categoria;
        }

        //Proceso conjunto con la clase CrearCodigo
        [JWT]
        [ActionName("ObtenerCategoriaID")]
        [HttpGet]
        public int Get(string id)
        {
            var categoria = bd.CategoriaProducto
                .Where(c => c.Nombre == id)
                .Select(c => c.ID).FirstOrDefault();

            return categoria;
        }

        //-------Controlador para Actualizar producto de la base de datos-----------//

        [JWT]
        [ActionName("ActualizarCategoria")]
        [HttpPut]
        public PeticionEstado ActualizarProducto(clsCategoriaProducto categoria)
        {
            JSONwt usuarioJwt = JsonConvert.DeserializeObject<JSONwt>(Request.Properties["payload"].ToString());

            //string NombrePerfil = bd.Usuario.Where(c => c.ID == usuarioJwt.iss).Select(c => c.perfil.Nombre).FirstOrDefault(); {COMPARO CON SUPER ADMIN}

            try
            {
                if (!accesoPerfil.ComprobarAcceso(usuarioJwt.uap))
                {
                    respuesta.AlertaJS = "PerfilDenegado()";
                    respuesta.Estado = false;
                    return respuesta;
                }


                var catExistente = bd.CategoriaProducto.FirstOrDefault(c => c.ID == categoria.ID);
                if (catExistente != null && categoria.Nombre != catExistente.Nombre)
                {
                    bool existencias = bd.CategoriaProducto.Any(c => c.Nombre == categoria.Nombre);
                    if (existencias)
                    {
                        respuesta.AlertaJS = "CategoriaDuplicada()";
                        respuesta.Estado = false;
                        return respuesta;
                    }
                }

                bd.Entry(catExistente).CurrentValues.SetValues(categoria);
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
    }
}
