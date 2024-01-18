using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ERP.Models
{
    [Table("Perfil")]
    public class clsPerfil
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [StringLength(50)]
        //[Index("NombreUnico", IsUnique = true)]
        public String Nombre { get; set; }

        [StringLength(4)]
        public String Accesos { get; set; }
    }
    
    public class PerfilDTO
    {
        public int ID { get; set; }
        public String Nombre { get; set; }
    }
}