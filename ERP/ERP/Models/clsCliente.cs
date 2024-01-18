using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ERP.Models
{
    [Table("Cliente")]
    public class clsCliente
    {
        [Key]
        public int ID { get; set; }

        [StringLength(13)]
        [Index("RFCUnico", IsUnique = true)]
        public String RFC { get; set; }

        [StringLength(50)]
        public String Nombre { get; set; }
        [StringLength(100)]
        public String Direccion { get; set; }
        [StringLength(10)]
        public String Telefono { get; set; }
        [StringLength(50)]
        public String Correo { get; set; }
    }
    
    public class ClienteDTO
    {
        public int ID { get; set; }
        public String RFC { get; set; }
        public String Nombre { get; set; }
        public String Direccion { get; set; }
        public String Telefono { get; set; }
        public String Correo { get; set; }
    }
}