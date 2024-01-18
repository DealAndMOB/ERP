using ERP.Complementos.jwt;
using ERP.Complementos.ProcesoBehind;
using ERP.Models;
using ERP.Vistas.Ventas;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ERP.Controllers
{
    public class RemisionesController : ApiController
    {
        Modelo bd = new Modelo();
        JsonRequest serializar = new JsonRequest();

        private AccesoPerfil accesoPerfil = new AccesoPerfil(Modulo.Ventas);
        private PeticionEstado respuesta = new PeticionEstado();

        [JWT]
        [ActionName("ComprobarFolio")]
        [HttpGet]
        public PeticionEstado ComprobarFolio(string id)
        {
            string FolioEvaluado = bd.Remision.Where(c => c.Folio == id).Select(c => c.Folio).FirstOrDefault();
            JSONwt usuarioJwt = JsonConvert.DeserializeObject<JSONwt>(Request.Properties["payload"].ToString());

            if (!accesoPerfil.ComprobarAcceso(usuarioJwt.uap))
            {
                respuesta.AlertaJS = "PerfilDenegado()";
                respuesta.Estado = false;
                return respuesta;
            }

            //¿El folio evaluado esta vacio?
            if (string.IsNullOrEmpty(FolioEvaluado))
            {
                respuesta.Estado = true;
                return respuesta;
            }
            else
            {
                respuesta.AlertaJS = "DobleConfirmacion()";
                respuesta.Estado = false;
                return respuesta;
            }
        }


        [JWT]
        [ActionName("ObtenerFolioFinal")]
        [HttpGet]
        public string ObtenerFolio()
        {
            var foliosDistintos = (from remision in bd.Remision
                                   select remision.Folio).ToList();

            if (!foliosDistintos.Any())
            {
                return "null";
            }

            var foliosYnumeros = foliosDistintos
                .Select(folio => (folio, int.Parse(new string(folio.Where(char.IsDigit).ToArray()))))
                .ToList();

            var folioFinal = foliosYnumeros
                .OrderByDescending(x => x.Item2)
                .First()
                .folio;

            return folioFinal;
        }

        [JWT]
        [ActionName("AgregarRemision")]
        [HttpPost]
        public bool AgregarRemision(clsRemision remision)
        {
            JSONwt usuarioJwt = JsonConvert.DeserializeObject<JSONwt>(Request.Properties["payload"].ToString());
            try
            {
                if (!accesoPerfil.ComprobarAcceso(usuarioJwt.uap))
                {
                    return false;
                }

                bd.Remision.Add(remision);
                bd.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        [JWT]
        [ActionName("MostrarRemisiones")]
        [HttpGet]
        public HttpResponseMessage MostrarRemisiones(string id)
        {
            IQueryable<RemisionDTO> Remision = from remisiones in bd.Remision
                                               where remisiones.FolioVenta == id
                                               select new RemisionDTO
                                               {
                                                   Folio = remisiones.Folio,
                                                   FolioVenta = remisiones.venta.Folio,
                                                   Cliente = remisiones.venta.cliente.Nombre,
                                                   FechaEntrega = remisiones.FechaEntrega
                                               };

            return serializar.CrearJsonRespuesta(Remision);
        }

        [JWT]
        [ActionName("BuscarRemision")]
        [HttpGet]
        public HttpResponseMessage BuscarRemision(string id, string folioVenta)
        {
            IQueryable<RemisionDTO> Remision = from remisiones in bd.Remision
                                               where remisiones.Folio.Contains(id) &&
                                                     remisiones.FolioVenta == folioVenta
                                               select new RemisionDTO
                                               {
                                                   Folio = remisiones.Folio,
                                                   FolioVenta = remisiones.venta.Folio,
                                                   Cliente = remisiones.venta.cliente.Nombre,
                                                   FechaEntrega = remisiones.FechaEntrega
                                               };

            return serializar.CrearJsonRespuesta(Remision);
        }

        [JWT]
        [HttpGet]
        [ActionName("ObtenerRemisionCompleta")]
        public HttpResponseMessage ObtenerRemisionCompleta(string id)
        {
            RemisionDTO remision = null;

            using (var bd = new Modelo())
            {
                remision = (from r in bd.Remision.Include("venta.cliente").Include("partidaCotizacionVenta.producto")
                            where r.Folio == id
                            select new RemisionDTO
                            {
                                Folio = r.Folio,
                                Direccion = r.venta.cliente.Direccion,
                                FolioVenta = r.FolioVenta,
                                Cliente = r.venta.cliente.Nombre,
                                FechaEntrega = r.FechaEntrega,
                                partidasRemision = (from pr in r.partidasRemision
                                                    select new PartidaRemisionDTO
                                                    {
                                                        ID = pr.ID,
                                                        Descripcion = pr.partidaCotizacionVenta.producto.Descripcion,
                                                        Imagen = pr.partidaCotizacionVenta.producto.Imagen,
                                                        Cantidad = pr.CantidadRemitida
                                                    }).ToList()
                            }).FirstOrDefault();
            }

            if (remision != null && remision.partidasRemision != null)
            {
                remision.partidasRemision.ForEach(p => p.Partida = remision.partidasRemision.IndexOf(p) + 1);
            }

            return serializar.CrearJsonRespuesta(remision);
        }

    }
}
