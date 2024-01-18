using ERP.Complementos.jwt;
using ERP.Complementos.ProcesoBehind;
using ERP.Models;
using HTTPupt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ERP
{
    public partial class Index : System.Web.UI.Page
    {
        PeticionHTTP peticion = new PeticionHTTP(Url.ObtenerUrlServidor());
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnIniciar_Click(object sender, EventArgs e)
        {
            InicioSesion datosInicio = new InicioSesion()
            {
                Correo = txtCorreo.Text.Trim(),
                Contraseña = txtContraseña.Text
            };

            String json = JsonConvertidor.Objeto_Json(datosInicio);
            peticion.PedirComunicacion("Usuarios/PedirToken", MetodoHTTP.POST, TipoContenido.JSON);
            peticion.enviarDatos(json);

            json = peticion.ObtenerJson();

            if (json != "null")
            {
                json = json.Substring(1, json.Length - 2);

                Session["jwt"] = json;

                peticion.PedirComunicacion("Usuarios/VerificarToken", MetodoHTTP.GET, TipoContenido.JSON, json);
                json = peticion.ObtenerJson();

                peticion.PedirComunicacion("Usuarios/ConsultarPorUsuario", MetodoHTTP.GET, TipoContenido.JSON, Session["jwt"].ToString());
                json = peticion.ObtenerJson();

                UsuarioDTO usuario = JsonConvertidor.Json_Objeto<UsuarioDTO>(json);

                Session["Inicio"] = true;
                Session["Accesos"] = usuario.Accesos;
                Session["Usuario"] = usuario.Nombre;

                Response.RedirectToRoute("Inicio");
            }
            else
            {
                ClientScript.RegisterStartupScript(GetType(), "script", "ErrorInicio()", true);
            }
        }



    }
}