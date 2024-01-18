using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ERP.Models
{
    [Table("PartidaPedido")]
    public class clsPartidaPedido
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public string Folio { get; set; }
        public virtual clsPedido pedido { get; set; }

        public int ProductoID { get; set; }
        public virtual clsProducto producto { get; set; }

        public int Unidades { get; set; }
        public float CostoUnitario { get; set; }
        public float TotalPartida { get; set; }
    }

    [Serializable]
    public class PartidaPedidoDTO
    {
        public int ID { get; set; }
        // Campos de interfaz - - - - - - - - - - - - - -
        public String CodigoProducto { get; set; }
        public int Partida { get; set; }
        public string Nombre { get; set; }
        public string Categoria { get; set; }
        public string Imagen { get; set; }
        public string DescripProducto { get; set; }
        public string CostoBase { get; set; }

        // Campos BD - - - - - - - - - - - - - - - - - - -
        public String Folio { get; set; }
        public int ProductoID { get; set; }
        public int Cantidad { get; set; } // - - > Unidades Vendidas
        public String CostoFormato { get; set; }
        public String TotalFormato { get; set; }

        // Conversiones - - - - - - - - - - - - - - - - - -
        public float Costo { get; set; }
        public float Total { get; set; }
    }
}