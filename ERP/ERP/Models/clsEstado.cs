using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ERP.Models
{
    [Table("Estado")]
    public class clsEstado
    {
        [Key]
        public int ID { get; set; }

        public int ZonaID { get; set; }
        public virtual clsZona zona { get; set; }

        [Index("NombreUnico", IsUnique = true)]
        [StringLength(30)]
        public String Nombre { get; set; }
    }
    public class EstadoDTO
    {
        public int ID { get; set; }
        public String Zona { get; set; }
        public String Nombre { get; set; }
        public int ZonaID { get; set; }
    }
}