<%@ Page Title="" Language="C#" MasterPageFile="~/Vistas/Master/plantilla.Master" AutoEventWireup="true" CodeBehind="Pedidos.aspx.cs" Inherits="ERP.Vistas.Compras.Pedidos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script>
        function BloquearEnter(event) {
            if (event.keyCode === 13) {
                event.preventDefault(); // Cancela la acción predeterminada del Enter
                return false; // Evita que el evento se propague
            }
        }
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%--- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - --%>
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <%--- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - --%>
    <section class="pedido text-center text-light justify-content-center align-content-center container-fluid pb-5">

       <h1 class="p-3">Pedido</h1>
        <article class="contenedor mb-2 border-Orange bg-dark rounded-2 justify-content-center">
            <%--- - - - - - - - - - - - - - - - - - - - - - - START UpdatePanel - - - - - - - - - - - - - - - - - - - - - - --%>
            <asp:UpdatePanel ID="UpdatePanelPedido" runat="server">
                <ContentTemplate>
                    <%--- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -  - - - - - - - - - - - - - - - - - - - - - - - --%>
                    <h2 class="pt-3">Nuevo pedido</h2>

                    <%-- Buscar Poveedor --%>
                    <div class="row mb-2 align-items-center p-2">
                        <%-- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - --%>
                        <asp:Label ID="lblErrorBusqueda" runat="server" ForeColor="Red" Class="mb-1 h5"></asp:Label>
                        <div class="col text-end">
                            <asp:Label ID="lblSearchCliente" runat="server" Text="Buscar proveedor"></asp:Label>
                        </div>

                        <div class="col">
                            <asp:TextBox ID="txtBusquedaProveedor" runat="server" CssClass="txtSeacr" onkeydown="return BloquearEnter(event);"></asp:TextBox>
                        </div>

                        <div class="col text-start">
                            <asp:ImageButton ID="btnBuscarProveedor" runat="server" ImageUrl="~/Media/Resources/Buscar.svg" CssClass="searchBtn rounded-circle p-2" OnClick="btnBuscarProveedor_Click" />
                        </div>
                        <%-- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - --%>
                    </div>

                    <%-- Resultados proveedor --%>
                    <section class="row contenedor border-White result rounded-2 Orange p-2 mb-4">
                        <h4 class="mt-2">Resultados de búsqueda</h4>
                        <%-- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - --%>
                        <div class="tabla">

                            <%-- Grid View Resultados de Poveedor --%>
                            <asp:GridView ID="gvBusquedaProveedor" runat="server" AutoGenerateColumns="False" Style="width: 100%;">
                                <Columns>
                                    <asp:BoundField DataField="RFC" HeaderText="RFC" />
                                    <asp:BoundField DataField="Empresa" HeaderText="Empresa" />
                                    <asp:BoundField DataField="NombreContacto" HeaderText="Contacto" />
                                    <asp:BoundField DataField="CorreoPagina" HeaderText="Correo / Página" />
                                    <asp:BoundField DataField="Categoria" HeaderText="Categoría" />
                                    <asp:BoundField DataField="Zona" HeaderText="Zona" />
                                    <asp:TemplateField HeaderText="Acción">
                                        <ItemTemplate>
                                            <%-- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - --%>
                                            <asp:Button ID="btnSeleccionarProveedor" runat="server" Text="Asignar Proveedor" Class="btnC rounded-3 m-2 p-2" OnClick="btnSeleccionarProveedor_Click" />
                                            <%-- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - --%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </section>
                    <div class="row result  mb-4 align-items-center align-content-center p-1">
                        <%-- Buscardor producto --%>
                        <asp:Label ID="lblErrorBusquedaProducto" runat="server" Text="" ForeColor="Red" Class="h5"></asp:Label>
                        <div class="row mb-2 align-items-center p-2">
                            <%-- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - --%>
                            <div class="col text-end">
                                <asp:Label ID="lblSearchProducto" runat="server" Text="Buscar producto"></asp:Label>
                            </div>
                            <%-- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - --%>
                            <div class="col">
                                <asp:TextBox ID="txtBusquedaProducto" runat="server" CssClass="txtSeacr" onkeydown="return BloquearEnter(event);"></asp:TextBox>
                            </div>
                            <%-- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - --%>
                            <div class="col text-start">
                                <asp:ImageButton ID="btnBusquedaProducto" runat="server" ImageUrl="~/Media/Resources/Buscar.svg" CssClass="searchBtn rounded-circle p-2" OnClick="btnBusquedaProducto_Click" />
                            </div>
                            <%-- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - --%>
                        </div>
                    </div>
                    <%-- Datos del pedido--%>
                    <div class="row border-Orange result rounded-3 White text-dark mb-4 align-items-center align-content-center p-1">
                        <%-- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - --%>
                        <div class="col-6 col-lg-4 text-end">
                            <h4>
                                <asp:Label Class="lblCliente" ID="lblProveedor" runat="server" Text="Seleccionar Proveedor"></asp:Label>
                            </h4>
                        </div>
                        <%-- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - --%>
                        <div class="col-6 col-lg-4 text-center">
                            <h4>Folio: 
                            <asp:Label ID="lblFolio" runat="server" Text=""></asp:Label>
                            </h4>
                        </div>
                        <%-- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - --%>
                        <div class="col-12 col-lg-4">
                            <div class="d-flex gap-3">
                                <h4 class="align-self-center">Número de unidades: </h4>
                                <asp:TextBox ID="txtCantidad" Style="width: 25%;" runat="server" PlaceHolder="Unidades" TextMode="Number" CssClass="Cantidad border-Orange text-center rounded-2 p-2" onkeydown="return BloquearEnter(event);"></asp:TextBox>
                            </div>
                            <%-- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - --%>
                            <div class="text-start">
                                <asp:Label ID="lblCantidad" runat="server" ForeColor="Red" Class="h6"></asp:Label>
                            </div>
                        </div>
                        <%-- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - --%>
                    </div>

                    <%-- Resultados producto --%>
                    <div class="row border-White result rounded-2 Green p-2 mb-4 descrip-center">
                        <h4 class="mt-2">Resultados de búsqueda</h4>
                        <%-- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - --%>
                        <div class="tabla">

                            <%-- Grid View resultado de la busqueda de producto--%>
                            <asp:GridView ID="gvProducto" runat="server" AutoGenerateColumns="False" Style="width: 100%;">
                                <Columns>
                                    <asp:BoundField DataField="Codigo" HeaderText="Código" />
                                    <asp:BoundField DataField="CostoFormato" HeaderText="Costo" />
                                    <asp:BoundField DataField="Nombre" HeaderText="Producto" />
                                    <asp:BoundField DataField="Descripcion" HeaderText="Descripción" />
                                    <asp:BoundField DataField="NombreCategoria" HeaderText="Categoría" />
                                    <asp:ImageField DataImageUrlField="Imagen" DataImageUrlFormatString="~/Multimedia/{0}" HeaderText="Imagen"></asp:ImageField>
                                    <asp:TemplateField HeaderText="Cotizar">
                                        <ItemTemplate>
                                            <%-- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -  --%>
                                            <asp:Button ID="btnCotizarProducto" runat="server" Text="Cotizar" Class="btnC rounded-3 m-2 p-2" OnClick="btnCotizarProducto_Click" />
                                            <%-- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -  --%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                        <%-- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -  --%>
                    </div>

                    <%-- Pedido --%>
                    <div class="row text-dark White border-Orange result rounded-2 p-2  descriPartida-center mb-3">
                        <h4>Pedido</h4>
                        <div class="tabla">
                            <%-- Grid View --%>
                            <asp:GridView ID="gvPedido" Style="width: 100%; margin-bottom: .5rem" runat="server" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="3" AutoGenerateColumns="False">
                                <Columns>
                                    <asp:BoundField DataField="Partida" HeaderText="Partida" />
                                    <asp:BoundField DataField="DescripProducto" HeaderText="Descripción" />
                                    <%--- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -  - - - - - - - - - - - - - - - - - - - - - - - --%>
                                    <asp:ImageField DataImageUrlField="Imagen" DataImageUrlFormatString="~/Multimedia/{0}" HeaderText="Imagen"></asp:ImageField>
                                    <%--- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -  - - - - - - - - - - - - - - - - - - - - - - - --%>
                                    <asp:BoundField DataField="Cantidad" HeaderText="Unidades" />
                                    <asp:BoundField DataField="CostoFormato" HeaderText="Costo" />
                                    <asp:BoundField DataField="TotalFormato" HeaderText="Total" />
                                    <%--- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -  - - - - - - - - - - - - - - - - - - - - - - - --%>
                                    <asp:TemplateField HeaderText="Acciones">
                                        <ItemTemplate>
                                            <div class="d-flex flex-column">
                                                <asp:Button ID="btnEliminarProducto" runat="server" Text="Eliminar" Class="btnC rounded-3 m-2 p-2" OnClick="btnEliminarProducto_Click" />
                                            </div>
                                        </ItemTemplate>
                                        <%-- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -  - - - - - - - - - - - - - - - - - - - - - - - --%>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>

                    <%--- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - --%>
                    <section class="pieCotizacion result">
                        <%--- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - --%>
                        <article class="condiciones position-relative">
                            <blockquote>
                                <asp:Label ID="lblCondiciones" runat="server" Class="h6 fs-5" Text="Condiciones de compra: "></asp:Label>
                            </blockquote>
                            <asp:TextBox ID="txtCondicion" runat="server" Style="height: 72%; width: 100%;" CssClass="rounded p-3" TextMode="MultiLine" onkeydown="return BloquearEnter(event);"></asp:TextBox>
                        </article>
                        <%--- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - --%>
                        <article class="iva">
                            <%-- Encabezado --%>
                            <div class="d-flex Orange text-dark p-2 rounded" style="border-bottom: solid 2px #212529;">
                                <div class="col text-end">
                                    <asp:Label ID="lblSubtotalTitle" runat="server" Text="Subtotal : $" Font-Bold="True"></asp:Label>
                                </div>
                                <%--- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - --%>
                                <div class="col text-end">
                                    <asp:Label ID="lblSubtotal" runat="server" Text="0.00"></asp:Label>
                                </div>
                            </div>

                            <%-- Cuerpo --%>
                            <div class="d-flex White p-2 text-dark">
                                <%--- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - --%>
                                <div class="col mt-2 text-end">
                                    <asp:Label ID="lblIVATitle" runat="server" Text="IVA : $" Font-Bold="True"></asp:Label>
                                </div>
                                <%--- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - --%>
                                <div class="col mt-2 text-end">
                                    <asp:Label ID="lblIVA" runat="server" Text="0.00"></asp:Label>
                                </div>
                                <%--- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - --%>
                            </div>

                            <div class="d-flex p-2 text-white Green rounded" style="border-top: solid 2px #212529;">
                                <%--- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - --%>
                                <div class="col text-end">
                                    <asp:Label ID="lblTotalTitle" runat="server" Text="Total : $" Font-Bold="True"></asp:Label>
                                </div>
                                <%--- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - --%>
                                <div class="col text-end">
                                    <asp:Label ID="lblTotal" runat="server" Text="0.00"></asp:Label>
                                </div>
                                <%--- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - --%>
                            </div>
                        </article>

                    </section>
                    <%--- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - --%>

                    <div class="row mt-3 justify-content-center text-center mb-4 pt-3">
                        <%--- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - --%>
                        <div class="col text-end">
                            <asp:ImageButton ID="btnDescargarPDF" runat="server" ImageUrl="~/Media/Resources/download.png" CssClass="confirmar rounded-circle p-2" OnClick="btnDescargarPDF_Click" />

                            <br />
                            <asp:Label ID="lblDescargar" runat="server" Text="Descargar"></asp:Label>
                        </div>
                        <%--- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - --%>
                        <div class="col">
                            <asp:ImageButton ID="btnAlertaConfirmacion" runat="server" ImageUrl="~/Media/Resources/check.svg" CssClass="confirmar rounded-circle p-1" OnClick="btnAlertaConfirmacion_Click" />
                            <br />
                            <asp:Label ID="lblCotizar" runat="server" Text="Comprar"></asp:Label>
                        </div>
                        <%--- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - --%>
                        <div class="col text-start">
                            <asp:ImageButton ID="btnNuevaPedido" runat="server" ImageUrl="~/Media/Resources/circle-plus-solid.svg" CssClass="confirmar rounded-circle p-2" OnClick="btnNuevaPedido_Click" />
                            <br />
                            <asp:Label ID="lblNuevaC" runat="server" Text="Nuevo Pedido"></asp:Label>
                        </div>
                        <%--- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - --%>
                    </div>

                    <%-- Alerta --%>
                    <article class="alerta alert rounded close">
                        <div class="row justify-content-center text-dark text-center">
                            <img src="../../Media/Resources/exclamation-mark-svgrepo-com.svg" alt="" class="shake iconos" style="width: 5rem;">
                            <h1 class="text-dark">¿Seguro que deseas continuar?</h1>
                            <p>
                                <br>
                                No podras deshacer este paso.
                            </p>
                            <div class="row mt-2">
                                <div class="col-5">
                                    <asp:Button ID="btnAlertcancel" runat="server" Text="Cancelar" class="cancelar" />
                                </div>
                                <div class="col-2"></div>
                                <div class="col-5">
                                    <asp:Button ID="btnConfirmarPedido" runat="server" Text="Aceptar" class="aceptar" OnClick="btnConfirmarPedido_Click" />
                                </div>
                            </div>
                        </div>
                    </article>
                    <%-- Fin alerta --%>

                    <%--- - - - - - - - - - - - - - - - - - - - - - - FIN UpdatePanel - - - - - - - - - - - - - - - - - - - - - - --%>
                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="btnDescargarPDF"/>
                </Triggers>
            </asp:UpdatePanel>
            <%--- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - --%>

            <%-- Pantalla de carga - - - - - -  - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - --%>
            <asp:UpdateProgress ID="UpdateProgressCliente" runat="server" AssociatedUpdatePanelID="UpdatePanelPedido">
                <ProgressTemplate>
                    <div class="overlay">
                        <div class="load-icon">
                            <div class="loader mb-2"></div>
                            <h1 class="text-black">Cargando...</h1>
                        </div>
                    </div>
                </ProgressTemplate>
            </asp:UpdateProgress>
            <%-- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - --%>
        </article>
    </section>
</asp:Content>
