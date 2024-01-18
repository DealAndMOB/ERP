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
    public class ZonasController : ApiController
    {
        private Modelo bd = new Modelo();
        private AccesoPerfil accesoPerfil = new AccesoPerfil(Modulo.Sistema);
        private PeticionEstado respuesta = new PeticionEstado();

        [JWT]
        [ActionName("AgregarZona")]
        [HttpPost]
        public PeticionEstado AgregarZona(clsZona zona)
        {
            JSONwt usuarioJwt = JsonConvert.DeserializeObject<JSONwt>(Request.Properties["payload"].ToString());
            bool coincidencias = bd.Zona.Any(p => p.Zona == zona.Zona);

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
                    respuesta.AlertaJS = "ZonaDuplicada()";
                    respuesta.Estado = false;
                    return respuesta;
                }

                bd.Zona.Add(zona);
                bd.SaveChangesAsync();

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
        [ActionName("MostrarZonas")]
        [HttpGet]
        public IQueryable<ZonaDTO> Get()
        {
            IQueryable<ZonaDTO> ZonasDTOs = from zonas in bd.Zona
                                            select new ZonaDTO
                                            {
                                                ID = zonas.ID,
                                                Zona = zonas.Zona,
                                            };
            return ZonasDTOs;
        }

        //Proceso conjunto con la clase CrearCodigo
        [JWT]
        [ActionName("ObtenerCategoriaID")]
        [HttpGet]
        public int Get(string id)
        {
            var categoria = bd.Zona
                .Where(c => c.Zona == id)
                .Select(c => c.ID).FirstOrDefault();

            return categoria;
        }

        [JWT]
        [ActionName("ActualizarZona")]
        [HttpPut]
        public PeticionEstado ActualizarProducto(clsZona zona)
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

                var zonaExistente = bd.Zona.FirstOrDefault(c => c.ID == zona.ID);
                if (zonaExistente != null && zona.Zona != zonaExistente.Zona)
                {
                    bool existencias = bd.Zona.Any(c => c.Zona == zona.Zona);
                    if (existencias)
                    {
                        respuesta.AlertaJS = "ZonaDuplicada()";
                        respuesta.Estado = false;
                        return respuesta;
                    }
                }

                bd.Entry(zonaExistente).CurrentValues.SetValues(zona);
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
