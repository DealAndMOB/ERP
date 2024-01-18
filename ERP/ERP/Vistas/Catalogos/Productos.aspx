<%@ Page Title="" Language="C#" MasterPageFile="~/Vistas/Master/plantilla.Master" AutoEventWireup="true" CodeBehind="Productos.aspx.cs" Inherits="ERP.Vistas.Catalogos.Productos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%-- Carrusel - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - --%>
    <link href="../../Content/OwlCarousel/owl.carousel.min.css" rel="stylesheet" />
    <link href="../../Content/OwlCarousel/owl.theme.default.min.css" rel="stylesheet" />
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
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <div class="inventario text-center text-light justify-content-center align-content-center container-fluid ">

        <h1 class="p-4">Catálogo de productos</h1>



        <section class="row justify-content-center">
            <%-- Resultado de productos --%>
            <article class="col-12 col-lg-8 border-Orange gv mb-3 rounded-3 bg-dark">

                <h2 class="m-5">Productos en el sistema</h2>

                <div class="row align-items-center justify-content-center">
                    <asp:Label ID="lblErrorBusqueda" runat="server" ForeColor="Red" Class="mb-2 h5"></asp:Label>
                    <div class="col text-end">
                        <asp:Label ID="lblSEarchProducto" runat="server" Text="Buscar producto"></asp:Label>
                    </div>
                    <div class="col">
                        <asp:TextBox ID="txtBusquedaProducto" runat="server" CssClass="txtSeacr" onkeydown="return BloquearEnter(event);"></asp:TextBox>
                    </div>

                    <div class="col">
                        <asp:DropDownList ID="ddlFiltroOrden" AppendDataBoundItems="true" runat="server" Class="ddRegistro bg-dark text-light text-center p-2 mb-4">
                            <asp:ListItem Selected="false" disabled="True" Value="">&lt;Categorías&gt;</asp:ListItem>
                            <asp:ListItem Value="Todos">Todos</asp:ListItem>
                        </asp:DropDownList>
                    </div>

                    <div class="col text-start">
                        <asp:ImageButton ID="btnBuscarProducto" runat="server" ImageUrl="~/Media/Resources/Buscar.svg" CssClass="searchBtn rounded-circle p-2" OnClick="btnBuscarProducto_Click" />
                    </div>
                </div>

                <h3 class="m-5">Resultados encontrados: 
                    <asp:Label ID="lblEncontrados" runat="server"></asp:Label>
                </h3>

                <%-- - - - - - - - - - - - - - - - - - - - - -  - - - - Carrusel - - - - - - - - - - - - - - - - - - - - - - - - -  --%>
                <div class="owl-carousel owl-theme carrusel p-4 position-relative">
                    <asp:Repeater ID="repProductos" runat="server">
                        <ItemTemplate>
                            <div class="item card p-2" id="TarjetaProducto" style="height:27rem">

                                <div id="ContImg">
                                    <img id="ProductoImg" src="../../Multimedia/<%#Eval("Imagen")%>" class="card-img-top" />
                                </div>

                                <div class="card-body text-start border border-3">
                                    <%-- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - --%>
                                    <h5 class="text-center mb-3"><b>Código:</b> <%# Eval("Codigo")%></h5>
                                    <%-- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - --%>
                                    <p><b>Nombre:</b> <%# Eval("Nombre")%></p>
                                    <%-- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - --%>
                                    <div class="row position-relative mb-3" style="text-align: justify; height: 10vh; overflow-y: scroll; overflow-x: hidden;">
                                        <div id="Descripcion" class="position-absolute"><b>Descripcion:</b> <%# Eval("Descripcion")%></div>
                                    </div>
                                    <%-- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - --%>
                                    <p><b>Costo de adquisición:</b> <small>$ <%# Eval("Costo") %></small> </p>
                                    <p><b>Precio de venta:</b> <small>$ <%# Eval("Precio") %></small></p>
                                    <%-- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - --%>
                                    <div class="d-flex flex-row align-items-center justify-content-between">
                                        <section>
                                            <b>Categoría:</b> <%# Eval("NombreCategoria")%>
                                        </section>
                                        <section>
                                            <asp:Button CssClass="btn btn-success" ID="btnActualizar" runat="server" Text="Actualizar" CommandArgument='<%# Eval("Codigo") %>' OnClick="btnActualizar_Click" />
                                            <%--<asp:Button CssClass="btn btn-danger"  ID="btnEliminar"   runat="server" Text="Eliminar"   CommandArgument='<%# Eval("ID") %>' OnClick="btnEliminar_Click" />--%>
                                        </section>
                                    </div>
                                    <%-- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - --%>
                                </div>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
                <%-- - - - - - - - - - - - - - - - - - - - - -  - - - - Carrusel - - - - - - - - - - - - - - - - - - - - - - - - -  --%>
            </article>

            <%-- Formulario de productos --%>
            <article class="col-12 col-lg-3 border-Green rounded-3 form bg-dark mb-3">
                <h3 class="mt-4 mb-4">Nuevo producto</h3>

                <div class="row justify-content-center text-center">
                    <%-- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - --%>
                    <asp:Label ID="lblErrorInterno" runat="server" ForeColor="Red" Class="h5"></asp:Label>
                    <%-- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - --%>

                    <asp:RequiredFieldValidator ID="RFV1" runat="server" ErrorMessage="CAMPO OBLIGATORIO" ForeColor="Red" ControlToValidate="txtNombreProducto" ValidationGroup="Grupo1"></asp:RequiredFieldValidator>
                    <asp:Label ID="lblNombreP" runat="server" Text="Nombre del producto"></asp:Label>
                    <asp:TextBox ID="txtNombreProducto" runat="server" CssClass="textBox mb-3 mt-2" onkeydown="return BloquearEnter(event);"></asp:TextBox>



                    <asp:RequiredFieldValidator ID="RFV2" runat="server" ErrorMessage="CAMPO OBLIGATORIO" ForeColor="Red" ControlToValidate="txtCostoProducto" ValidationGroup="Grupo1"></asp:RequiredFieldValidator>
                    <%-- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - --%>
                    <asp:Label ID="lblErrorCosto" runat="server" ForeColor="Red"></asp:Label>
                    <%-- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - --%>
                    <asp:Label ID="lblCostoUnitario" runat="server" Text="Costo unitario"></asp:Label>
                    <asp:TextBox ID="txtCostoProducto" runat="server" CssClass="textBox mb-1 mt-2" onkeydown="return BloquearEnter(event);"></asp:TextBox>
                    <%-- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - --%>
                    <asp:RegularExpressionValidator ID="REV1" runat="server" ControlToValidate="txtCostoProducto" ErrorMessage="Valor Inválido" ForeColor="Red"
                        ValidationExpression="^(?!0+(?:\.0+)?$)(?:[1-9]\d*|0)(?:\.\d+)?$" ValidationGroup="Grupo1"></asp:RegularExpressionValidator>



                    <asp:RequiredFieldValidator ID="RFV3" runat="server" ErrorMessage="CAMPO OBLIGATORIO" ForeColor="Red" ControlToValidate="txtPrecioProducto" ValidationGroup="Grupo1"></asp:RequiredFieldValidator>
                    <%-- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - --%>
                    <asp:Label ID="lblErrorPrecio" runat="server" ForeColor="Red"></asp:Label>
                    <%-- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - --%>
                    <asp:Label ID="lblPrecioUnitario" runat="server" Text="Precio unitario"></asp:Label>
                    <asp:TextBox ID="txtPrecioProducto" runat="server" CssClass="textBox mb-1 mt-2" onkeydown="return BloquearEnter(event);"></asp:TextBox>
                    <%-- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - --%>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtPrecioProducto" ErrorMessage="Valor Inválido" ForeColor="Red"
                        ValidationExpression="^(?!0+(?:\.0+)?$)(?:[1-9]\d*|0)(?:\.\d+)?$" ValidationGroup="Grupo1"></asp:RegularExpressionValidator>




                    <asp:RequiredFieldValidator ID="RFV4" runat="server" ErrorMessage="CAMPO OBLIGATORIO" ForeColor="Red" ControlToValidate="txtDescripcionProducto" ValidationGroup="Grupo1"></asp:RequiredFieldValidator>
                    <asp:Label ID="lblDescripcion" runat="server" Text="Descripción del producto"></asp:Label>
                    <asp:TextBox ID="txtDescripcionProducto" runat="server" CssClass="textBox descripcion mb-3 mt-2" TextMode="MultiLine" onkeydown="return BloquearEnter(event);"></asp:TextBox>

                    <%-- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - --%>
                    <asp:Label ID="lblCategoria" runat="server" Text="Categoría"></asp:Label>
                    <asp:RequiredFieldValidator ID="RFV5" runat="server" ErrorMessage="CAMPO OBLIGATORIO" ForeColor="Red" ControlToValidate="ddlCategoriaProducto" ValidationGroup="Grupo1"></asp:RequiredFieldValidator>
                    <asp:DropDownList ID="ddlCategoriaProducto" AppendDataBoundItems="true" runat="server" Class="ddRegistro mt-2 rounded-3 mb-4 text-center p-1 mb-4">
                        <asp:ListItem Value="">&lt;Categorías&gt;</asp:ListItem>
                    </asp:DropDownList>
                    <%-- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - --%>

                    <div class="col-12 mb-3 px-5 justify-content-center">
                        <label for="images" class="drop-container">
                            <span class="drop-title">Imagen del producto</span>
                            <asp:FileUpload ID="flUpImagen" runat="server" />
                        </label>
                    </div>
                </div>
                <div class="row mb-4">
                    <%-- btn1 - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - --%>
                    <section class="ContenedorCancelar col align-items-center close">
                        <asp:ImageButton ID="CancelarActuBTN" runat="server" ImageUrl="~/Media/Resources/close.png" CssClass="confirmar rounded-circle p-2 " OnClick="CancelarActuBTN_Click" />
                        <br />
                        <asp:Label ID="Label1" runat="server" Text="Cancelar"></asp:Label>
                    </section>
                    <%-- btn2 - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - --%>
                    <section class="ContenedorAgregar col align-items-center subir">
                        <asp:ImageButton ID="btnSubirProducto" runat="server" ImageUrl="~/Media/Resources/circle-plus-solid.svg" CssClass="confirmar rounded-circle p-2 " OnClick="btnSubirProducto_Click" ValidationGroup="Grupo1" />
                        <br />
                        <asp:Label ID="lblAgregarProducto" runat="server" Text="Registar producto"></asp:Label>
                    </section>
                    <%-- btn3 - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - --%>
                    <section class="ContenedorActualizar col align-items-center close">
                        <asp:ImageButton ID="btnActualizarProducto" runat="server" ImageUrl="~/Media/Resources/arrows-spin-solid.svg" CssClass="confirmar rounded-circle p-2 " OnClick="btnActualizarProducto_Click" ValidationGroup="Grupo1" />
                        <br />
                        <asp:Label ID="lblActualizarProducto" runat="server" Text="ActualizarProducto"></asp:Label>
                    </section>
                </div>
            </article>
        </section>

    </div>
    <%-- JS - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - --%>
    <script src="../../Scripts/owl.carousel.min.js"></script>
    <script src="../../Scripts/ScriptsAsp/Carrusel.js"></script>
</asp:Content>
