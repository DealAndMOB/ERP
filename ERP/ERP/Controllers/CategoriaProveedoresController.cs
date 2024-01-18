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
    public class CategoriaProveedoresController : ApiController
    {
        private Modelo bd = new Modelo();
        private AccesoPerfil accesoPerfil = new AccesoPerfil(Modulo.Sistema);
        private PeticionEstado respuesta = new PeticionEstado();

        [JWT]
        [ActionName("AgregarCategoriaProveedor")]
        [HttpPost]
        public PeticionEstado AgregarCategoriaProveedor(clsCategoriaProveedor categoriaProveedor)
        {
            JSONwt usuarioJwt = JsonConvert.DeserializeObject<JSONwt>(Request.Properties["payload"].ToString());
            bool coincidencias = bd.CategoriaProveedor.Any(p => p.Nombre == categoriaProveedor.Nombre);

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

                bd.CategoriaProveedor.Add(categoriaProveedor);
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
        [ActionName("MostrarCategoriasProveedor")]
        [HttpGet]
        public IQueryable<CategoriaProveedorDTO> Get()
        {
            IQueryable<CategoriaProveedorDTO> CategoriaProveedorDTOs = from categoriasProveedor in bd.CategoriaProveedor
                                                                       select new CategoriaProveedorDTO
                                                                       {
                                                                           ID = categoriasProveedor.ID,
                                                                           Nombre = categoriasProveedor.Nombre,
                                                                       };
            return CategoriaProveedorDTOs;
        }

        //Proceso conjunto con la clase CrearCodigo
        [JWT]
        [ActionName("ObtenerCategoriaID")]
        [HttpGet]
        public int Get(string id)
        {
            var categoria = bd.CategoriaProveedor
                .Where(c => c.Nombre == id)
                .Select(c => c.ID).FirstOrDefault();

            return categoria;
        }

        [JWT]
        [ActionName("ActualizarCategoria")]
        [HttpPut]
        public PeticionEstado ActualizarProducto(clsCategoriaProveedor categoria)
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

                var catExistente = bd.CategoriaProveedor.FirstOrDefault(c => c.ID == categoria.ID);
                if (catExistente != null && categoria.Nombre != catExistente.Nombre)
                {
                    bool existencias = bd.CategoriaProveedor.Any(c => c.Nombre == categoria.Nombre);
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
