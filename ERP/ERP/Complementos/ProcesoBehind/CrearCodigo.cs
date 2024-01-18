using HTTPupt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace ERP.Complementos.ProcesoBehind
{
    public class CrearCodigo
    {
        PeticionHTTP peticion = new PeticionHTTP(Url.ObtenerUrlServidor());
        string folio, cadena;
        int contador;
        public string GenerarCodigo(string prefijo, string getFolio)
        {
            int longitud = (getFolio.Length - prefijo.Length);

            cadena = new string(getFolio.Where(char.IsDigit).ToArray());
            int numero = int.Parse(cadena);
            contador = numero + 1;
            cadena = contador.ToString();

            if (longitud > cadena.Length)
            {
                //En este punto se rellena el caracter sistematizadamente, siempre y cuando la longitud 
                //se mayor a las posiciones disponibles
                string formato = prefijo + new string('0', (longitud - cadena.Length));

                folio =
                    string.Format($"{formato}{contador}");
            }
            else if (longitud < cadena.Length || longitud == cadena.Length) // Se supero o alcanzó la longitud predeterminada
            {
                string formato =
                    string.Format($"{prefijo}{'0'}");

                folio =
                    string.Format($"{formato}{'0'}");
            }

            return folio;
        }

        //¡SOLO SIRVE PARA CREAR EL PRIMER FOLIO!
        public string GenerarCodigo(string prefijo, int posiciones)
        {
            cadena = prefijo + new string('0', (posiciones - 1));
            contador = 1;

            folio = string.Format($"{cadena}{contador}");
            return folio;
        }

        public Dictionary<string, string> ProcesarDatosProductos(DropDownList ddlCategoriaProducto, string JWT)
        {
            int categoriaID = int.Parse(ddlCategoriaProducto.SelectedValue);
            peticion.PedirComunicacion("CategoriaProductos/ObtenerCategoria/" + categoriaID, MetodoHTTP.GET, TipoContenido.JSON, JWT);
            string categoria = peticion.ObtenerJson(); //Categoria seleccionada

            if(categoria == null )
                return null;

            // Quito las comillas
            string nombreCategoria = categoria.Substring(1, (categoria.Length - 2));
            string prefijo = nombreCategoria.Substring(0, 3).ToUpper(); //Extraccion de las 3 letras en mayusculas

            // Quito las comillas
            peticion.PedirComunicacion("Productos/obtenerCodigoFinal/" + prefijo, MetodoHTTP.GET, TipoContenido.JSON, JWT);
            string codigo = peticion.ObtenerJson(); //último Código

            if (codigo == null)
                return null;

            string CodigoFinal = codigo.Substring(1, (codigo.Length - 2)); 

            Dictionary<string, string> datos = new Dictionary<string, string>()
            {
                {"Prefijo"        , prefijo},
                {"Codigo anterior" , CodigoFinal}
            };

            return datos;
        }

    }

}