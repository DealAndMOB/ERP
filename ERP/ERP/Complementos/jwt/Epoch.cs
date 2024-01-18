using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERP.Complementos.jwt
{
    public class Epoch
    {
        //Obtengo los segundos totales
        public double convertirEpoch(DateTime fecha)
        {
            DateTime fechaInicial = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            double epoch = Math.Round((fecha.ToUniversalTime() - fechaInicial).TotalSeconds, 0);

            return epoch;
        }

        //Obtengo la fecha concreta a la que hace referencia el epoch
        public DateTime convertirFecha(double epoch)
        {
            DateTime fechaInicial = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            fechaInicial = fechaInicial.AddSeconds(epoch).ToLocalTime();

            return fechaInicial;
        }
    }
}