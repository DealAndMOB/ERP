using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ERP.Models
{
    [Table("Proveedor")]
    public class clsProveedor
    {
        [Key]
        public int ID { get; set; }

        [StringLength(13)]
        [Index("RFCUnico", IsUnique = true)]
        public String RFC { get; set; }

        [Required]
        public int CategoriaProveedorID { get; set; }
        public virtual clsCategoriaProveedor CategoriaProveedor { get; set; }
        public int EstadoID { get; set; }
        public virtual clsEstado estado { get; set; }

        [StringLength(50)]
        public String Empresa { get; set; }
        [StringLength(50)]
        public String RazonSocial { get; set; }
        [StringLength(50)]
        public String NombreContacto { get; set; }
        [StringLength(150)]
        public String Descripcion { get; set; }
        [StringLength(50)]
        public String CorreoPagina { get; set; }
        [StringLength(10)]
        public String Telefono { get; set; }
        [StringLength(100)]
        public String Direccion { get; set; }
    }

    public class ProveedorDTO
    {
        public int ID { get; set; }
        public String RFC { get; set; }
        public String Empresa { get; set; }
        public String RazonSocial { get; set; }
        public String NombreContacto { get; set; }
        public String Descripcion { get; set; }
        public String CorreoPagina { get; set; }
        public String Telefono { get; set; }
        public String Direccion { get; set; }
        public String Categoria { get; set; }
        public String Zona { get; set; }
        public String Estado { get; set; }
        
        public int CategoriaID { get; set; }
        public int EstadoID { get; set; }
    }

    //public class ProveedorDTO : BaseProveedorDTO
    //{
    //    public String Zona { get; set; }
    //}

}