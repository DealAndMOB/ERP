using ERP.Complementos.jwt;
using ERP.Complementos.ProcesoBehind;
using ERP.Models;
using ERP.Vistas.AdminSistema;
using ERP.Vistas.Catalogos;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ERP.Controllers
{
    public class PerfilesController : ApiController
    {
        Modelo bd = new Modelo();
        private AccesoPerfil accesoPerfil = new AccesoPerfil(Modulo.Sistema);
        private PeticionEstado respuesta = new PeticionEstado();

        [JWT]
        [ActionName("AgregarPerfil")]
        [HttpPost]
        public PeticionEstado AgregarProducto(clsPerfil perfil)
        {
            JSONwt usuarioJwt = JsonConvert.DeserializeObject<JSONwt>(Request.Properties["payload"].ToString());
            string NombrePerfil = bd.Usuario.Where(c => c.ID == usuarioJwt.iss).Select(c => c.perfil.Nombre).FirstOrDefault();

            try
            {
                if (!accesoPerfil.ComprobarAcceso(usuarioJwt.uap) || !NombrePerfil.Equals("Super Admin"))
                {
                    respuesta.AlertaJS = "PerfilDenegado()";
                    respuesta.Estado = false;
                    return respuesta;
                }

                if (!(ValidarCaracteres.Letras(perfil.Nombre)))
                {
                    respuesta.AlertaJS = "NombrePerfilInvalido()";
                    respuesta.Estado = false;
                    return respuesta;
                }

                bool coincidenciasNombres = bd.Perfil.Any(p => p.Nombre == perfil.Nombre);
                if (coincidenciasNombres)
                {
                    respuesta.AlertaJS = "PerfilReplicado()";
                    respuesta.Estado = false;
                    return respuesta;
                }

                bd.Perfil.Add(perfil);
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
        [ActionName("MostrarPerfiles")]
        [HttpGet]
        public IQueryable<PerfilDTO> MostrarPerfiles()
        {
            IQueryable<PerfilDTO> perfilDTOs = from perfiles in bd.Perfil
                                               select new PerfilDTO
                                               {
                                                   ID = perfiles.ID,
                                                   Nombre = perfiles.Nombre,
                                               };
            return perfilDTOs;
        }

        [JWT]
        [ActionName("ObtenerAccesos")]
        [HttpGet]
        public string ObtenerAccesos(int id)
        {
            string accesos = bd.Perfil.Where(c => c.ID == id).Select(c => c.Accesos).FirstOrDefault();

            return accesos;
        }

        [JWT]
        [ActionName("BorrarPerfil")]
        [HttpDelete]

        public PeticionEstado BorrarPerfil(int id)
        {
            JSONwt usuarioJwt = JsonConvert.DeserializeObject<JSONwt>(Request.Properties["payload"].ToString());
            string NombrePerfil = bd.Usuario.Where(c => c.ID == usuarioJwt.iss).Select(c => c.perfil.Nombre).FirstOrDefault();

            clsPerfil Perfil = bd.Perfil.Where(c => c.ID == id).FirstOrDefault();

            try
            {

                if (!accesoPerfil.ComprobarAcceso(usuarioJwt.uap) || !NombrePerfil.Equals("Super Admin"))
                {
                    respuesta.AlertaJS = "PerfilDenegado()";
                    respuesta.Estado = false;
                    return respuesta;
                }

                if (Perfil.Nombre.Equals("Super Admin"))
                {
                    respuesta.AlertaJS = "NoEliminarPerfil()";
                    respuesta.Estado = false;
                    return respuesta;
                }

                clsPerfil perfil = bd.Perfil.Find(id);
                bd.Entry(perfil).State = System.Data.Entity.EntityState.Deleted;
                bd.SaveChanges();

                respuesta.AlertaJS = "Eliminado()";
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
        [ActionName("ActualizarPerfil")]
        [HttpPut]
        public PeticionEstado ActualizarPerfil(clsPerfil perfil)
        {
            JSONwt usuarioJwt = JsonConvert.DeserializeObject<JSONwt>(Request.Properties["payload"].ToString());
            string NombrePerfil = bd.Usuario.Where(c => c.ID == usuarioJwt.iss).Select(c => c.perfil.Nombre).FirstOrDefault();

            var PerfilExistente = bd.Perfil.FirstOrDefault(p => p.ID == perfil.ID);

            try
            {
                if (!accesoPerfil.ComprobarAcceso(usuarioJwt.uap) || !NombrePerfil.Equals("Super Admin"))
                {
                    respuesta.AlertaJS = "PerfilDenegado()";
                    respuesta.Estado = false;
                    return respuesta;
                }

                if (PerfilExistente.Nombre.Equals("Super Admin"))
                {
                    respuesta.AlertaJS = "NoActualizarPerfil()";
                    respuesta.Estado = false;
                    return respuesta;
                }

                if (PerfilExistente != null && perfil.Nombre != PerfilExistente.Nombre)
                {
                    bool coincidencias = bd.Perfil.Any(p => p.Nombre == perfil.Nombre);
                    if (coincidencias)
                    {
                        respuesta.AlertaJS = "PerfilReplicado()";
                        respuesta.Estado = false;
                        return respuesta;
                    }
                }

                if (!(ValidarCaracteres.Letras(perfil.Nombre)))
                {
                    respuesta.AlertaJS = "NombrePerfilInvalido()";
                    respuesta.Estado = false;
                    return respuesta;
                }

                // Actualizar las propiedades del cliente existente con los valores del cliente actualizado
                bd.Entry(PerfilExistente).CurrentValues.SetValues(perfil);
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
