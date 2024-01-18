<%@ Page Title="" Language="C#" MasterPageFile="~/Vistas/Master/plantilla.Master" AutoEventWireup="true" CodeBehind="Cotizar.aspx.cs" Inherits="ERP.Vistas.Ventas.Cotizar" %>

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
    <main class="Cotizaciones text-center text-light justify-content-center align-content-center container-fluid pb-5">

        <h1 class="p-3">Cotizaciones</h1>

        <section class="row position-relative justify-content-center align-items-center">

            <article class="contenedor mb-2 border-Orange bg-dark rounded-2 justify-content-center">
                <%--- - - - - - - - - - - - - - - - - - - - - - - START UpdatePanel - - - - - - - - - - - - - - - - - - - - - - --%>
                <asp:UpdatePanel ID="UpdatePanelCotizacion" runat="server">
                    <ContentTemplate>
                        <%--- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -  - - - - - - - - - - - - - - - - - - - - - - - --%>
                        <h2 class="pt-3">Nueva cotización</h2>

                        <%-- CLiente --%>
                        <div class="row align-items-center py-4">
                            <%-- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - --%>
                            <asp:Label ID="lblErrorBusqueda" runat="server" ForeColor="Red" Class="mb-1 h5"></asp:Label>
                            <div class="col text-end">
                                <asp:Label ID="lblSearchCliente" runat="server" Text="Buscar cliente"></asp:Label>
                            </div>

                            <div class="col">
                                <asp:TextBox ID="txtBusquedaCliente" runat="server" CssClass="txtSeacr" onkeydown="return BloquearEnter(event);"></asp:TextBox>
                            </div>

                            <div class="col text-start">
                                <asp:ImageButton ID="btnBuscarCliente" runat="server" ImageUrl="~/Media/Resources/Buscar.svg" CssClass="searchBtn rounded-circle p-2" OnClick="btnBuscarCliente_Click" />
                            </div>
                            <%-- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - --%>
                        </div>

                        <%-- Resultados Cliente --%>
                        <div class="row border-Green result rounded-3 Orange pt-2 mb-4">
                            <h4 class="mt-2">Resultados de búsqueda</h4>
                            <%-- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - --%>
                            <div class="tabla">

                                <asp:GridView ID="gvBusquedaCliente" Style="width: 100%; margin: .5rem 0;" runat="server" AutoGenerateColumns="False" BackColor="White" BorderColor="#336666" BorderStyle="Double" BorderWidth="3px" CellPadding="4" GridLines="Horizontal">
                                    <Columns>
                                        <asp:BoundField DataField="RFC" HeaderText="RFC" />
                                        <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
                                        <asp:BoundField DataField="Direccion" HeaderText="Dirección" />
                                        <asp:BoundField DataField="Telefono" HeaderText="Teléfono" />
                                        <asp:BoundField DataField="Correo" HeaderText="Correo" />
                                        <asp:TemplateField HeaderText="Acción">
                                            <ItemTemplate>
                                                <%-- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - --%>
                                                <asp:Button ID="btnSeleccionarCliente" runat="server" Text="Asignar Cliente" Class="btnC rounded-3 m-2 p-2" OnClick="btnSeleccionarCliente_Click" />
                                                <%-- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - --%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                        <%-- Producto --%>

                        <asp:Label ID="lblErrorBusquedaProducto" runat="server" Text="" ForeColor="Red" Class="h5"></asp:Label>
                        <div class="row align-items-center pb-4">
                            <%-- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -  --%>
                            <div class="col text-end">
                                <asp:Label ID="lblBuscarProducto" runat="server" Text="Buscar productos"></asp:Label>
                            </div>
                            <%-- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -  --%>
                            <div class="col">
                                <asp:TextBox ID="txtBusquedaProducto" runat="server" CssClass="txtSeacr" onkeydown="return BloquearEnter(event);"></asp:TextBox>
                            </div>
                            <%-- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -  --%>
                            <div class="col text-start">
                                <asp:ImageButton ID="btnBuscarProducto" runat="server" ImageUrl="~/Media/Resources/Buscar.svg" CssClass="searchBtn rounded-circle p-2" OnClick="btnBuscarProducto_Click" />
                            </div>
                            <%-- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -  --%>
                        </div>

                        <%-- Datos de la Cotización --%>
                        <div class="row border-Orange result rounded-3 White text-dark mb-4 py-3">
                            <div class="col text-end">
                                <h3>
                                    <asp:Label Class="lblCliente" ID="lblCliente" runat="server" Text="Seleccionar Cliente"></asp:Label>
                                </h3>
                            </div>

                            <div class="col text-center">
                                <h3>Folio: 
                            <asp:Label ID="lblFolio" runat="server" Text=""></asp:Label>
                                </h3>
                            </div>

                            <div class="col hola">
                                <div class="d-flex gap-3">
                                    <h3>Número de unidades: </h3>
                                    <asp:TextBox ID="txtCantidad" Style="width: 25%;" runat="server" PlaceHolder="Unidades" TextMode="Number" CssClass="Cantidad border-Orange text-center rounded-2 p-2" onkeydown="return BloquearEnter(event);"></asp:TextBox>
                                </div>
                                <div class="text-start">
                                    <asp:Label ID="lblCantidad" runat="server" ForeColor="Red" Class="h6"></asp:Label>
                                </div>
                            </div>
                        </div>
                        <%-- Resultados Productos --%>
                        <div class="row border-White result rounded-3 Green descrip-center pt-2 mb-4">
                            <h4 class="mt-2">Resultados de búsqueda</h4>
                            <%-- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -  --%>
                            <div class="tabla">
                                <asp:GridView ID="gvProductos" Style="width: 100%; margin-bottom: .5rem;" runat="server" AutoGenerateColumns="False" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="3">
                                    <Columns>
                                        <asp:BoundField DataField="Codigo" HeaderText="Código" />
                                        <asp:BoundField DataField="PrecioFormato" HeaderText="Precio Unitario" />
                                        <asp:BoundField DataField="Nombre" HeaderText="Producto" />
                                        <asp:BoundField DataField="Descripcion" HeaderText="Descripción" />
                                        <asp:BoundField DataField="NombreCategoria" HeaderText="Categoría" />
                                        <asp:ImageField DataImageUrlField="Imagen" DataImageUrlFormatString="~/Multimedia/{0}" HeaderText="Imagen"></asp:ImageField>
                                        <%-- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -  --%>
                                        <asp:TemplateField HeaderText="Cotizar">
                                            <ItemTemplate>
                                                <asp:Button ID="btnCotizar" runat="server" Text="Cotizar" Class="btnC rounded-3 m-2 p-2" OnClick="btnCotizar_Click" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                            <%-- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -  --%>
                        </div>
                        <%-- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -  --%>

                        <div class="row border-Orange result rounded-3 White text-dark mb-4 pt-2 descriPartida-center">
                            <h4 class="mt-2">Cotización</h4>
                            <div class="row px-4">
                                <div class="table-descripcion">
                                    <asp:GridView ID="gvCotizado" Style="width: 100%; margin-bottom: .5rem" runat="server" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="3" AutoGenerateColumns="False">
                                        <Columns>
                                            <asp:BoundField DataField="Partida" HeaderText="Partida" />
                                            <asp:BoundField DataField="DescripProducto" HeaderText="Descripcion" />
                                            <%--- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -  - - - - - - - - - - - - - - - - - - - - - - - --%>
                                            <asp:ImageField DataImageUrlField="Imagen" DataImageUrlFormatString="~/Multimedia/{0}" HeaderText="Imagen"></asp:ImageField>
                                            <%--- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -  - - - - - - - - - - - - - - - - - - - - - - - --%>
                                            <asp:BoundField DataField="Cantidad" HeaderText="Unidades" />
                                            <asp:BoundField DataField="PrecioFormato" HeaderText="Precio" />
                                            <asp:BoundField DataField="TotalFormato" HeaderText="Total" />
                                            <%--- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -  - - - - - - - - - - - - - - - - - - - - - - - --%>
                                            <asp:TemplateField HeaderText="Acciones">
                                                <ItemTemplate>
                                                    <div class="d-flex flex-column">
                                                        <asp:Button ID="btnAumentar" runat="server" Text="Agregar %" Class="btnC rounded-3 m-2 p-2" OnClick="btnAumentar_Click" />
                                                        <asp:Button ID="btnEliminar" runat="server" Text="Eliminar" Class="btnC rounded-3 m-2 p-2" OnClick="btnEliminar_Click" />
                                                    </div>
                                                </ItemTemplate>
                                                <%--- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -  - - - - - - - - - - - - - - - - - - - - - - - --%>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>

                        <%--- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - --%>
                        <section class="pieCotizacion result">
                            <article class="condiciones position-relative">
                                <blockquote>
                                    <asp:Label ID="lblCondiciones" runat="server" Class="h6 fs-5" Text="Condiciones de venta: "></asp:Label>
                                </blockquote>
                                <asp:TextBox ID="txtCondicion" runat="server" Style="height: 72%; width: 100%;" CssClass="rounded p-3" TextMode="MultiLine" onkeydown="return BloquearEnter(event);"></asp:TextBox>
                            </article>

                            <article class="iva">
                                <%-- Encabezado --%>
                                <div class="d-flex Orange text-dark p-2 rounded" style="border-bottom: solid 2px #212529;">
                                    <div class="col text-end">
                                        <asp:Label ID="lblSubtotalTitle" runat="server" Text="Subtotal : $" Font-Bold="True"></asp:Label>
                                    </div>

                                    <div class="col text-end">
                                        <asp:Label ID="lblSubtotal" runat="server" Text="0.00"></asp:Label>
                                    </div>

                                </div>

                                <%-- Cuerpo --%>
                                <div class="d-flex White p-2 text-dark">

                                    <div class="col mt-2 text-end">
                                        <asp:Label ID="lblIVATitle" runat="server" Text="IVA : $" Font-Bold="True"></asp:Label>
                                    </div>

                                    <div class="col mt-2 text-end">
                                        <asp:Label ID="lblIVA" runat="server" Text="0.00"></asp:Label>
                                    </div>

                                </div>

                                <div class="d-flex p-2 text-white Green rounded" style="border-top: solid 2px #212529;">

                                    <div class="col text-end">
                                        <asp:Label ID="lblTotalTitle" runat="server" Text="Total : $" Font-Bold="True"></asp:Label>
                                    </div>
                                    <div class="col text-end">
                                        <asp:Label ID="lblTotal" runat="server" Text="0.00"></asp:Label>

                                    </div>
                                </div>
                            </article>

                        </section>
                        <%--- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - --%>

                        <div class="row mt-3 justify-content-center text-center mb-4 pt-3">

                            <div class="col-3 text-end">


                                <asp:ImageButton ID="btnDescargarPDF" runat="server" ImageUrl="~/Media/Resources/download.png" CssClass="confirmar rounded-circle p-2" OnClick="btnDescargarPDF_Click" />
                                <br />
                                <asp:Label ID="lblDescargar" runat="server" Text="Descargar"></asp:Label>

                            </div>

                            <div class="col-3">

                                <asp:ImageButton ID="btnAlertaConfirmacion" runat="server" ImageUrl="~/Media/Resources/check.svg" CssClass="confirmar rounded-circle p-1" OnClick="btnAlertaConfirmacion_Click" />
                                <br />
                                <asp:Label ID="lblCotizar" runat="server" Text="Cotizar"></asp:Label>

                            </div>

                            <div class="col-3 text-start ">

                                <div class="row text-center btnNuevaCotizacion">
                                    <asp:ImageButton ID="btnNuevaCotizacion" runat="server" ImageUrl="~/Media/Resources/circle-plus-solid.svg" CssClass="confirmar rounded-circle p-2 " OnClick="btnNuevaCotizacion_Click" />
                                </div>
                                <div class="row text-start">
                                <asp:Label ID="lblNuevaC" runat="server" Text="Nueva cotización"></asp:Label>

                                </div>

                            </div>

                        </div>

                        <%-- Alerta --%>
                        <article class="alerta alert rounded close">
                            <div class="row justify-content-center text-dark text-center">
                                <img src="../../Media/Resources/exclamation-mark-svgrepo-com.svg" alt="" class="shake iconos" style="width:5rem;">
                                <%--<i class="fa-solid fa-circle-exclamation fa-shake" style="color: #ee9510; font-size: ;"></i>--%>
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
                                        <asp:Button ID="btnConfirmar" runat="server" Text="Aceptar" class="aceptar" OnClick="btnConfirmar_Click" />
                                    </div>
                                </div>
                            </div>
                        </article>
                        <%-- Fin alerta --%>

                        <%--- - - - - - - - - - - - - - - - - - - - - - - FIN UpdatePanel - - - - - - - - - - - - - - - - - - - - - - --%>
                    </ContentTemplate>
                    <Triggers>
                        <asp:PostBackTrigger ControlID="btnDescargarPDF" />
                    </Triggers>
                </asp:UpdatePanel>
                <%--- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - --%>
            </article>

            <%-- Otros Gastos --%>
            <%--- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - --%>
            <asp:UpdatePanel ID="UpdatePanelForms" runat="server">
                <ContentTemplate>
                    <article class="aumentos bg-dark border-Green text-light position-absolute rounded">

                        <div class="row justify-content-end mt-2">
                            <asp:ImageButton ID="imbtnClose" runat="server" ImageUrl="~/Media/Resources/close.png" CssClass="closeBtn" />
                        </div>

                        <h3 class="mt-2">Nuevo Aumento</h3>

                        <div class="row justify-content-center text-center">
                            <%-- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - --%>
                            <asp:Label ID="lblErrorInterno" runat="server" ForeColor="Red" Class="h5"></asp:Label>
                            <%-- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - --%>

                            <asp:RequiredFieldValidator ID="RFV1" runat="server" ErrorMessage="CAMPO OBLIGATORIO" ForeColor="Red" ControlToValidate="txtNombreAumento" ValidationGroup="Grupo1"></asp:RequiredFieldValidator>
                            <asp:Label ID="lblNombreCriterio" runat="server" Text="Descripción Breve"></asp:Label>
                            <asp:TextBox ID="txtNombreAumento" runat="server" CssClass="textBox mb-4 mt-2" Text="" onkeydown="return BloquearEnter(event);"></asp:TextBox>
                            <%-- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - --%>

                            <asp:RequiredFieldValidator ID="RFV2" runat="server" ErrorMessage="CAMPO OBLIGATORIO" ForeColor="Red" ControlToValidate="txtPorcentaje" ValidationGroup="Grupo1"></asp:RequiredFieldValidator>

                            <asp:Label ID="lblPorcentaje" runat="server" Text="Porcentaje %"></asp:Label>
                            <asp:TextBox ID="txtPorcentaje" runat="server" CssClass="textBox mt-2" Text="" onkeydown="return BloquearEnter(event);"></asp:TextBox>

                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server"
                                ControlToValidate="txtPorcentaje" ErrorMessage="Debe ingresar un número decimal válido"
                                ForeColor="Red" ValidationExpression="^\d+(\.\d+)?$" ValidationGroup="Grupo1">
                            </asp:RegularExpressionValidator>
                            <%-- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - --%>
                        </div>

                        <div class="row text-center justify-content-center mt-2 mb-3">
                            <asp:ImageButton ID="btnRegistrarAumento" runat="server" ImageUrl="~/Media/Resources/circle-plus-solid.svg"
                                CssClass="confirmar rounded-circle" OnClick="btnRegistrarAumento_Click" ValidationGroup="Grupo1" />
                            <asp:Label ID="lblAgregarCliente" runat="server" Text="Registar"></asp:Label>
                        </div>
                    </article>
                </ContentTemplate>
            </asp:UpdatePanel>
        </section>
        <%-- Pantalla de carga - - - - - -  - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - --%>
        <asp:UpdateProgress ID="UpdateProgressCliente" runat="server" AssociatedUpdatePanelID="UpdatePanelCotizacion">
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
    </main>
</asp:Content>
