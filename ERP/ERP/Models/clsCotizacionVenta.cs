using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ERP.Models
{
    [Table("CotizacionVenta")]
    public class clsCotizacionVenta
    {
        [Key]
        public String Folio { get; set; } // --> PK

        public int ClienteID { get; set; }
        public virtual clsCliente cliente { get; set; }
        
        public String Condiciones { get; set; }
        public float Total { get; set; }
        public bool Estado { get; set; }

        public DateTime FechaCotizacion { get; set; }
        public DateTime ?FechaVenta { get; set; }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public virtual List<clsPartidaCotizacionVenta> partidasCotizacionVenta { get; set; }
        public virtual List<clsRemision> Remisiones { get; set; }
    }

    [Serializable]
    public class CotizacionDTO
    {
        public int ClienteID { get; set; }
        // Campos BD- - - - - - - - - - - - - - -
        public String Folio { get; set; }
        public String RFC { get; set; }
        public float Total { get; set; }
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public DateTime FechaCotizacion { get; set; }
        public DateTime FechaVenta { get; set; }
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public String Condiciones { get; set; }
        public bool Estatus { get; set; }

        // Campos Interfaz- - - - - - - - - - - - - - -
        public String Cliente { get; set; }
        public String Direccion { get; set; }
        public float SubTotal { get; set; }
        public float DiferenciaIVA { get; set; }
        public List<PartidaCotizacionDTO> Partidas { get; set; }

        // Campos de salida - - - - - - - - - - - - - - -
        public string TotalFormato { get; set; }
        public string FechaCorta { get; set; }

        public string FechaAlterada = string.Empty;
    }
}