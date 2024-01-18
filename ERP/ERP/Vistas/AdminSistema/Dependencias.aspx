<%@ Page Title="" Language="C#" MasterPageFile="~/Vistas/Master/plantilla.Master" AutoEventWireup="true" CodeBehind="Dependencias.aspx.cs" Inherits="ERP.Vistas.AdminSistema.Dependencias" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <meta charset="UTF-8">
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
    <div class="categoria text-center text-light justify-content-center align-content-center container-fluid pb-5">

        <h1 class="p-3">Categorías</h1>

        <section class="row position-relative justify-content-center align-items-center border-Green rounded-3 bg-dark conten py-5">

            <%-- Primer fila --%>
            <article class="row justify-content-center gap-2 ">

                <div class="col-12 col-lg-5 rounded-2">
                    <h3 class="mt-1">Categoría de Productos</h3>
                    <div class="contaner  mt-3 ">
                        <div class="tabla-dependecia">
                            <%-- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - --%>
                            <asp:GridView ID="gvCatProducto" Style="width: 100%" runat="server" OnRowUpdating="gvCatProducto_RowUpdating" AutoGenerateColumns="False" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="4" ForeColor="Black" GridLines="Horizontal">
                                <Columns>

                                    <asp:BoundField DataField="Nombre" HeaderText="Categoría" />
                                    <%-- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - --%>
                                    <asp:ButtonField ControlStyle-CssClass="btn btn-success" CommandName="Update" Text="Actualizar" ButtonType="Button" ShowHeader="True" HeaderText="Actualizar"></asp:ButtonField>
                                    <%-- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - --%>
                                </Columns>

                            </asp:GridView>
                        </div>

                        <div class="row mt-1 justify-content-center">
                            <asp:ImageButton ID="btnProductos" runat="server" ImageUrl="~/Media/Resources/circle-plus-solid.svg" CssClass="mt-3 confirmar rounded-circle" OnClick="btnProductos_Click" />
                            <asp:Label ID="lblConfirmar" runat="server" Text="Nueva categoría"></asp:Label>
                        </div>
                    </div>
                    <%-- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - --%>
                </div>

                <div class="col-12 col-lg-5 rounded-2">
                    <h3 class="mt-1">Categoría de Proveedores</h3>
                    <div class="contaner mt-3">
                        <div class="tabla-dependecia">
                            <%-- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - --%>
                            <asp:GridView ID="gvCatProveedor" Style="width: 100%" runat="server" OnRowUpdating="gvCatProveedor_RowUpdating" AutoGenerateColumns="False" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="4" ForeColor="Black" GridLines="Horizontal">
                                <Columns>

                                    <asp:BoundField DataField="Nombre" HeaderText="Categoría" />
                                    <%-- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - --%>
                                    <asp:ButtonField ControlStyle-CssClass="btn btn-success" CommandName="Update" Text="Actualizar" ButtonType="Button" ShowHeader="True" HeaderText="Actualizar"></asp:ButtonField>
                                    <%-- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - --%>
                                </Columns>

                            </asp:GridView>
                            <%-- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - --%>
                        </div>

                        <div class="row mt-1 justify-content-center">
                            <asp:ImageButton ID="btnProveedores" runat="server" ImageUrl="~/Media/Resources/circle-plus-solid.svg" CssClass="mt-3 confirmar rounded-circle" OnClick="btnProveedores_Click" />
                            <asp:Label ID="Label1" runat="server" Text="Nueva categoría"></asp:Label>
                        </div>
                    </div>
                    <%-- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - --%>
                </div>

            </article>

            <%-- Segunda Fila --%>
            <article class="row mt-4 justify-content-center gap-2">

                <div class="col-12 col-lg-5 rounded-2">
                    <h3 class="mt-1">Zonas</h3>
                    <div class="contaner mt-3">
                        <div class="tabla-dependecia">
                            <%-- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - --%>
                            <asp:GridView ID="gvZonas" Style="width: 100%" runat="server" OnRowUpdating="gvZonas_RowUpdating" AutoGenerateColumns="False" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="4" ForeColor="Black" GridLines="Horizontal">
                                <Columns>

                                    <asp:BoundField DataField="Zona" HeaderText="Zona" />
                                    <%-- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - --%>
                                    <asp:ButtonField ControlStyle-CssClass="btn btn-success" CommandName="Update" Text="Actualizar" ButtonType="Button" ShowHeader="True" HeaderText="Actualizar"></asp:ButtonField>
                                    <%-- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - --%>
                                </Columns>

                            </asp:GridView>
                            <%-- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - --%>
                        </div>

                        <div class="row mt-1 justify-content-center">
                            <asp:ImageButton ID="btnZonas" runat="server" ImageUrl="~/Media/Resources/circle-plus-solid.svg" CssClass="mt-3 confirmar rounded-circle" OnClick="btnZonas_Click" />
                            <asp:Label ID="Label2" runat="server" Text="Nueva zona"></asp:Label>
                        </div>
                    </div>
                    <%-- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - --%>
                </div>

                <div class="col-12 col-lg-5 rounded-2">
                    <h3 class="mt-1">Estados </h3>
                    <div class="contaner mt-3">
                        <div class="tabla-dependecia">
                            <%-- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - --%>
                            <asp:GridView ID="gvEstados" Style="width: 100%" runat="server" OnRowUpdating="gvEstados_RowUpdating" AutoGenerateColumns="False" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="4" ForeColor="Black" GridLines="Horizontal">
                                <Columns>

                                    <asp:BoundField DataField="Nombre" HeaderText="Estado" />
                                    <asp:BoundField DataField="Zona" HeaderText="Zona" />
                                    <%-- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - --%>
                                    <asp:ButtonField ControlStyle-CssClass="btn btn-success" CommandName="Update" Text="Actualizar" ButtonType="Button" ShowHeader="True" HeaderText="Actualizar"></asp:ButtonField>
                                    <%-- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - --%>
                                </Columns>

                            </asp:GridView>

                            <%-- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - --%>
                        </div>

                        <div class="row mt-1 justify-content-center">
                            <asp:ImageButton ID="imgbtnEstados" runat="server" ImageUrl="~/Media/Resources/circle-plus-solid.svg" CssClass="mt-3 confirmar rounded-circle" OnClick="imgbtnEstados_Click" />
                            <asp:Label ID="Label3" runat="server" Text="Nuevo estado"></asp:Label>
                        </div>
                        <%-- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - --%>
                    </div>
                </div>

            </article>
            <%-- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - --%>
            <%-- Subir CatProductos --%>
            <article class="bg-dark border-Orange  text-light position-absolute rounded categorias close ProductoC">
                <div class="row justify-content-end mt-2 m-2">
                    <asp:ImageButton ID="imbtnCloseP" runat="server" ImageUrl="~/Media/Resources/close.png" CssClass="closeBtn" />
                </div>
                <h4>Nueva categoría para productos</h4>
                <br />
                <asp:RequiredFieldValidator ID="RFV1" runat="server" ErrorMessage="CAMPO OBLIGATORIO" ForeColor="Red" ControlToValidate="txtAddCatProducto" ValidationGroup="Grupo1"></asp:RequiredFieldValidator>
                <br />
                <asp:Label ID="lblAddProducto" runat="server" Text="Nombre"></asp:Label>
                <br />
                <asp:TextBox ID="txtAddCatProducto" runat="server" CssClass="textBox mt-2 mb-3" onkeydown="return BloquearEnter(event);"></asp:TextBox>
                <br />
                <asp:Button ID="btnSubirCatProducto" runat="server" Text="Añadir" CssClass="btns mb-3 Green border-Orange" OnClick="btnSubirCatProducto_Click" ValidationGroup="Grupo1" />
            </article>

            <%-- Actualizar CatProductos --%>
            <article class="bg-dark border-Orange  text-light position-absolute rounded categorias close UpdateProductosC">
                <div class="row justify-content-end mt-2 m-2">
                    <asp:ImageButton ID="btnUpdateCloseP" runat="server" ImageUrl="~/Media/Resources/close.png" CssClass="closeBtn" />
                </div>
                <h4>Nueva categoria para productos</h4>
                <br />
                <asp:RequiredFieldValidator ID="RFV2" runat="server" ErrorMessage="CAMPO OBLIGATORIO" ForeColor="Red" ControlToValidate="txtUpdateCatProducto" ValidationGroup="Grupo2"></asp:RequiredFieldValidator>
                <br />
                <asp:Label ID="lblUpdateProducto" runat="server" Text="Nombre"></asp:Label>
                <br />
                <asp:TextBox ID="txtUpdateCatProducto" runat="server" CssClass="textBox mt-2 mb-3" onkeydown="return BloquearEnter(event);"></asp:TextBox>
                <br />
                <asp:Button ID="btnActualizarCatProducto" runat="server" Text="Actualizar" CssClass="btns mb-3 Green border-Orange" OnClick="btnActualizarCatProducto_Click" ValidationGroup="Grupo2" />
            </article>
            <%-- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - --%>

            <%-- Subir CatProveedores --%>
            <article class="bg-dark border-Orange  text-light position-absolute rounded categorias close ProveedorC">
                <div class="row justify-content-end mt-2 m-2">
                    <asp:ImageButton ID="btnAddCloseProvee" runat="server" ImageUrl="~/Media/Resources/close.png" CssClass="closeBtn" />
                </div>
                <h4>Nueva categora para proveedores</h4>
                <br />
                <asp:RequiredFieldValidator ID="RFV3" runat="server" ErrorMessage="CAMPO OBLIGATORIO" ForeColor="Red" ControlToValidate="txtAddCatProveedor" ValidationGroup="Grupo3"></asp:RequiredFieldValidator>
                <br />
                <asp:Label ID="lblAddProvee" runat="server" Text="Nombre"></asp:Label>
                <br />
                <asp:TextBox ID="txtAddCatProveedor" runat="server" CssClass="textBox mb-3" onkeydown="return BloquearEnter(event);"></asp:TextBox>
                <br />
                <asp:Button ID="btnSubirCatProveedor" runat="server" Text="Añadir" CssClass="btns mb-3 Green border-Orange" OnClick="btnSubirCatProveedor_Click" ValidationGroup="Grupo3" />
            </article>

            <%-- Actualizar CatProveedores --%>
            <article class="bg-dark border-Orange  text-light position-absolute rounded categorias close UpdateProveedorC">
                <div class="row justify-content-end mt-2 m-2">
                    <asp:ImageButton ID="btnUpdateCloseProvee" runat="server" ImageUrl="~/Media/Resources/close.png" CssClass="closeBtn" />
                </div>
                <h4>Nueva categoria para proveedores</h4>
                <br />
                <asp:RequiredFieldValidator ID="RFV4" runat="server" ErrorMessage="CAMPO OBLIGATORIO" ForeColor="Red" ControlToValidate="txtUpdateeCatProveedor" ValidationGroup="Grupo4"></asp:RequiredFieldValidator>
                <br />
                <asp:Label ID="lblUpdateProvee" runat="server" Text="Nombre"></asp:Label>
                <br />
                <asp:TextBox ID="txtUpdateeCatProveedor" runat="server" CssClass="textBox mb-3" onkeydown="return BloquearEnter(event);"></asp:TextBox>
                <br />
                <asp:Button ID="btnActualizarCatProveedor" runat="server" Text="Actualizar" CssClass="btns mb-3 Green border-Orange" OnClick="btnActualizarCatProveedor_Click" ValidationGroup="Grupo4" />
            </article>

            <%-- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - --%>

            <%--Subir Zona --%>
            <article class="bg-dark border-Orange text-light position-absolute rounded categorias close fomularioZona">
                <div class="row justify-content-end mt-2 m-2">
                    <asp:ImageButton ID="btnAddCloseZona" runat="server" ImageUrl="~/Media/Resources/close.png" CssClass="closeBtn" />
                </div>
                <h4>Zona</h4>
                <br />
                <asp:RequiredFieldValidator ID="RFV5" runat="server" ErrorMessage="CAMPO OBLIGATORIO" ForeColor="Red" ControlToValidate="txtAddZona" ValidationGroup="Grupo5"></asp:RequiredFieldValidator>
                <br />
                <asp:Label ID="lblAddZona" runat="server" Text="Nombre de la Zona"></asp:Label>
                <br />
                <asp:TextBox ID="txtAddZona" runat="server" TextMode="SingleLine" CssClass="textBox mb-3 mt-2" onkeydown="return BloquearEnter(event);"></asp:TextBox>
                <br />
                <asp:Button ID="btnSubirZona" runat="server" Text="Añadir" CssClass="btns mb-3 Green border-Orange" ValidationGroup="Grupo5" OnClick="btnSubirZona_Click" />
            </article>

            <%-- Actualizar Zona --%>
            <article class="bg-dark border-Orange text-light position-absolute rounded categorias  close UpdateZona">
                <div class="row justify-content-end mt-2 m-2">
                    <asp:ImageButton ID="btnUpdateCloseZona" runat="server" ImageUrl="~/Media/Resources/close.png" CssClass="closeBtn" />
                </div>
                <h4>Zona</h4>
                <br />
                <asp:RequiredFieldValidator ID="RFV6" runat="server" ErrorMessage="CAMPO OBLIGATORIO" ForeColor="Red" ControlToValidate="txtUpdateZona" ValidationGroup="Grupo6"></asp:RequiredFieldValidator>
                <br />
                <asp:Label ID="lblUpdateZona" runat="server" Text="Nombre de la Zona"></asp:Label>
                <br />
                <asp:TextBox ID="txtUpdateZona" runat="server" TextMode="SingleLine" CssClass="textBox mb-3 mt-2" onkeydown="return BloquearEnter(event);"></asp:TextBox>
                <br />
                <asp:Button ID="btnActualizarZona" runat="server" Text="Actualizar" CssClass="btns mb-3 Green border-Orange" OnClick="btnActualizarZona_Click" ValidationGroup="Grupo6" />
            </article>

            <%-- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - --%>

            <%-- Subir Estado --%>
            <article class="bg-dark border-Orange text-light position-absolute rounded categorias close fomularioEstado">
                <div class="row justify-content-end mt-2 m-2">
                    <asp:ImageButton ID="btnAddCloseEstado" runat="server" ImageUrl="~/Media/Resources/close.png" CssClass="closeBtn" />
                </div>
                <h4>Estados</h4>
                <br />
                <asp:RequiredFieldValidator ID="RFV7" runat="server" ErrorMessage="CAMPO OBLIGATORIO" ForeColor="Red" ControlToValidate="txtAddEstado" ValidationGroup="Grupo7"></asp:RequiredFieldValidator>
                <br />
                <asp:Label ID="lblAddEstado" runat="server" Text="Nombre"></asp:Label>
                <br />
                <asp:TextBox ID="txtAddEstado" runat="server" CssClass="textBox mb-3 mt-2" onkeydown="return BloquearEnter(event);"></asp:TextBox>
                <br />
                <asp:RequiredFieldValidator ID="RFV8" runat="server" ErrorMessage="CAMPO OBLIGATORIO" ForeColor="Red" ControlToValidate="ddlAddZona" ValidationGroup="Grupo7"></asp:RequiredFieldValidator>
                <br />
                <asp:Label ID="lblEstado" runat="server" Text="Zona"></asp:Label>
                <br />
                <asp:DropDownList ID="ddlAddZona" AppendDataBoundItems="true" runat="server" Class="ddRegistro mt-2 rounded-3 mb-4 text-center p-1 mb-4">
                    <asp:ListItem Value="">&lt;Zonas&gt;</asp:ListItem>
                </asp:DropDownList>
                <br />
                <asp:Button ID="btnSubirEstado" runat="server" Text="Añadir" CssClass="btns mb-3 Green border-Orange" OnClick="btnSubirEstado_Click" ValidationGroup="Grupo7" />
            </article>

            <%-- Actualizar Estado --%>
            <article class="bg-dark border-Orange text-light position-absolute rounded categorias close UpdateEstado">
                <div class="row justify-content-end mt-2 m-2">
                    <asp:ImageButton ID="btnUpdateCloseEstado" runat="server" ImageUrl="~/Media/Resources/close.png" CssClass="closeBtn" />
                </div>
                <h4>Estados</h4>
                <br />
                <asp:RequiredFieldValidator ID="RFV9" runat="server" ErrorMessage="CAMPO OBLIGATORIO" ForeColor="Red" ControlToValidate="txtUpdateEstado" ValidationGroup="Grupo8"></asp:RequiredFieldValidator>
                <br />
                <asp:Label ID="lblUpdateEstado" runat="server" Text="Nombre"></asp:Label>
                <br />
                <asp:TextBox ID="txtUpdateEstado" runat="server" CssClass="textBox mb-3 mt-2" onkeydown="return BloquearEnter(event);"></asp:TextBox>
                <br />
                <asp:Label ID="lblEstadoddl" runat="server" Text="Zona"></asp:Label>
                <br />
                <asp:RequiredFieldValidator ID="RFV10" runat="server" ErrorMessage="CAMPO OBLIGATORIO" ForeColor="Red" ControlToValidate="ddlUpdateZona" ValidationGroup="Grupo8"></asp:RequiredFieldValidator>
                <br />
                <asp:DropDownList ID="ddlUpdateZona" AppendDataBoundItems="true" runat="server" Class="ddRegistro mt-2 rounded-3 mb-4 text-center p-1 mb-4">
                    <asp:ListItem Value="">&lt;Zonas&gt;</asp:ListItem>
                </asp:DropDownList>
                <br />
                <asp:Button ID="btnActualizarEstado" runat="server" Text="Actualizar" CssClass="btns mb-3 Green border-Orange" OnClick="btnActualizarEstado_Click" ValidationGroup="Grupo8" />
            </article>

            <%-- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - --%>
        </section>
    </div>
</asp:Content>
