using ERP.Vistas.Ventas;
using Newtonsoft.Json;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Web;

namespace ERP.Complementos.ProcesoBehind
{
    public class JsonRequest
    {
        public HttpResponseMessage CrearJsonRespuesta(object data)
        {
            string json = JsonConvert.SerializeObject(data);
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = new StringContent(json, Encoding.UTF8, "application/json");

            return response;
        }
    }
}