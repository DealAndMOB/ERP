using ERP.Complementos.jwt;
using ERP.Complementos.ProcesoBehind;
using ERP.Models;
using ERP.Vistas.Catalogos;
using iTextSharp.text.pdf.codec.wmf;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ERP.Controllers
{
    public class ProveedoresController : ApiController
    {
        private Modelo bd = new Modelo();
        private AccesoPerfil accesoPerfil = new AccesoPerfil(Modulo.Catalogos);
        private PeticionEstado respuesta = new PeticionEstado();

        //Controoador para agregar un proveedor

        [JWT]
        [ActionName("AgregarProveedor")]
        [HttpPost]
        public PeticionEstado AgregarProveedor(clsProveedor proveedor)
        {
            JSONwt usuarioJwt = JsonConvert.DeserializeObject<JSONwt>(Request.Properties["payload"].ToString());
            bool coincidencias = bd.Proveedor.Any(p => p.RFC == proveedor.RFC);

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
                    respuesta.AlertaJS = "RFCProveedorDuplicado()";
                    respuesta.Estado = false;
                    return respuesta;
                }

                if (!(proveedor.RFC.Length >= 12 && proveedor.RFC.Length <= 13))
                {
                    respuesta.AlertaJS = "ErrorRFC()";
                    respuesta.Estado = false;
                    return respuesta;
                }

                if (!ValidarCaracteres.RFC(proveedor.RFC))
                {
                    respuesta.AlertaJS = "RFCinvalido()";
                    respuesta.Estado = false;
                    return respuesta;
                }

                if (proveedor.Telefono.Length != 10)
                {
                    respuesta.AlertaJS = "ErrorTelefono()";
                    respuesta.Estado = false;
                    return respuesta;
                }

                if (!(ValidarCaracteres.Numeros(proveedor.Telefono)))
                {
                    respuesta.AlertaJS = "TelefonoInvalido()";
                    respuesta.Estado = false;
                    return respuesta;
                }

                // - - - - - - - - - - - - - - -
                bd.Proveedor.Add(proveedor);
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
        [ActionName("BusquedaCombinada")]
        [HttpGet]
        public IQueryable<ProveedorDTO> BusquedaCombinada(int ZonaID, int CategoriaID)
        {
            IQueryable<ProveedorDTO> proveedorDTOs = from proveedor in bd.Proveedor
                                                     where proveedor.estado.zona.ID == ZonaID && proveedor.CategoriaProveedorID == CategoriaID
                                                     select new ProveedorDTO
                                                     {
                                                         ID = proveedor.ID,
                                                         RFC = proveedor.RFC,
                                                         Categoria = proveedor.CategoriaProveedor.Nombre,
                                                         Zona = proveedor.estado.zona.Zona,
                                                         Empresa = proveedor.Empresa,
                                                         RazonSocial = proveedor.RazonSocial,
                                                         NombreContacto = proveedor.NombreContacto,
                                                         Descripcion = proveedor.Descripcion,
                                                         CorreoPagina = proveedor.CorreoPagina,
                                                         Telefono = proveedor.Telefono,
                                                         Direccion = proveedor.Direccion
                                                     };
            return proveedorDTOs;
        }

        [JWT]
        [ActionName("BusquedaIndividual")]
        [HttpGet]
        public IQueryable<ProveedorDTO> BusquedaIndividual(int ZonaID, int CategoriaID)
        {
            IQueryable<ProveedorDTO> proveedorDTOs = null;

            if (!ZonaID.Equals(0))
            {
                proveedorDTOs = from proveedor in bd.Proveedor
                                where proveedor.estado.zona.ID == ZonaID
                                select new ProveedorDTO
                                {
                                    ID = proveedor.ID,
                                    RFC = proveedor.RFC,
                                    Categoria = proveedor.CategoriaProveedor.Nombre,
                                    Zona = proveedor.estado.zona.Zona,
                                    Empresa = proveedor.Empresa,
                                    RazonSocial = proveedor.RazonSocial,
                                    NombreContacto = proveedor.NombreContacto,
                                    Descripcion = proveedor.Descripcion,
                                    CorreoPagina = proveedor.CorreoPagina,
                                    Telefono = proveedor.Telefono,
                                    Direccion = proveedor.Direccion
                                };
            }
            else if (!CategoriaID.Equals(0))
            {
                proveedorDTOs = from proveedor in bd.Proveedor
                                where proveedor.CategoriaProveedorID == CategoriaID
                                select new ProveedorDTO
                                {
                                    ID = proveedor.ID,
                                    RFC = proveedor.RFC,
                                    Categoria = proveedor.CategoriaProveedor.Nombre,
                                    Zona = proveedor.estado.zona.Zona,
                                    Empresa = proveedor.Empresa,
                                    RazonSocial = proveedor.RazonSocial,
                                    NombreContacto = proveedor.NombreContacto,
                                    Descripcion = proveedor.Descripcion,
                                    CorreoPagina = proveedor.CorreoPagina,
                                    Telefono = proveedor.Telefono,
                                    Direccion = proveedor.Direccion
                                };
            }

            return proveedorDTOs;
        }


        [JWT]
        [ActionName("BusquedaPorCategoria")]
        [HttpGet]
        public IQueryable<ProveedorDTO> BusquedaPorCategoria(int ZonaID, int CategotiaID)
        {
            IQueryable<ProveedorDTO> proveedorDTOs = from proveedor in bd.Proveedor
                                                     where proveedor.estado.zona.ID == ZonaID && proveedor.CategoriaProveedorID == CategotiaID
                                                     select new ProveedorDTO
                                                     {
                                                         ID = proveedor.ID,
                                                         RFC = proveedor.RFC,
                                                         Categoria = proveedor.CategoriaProveedor.Nombre,
                                                         Zona = proveedor.estado.zona.Zona,
                                                         Empresa = proveedor.Empresa,
                                                         RazonSocial = proveedor.RazonSocial,
                                                         NombreContacto = proveedor.NombreContacto,
                                                         Descripcion = proveedor.Descripcion,
                                                         CorreoPagina = proveedor.CorreoPagina,
                                                         Telefono = proveedor.Telefono,
                                                         Direccion = proveedor.Direccion
                                                     };
            return proveedorDTOs;
        }

        [JWT]
        [ActionName("MostrarProveedores")]
        [HttpGet]
        public IQueryable<ProveedorDTO> Get()
        {
            IQueryable<ProveedorDTO> proveedorDTOs = from proveedor in bd.Proveedor
                                                     select new ProveedorDTO
                                                     {
                                                         ID = proveedor.ID,
                                                         RFC = proveedor.RFC,
                                                         Categoria = proveedor.CategoriaProveedor.Nombre,
                                                         Zona = proveedor.estado.zona.Zona,
                                                         Estado = proveedor.estado.Nombre,
                                                         Empresa = proveedor.Empresa,
                                                         RazonSocial = proveedor.RazonSocial,
                                                         NombreContacto = proveedor.NombreContacto,
                                                         Descripcion = proveedor.Descripcion,
                                                         CorreoPagina = proveedor.CorreoPagina,
                                                         Telefono = proveedor.Telefono,
                                                         Direccion = proveedor.Direccion
                                                     };
            return proveedorDTOs;
        }

        [JWT]
        [ActionName("BuscarProveedor")]
        [HttpGet]
        public IQueryable<ProveedorDTO> Buscar(String id)
        {
            IQueryable<ProveedorDTO> ProveedoresDTOs = from proveedor in bd.Proveedor
                                                       where proveedor.RFC.Contains(id)
                                                       select new ProveedorDTO
                                                       {
                                                           ID = proveedor.ID,
                                                           RFC = proveedor.RFC,
                                                           Empresa = proveedor.Empresa,
                                                           RazonSocial = proveedor.RazonSocial,
                                                           NombreContacto = proveedor.NombreContacto,
                                                           Descripcion = proveedor.Descripcion,
                                                           CorreoPagina = proveedor.CorreoPagina,
                                                           Telefono = proveedor.Telefono,
                                                           Direccion = proveedor.Direccion,
                                                           Categoria = proveedor.CategoriaProveedor.Nombre,
                                                           Zona = proveedor.estado.zona.Zona,

                                                           CategoriaID = proveedor.CategoriaProveedorID,
                                                           EstadoID = proveedor.EstadoID
                                                       };
            return ProveedoresDTOs;
        }

        [JWT]
        [ActionName("ActualizarProveedor")]
        [HttpPut]
        public PeticionEstado ActualizarCliente(clsProveedor proveedor)

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

                var proveedorExistente = bd.Proveedor.FirstOrDefault(p => p.ID == proveedor.ID);
                if (proveedorExistente != null && proveedor.RFC != proveedorExistente.RFC)
                {
                    bool coincidencias = bd.Proveedor.Any(p => p.RFC == proveedor.RFC);
                    if (coincidencias)
                    {
                        respuesta.AlertaJS = "RFCProveedorDuplicado()";
                        respuesta.Estado = false;
                        return respuesta;
                    }
                }

                if (!(proveedor.RFC.Length >= 12 && proveedor.RFC.Length <= 13))
                {
                    respuesta.AlertaJS = "ErrorRFC()";
                    respuesta.Estado = false;
                    return respuesta;
                }

                if (!ValidarCaracteres.RFC(proveedor.RFC))
                {
                    respuesta.AlertaJS = "RFCinvalido()";
                    respuesta.Estado = false;
                    return respuesta;
                }

                if (proveedor.Telefono.Length != 10)
                {
                    respuesta.AlertaJS = "ErrorTelefono()";
                    respuesta.Estado = false;
                    return respuesta;
                }

                if (!(ValidarCaracteres.Numeros(proveedor.Telefono)))
                {
                    respuesta.AlertaJS = "TelefonoInvalido()";
                    respuesta.Estado = false;
                    return respuesta;
                }

                bd.Entry(proveedorExistente).CurrentValues.SetValues(proveedor);
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
