using ERP.Complementos.jwt;
using ERP.Complementos.ProcesoBehind;
using ERP.Models;
using ERP.Vistas.AdminSistema;
using ERP.Vistas.Catalogos;
using HTTPupt;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Web.Http;

namespace ERP.Controllers
{
    public class UsuariosController : ApiController
    {
        private Modelo bd = new Modelo();
        private AccesoPerfil accesoPerfil = new AccesoPerfil(Modulo.Sistema);
        private PeticionEstado respuesta = new PeticionEstado();
        //- - - - - - - Controlador para agregar usuario a la base de datos - - - - - - - - - - - //

        [JWT]
        [ActionName("AgregarUsuario")]
        [HttpPost]

        public PeticionEstado AgregarUsuario(clsUsuario usuario)
        {
            JSONwt usuarioJwt = JsonConvert.DeserializeObject<JSONwt>(Request.Properties["payload"].ToString());
            string NombrePerfil = bd.Usuario.Where(c => c.ID == usuarioJwt.iss).Select(c => c.perfil.Nombre).FirstOrDefault();

            List<clsUsuario> superAdmin = bd.Usuario.Where(c => c.perfil.Nombre == "Super Admin").ToList();
            string PerfilExistente = bd.Perfil.Where(c => c.ID == usuario.PerfilID).Select(c => c.Nombre).FirstOrDefault();

            try
            {
                if (!accesoPerfil.ComprobarAcceso(usuarioJwt.uap) || !NombrePerfil.Equals("Super Admin"))
                {
                    respuesta.AlertaJS = "PerfilDenegado()";
                    respuesta.Estado = false;
                    return respuesta;
                }

                if (superAdmin.Count == 1 && PerfilExistente.Equals("Super Admin"))
                {
                    respuesta.AlertaJS = "SuperAdminUnico()";
                    respuesta.Estado = false;
                    return respuesta;
                }

                if (!(ValidarCaracteres.Correo(usuario.Correo)))
                {
                    respuesta.AlertaJS = "ErrorCorreo()";
                    respuesta.Estado = false;
                    return respuesta;
                }

                bool coincidenciasNombres = bd.Usuario.Any(p => p.Nombre == usuario.Nombre);
                if (coincidenciasNombres)
                {
                    respuesta.AlertaJS = "UsuarioReplicado()";
                    respuesta.Estado = false;
                    return respuesta;
                }

                bool coincidenciasCorreos = bd.Usuario.Any(p => p.Correo == usuario.Correo);
                if (coincidenciasCorreos)
                {
                    respuesta.AlertaJS = "UsuarioReplicado()";
                    respuesta.Estado = false;
                    return respuesta;
                }

                bd.Usuario.Add(usuario);
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

        //- - - - - - - Controlador para mostrar Usuarios de la base de datos - - - - - - - //

        [JWT]
        [ActionName("MostrarUsuarios")]
        [HttpGet]

        public IQueryable<UsuarioDTO> MostrarUsuarios()
        {
            IQueryable<UsuarioDTO> Consulta = from usuario in bd.Usuario
                                              select new UsuarioDTO
                                              {
                                                  ID = usuario.ID,
                                                  Nombre = usuario.Nombre,
                                                  Correo = usuario.Correo,
                                                  Contraseña = usuario.Contraseña,
                                                  Perfil = usuario.perfil.Nombre,
                                              };
            return Consulta;
        }

        //- - - - - - - Controlador para mostrar Usuarios de la base de datos - - - - - - - //
        [JWT]
        [ActionName("ObtenerUsuario")]
        [HttpGet]

        public IQueryable<UsuarioDTO> ObtenerUsuario(int id)
        {
            IQueryable<UsuarioDTO> Consulta = from usuario in bd.Usuario
                                              where id == usuario.ID
                                              select new UsuarioDTO
                                              {
                                                  ID = usuario.ID,
                                                  Nombre = usuario.Nombre,
                                                  Correo = usuario.Correo,
                                                  Contraseña = usuario.Contraseña,
                                                  PerfilID = usuario.PerfilID,
                                              };
            return Consulta;
        }

        //- - - - - - - Controlador para Actualizar Usuario de la base de datos - - - - - - - //

        [JWT]
        [ActionName("ActualizarUsuario")]
        [HttpPut]
        public PeticionEstado ActualizarUsuario(clsUsuario usuario)
        {
            JSONwt usuarioJwt = JsonConvert.DeserializeObject<JSONwt>(Request.Properties["payload"].ToString());
            string NombrePerfil = bd.Usuario.Where(c => c.ID == usuarioJwt.iss).Select(c => c.perfil.Nombre).FirstOrDefault();

            //Obtengo el usuario que se quiere actualizar
            clsUsuario UsuarioExistente = bd.Usuario.FirstOrDefault(c => c.ID == usuario.ID);

            // Obtengo el nombre del perfil con el que se quiere actualizar el usuario
            string PerfilExistente = bd.Perfil.Where(c => c.ID == usuario.PerfilID).Select(c => c.Nombre).FirstOrDefault();
            int SuperAdminID = bd.Perfil.Where(c => c.Nombre.Equals("Super Admin")).Select(c => c.ID).FirstOrDefault();

            try
            {
                if (!accesoPerfil.ComprobarAcceso(usuarioJwt.uap) || !NombrePerfil.Equals("Super Admin"))
                {
                    respuesta.AlertaJS = "PerfilDenegado()";
                    respuesta.Estado = false;
                    return respuesta;
                }

                //Si es Super Admin, y se quiere actualizar su perfil no se permitirá
                if (UsuarioExistente.perfil.Nombre.Equals("Super Admin") && UsuarioExistente.PerfilID != usuario.PerfilID)
                {
                    respuesta.AlertaJS = "NoActualizarUsuario()";
                    respuesta.Estado = false;
                    return respuesta;
                }

                //Si el perfil es diferente a Super Admin, y se le quiere asignar dicho perfil no se permitira
                if (!UsuarioExistente.perfil.Nombre.Equals("Super Admin") && usuario.PerfilID == SuperAdminID)
                {
                    respuesta.AlertaJS = "SuperAdminUnico()";
                    respuesta.Estado = false;
                    return respuesta;
                }

                if (!(ValidarCaracteres.Correo(usuario.Correo)))
                {
                    respuesta.AlertaJS = "ErrorCorreo()";
                    respuesta.Estado = false;
                    return respuesta;
                }

                if (UsuarioExistente != null && usuario.Nombre != UsuarioExistente.Nombre)
                {
                    bool coincidenciasNombres = bd.Usuario.Any(p => p.Nombre == usuario.Nombre);
                    if (coincidenciasNombres)
                    {
                        respuesta.AlertaJS = "UsuarioReplicado()";
                        respuesta.Estado = false;
                        return respuesta;
                    }
                }

                if (UsuarioExistente != null && usuario.Correo != UsuarioExistente.Correo)
                {
                    bool coincidenciasCorreos = bd.Usuario.Any(p => p.Correo == usuario.Correo);
                    if (coincidenciasCorreos)
                    {
                        respuesta.AlertaJS = "UsuarioReplicado()";
                        respuesta.Estado = false;
                        return respuesta;
                    }
                }

                // Actualizar las propiedades del cliente existente con los valores del cliente actualizado
                bd.Entry(UsuarioExistente).CurrentValues.SetValues(usuario);
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

        //- - - - - - - Controlador para eliminar usuario de la base de datos - - - - - - - //

        [JWT]
        [ActionName("BorrarUsuario")]
        [HttpDelete]

        public PeticionEstado BorrarUsuario(int id)
        {
            JSONwt usuarioJwt = JsonConvert.DeserializeObject<JSONwt>(Request.Properties["payload"].ToString());
            string NombrePerfil = bd.Usuario.Where(c => c.ID == usuarioJwt.iss).Select(c => c.perfil.Nombre).FirstOrDefault();

            string usuarioABorrar = bd.Usuario.Where(c => c.ID == id).Select(c => c.perfil.Nombre).FirstOrDefault();
            try
            {

                if (!accesoPerfil.ComprobarAcceso(usuarioJwt.uap) || !NombrePerfil.Equals("Super Admin"))
                {
                    respuesta.AlertaJS = "PerfilDenegado()";
                    respuesta.Estado = false;
                    return respuesta;
                }

                if (usuarioABorrar.Equals("Super Admin"))
                {
                    respuesta.AlertaJS = "NoEliminarUsuario()";
                    respuesta.Estado = false;
                    return respuesta;
                }

                clsUsuario usuario = bd.Usuario.Find(id);
                bd.Entry(usuario).State = System.Data.Entity.EntityState.Deleted;
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

        // - - - - - - - - - - - - - - - - - Clases y Controladores JWT - - - - - - - - - - - - - - - - - - //
        JWT jwt = new JWT();
        // - - - - - - - - - - - - - - - - - - Controlador que crea JWT - - - - - - - - - - - - - - - - - - //

        [ActionName("PedirToken")]
        [HttpPost]

        public String PedirToken(InicioSesion DatosInicio)
        {
            var Usuario = bd.Usuario.Where(c => c.Correo == DatosInicio.Correo && c.Contraseña == DatosInicio.Contraseña).FirstOrDefault();
            if (Usuario == null) { return null; }

            bool password = string.Equals(Usuario.Contraseña, DatosInicio.Contraseña, StringComparison.Ordinal);

            if (password)
            {
                DateTime expira;

                expira = DateTime.Now.AddMinutes(20);
                Epoch epoch = new Epoch();

                Dictionary<string, object> payload = new Dictionary<string, object>()
                {
                    {"iss", Usuario.ID},
                    {"Exp", epoch.convertirEpoch(expira)},
                    {"iat", epoch.convertirEpoch(DateTime.Now)},
                    {"uap", Usuario.perfil.Accesos}
                };

                return jwt.GetJWT(payload);
            }
            else
            {
                return null;
            }
        }

        [JWT]
        [ActionName("ActualizarJWT")]
        [HttpGet]
        public String ActualizarJWT()
        {
            JSONwt usuarioJwt = JsonConvert.DeserializeObject<JSONwt>(Request.Properties["payload"].ToString());

            if (usuarioJwt != null)
            {
                DateTime expira;

                expira = DateTime.Now.AddMinutes(20);
                Epoch epoch = new Epoch();

                Dictionary<string, object> payload = new Dictionary<string, object>()
                {
                    {"iss", usuarioJwt.iss},
                    {"Exp", epoch.convertirEpoch(expira)},
                    {"iat", epoch.convertirEpoch(DateTime.Now)},
                    {"uap", usuarioJwt.uap}
                };

                return jwt.GetJWT(payload);
            }
            else
            {
                return null;
            }
        }
        // - - - - - - - - - - - - - - - - - - Controlador que consulta JWT - - - - - - - - - - - - - - - - - - //

        [JWT]
        [ActionName("ConsultarPorUsuario")]
        [HttpGet]
        public UsuarioDTO ConsultarPorUsuario()
        {
            JSONwt usuarioJwt = JsonConvert.DeserializeObject<JSONwt>(Request.Properties["payload"].ToString());

            var usuario = (from consulta in bd.Usuario
                           where consulta.ID == usuarioJwt.iss
                           select new UsuarioDTO
                           {
                               ID = consulta.ID,
                               Nombre = consulta.Nombre,
                               Correo = consulta.Correo,
                               Contraseña = consulta.Contraseña,
                               Accesos = consulta.perfil.Accesos
                           }).FirstOrDefault();

            return usuario;
        }

        // - - - - - - - - - - - - - - - - - - Controlador que verifica JWT - - - - - - - - - - - - - - - - - - //

        [JWT]
        [ActionName("VerificarToken")]
        [HttpGet]
        public JSONwt VerificarToken()
        {
            JSONwt usuarioJwt = JsonConvert.DeserializeObject<JSONwt>(Request.Properties["payload"].ToString());

            return usuarioJwt;
        }

        // - - - - - - - - - - - - - - - - - - Controladores de testeo - - - - - - - - - - - - - - - - - - - - - //
        [JWT]
        [ActionName("Expirar")]
        [HttpGet]
        public bool boton()
        {
            return true;
        }
    }
}
