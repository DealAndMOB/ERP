using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace ERP.Complementos.jwt
{
    public class JWT
    {//Darto encriptado
        private string jwt = "";
        //Diccionario de obbjetos con una key = string y un value = Object
        private Dictionary<string, object> headers = new Dictionary<string, object>()
            {
                 { "typ", "JWT" },
                 { "alg", "HS256" },
            };

        //private byte[] secretKey = Encoding.ASCII.GetBytes("jwt-&hall$2020#");
        private byte[] secretKey = Encoding.UTF8.GetBytes("&//jwt-My#Super#Secret#Key//&");

        // Métodos ------------------------------------------------------------------------------------------

        //Método get para obtener el valor de mi llave secreta
        public byte[] GetSecretKey()
        {
            return secretKey;
        }

        //Metodo get para obtener el dato encriptado, además se le agrega un encabezado extra 
        public string GetJWT(Dictionary<string, object> CargaUtil)
        {
            jwt += Jose.JWT.Encode(CargaUtil, secretKey, Jose.JwsAlgorithm.HS256, extraHeaders: headers);
            return jwt;
        }

        public String GetPayLoad(String token)
        {
            var h = Jose.JWT.Decode(token, secretKey, Jose.JwsAlgorithm.HS256);
            return h;
        }
    }
}