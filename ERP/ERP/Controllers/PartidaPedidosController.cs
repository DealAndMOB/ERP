using ERP.Complementos.jwt;
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
    public class PartidaPedidosController : ApiController
    {
        Modelo bd = new Modelo();
        private AccesoPerfil accesoPerfil = new AccesoPerfil(Modulo.Compras);

        //-------Controlador para agregar productos a la base de datos-----------//
        [JWT]
        [ActionName("AgregarPartidas")]
        [HttpPost]
        public bool AgregarPedidos(List<clsPartidaPedido> partidas)
        {
            JSONwt usuarioJwt = JsonConvert.DeserializeObject<JSONwt>(Request.Properties["payload"].ToString());

            string folio = partidas.FirstOrDefault()?.Folio; // Obtener el folio de la primera partida
            //Compruebo si en esta ronda el folio ya esta registrado en la bd para no generar replicas
            string FolioEncontrado = bd.PartidaPedido.Where(c => c.Folio == folio).Select(c => c.Folio).FirstOrDefault();

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
                bd.PartidaPedido.Add(Subiendo);
            }

            bd.SaveChangesAsync();

            return true;
        }

        [JWT]
        [ActionName("ActualizarPartidas")]
        [HttpPut]
        public bool ActualizarPedidoCompleto(List<clsPartidaPedido> partidas)
        {
            JSONwt usuarioJwt = JsonConvert.DeserializeObject<JSONwt>(Request.Properties["payload"].ToString());

            try
            {
                if (!accesoPerfil.ComprobarAcceso(usuarioJwt.uap))
                {
                    return false;
                }

                foreach (var partida in partidas)
                {
                    bd.Entry(partida).State = System.Data.Entity.EntityState.Modified;
                }

                bd.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        //-------Controlador para eliminar cotizacion de la base de datos-----------//
        [JWT]
        [ActionName("BorrarPartidas")]
        [HttpDelete]
        public bool BorrarPartidas(List<int> partidasID)
        {
            JSONwt usuarioJwt = JsonConvert.DeserializeObject<JSONwt>(Request.Properties["payload"].ToString());

            try
            {
                if (!accesoPerfil.ComprobarAcceso(usuarioJwt.uap))
                {
                    return false;
                }

                foreach (var id in partidasID)
                {
                    clsPartidaPedido partida = bd.PartidaPedido.Find(id);
                    bd.Entry(partida).State = System.Data.Entity.EntityState.Deleted;
                }

                bd.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

    }
}
