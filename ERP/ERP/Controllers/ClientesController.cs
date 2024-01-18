using ERP.Complementos.jwt;
using ERP.Complementos.ProcesoBehind;
using ERP.Models;
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
    public class ClientesController : ApiController
    {
        private Modelo bd = new Modelo();
        private AccesoPerfil accesoPerfil = new AccesoPerfil(Modulo.Catalogos);
        private PeticionEstado respuesta = new PeticionEstado();

        [JWT]
        [ActionName("AgregarCliente")]
        [HttpPost]

        //-------Controlador para agregar clientes a la base de datos-----------//
        public PeticionEstado IngresarCliente(clsCliente cliente)
        {
            JSONwt usuarioJwt = JsonConvert.DeserializeObject<JSONwt>(Request.Properties["payload"].ToString());
            bool coincidencias = bd.Cliente.Any(p => p.RFC == cliente.RFC);

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
                    respuesta.AlertaJS = "RFCClienteDuplicado()";
                    respuesta.Estado = false;
                    return respuesta;
                }

                if (!(cliente.RFC.Length >= 12 && cliente.RFC.Length <= 13))
                {
                    respuesta.AlertaJS = "ErrorRFC()";
                    respuesta.Estado = false;
                    return respuesta;
                }

                if (!ValidarCaracteres.RFC(cliente.RFC))
                {
                    respuesta.AlertaJS = "RFCinvalido()";
                    respuesta.Estado = false;
                    return respuesta;
                }

                if (cliente.Telefono.Length != 10)
                {
                    respuesta.AlertaJS = "ErrorTelefono()";
                    respuesta.Estado = false;
                    return respuesta;
                }

                if (!(ValidarCaracteres.Numeros(cliente.Telefono)))
                {
                    respuesta.AlertaJS = "TelefonoInvalido()";
                    respuesta.Estado = false;
                    return respuesta;
                }
                
                if (!(ValidarCaracteres.Correo(cliente.Correo)))
                {
                    respuesta.AlertaJS = "ErrorCorreo()";
                    respuesta.Estado = false;
                    return respuesta;
                }

                bd.Cliente.Add(cliente);
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

        //-------Controlador para mostrar un cliente de la base de datos-----------//
        [JWT]
        [ActionName("BuscarCliente")]
        [HttpGet]

        public IQueryable<ClienteDTO> Buscar(String id)
        {
            IQueryable<ClienteDTO> clientesDTOs = from cliente in bd.Cliente
                                                  where cliente.RFC.Contains(id)
                                                  select new ClienteDTO
                                                  {
                                                      ID = cliente.ID,
                                                      RFC = cliente.RFC,
                                                      Nombre = cliente.Nombre,
                                                      Direccion = cliente.Direccion,
                                                      Telefono = cliente.Telefono,
                                                      Correo = cliente.Correo
                                                  };
            return clientesDTOs;
        }

        //-------Controlador para mostrar los clientes de la base de datos-----------//

        [JWT]
        [ActionName("MostrarClientes")]
        [HttpGet]

        public IQueryable<ClienteDTO> Get()
        {
            IQueryable<ClienteDTO> clientesDTOs = from clientes in bd.Cliente
                                                  select new ClienteDTO
                                                  {
                                                      ID = clientes.ID,
                                                      RFC = clientes.RFC,
                                                      Nombre = clientes.Nombre,
                                                      Direccion = clientes.Direccion,
                                                      Telefono = clientes.Telefono,
                                                      Correo = clientes.Correo,
                                                  };
            return clientesDTOs;
        }

        //-------Controlador para Actualizar clientes de la base de datos-----------//

        [JWT]
        [ActionName("ActualizarCliente")]
        [HttpPut]
        public PeticionEstado ActualizarCliente(clsCliente cliente)
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

                var ClienteExistente = bd.Cliente.FirstOrDefault(p => p.ID == cliente.ID);
                if (ClienteExistente != null && cliente.RFC != ClienteExistente.RFC)
                {
                    bool coincidencias = bd.Cliente.Any(p => p.RFC == cliente.RFC);
                    if (coincidencias)
                    {
                        respuesta.AlertaJS = "RFCClienteDuplicado()";
                        respuesta.Estado = false;
                        return respuesta;
                    }
                }

                if (!(cliente.RFC.Length >= 12 && cliente.RFC.Length <= 13))
                {
                    respuesta.AlertaJS = "ErrorRFC()";
                    respuesta.Estado = false;
                    return respuesta;
                }

                if (!ValidarCaracteres.RFC(cliente.RFC))
                {
                    respuesta.AlertaJS = "RFCinvalido()";
                    respuesta.Estado = false;
                    return respuesta;
                }

                if (cliente.Telefono.Length != 10)
                {
                    respuesta.AlertaJS = "ErrorTelefono()";
                    respuesta.Estado = false;
                    return respuesta;
                }

                if (!(ValidarCaracteres.Numeros(cliente.Telefono)))
                {
                    respuesta.AlertaJS = "TelefonoInvalido()";
                    respuesta.Estado = false;
                    return respuesta;
                }

                if (!(ValidarCaracteres.Correo(cliente.Correo)))
                {
                    respuesta.AlertaJS = "ErrorCorreo()";
                    respuesta.Estado = false;
                    return respuesta;
                }

                // Actualizar las propiedades del cliente existente con los valores del cliente actualizado
                bd.Entry(ClienteExistente).CurrentValues.SetValues(cliente);
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
        [ActionName("ObtenerClienteID")]
        [HttpGet]
        public int ObtenerClienteID(string id)
        {
            int ID = bd.Cliente.Where(c => c.RFC == id).Select(c => c.ID).FirstOrDefault();

            return ID;
        }

        //-------Controlador para eliminar cliente de la base de datos-----------//
        //[JWT]
        //[ActionName("BorrarCliente")]
        //[HttpDelete]

        //public bool BorrarCliente(string id)
        //{
        //    try
        //    {
        //        clsCliente clientes = bd.Cliente.Find(id);
        //        bd.Entry(clientes).State = System.Data.Entity.EntityState.Deleted;
        //        bd.SaveChanges();
        //        return true;
        //    }
        //    catch
        //    {
        //        return false;
        //    }
        //}
    }
}
