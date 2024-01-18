using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ERP.Models
{
    [Table("Zona")]
    public class clsZona
    {
        [Key]
        public int ID { get; set; }

        [Index("NombreUnico", IsUnique = true)]
        [StringLength(30)]
        public String Zona { get; set; }
    }
    public class ZonaDTO
    {
        public int ID { get; set; }
        public String Zona { get; set; }
    }
}