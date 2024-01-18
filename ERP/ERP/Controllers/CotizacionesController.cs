using ERP.Complementos.jwt;
using ERP.Complementos.ProcesoBehind;
using ERP.Models;
using ERP.Vistas.Ventas;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;

namespace ERP.Controllers
{
    public class CotizacionesController : ApiController
    {
        private Modelo bd = new Modelo();
        private JsonRequest serializar = new JsonRequest();

        private PeticionEstado respuesta = new PeticionEstado();
        private AccesoPerfil accesoPerfil = new AccesoPerfil(Modulo.Ventas);

        [JWT]
        [ActionName("ComprobarFolio")]
        [HttpGet]
        public PeticionEstado ComprobarFolio(string id)
        {
            string FolioEvaluado = bd.CotizacionVenta.Where(c => c.Folio == id).Select(c => c.Folio).FirstOrDefault();
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

        // controladores para agregar cotizacion
        [JWT]
        [ActionName("AgregarCotizacion")]
        [HttpPost]
        public bool AgregarCotizacion(clsCotizacionVenta cotizacionVenta)
        {
            JSONwt usuarioJwt = JsonConvert.DeserializeObject<JSONwt>(Request.Properties["payload"].ToString());
            var cotizacionesPrevias = bd.CotizacionVenta.Where(p => p.Folio == cotizacionVenta.Folio); // Buscar partidas con el mismo folio

            try
            {
                if (!accesoPerfil.ComprobarAcceso(usuarioJwt.uap))
                {
                    return false;
                }

                if (cotizacionesPrevias.Any()) // Si hay cotizaciones previas con el mismo folio, no agregar las nuevas partidas
                {
                    return false;
                }

                bd.CotizacionVenta.Add(cotizacionVenta);
                bd.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        [JWT]
        [ActionName("ObtenerFolioFinal")]
        [HttpGet]
        public string ObtenerFolio()
        {
            var foliosDistintos = (from cotizacion in bd.CotizacionVenta
                                   select cotizacion.Folio).ToList();

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

        //controlador para mostrar una cotizacion
        [JWT]
        [ActionName("MostrarCotizaciones")]
        [HttpGet]
        public HttpResponseMessage MostrarCotizaciones(bool id)
        {
            var cotizaciones = from cotizacion in bd.CotizacionVenta
                               where cotizacion.Estado == id
                               select new CotizacionDTO
                               {
                                   Folio = cotizacion.Folio,
                                   Cliente = cotizacion.cliente.Nombre,
                                   Total = cotizacion.Total,
                                   FechaCotizacion = cotizacion.FechaCotizacion,
                                   FechaVenta = cotizacion.FechaVenta ?? DateTime.MinValue,
                                   Estatus = cotizacion.Estado
                               };

            return serializar.CrearJsonRespuesta(cotizaciones);
        }

        [JWT]
        [ActionName("BuscarFolioCotizacion")]
        [HttpGet]
        public HttpResponseMessage Buscar(String id)
        {
            var cotizaciones = from cotizacion in bd.CotizacionVenta
                               where cotizacion.Folio.Contains(id)
                               select new CotizacionDTO
                               {
                                   Folio = cotizacion.Folio,
                                   Cliente = cotizacion.cliente.Nombre,
                                   Total = cotizacion.Total,
                                   FechaCotizacion = cotizacion.FechaCotizacion,
                                   FechaVenta = cotizacion.FechaVenta ?? DateTime.MinValue,
                                   Estatus = cotizacion.Estado
                               };

            return serializar.CrearJsonRespuesta(cotizaciones);
        }

        [JWT]
        [ActionName("ObtenerCotizacionCompleta")]
        [HttpGet]
        public HttpResponseMessage MostrarCotizaciones(string id)
        {
            CotizacionDTO cotizacion = null;

            using (var bd = new Modelo())
            {
                cotizacion = (from c in bd.CotizacionVenta.Include("cliente").Include("partidaCotizacion.producto")
                              where c.Folio == id
                              select new CotizacionDTO
                              {
                                  Folio = c.Folio,
                                  ClienteID = c.ClienteID,
                                  Cliente = c.cliente.Nombre,
                                  Direccion = c.cliente.Direccion,
                                  RFC = c.cliente.RFC,
                                  Total = c.Total,
                                  FechaCotizacion = c.FechaCotizacion,
                                  FechaVenta = c.FechaVenta ?? DateTime.MinValue,
                                  Condiciones = c.Condiciones,
                                  Estatus = c.Estado,
                                  Partidas = (from p in c.partidasCotizacionVenta
                                              select new PartidaCotizacionDTO
                                              {
                                                  ID = p.ID,
                                                  Folio = p.Folio,
                                                  CodigoProducto = p.producto.Codigo,
                                                  NombreProducto = p.producto.Nombre,
                                                  ProductoID = p.producto.ID,
                                                  Categoria = p.producto.CategoriaProducto.Nombre,
                                                  Cantidad = p.Unidades,
                                                  Total = p.TotalPartida,
                                                  Imagen = p.producto.Imagen,
                                                  DescripProducto = p.producto.Descripcion,
                                                  Estado = p.Estado,
                                                  // - - - - - - - - - - - - - - - - - - - - - - - - 
                                                  Costo = p.CostoCapturado,
                                                  Precio = p.PrecioUnitario,
                                                  PrecioBase = p.producto.Precio,
                                                  CriterioAumento = p.CriterioAumento,
                                                  PorcentajeAumento = p.PorcentajeAumento ?? 0
                                              }).ToList()
                              }).FirstOrDefault();
            }

            if (cotizacion != null && cotizacion.Partidas != null)
            {
                cotizacion.Partidas.ForEach(p => p.Partida = cotizacion.Partidas.IndexOf(p) + 1);
            }

            return serializar.CrearJsonRespuesta(cotizacion);
        }

        [JWT]
        [ActionName("ActualizarCondiciones")]
        [HttpGet]
        public bool ActualizarCondiciones(string id, string condiciones)
        {
            JSONwt usuarioJwt = JsonConvert.DeserializeObject<JSONwt>(Request.Properties["payload"].ToString());
            var cotizacion = bd.CotizacionVenta.Where(r => r.Folio == id).FirstOrDefault();

            try
            {
                if (!accesoPerfil.ComprobarAcceso(usuarioJwt.uap))
                {
                    return false;
                }

                if (cotizacion != null)
                {
                    cotizacion.Condiciones = condiciones;
                    bd.SaveChangesAsync();

                    return true;
                }

                return false;
            }
            catch
            {
                return false;
            }
        }

        //-------Controlador para Actualizar cotizacion de la base de datos-----------//
        [JWT]
        [ActionName("ActualizarCotizacion")]
        [HttpPut]

        public bool ActualizarCotizacion(clsCotizacionVenta cotizacion)
        {
            JSONwt usuarioJwt = JsonConvert.DeserializeObject<JSONwt>(Request.Properties["payload"].ToString());
            try
            {
                if (!accesoPerfil.ComprobarAcceso(usuarioJwt.uap))
                {
                    return false;
                }

                bd.Entry(cotizacion).State = System.Data.Entity.EntityState.Modified;
                bd.SaveChangesAsync();

                return true;
            }
            catch
            {
                return false;
            }
        }

        //-------Controlador para eliminar cotizacion de la base de datos-----------//

        //[JWT]
        //[ActionName("BorrarCotizacion")]
        //[HttpDelete]

        //public bool BorrarCotizacion(int id /*String folio*/)
        //{
        //    try
        //    {
        //        clsCotizacionVenta cotizacion = bd.CotizacionVenta.Find(id);
        //        bd.Entry(cotizacion).State = System.Data.Entity.EntityState.Deleted;
        //        bd.SaveChanges();
        //        return true;
        //    }
        //    catch
        //    {
        //        return false;
        //    }
        //}

        [JWT]
        [ActionName("ObtenerPartidas")]
        [HttpGet]
        public HttpResponseMessage ObtenerPartidas(string id)
        {
            List<DatosReportePartidas> partidasVenta = (from partidas in bd.PartidaCotizacionVenta
                                                        where partidas.Folio == id
                                                        select new DatosReportePartidas
                                                        {
                                                            Folio = partidas.Folio,
                                                            NombreProducto = partidas.producto.Nombre,
                                                            // - - - - - - - - - - - - - - - - - - - - - - 
                                                            Costo = partidas.CostoCapturado,
                                                            PrecioFinal = partidas.PrecioUnitario,
                                                            PorcentajeAumento = partidas.PorcentajeAumento ?? 0,
                                                            // - - - - - - - - - - - - - - - - - - - - - - 
                                                            CriterioAumento = partidas.CriterioAumento,
                                                            Unidades = partidas.Unidades
                                                        }).ToList();

            partidasVenta.ForEach(p => p.Partida = partidasVenta.IndexOf(p) + 1);

            partidasVenta.ForEach(p =>
            {
                p.TotalPartidaCosto = (p.Costo * p.Unidades);
                p.TotalPartidaCostoFormato = (p.TotalPartidaCosto).ToString("C2", new CultureInfo("es-MX"));

                p.TotalPartidaVenta = (p.PrecioFinal * p.Unidades);
                p.TotalPartidaVentaFormato = (p.TotalPartidaVenta).ToString("C2", new CultureInfo("es-MX"));

                p.MargenBruto = (p.TotalPartidaVenta - p.TotalPartidaCosto).ToString("C2", new CultureInfo("es-MX"));

                if (p.CriterioAumento != null)
                {
                    p.precioBase = p.PrecioFinal / (1 + (p.PorcentajeAumento / 100));
                    p.MontoAumento = ((p.precioBase * p.PorcentajeAumento) / 100).ToString("C2", new CultureInfo("es-MX"));
                }
            });

            return serializar.CrearJsonRespuesta(partidasVenta);
        }

    }
}
