<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="inicio.aspx.cs" Inherits="ERP.Views.inicio" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Inicio</title>
    <link href="../Media/Resources/LOGO CUBO.png" rel="Icon" />
    <link href="../Content/bootstrap.min.css" rel="stylesheet" />

    <script src="../Scripts/ScriptsAsp/scripsFront.js"></script>
    <script src="../Scripts/ScriptsAsp/Alertas.js"></script>
    <script src="https://kit.fontawesome.com/b0cb49c5f4.js" crossorigin="anonymous"></script>
</head>
<body>
    <main class="Inicio" id="pantallaBloqueo">
        <form id="form1" runat="server">
            <header>
                <nav class="navbar navbar-dark bg-dark navbar-expand-lg bg-body-tertiary text-center">
                    <div class="container-fluid text-light">
                        <asp:ImageButton ID="btnInicio" runat="server" ImageUrl="~/Media/Resources/LOGO CUBO.png" alt="Logo" Width="80" Height="60" OnClick="btnInicio_Click" />
                        <li id="IconoSalir" class="mt-2">
                            <asp:ImageButton ID="btnLogout" runat="server" ImageUrl="~/Media/Resources/Power.png" OnClick="btnLogout_Click1" />
                            <a href="#" class="navbar-brand">
                                <font>CERRAR SESIÓN</font>
                            </a>
                        </li>

                    </div>
                </nav>
            </header>
            <%-- Contenido --%>
            <section class="container container-fluid text-center">
                <asp:Image ID="imgLogo" Class="imgInicio mt-2" runat="server" ImageUrl="~/Media/Resources/LOGOS AGC - LINEA DE PRODUCTOS.png" />
                <p class="mt-4"><sup>#tusproyectosdeprincipioafin</sup></p>
            </section>

            <%-- Cartas --%>
            <section class="content bg-dark rounded-3 mt-2 text-center mb-5 border-Orange">
                <h1 class="mb-4 mt-2 text-light"><b>Módulos</b></h1>
                <article class="row justify-content-around  p-2 gap-1 pb-2">

                    <div class="col-12 col-md-6 col-lg-3 cards mb-3" id="ventas">
                        <div class="card bg-dark border-Green">
                            <asp:Image runat="server" ImageUrl="~/Media/Resources/Cotización.jpg" alt="Ventas" class="card-img-top align-self-center" />
                            <div class="card-body bg-dark">

                                <div class="dropdown text-light">
                                    <asp:Label ID="lblVentas" runat="server">Módulo de ventas<i class="fa-solid fa-chevron-down down"></i></asp:Label>
                                    <div class="dropdown-content container ">
                                        <asp:Label ID="lblVentasNone" runat="server" ForeColor="#999999">Módulo de ventas</asp:Label>
                                        <div class="row">
                                            <asp:Button ID="btnCotizar" runat="server" Text="Crear cotización" CssClass="option" OnClick="btnCotizar_Click" />
                                        </div>
                                        <div class="row">
                                            <asp:Button ID="btnOrden" runat="server" Text="Historial de cotización venta" CssClass="option" OnClick="btnOrden_Click" />
                                        </div>
                                    </div>
                                </div>

                            </div>
                        </div>
                    </div>

                    <div class="col-12 col-md-6 col-lg-3 cards mb-3" id="catalogos">
                        <div class="card bg-dark border-Green">
                            <asp:Image runat="server" ImageUrl="~/Media/Resources/Catalogos.jpg" alt="Catalogos" class="card-img-top align-self-center imgCard" />
                            <div class="card-body bg-dark">

                                <div class="dropdown text-light">
                                    <asp:Label ID="lblCatalogos" runat="server">Módulo de catálogos<i class="fa-solid fa-chevron-down down"></i></asp:Label>
                                    <div class="dropdown-content container ">
                                        <asp:Label ID="lblCatalogosNone" runat="server" ForeColor="#999999">Módulo de catálogos</asp:Label>
                                        <div class="row">
                                            <asp:Button ID="btnProductos" runat="server" Text="Productos" CssClass="option" OnClick="btnProductos_Click" />
                                        </div>
                                        <div class="row">
                                            <asp:Button ID="btnClientes" runat="server" Text="Clientes" CssClass="option" OnClick="btnClientes_Click" />
                                        </div>
                                        <div class="row">
                                            <asp:Button ID="btnProveedor" runat="server" Text="Proveedores" CssClass="option" OnClick="btnProveedor_Click" />
                                        </div>
                                    </div>
                                </div>

                            </div>
                        </div>
                    </div>

                    <div class="col-12 col-md-6 col-lg-3 cards mb-3" id="compras">
                        <div class="card bg-dark border-Green">
                            <asp:Image runat="server" ImageUrl="~/Media/Resources/Pedidos.jpg" alt="Compras" class="card-img-top align-self-center imgCard" />
                            <div class="card-body bg-dark">

                                <div class="dropdown text-light">
                                    <asp:Label ID="lblCompras" runat="server">Módulo de compras<i class="fa-solid fa-chevron-down down"></i></asp:Label>
                                    <div class="dropdown-content container ">
                                        <asp:Label ID="lblComprasNone" runat="server" ForeColor="#999999">Módulo de compras</asp:Label>
                                        <div class="row">
                                            <asp:Button ID="btnPedido" runat="server" Text="Crear pedido" CssClass="option" OnClick="btnPedido_Click" />
                                        </div>
                                        <div class="row">
                                            <asp:Button ID="btnHistorial" runat="server" Text="Historial de pedidos" CssClass="option" OnClick="btnHistorial_Click" />
                                        </div>
                                    </div>
                                </div>

                            </div>
                        </div>
                    </div>

                    <div class="col-12 col-md-6 col-lg-3 cards mb-3" id="sistema">
                        <div class="card bg-dark border-Green">
                            <asp:Image runat="server" ImageUrl="~/Media/Resources/administración.jpg" alt="Compras" class="card-img-top align-self-center imgCard" />
                            <div class="card-body bg-dark">

                                <div class="dropdown text-light">
                                    <asp:Label ID="lblAdministracion" runat="server">Módulo de administración<i class="fa-solid fa-chevron-down down"></i></asp:Label>
                                    <div class="dropdown-content container ">
                                        <asp:Label ID="lblAdministracionNone" runat="server" ForeColor="#999999">Módulo de administración</asp:Label>
                                        <div class="row">
                                            <asp:Button ID="btnUsuarios" runat="server" Text="Usuarios" CssClass="option" OnClick="btnUsuarios_Click" />
                                        </div>
                                        <div class="row">
                                            <asp:Button ID="btnDependencias" runat="server" Text="Dependencias" CssClass="option" OnClick="btnDependencias_Click" />
                                        </div>
                                    </div>
                                </div>

                            </div>
                        </div>
                    </div>

                </article>
            </section>

        </form>
        <footer class="footer bg-dark text-center">
            <section class="container-fluid">
                <div class="row justify-content-center">
                    <div class="col-12 col-lg-3 p-2">
                        <asp:Image ID="LogoFooter" runat="server" ImageUrl="~/Media/Resources/LOGOS AGC - LINEA DE PRODUCTOS.png" alt="Logo" />
                    </div>
                    <div class="col-12 col-lg-3 align-self-center">
                        <asp:Label ID="lbFooter" runat="server" Text="Priv. Héroes de Nacozari s/n, Ozumbilla,Tecámac, Santa María Ozumbilla, C.P.55760, Edo. Méx." ForeColor="#0F777E"></asp:Label>
                        <div class="row mb-1">
                            <div class="col contacto">
                                <asp:HyperLink ID="hlTel" runat="server" NavigateUrl="tel:5562351766"><i class="fa-solid fa-phone m-2 p-2 text-Footer"></i> 55-6235-1766</asp:HyperLink><br />
                            </div>
                            <div class="col contacto">
                                <asp:HyperLink ID="hlWhats" runat="server" NavigateUrl="https://wa.link/sc3elf"><i class="fa-brands fa-whatsapp m-2 p-2 text-footer"></i> 779 102 3377</asp:HyperLink><br />
                            </div>
                        </div>
                        <asp:Label ID="lbCopy" runat="server" Text="Derechos reservados - Centro CCAI © 2023"></asp:Label>
                    </div>
                    <div class="col-12 col-lg-3 align-self-center">
                        <div class="row justify-content-center">
                           <div class="col-12">
                                <asp:HyperLink ID="hlFace" runat="server" NavigateUrl="https://www.facebook.com/agccomercialequipamiento"><i class="fa-brands fa-facebook m-2 text-footer"></i> @agccomercialequipamiento</asp:HyperLink><br />
                            </div>
                            <div class="col">
                                <asp:HyperLink ID="hlIg" runat="server" NavigateUrl="https://www.instagram.com/agc.comercial/"><i class="fa-brands fa-instagram m-2 text-footer"></i> @agccomercial</asp:HyperLink><br />
                            </div>
                        </div>
                    </div>
                </div>
            </section>
        </footer>
    </main>
    <script src="../Scripts/bootstrap.min.js"></script>
</body>
</html>
