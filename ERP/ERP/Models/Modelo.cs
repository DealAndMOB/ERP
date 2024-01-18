using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace ERP.Models
{
    public partial class Modelo : DbContext
    {
        public Modelo()
            : base("name=Modelo")
        {
        }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
        public virtual DbSet<clsCategoriaProducto> CategoriaProducto { get; set; }
        public virtual DbSet<clsCategoriaProveedor> CategoriaProveedor { get; set; }
        public virtual DbSet<clsCliente> Cliente { get; set; }
        public virtual DbSet<clsCotizacionVenta> CotizacionVenta { get; set; }
        public virtual DbSet<clsEstado> Estado { get; set; }
        public virtual DbSet<clsPartidaCotizacionVenta> PartidaCotizacionVenta { get; set; }
        public virtual DbSet<clsPartidaPedido> PartidaPedido { get; set; }
        public virtual DbSet<clsPartidaRemision> PartidaRemision { get; set; }
        public virtual DbSet<clsPedido> Pedido { get; set; }
        public virtual DbSet<clsPerfil> Perfil { get; set; }
        public virtual DbSet<clsProducto> Producto { get; set; }
        public virtual DbSet<clsProveedor> Proveedor { get; set; }
        public virtual DbSet<clsRemision> Remision { get; set; }
        public virtual DbSet<clsUsuario> Usuario { get; set; }
        public virtual DbSet<clsZona> Zona { get; set; }
    }
}
