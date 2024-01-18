using ERP.Complementos.jwt;
using ERP.Complementos.ProcesoBehind;
using ERP.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Policy;
using System.Web.Http;

namespace ERP.Controllers
{
    public class EstadosController : ApiController
    {
        private Modelo bd = new Modelo();
        private AccesoPerfil accesoPerfil = new AccesoPerfil(Modulo.Sistema);
        private PeticionEstado respuesta = new PeticionEstado();

        [JWT]
        [ActionName("AgregarEstado")]
        [HttpPost]
        public PeticionEstado AgregarEstado(clsEstado estado)
        {
            JSONwt usuarioJwt = JsonConvert.DeserializeObject<JSONwt>(Request.Properties["payload"].ToString());
            bool coincidencias = bd.Estado.Any(p => p.Nombre == estado.Nombre);

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
                    respuesta.AlertaJS = "EstadoDuplicado()";
                    respuesta.Estado = false;
                    return respuesta;
                }

                bd.Estado.Add(estado);
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
        [ActionName("MostrarEstados")]
        [HttpGet]
        public IQueryable<EstadoDTO> Get()
        {
            IQueryable<EstadoDTO> estadosDTOs = from estados in bd.Estado
                                                select new EstadoDTO
                                                {
                                                    ID = estados.ID,
                                                    Zona = estados.zona.Zona,
                                                    Nombre = estados.Nombre,
                                                };
            return estadosDTOs;
        }

        [JWT]
        [ActionName("ObtenerEstado")]
        [HttpGet]

        public IQueryable<EstadoDTO> ObtenerEstado(String id)
        {
            IQueryable<EstadoDTO> EstadotosDTOs = from estado in bd.Estado
                                                  where estado.Nombre.Contains(id)
                                                  select new EstadoDTO
                                                  {
                                                      ID = estado.ID,
                                                      Nombre = estado.Nombre,
                                                      ZonaID = estado.ZonaID
                                                  };
            return EstadotosDTOs;
        }

        [JWT]
        [ActionName("ActualizarEstado")]
        [HttpPut]
        public PeticionEstado ActualizarEstado(clsEstado estado)
        {
            JSONwt usuarioJwt = JsonConvert.DeserializeObject<JSONwt>(Request.Properties["payload"].ToString());
            string NombrePerfil = bd.Usuario.Where(c => c.ID == usuarioJwt.iss).Select(c => c.perfil.Nombre).FirstOrDefault();

            try
            {
                if (!accesoPerfil.ComprobarAcceso(usuarioJwt.uap))
                {
                    respuesta.AlertaJS = "PerfilDenegado()";
                    respuesta.Estado = false;
                    return respuesta;
                }

                var estadoExistente = bd.Estado.FirstOrDefault(c => c.ID == estado.ID);
                if (estadoExistente != null && estado.Nombre != estadoExistente.Nombre)
                {
                    bool existencias = bd.Estado.Any(c => c.Nombre == estado.Nombre);
                    if (existencias)
                    {
                        respuesta.AlertaJS = "EstadoDuplicado()";
                        respuesta.Estado = false;
                        return respuesta;
                    }
                }

                bd.Entry(estadoExistente).CurrentValues.SetValues(estado);
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
