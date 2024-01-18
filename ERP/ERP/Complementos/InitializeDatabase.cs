using ERP.Complementos.ProcesoBehind;
using ERP.Models;
using ERP.Vistas.AdminSistema;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ERP.Complementos
{
    public static class InitializeDatabase
    {
        private static Modelo bd = new Modelo();

        public static void IniciarBD()
        {
            SubirPerfil();
            SubirUsuario();
            SubirZona();
            SubirEstado();
        }

        private static void SubirPerfil()
        {
            var coincidencia = bd.Perfil.FirstOrDefault(c => c.Nombre == "Super Admin");

            clsPerfil perfil = new clsPerfil()
            {
                Nombre = "Super Admin",
                Accesos = "1111"
            };

            try
            {
                if (coincidencia != null) { return; }

                bd.Perfil.Add(perfil);
                bd.SaveChanges();
            }
            catch
            {
                return;
            }
        }

        private static void SubirUsuario()
        {
            int perfilID = bd.Perfil.Where(c => c.Nombre == "Super Admin").Select(c => c.ID).FirstOrDefault();
            var coincidencia = bd.Usuario.FirstOrDefault(c => c.perfil.Nombre == "Super Admin");

            clsUsuario UserSuperAdmin = new clsUsuario()
            {
                PerfilID = perfilID,
                Nombre = "Usuario Super Admin",
                Correo = "correoSuperAdmin@gmail.com",
                Contraseña = "AGCcomercial"
            };

            try
            {
                if (coincidencia != null) { return; }

                bd.Usuario.Add(UserSuperAdmin);
                bd.SaveChanges();
            }
            catch
            {
                return;
            }
        }

        private static void SubirZona()
        {
            List<string> zonas = new List<string> { "Noroeste", "Noreste", "Oeste", "Oriente", "CentroNorte", "CentroSur", "Suroeste", "Sureste" };

            try
            {
                foreach (string zonaNombre in zonas)
                {
                    var coincidencia = bd.Zona.FirstOrDefault(z => z.Zona == zonaNombre);

                    if (coincidencia == null)
                    {
                        clsZona zona = new clsZona()
                        {
                            Zona = zonaNombre
                        };

                        bd.Zona.Add(zona);
                        bd.SaveChanges();
                    }
                }
            }
            catch
            {
                return;
            }
        }

        private static void SubirEstado()
        {
            Dictionary<string, List<string>> estadosPorZona = new Dictionary<string, List<string>>
            {
                { "Noroeste"    , new List<string> { "Baja California Sur", "Baja California", "Chihuahua", "Durango", "Sinaloa", "Sonora" } },
                { "Noreste"     , new List<string> { "Coahuila", "Nuevo León", "Tamaulipas" } },
                { "Oeste"       , new List<string> { "Colima", "Jalisco", "Michoacán", "Nayarit" } },
                { "Oriente"     , new List<string> { "Hidalgo", "Puebla", "Tlaxcala", "Veracruz" } },
                { "CentroNorte" , new List<string> { "Aguascalientes", "Guanajuato", "Querétaro", "San Luis Potosí", "Zacatecas" } },
                { "CentroSur"   , new List<string> { "CDMX", "EDOMEX", "Morelos" } },
                { "Suroeste"    , new List<string> { "Chiapas", "Guerrero", "Oaxaca" } },
                { "Sureste"     , new List<string> { "Campeche", "Quintana Roo", "Tabasco", "Yucatán" } }
            };

            try
            {
                foreach (var zonaEstado in estadosPorZona)
                {
                    var zona = bd.Zona.FirstOrDefault(z => z.Zona == zonaEstado.Key);
                    if (zona == null)
                        continue;

                    foreach (string estadoNombre in zonaEstado.Value)
                    {
                        var coincidencia = bd.Estado.FirstOrDefault(e => e.Nombre == estadoNombre);

                        if (coincidencia == null)
                        {
                            clsEstado estado = new clsEstado()
                            {
                                Nombre = estadoNombre,
                                ZonaID = zona.ID,
                                zona = zona
                            };

                            bd.Estado.Add(estado);
                            bd.SaveChanges();
                        }
                    }
                }
            }
            catch
            {
                return;
            }
        }

    }
}