namespace ERP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitBD : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CategoriaProducto",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Nombre = c.String(maxLength: 30),
                    })
                .PrimaryKey(t => t.ID)
                .Index(t => t.Nombre, unique: true, name: "NombreUnico");
            
            CreateTable(
                "dbo.CategoriaProveeedor",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Nombre = c.String(maxLength: 30),
                    })
                .PrimaryKey(t => t.ID)
                .Index(t => t.Nombre, unique: true, name: "NombreUnico");
            
            CreateTable(
                "dbo.Cliente",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        RFC = c.String(maxLength: 13),
                        Nombre = c.String(maxLength: 50),
                        Direccion = c.String(maxLength: 100),
                        Telefono = c.String(maxLength: 10),
                        Correo = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.ID)
                .Index(t => t.RFC, unique: true, name: "RFCUnico");
            
            CreateTable(
                "dbo.CotizacionVenta",
                c => new
                    {
                        Folio = c.String(nullable: false, maxLength: 128),
                        ClienteID = c.Int(nullable: false),
                        Condiciones = c.String(),
                        Total = c.Single(nullable: false),
                        Estado = c.Boolean(nullable: false),
                        FechaCotizacion = c.DateTime(nullable: false),
                        FechaVenta = c.DateTime(),
                    })
                .PrimaryKey(t => t.Folio)
                .ForeignKey("dbo.Cliente", t => t.ClienteID, cascadeDelete: true)
                .Index(t => t.ClienteID);
            
            CreateTable(
                "dbo.PartidaCotizacionVenta",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Folio = c.String(nullable: false, maxLength: 128),
                        ProductoID = c.Int(nullable: false),
                        Unidades = c.Int(nullable: false),
                        CostoCapturado = c.Single(nullable: false),
                        PorcentajeAumento = c.Single(),
                        PrecioUnitario = c.Single(nullable: false),
                        TotalPartida = c.Single(nullable: false),
                        CriterioAumento = c.String(),
                        Estado = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.CotizacionVenta", t => t.Folio, cascadeDelete: true)
                .ForeignKey("dbo.Producto", t => t.ProductoID, cascadeDelete: true)
                .Index(t => t.Folio)
                .Index(t => t.ProductoID);
            
            CreateTable(
                "dbo.PartidaRemision",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Folio = c.String(maxLength: 128),
                        PartidaCotizacionVentaID = c.Int(nullable: false),
                        CantidadRemitida = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.PartidaCotizacionVenta", t => t.PartidaCotizacionVentaID, cascadeDelete: true)
                .ForeignKey("dbo.Remision", t => t.Folio)
                .Index(t => t.Folio)
                .Index(t => t.PartidaCotizacionVentaID);
            
            CreateTable(
                "dbo.Remision",
                c => new
                    {
                        Folio = c.String(nullable: false, maxLength: 128),
                        FolioVenta = c.String(maxLength: 128),
                        FechaEntrega = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Folio)
                .ForeignKey("dbo.CotizacionVenta", t => t.FolioVenta)
                .Index(t => t.FolioVenta);
            
            CreateTable(
                "dbo.Producto",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Codigo = c.String(maxLength: 20),
                        CategoriaProductoID = c.Int(nullable: false),
                        Nombre = c.String(maxLength: 200),
                        Descripcion = c.String(),
                        Costo = c.Single(nullable: false),
                        Precio = c.Single(nullable: false),
                        Imagen = c.String(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.CategoriaProducto", t => t.CategoriaProductoID, cascadeDelete: true)
                .Index(t => t.Codigo, unique: true, name: "CodigoUnico")
                .Index(t => t.CategoriaProductoID)
                .Index(t => t.Nombre, unique: true, name: "NombreUnico");
            
            CreateTable(
                "dbo.Estado",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ZonaID = c.Int(nullable: false),
                        Nombre = c.String(maxLength: 30),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Zona", t => t.ZonaID, cascadeDelete: true)
                .Index(t => t.ZonaID)
                .Index(t => t.Nombre, unique: true, name: "NombreUnico");
            
            CreateTable(
                "dbo.Zona",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Zona = c.String(maxLength: 30),
                    })
                .PrimaryKey(t => t.ID)
                .Index(t => t.Zona, unique: true, name: "NombreUnico");
            
            CreateTable(
                "dbo.PartidaPedido",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Folio = c.String(nullable: false, maxLength: 25),
                        ProductoID = c.Int(nullable: false),
                        Unidades = c.Int(nullable: false),
                        CostoUnitario = c.Single(nullable: false),
                        TotalPartida = c.Single(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Pedido", t => t.Folio, cascadeDelete: true)
                .ForeignKey("dbo.Producto", t => t.ProductoID, cascadeDelete: true)
                .Index(t => t.Folio)
                .Index(t => t.ProductoID);
            
            CreateTable(
                "dbo.Pedido",
                c => new
                    {
                        Folio = c.String(nullable: false, maxLength: 25),
                        ProveedorID = c.Int(nullable: false),
                        Total = c.Single(nullable: false),
                        Fecha = c.DateTime(nullable: false),
                        Condiciones = c.String(),
                        Estado = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Folio)
                .ForeignKey("dbo.Proveedor", t => t.ProveedorID, cascadeDelete: true)
                .Index(t => t.ProveedorID);
            
            CreateTable(
                "dbo.Proveedor",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        RFC = c.String(maxLength: 13),
                        CategoriaProveedorID = c.Int(nullable: false),
                        EstadoID = c.Int(nullable: false),
                        Empresa = c.String(maxLength: 50),
                        RazonSocial = c.String(maxLength: 50),
                        NombreContacto = c.String(maxLength: 50),
                        Descripcion = c.String(maxLength: 150),
                        CorreoPagina = c.String(maxLength: 50),
                        Telefono = c.String(maxLength: 10),
                        Direccion = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.CategoriaProveeedor", t => t.CategoriaProveedorID, cascadeDelete: true)
                .ForeignKey("dbo.Estado", t => t.EstadoID, cascadeDelete: true)
                .Index(t => t.RFC, unique: true, name: "RFCUnico")
                .Index(t => t.CategoriaProveedorID)
                .Index(t => t.EstadoID);
            
            CreateTable(
                "dbo.Perfil",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Nombre = c.String(nullable: false, maxLength: 50),
                        Accesos = c.String(maxLength: 4),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Usuario",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        PerfilID = c.Int(nullable: false),
                        Nombre = c.String(maxLength: 50),
                        Correo = c.String(maxLength: 50),
                        Contraseña = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Perfil", t => t.PerfilID, cascadeDelete: true)
                .Index(t => t.PerfilID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Usuario", "PerfilID", "dbo.Perfil");
            DropForeignKey("dbo.PartidaPedido", "ProductoID", "dbo.Producto");
            DropForeignKey("dbo.Pedido", "ProveedorID", "dbo.Proveedor");
            DropForeignKey("dbo.Proveedor", "EstadoID", "dbo.Estado");
            DropForeignKey("dbo.Proveedor", "CategoriaProveedorID", "dbo.CategoriaProveeedor");
            DropForeignKey("dbo.PartidaPedido", "Folio", "dbo.Pedido");
            DropForeignKey("dbo.Estado", "ZonaID", "dbo.Zona");
            DropForeignKey("dbo.PartidaCotizacionVenta", "ProductoID", "dbo.Producto");
            DropForeignKey("dbo.Producto", "CategoriaProductoID", "dbo.CategoriaProducto");
            DropForeignKey("dbo.Remision", "FolioVenta", "dbo.CotizacionVenta");
            DropForeignKey("dbo.PartidaRemision", "Folio", "dbo.Remision");
            DropForeignKey("dbo.PartidaRemision", "PartidaCotizacionVentaID", "dbo.PartidaCotizacionVenta");
            DropForeignKey("dbo.PartidaCotizacionVenta", "Folio", "dbo.CotizacionVenta");
            DropForeignKey("dbo.CotizacionVenta", "ClienteID", "dbo.Cliente");
            DropIndex("dbo.Usuario", new[] { "PerfilID" });
            DropIndex("dbo.Proveedor", new[] { "EstadoID" });
            DropIndex("dbo.Proveedor", new[] { "CategoriaProveedorID" });
            DropIndex("dbo.Proveedor", "RFCUnico");
            DropIndex("dbo.Pedido", new[] { "ProveedorID" });
            DropIndex("dbo.PartidaPedido", new[] { "ProductoID" });
            DropIndex("dbo.PartidaPedido", new[] { "Folio" });
            DropIndex("dbo.Zona", "NombreUnico");
            DropIndex("dbo.Estado", "NombreUnico");
            DropIndex("dbo.Estado", new[] { "ZonaID" });
            DropIndex("dbo.Producto", "NombreUnico");
            DropIndex("dbo.Producto", new[] { "CategoriaProductoID" });
            DropIndex("dbo.Producto", "CodigoUnico");
            DropIndex("dbo.Remision", new[] { "FolioVenta" });
            DropIndex("dbo.PartidaRemision", new[] { "PartidaCotizacionVentaID" });
            DropIndex("dbo.PartidaRemision", new[] { "Folio" });
            DropIndex("dbo.PartidaCotizacionVenta", new[] { "ProductoID" });
            DropIndex("dbo.PartidaCotizacionVenta", new[] { "Folio" });
            DropIndex("dbo.CotizacionVenta", new[] { "ClienteID" });
            DropIndex("dbo.Cliente", "RFCUnico");
            DropIndex("dbo.CategoriaProveeedor", "NombreUnico");
            DropIndex("dbo.CategoriaProducto", "NombreUnico");
            DropTable("dbo.Usuario");
            DropTable("dbo.Perfil");
            DropTable("dbo.Proveedor");
            DropTable("dbo.Pedido");
            DropTable("dbo.PartidaPedido");
            DropTable("dbo.Zona");
            DropTable("dbo.Estado");
            DropTable("dbo.Producto");
            DropTable("dbo.Remision");
            DropTable("dbo.PartidaRemision");
            DropTable("dbo.PartidaCotizacionVenta");
            DropTable("dbo.CotizacionVenta");
            DropTable("dbo.Cliente");
            DropTable("dbo.CategoriaProveeedor");
            DropTable("dbo.CategoriaProducto");
        }
    }
}
