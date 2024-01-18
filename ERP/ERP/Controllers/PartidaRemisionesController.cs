using ERP.Complementos.jwt;
using ERP.Complementos.ProcesoBehind;
using ERP.Models;
using ERP.Vistas.Ventas;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace ERP.Controllers
{
    public class PartidaRemisionesController : ApiController
    {
        Modelo bd = new Modelo();
        JsonRequest serializar = new JsonRequest();
        private AccesoPerfil accesoPerfil = new AccesoPerfil(Modulo.Ventas);

        [JWT]
        [ActionName("AgregarPartidas")]
        [HttpPost]
        public bool AgregarCotizaciones(List<clsPartidaRemision> partidas)
        {
            JSONwt usuarioJwt = JsonConvert.DeserializeObject<JSONwt>(Request.Properties["payload"].ToString());

            string folio = partidas.FirstOrDefault()?.Folio; // Obtener el folio de la primera partida
            //Compruebo si en esta ronda el folio ya esta registrado en la bd para no generar replicas
            string FolioEncontrado = bd.PartidaRemision.Where(c => c.Folio == folio).Select(c => c.Folio).FirstOrDefault();

            if (!accesoPerfil.ComprobarAcceso(usuarioJwt.uap))
            {
                return false;
            }

            if (!string.IsNullOrEmpty(FolioEncontrado)) // Si la cadena esta llena se detiene el insert a la bd en esta ronda
            {
                return false;
            }

            foreach (var Subiendo in partidas)
            {
                bd.PartidaRemision.Add(Subiendo);
            }

            bd.SaveChangesAsync();
            return true;
        }

        [JWT]
        [ActionName("TotalesPorRemision")]
        [HttpGet]
        public async Task<HttpResponseMessage> TotalesPorRemision(string id)
        {
            // Lista de coincidencias de PartidaCotizacionVenta.Folio == id
            // Obtener los registros que deseas actualizar
            var partidasCotizacionVenta = bd.PartidaCotizacionVenta.Where(r => r.Folio == id);

            List<PartidaRemisionDTO> partidasRemision = (from pcv in bd.PartidaCotizacionVenta
                                                         where pcv.Folio == id && 
                                                         pcv.cotizacionVenta.Estado == true && pcv.Estado == false
                                                         select new PartidaRemisionDTO
                                                         {
                                                             PartidaVentaID = pcv.ID, // --> ID de la partida de la cotización
                                                             Nombre = pcv.producto.Nombre,
                                                             Categoria = pcv.producto.CategoriaProducto.Nombre,
                                                             Imagen = pcv.producto.Imagen,
                                                             Cantidad = pcv.Unidades
                                                         }).ToList();

            if (partidasRemision != null)
            {
                partidasRemision.ForEach(p => p.Partida = partidasRemision.IndexOf(p) + 1);
            }

            foreach (var partida in partidasRemision)
            {
                var partidas = await bd.PartidaRemision.Where(pr => pr.PartidaCotizacionVentaID == partida.PartidaVentaID).ToListAsync();

                int remitidosTotales = partidas.Sum(pr => pr.CantidadRemitida);
                partida.Restantes = (partida.Cantidad - remitidosTotales);
            }

            List<int> VTASaldadasID = partidasRemision.Where(pr => pr.Restantes == 0).Select(pr => pr.PartidaVentaID).ToList();
           

            for (int i = 0; i < VTASaldadasID.Count; i++)
            {
                foreach (var partidaSaldada in partidasCotizacionVenta)
                {
                    if (partidaSaldada.ID == VTASaldadasID[i])
                    {
                        partidaSaldada.Estado = true;
                    }
                }
            }

            bd.SaveChanges();
            return serializar.CrearJsonRespuesta(partidasRemision);
        }


    }
}
