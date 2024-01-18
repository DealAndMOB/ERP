using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ERP.Models
{
    [Table("Pedido")]
    public class clsPedido
    {
        [Key]
        [StringLength(25)]
        public String Folio { get; set; }

        [Required]
        public int ProveedorID { get; set; }
        public virtual clsProveedor proveedor { get; set; }
        public float Total { get; set; }
        public DateTime Fecha { get; set; }

        public String Condiciones { get; set; }
        public bool Estado { get; set; }

        public virtual List<clsPartidaPedido> partidaPedido { get; set; }
    }


    [Serializable]
    public class PedidoDTO
    {
        public int ProveedorID { get; set; }

        // Campos BD- - - - - - - - - - - - - - -
        public String Folio { get; set; }
        public String RFC { get; set; }
        public String Telefono { get; set; }
        public String RazonSocial { get; set; }
        public String Email { get; set; }

        public float Total { get; set; }
        public DateTime Fecha { get; set; }
        public String Condiciones { get; set; }
        public bool Estatus { get; set; }

        // Campos Interfaz- - - - - - - - - - - - - - -
        public String Proveedor { get; set; }
        public String Contacto { get; set; }
        public String Direccion { get; set; }
        public float SubTotal { get; set; }
        public float DiferenciaIVA { get; set; }
        
        public string TotalFormato { get; set; }
        public string FechaCorta { get; set; }
        public List<PartidaPedidoDTO> Partidas { get; set; }


        public string FechaAlterada = string.Empty;
    }
}