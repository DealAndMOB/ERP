using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ERP.Models
{
    [Table("PartidaCotizacionVenta")]
    public class clsPartidaCotizacionVenta
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public String Folio { get; set; }
        public virtual clsCotizacionVenta cotizacionVenta { get; set; }

        public int ProductoID { get; set; }
        public virtual clsProducto producto { get; set; }   

        public int Unidades { get; set; }
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        public float CostoCapturado { get; set; }
        public float ?PorcentajeAumento { get; set; }
        public float PrecioUnitario { get; set; }
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        public float TotalPartida { get; set; }
        public String CriterioAumento { get; set; }
        public bool Estado { get; set; }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        public virtual List<clsPartidaRemision> partidasRemision { get; set; }
    }

    [Serializable]
    public class PartidaCotizacionDTO
    {
        public int ID { get; set; }
        public string CodigoProducto { get; set; }
        //Campos interfaz - - - - - - - - - - - - - - - - -
        public int Partida { get; set; }
        public string NombreProducto { get; set; }
        public string Categoria { get; set; }
        public string Imagen { get; set; }
        public string DescripProducto { get; set; }
        public string PrecioBaseFormato { get; set; } // precio con formato del producto base sin aumento

        //Campos BD - - - - - - - - - - - - - - - - - - - 
        public String Folio { get; set; }
        public int ProductoID { get; set; }
        public int Cantidad { get; set; } // - - > Unidades Vendidas
        public String PrecioFormato { get; set; } //Precio con formato del producto base con aumento 
        public String TotalFormato { get; set; }
        public String CriterioAumento { get; set; }
        public bool Estado { get; set; }

        //Conversiones - - - - - - - - - - - - - - - - - - - - - 
        public float Costo { get; set; } // Precio del producto base sin aumento
        public float Precio { get; set; } //Precio final del producto más aumento
        public float PorcentajeAumento { get; set; } // Porcentaje de aumento sobre precio base
        public float Total { get; set; } // Unidades * precio final (Más aumento)
        public float PrecioBase { get; set; } // Precio del producto sin aumento
    }

    [Serializable]
    public class DatosReportePartidas
    {
        public string Folio { get; set; }
        // Campos Entrada - - - - - - - - - - - - - - - - - - -
        public float Costo { get; set; }
        public float precioBase { get; set; }
        public float PrecioFinal { get; set; }

        // Campos Salida - - - - - - - - - - - - - - - - - - -
        public int Partida { get; set; }
        public string NombreProducto { get; set; }
        public string CostoFormato { get; set; }
        public string BaseFormato { get; set; }
        public string PrecioFormato { get; set; }
        public string Porcentaje { get; set; }
        public string CriterioAumento { get; set; }
        public string MontoAumento { get; set; }
        public int Unidades { get; set; }

        public float TotalPartidaVenta { get; set; }
        public float TotalPartidaCosto { get; set; }
        public float PorcentajeAumento { get; set; } // Porcentaje de aumento sobre precio base

        public string TotalPartidaVentaFormato { get; set; }
        public string TotalPartidaCostoFormato { get; set; }
        public string MargenBruto { get; set; }
    }
}