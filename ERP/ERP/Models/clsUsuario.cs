using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ERP.Models
{
    [Table("Usuario")]
    public class clsUsuario
    {
        [Key]
        public int ID { get; set; }

        public int PerfilID { get; set; }
        public virtual clsPerfil perfil { get; set; }

        [StringLength(50)]
        public String Nombre { get; set; }
        [StringLength(50)]
        public String Correo { get; set; }
        [StringLength(50)]
        public String Contraseña { get; set; }
    }

    public class UsuarioDTO
    {
        public int ID { get; set; }
        public String Nombre { get; set; }
        public String Correo { get; set; }
        public String Contraseña { get; set; }
        public string Accesos { get; set; }
        public int PerfilID { get; set; }
        public String Perfil { get; set; }
    }

}