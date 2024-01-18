using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ERP.Models
{
    [Table("Remision")]
    public class clsRemision
    {
        [Key]
        public String Folio { get; set; }

        [ForeignKey("venta")]
        public String FolioVenta { get; set; }
        public virtual clsCotizacionVenta venta { get; set; }

        public DateTime FechaEntrega { get; set; }

        public virtual List<clsPartidaRemision> partidasRemision { get; set; }
    }

    [Serializable]
    public class RemisionDTO
    {
        public String Folio { get; set; }
        public String Direccion { get; set; }
        public String FolioVenta { get; set; }
        public String Cliente { get; set; }
        public DateTime FechaEntrega { get; set; }
        public String FechaFormato { get; set; }
        public virtual List<PartidaRemisionDTO> partidasRemision { get; set; }

        public string FechaAlterada = string.Empty;
    }
}