using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace ERP.Complementos.ProcesoBehind
{
    public static class ValidarCaracteres
    {
        public static bool RFC(string RFC)
        {
            string PatronRFC = @"^([A-ZÑ&]{3,4})(\d{2})(0[1-9]|1[0-2])(0[1-9]|[1-2]\d|3[01])([A-Z\d]{2})([A\d])$";
            Regex regex = new Regex(PatronRFC);
            return regex.IsMatch(RFC);
        }
        
        public static bool Correo(string correo)
        {
            string PatronCorreo = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            Regex regex = new Regex(PatronCorreo);
            return regex.IsMatch(correo);
        }
        public static bool Letras(string texto)
        {
            string patron = @"^[A-Za-záéíóúÁÉÍÓÚüÜñÑ\s]*$";
            Regex regex = new Regex(patron);
            return regex.IsMatch(texto);
        }

        public static bool Numeros(string texto)
        {
            string patron = "^[0-9]*$";
            Regex regex = new Regex(patron);
            return regex.IsMatch(texto);
        }

        public static bool NumerosYLetras(string texto)
        {
            string patron = @"^[A-Za-z0-9áéíóúÁÉÍÓÚüÜñÑ\s]+$";
            Regex regex = new Regex(patron);
            return regex.IsMatch(texto);
        }
    }
}