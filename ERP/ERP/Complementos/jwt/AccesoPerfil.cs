using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERP.Complementos.jwt
{
    public enum Modulo
    {
        Compras,
        Ventas,
        Catalogos,
        Sistema
    }

    public class AccesoPerfil
    {
        private int indice;
        public AccesoPerfil(Modulo modulo)
        {
            indice = (int)modulo;
        }
        public bool ComprobarAcceso(string accesos)
        {
            return accesos[indice] == '1';
        }
    }
}