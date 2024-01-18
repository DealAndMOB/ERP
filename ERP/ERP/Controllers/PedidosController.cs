using ERP.Complementos.jwt;
using ERP.Complementos.ProcesoBehind;
using ERP.Models;
using ERP.Vistas.Compras;
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
    public class PedidosController : ApiController
    {
        private Modelo bd = new Modelo();
        private JsonRequest serializar = new JsonRequest();

        private PeticionEstado respuesta = new PeticionEstado();
        private AccesoPerfil accesoPerfil = new AccesoPerfil(Modulo.Compras);

        [JWT]
        [ActionName("ComprobarFolio")]
        [HttpGet]
        public PeticionEstado ComprobarFolio(string id)
        {
            string FolioEvaluado = bd.Pedido.Where(c => c.Folio == id).Select(c => c.Folio).FirstOrDefault();
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

        // controladores para agregar pedidos
        [JWT]
        [ActionName("AgregarPedido")]
        [HttpPost]
        public bool AgregarPedido(clsPedido pedido)
        {
            JSONwt usuarioJwt = JsonConvert.DeserializeObject<JSONwt>(Request.Properties["payload"].ToString());
            var pedidosPrevios = bd.Pedido.Where(p => p.Folio == pedido.Folio); // Buscar partidas con el mismo folio

            try
            {
                if (!accesoPerfil.ComprobarAcceso(usuarioJwt.uap))
                {
                    return false;
                }

                if (pedidosPrevios.Any()) // Si hay cotizaciones previas con el mismo folio, no agregar las nuevas partidas
                {
                    return false;
                }

                bd.Pedido.Add(pedido);
                bd.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        [JWT]
        [HttpGet]
        [ActionName("ObtenerFolioFinal")]
        public string ObtenerFolio()
        {
            var foliosDistintos = (from pedido in bd.Pedido
                                   select pedido.Folio).ToList();

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
        [ActionName("BuscarPedido")]
        [HttpGet]
        public HttpResponseMessage Buscar(String id)
        {
            IQueryable<PedidoDTO> PedidosDTO = from pedido in bd.Pedido
                                               where pedido.Folio.Contains(id)
                                               select new PedidoDTO
                                               {
                                                   Folio = pedido.Folio,
                                                   Proveedor = pedido.proveedor.Empresa,
                                                   Total = pedido.Total,
                                                   Fecha = pedido.Fecha,
                                                   Estatus = pedido.Estado
                                               };

            return serializar.CrearJsonRespuesta(PedidosDTO);
        }

        [JWT]
        [ActionName("MostrarPedido")]
        [HttpGet]
        public HttpResponseMessage Get(bool id)
        {
            IQueryable<PedidoDTO> Pedido = from pedidos in bd.Pedido
                                           where pedidos.Estado == id
                                           select new PedidoDTO
                                           {
                                               Folio = pedidos.Folio,
                                               Proveedor = pedidos.proveedor.Empresa,
                                               Total = pedidos.Total,
                                               Fecha = pedidos.Fecha,
                                               Estatus = pedidos.Estado
                                           };

            return serializar.CrearJsonRespuesta(Pedido);
        }

        [JWT]
        [HttpGet]
        [ActionName("ObtenerPedidoCompleto")]
        public HttpResponseMessage ObtenerPedido(string id)
        {
            PedidoDTO pedido = null;

            using (var bd = new Modelo())
            {
                pedido = (from c in bd.Pedido.Include("proveedor").Include("partidaPedido.producto")
                          where c.Folio == id
                          select new PedidoDTO
                          {
                              Folio = c.Folio,
                              ProveedorID = c.ProveedorID,
                              Proveedor = c.proveedor.Empresa,
                              Contacto = c.proveedor.NombreContacto,
                              RFC = c.proveedor.RFC,
                              Direccion = c.proveedor.Direccion,
                              Telefono = c.proveedor.Telefono,
                              Email = c.proveedor.CorreoPagina,
                              Total = c.Total,
                              Fecha = c.Fecha,
                              Condiciones = c.Condiciones,
                              Estatus = c.Estado,
                              Partidas = (from p in c.partidaPedido
                                          select new PartidaPedidoDTO
                                          {
                                              ID = p.ID,
                                              Folio = p.Folio,
                                              Nombre = p.producto.Nombre,
                                              ProductoID = p.producto.ID,
                                              CodigoProducto = p.producto.Codigo,
                                              Categoria = p.producto.CategoriaProducto.Nombre,
                                              Costo = p.CostoUnitario,
                                              Cantidad = p.Unidades,
                                              Total = p.TotalPartida,
                                              Imagen = p.producto.Imagen,
                                              DescripProducto = p.producto.Descripcion
                                          }).ToList()
                          }).FirstOrDefault();
            }

            if (pedido != null && pedido.Partidas != null)
            {
                pedido.Partidas.ForEach(p => p.Partida = pedido.Partidas.IndexOf(p) + 1);
            }

            return serializar.CrearJsonRespuesta(pedido);
        }

        [JWT]
        [ActionName("ActualizarPedido")]
        [HttpPut]
        public bool ActualizarPedido(clsPedido pedido)
        {
            JSONwt usuarioJwt = JsonConvert.DeserializeObject<JSONwt>(Request.Properties["payload"].ToString());

            try
            {
                if (!accesoPerfil.ComprobarAcceso(usuarioJwt.uap))
                {
                    return false;
                }

                bd.Entry(pedido).State = System.Data.Entity.EntityState.Modified;
                bd.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        [JWT]
        [ActionName("ActualizarCondiciones")]
        [HttpGet]
        public bool ActualizarCondiciones(string id, string condiciones)
        {
            JSONwt usuarioJwt = JsonConvert.DeserializeObject<JSONwt>(Request.Properties["payload"].ToString());
            var pedido = bd.Pedido.Where(r => r.Folio == id).FirstOrDefault();

            try
            {
                if (!accesoPerfil.ComprobarAcceso(usuarioJwt.uap))
                {
                    return false;
                }

                if (pedido != null)
                {
                    pedido.Condiciones = condiciones;
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

    }
}
