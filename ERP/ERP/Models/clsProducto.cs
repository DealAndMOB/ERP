using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ERP.Models
{
    [Table("Producto")]
    public class clsProducto
    {
        [Key]
        public int ID { get; set; }

        [StringLength(20)]
        [Index("CodigoUnico", IsUnique = true)]
        public String Codigo { get; set; }

        public int CategoriaProductoID { get; set; }
        public virtual clsCategoriaProducto CategoriaProducto { get; set; }

        [Index("NombreUnico", IsUnique = true)]
        [StringLength(200)]
        public String Nombre { get; set; }

        public String Descripcion { get; set; }
        public float Costo { get; set; }
        public float Precio { get; set; }
        public String Imagen { get; set; }
    }

    public class ProductoDTO
    {
        public int ID { get; set; }
        public String Codigo { get; set; }
        public String Nombre { get; set; }
        public String Descripcion { get; set; }
        // - - - - - - - - - - - - - - - - - - - - - 
        public float Costo { get; set; }
        public float Precio { get; set; }
        public string PrecioFormato { get; set; }
        public string CostoFormato { get; set; }

        // - - - - - - - - - - - - - - - - - - - - - 
        public String Imagen { get; set; }

        // - - - - - - - - - - - - - - - - - - - - - 
        public String NombreCategoria { get; set; }
        public int CategoriaID { get; set; }
    }

}