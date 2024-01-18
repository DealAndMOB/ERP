using HTTPupt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace ERP.Complementos.ProcesoBehind
{
    public class Url
    {
        private static readonly string UrlServidor = "http://localhost:62663/";
        //private static readonly string UrlServidor = "http://localhost:62663/";

        private static readonly PeticionHTTP peticion = new PeticionHTTP(UrlServidor);
        public static string ObtenerUrlServidor()
        {
            return UrlServidor;
        }

        public static void CerrarSesion(Page page)
        {
            string logoutUrl = $"{UrlServidor}BorrarJWT";

            string script = $"SesionExpirada(); setTimeout(function() {{window.location.href='{logoutUrl}';}}, 2000);";

            // Registro del script utilizando HttpContext
            if (HttpContext.Current != null && HttpContext.Current.Handler is Page currentPage)
            {
                ScriptManager.RegisterStartupScript(currentPage, currentPage.GetType(), "SesionExpirada1", script, true);
            }
            else
            {
                // Si no se encuentra el HttpContext o no es una página válida, utiliza RegisterStartupScript directamente en la página
                page.ClientScript.RegisterStartupScript(page.GetType(), "SesionExpirada2", script, true);
            }
        }

        public static string ActualizarJWT(string jwt)
        {
            peticion.PedirComunicacion("Usuarios/ActualizarJWT", MetodoHTTP.GET, TipoContenido.JSON, jwt);
            string newJwt = peticion.ObtenerJson();

            if (newJwt != null)
                return newJwt.Substring(1, newJwt.Length - 2);

            return null;
        }
    }
}