using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ERP.Models
{
    [Table("PartidaRemision")]
    public class clsPartidaRemision
    {
        [Key]
        public int ID { get; set; }
        public String Folio { get; set; }
        public virtual clsRemision remision { get; set; }

        public int PartidaCotizacionVentaID { get; set; }
        public virtual clsPartidaCotizacionVenta partidaCotizacionVenta { get; set; }

        public int CantidadRemitida { get; set; }
    }

    [Serializable]
    public class PartidaRemisionDTO
    {
        public int ID { get; set; }
        public int PartidaVentaID { get; set; } // ID de la partida que esta vículada a la CotVta
        public int Partida { get; set; }
        public string Descripcion { get; set; }
        public string Imagen { get; set; }
        public string Nombre { get; set; }
        public int Cantidad { get; set; } // - - > Unidades Vendidas
        public string Categoria { get; set; }

        // Unidades de remisión - - - - - - - - - - - - - - - - - -
        public int Restantes { get; set; } // (Cantidad de unidades vendidas - Cantidad de unidades remitidas)
    }
}