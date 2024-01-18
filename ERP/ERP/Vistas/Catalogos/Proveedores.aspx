<%@ Page Title="" Language="C#" MasterPageFile="~/Vistas/Master/plantilla.Master" AutoEventWireup="true" CodeBehind="Proveedores.aspx.cs" Inherits="ERP.Vistas.Catalogos.Proveedores" %>

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

    <main class="Proveedores text-center text-light justify-content-center align-content-center container-fluid pb-5">

        <h1 class="p-3">Catálogo de proveedores</h1>

        <section class="row justify-content-center">

            <article class="col-12 col-lg-9 border-Orange gvProveedor mb-2 rounded-3 bg-dark">
                <h2 class="p-4">Proveedores registrados</h2>

                <asp:Label ID="lblErrorBusqueda" runat="server" ForeColor="Red" Class="mb-2 h5"></asp:Label>

                <div class="row align-items-center">

                    <div class="col text-end">
                        <asp:Label ID="lblSearchProveedor" runat="server" Text="Buscar proveedor por zona"></asp:Label>
                    </div>
                    <%--nuevo--%>
                    <div class="col">
                        <asp:DropDownList ID="ddlFiltroZonas" AppendDataBoundItems="true" runat="server" Class="Drop bg-dark text-light text-center p-2">
                            <asp:ListItem Disabled="false" Value="">&lt;Zonas&gt;</asp:ListItem>
                            <asp:ListItem Value="0">Todos</asp:ListItem>
                        </asp:DropDownList>
                    </div>

                    <div class="col">
                        <asp:Label ID="lblOR" runat="server" Text="y por categoría"></asp:Label>
                    </div>

                    <div class="col">
                        <asp:DropDownList ID="ddlFiltroCategorias" AppendDataBoundItems="true" runat="server" Class="Drop bg-dark text-light text-center p-2">
                            <asp:ListItem Disabled="false" Value="">&lt;Categorias&gt;</asp:ListItem>
                            <asp:ListItem Value="0">Todos</asp:ListItem>
                        </asp:DropDownList>
                    </div>

                    <div class="col text-start">
                        <asp:ImageButton ID="btnBuscar" runat="server" ImageUrl="~/Media/Resources/Buscar.svg" CssClass="searchBtn rounded-circle p-2" OnClick="btnBuscar_Click" />
                    </div>

                </div>

                <div class="row px-4">

                    <h3 class="mb-5 mt-5">Resultados</h3>

                    <div class="tablaProveedor">
                        <asp:GridView ID="gvProveedores" Style="width: 100%" runat="server" OnRowUpdating="gvProveedores_RowUpdating" CellPadding="4" ForeColor="#333333" GridLines="None" AutoGenerateColumns="False">
                            <Columns>
                                <asp:BoundField DataField="RFC" HeaderText="RFC" />
                                <asp:BoundField DataField="RazonSocial" HeaderText="Razon Social" />
                                <asp:BoundField DataField="NombreContacto" HeaderText="Contacto" />
                                <asp:BoundField DataField="CorreoPagina" HeaderText="Correo / Pagina" />
                                <asp:BoundField DataField="Telefono" HeaderText="Telefono" />
                                <asp:BoundField DataField="Direccion" HeaderText="Direccion" />
                                <asp:BoundField DataField="Categoria" HeaderText="Categoría" />
                                <asp:BoundField DataField="Zona" HeaderText="Zona" />
                                <%-- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - --%>
                                <asp:ButtonField ControlStyle-CssClass="btn btn-success" CommandName="Update" Text="Actualizar" ButtonType="Button" ShowHeader="True" HeaderText="Actualizar"></asp:ButtonField>
                                <%-- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - --%>
                            </Columns>
                        </asp:GridView>
                    </div>

                </div>

            </article>

            <article class="col-12 col-lg-2 border-Green form rounded-3 bg-dark mb-1">

                <h4 class="mt-4">Nuevo proveedor</h4>

                <%-- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - --%>
                <asp:Label ID="lblErrorInterno" runat="server" ForeColor="Red" Class="mb-1 h5"></asp:Label>
                <%-- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - --%>

                <div class="row justify-content-center">
                    <asp:RequiredFieldValidator ID="RFV1" runat="server" ErrorMessage="CAMPO OBLIGATORIO" ForeColor="Red" ControlToValidate="txtRFCProveedor" ValidationGroup="Grupo1"></asp:RequiredFieldValidator>
                    <asp:Label ID="lblRFC" runat="server" Text="RFC" CssClass="lbl"></asp:Label>
                    <asp:TextBox ID="txtRFCProveedor" runat="server" CssClass="textBoxProvedor mb-1 mt-1" onkeydown="return BloquearEnter(event);"></asp:TextBox>


                    <asp:RequiredFieldValidator ID="RFV2" runat="server" ErrorMessage="CAMPO OBLIGATORIO" ForeColor="Red" ControlToValidate="txtEmpresaProveedor" ValidationGroup="Grupo1"></asp:RequiredFieldValidator>
                    <asp:Label ID="Label1" runat="server" Text="Empresa" CssClass="lbl"></asp:Label>
                    <asp:TextBox ID="txtEmpresaProveedor" runat="server" CssClass="textBoxProvedor mb-1 mt-1" onkeydown="return BloquearEnter(event);"></asp:TextBox>

                    <asp:RequiredFieldValidator ID="RFV3" runat="server" ErrorMessage="CAMPO OBLIGATORIO" ForeColor="Red" ControlToValidate="txtRazonSocialProveedor" ValidationGroup="Grupo1"></asp:RequiredFieldValidator>
                    <asp:Label ID="lblRazon" runat="server" Text="Razón Social" CssClass="lbl"></asp:Label>
                    <asp:TextBox ID="txtRazonSocialProveedor" runat="server" CssClass="textBoxProvedor mb-1 mt-1" onkeydown="return BloquearEnter(event);"></asp:TextBox>

                    <asp:RequiredFieldValidator ID="RFV4" runat="server" ErrorMessage="CAMPO OBLIGATORIO" ForeColor="Red" ControlToValidate="txtContactoProveedor" ValidationGroup="Grupo1"></asp:RequiredFieldValidator>
                    <asp:Label ID="lblContacto" runat="server" Text="Nombre del contacto" CssClass="lbl"></asp:Label>
                    <asp:TextBox ID="txtContactoProveedor" runat="server" CssClass="textBoxProvedor mb-1 mt-1" onkeydown="return BloquearEnter(event);"></asp:TextBox>

                    <asp:RequiredFieldValidator ID="RFV5" runat="server" ErrorMessage="CAMPO OBLIGATORIO" ForeColor="Red" ControlToValidate="txtDescripcionProveedor" ValidationGroup="Grupo1"></asp:RequiredFieldValidator>
                    <asp:Label ID="lblDescripcion" runat="server" Text="Descripción" CssClass="lbl"></asp:Label>
                    <asp:TextBox ID="txtDescripcionProveedor" runat="server" CssClass="textBoxProvedor mb-1 mt-1" TextMode="MultiLine" onkeydown="return BloquearEnter(event);"></asp:TextBox>

                    <asp:RequiredFieldValidator ID="RFV6" runat="server" ErrorMessage="CAMPO OBLIGATORIO" ForeColor="Red" ControlToValidate="txtCorreoPaginaProveedor" ValidationGroup="Grupo1"></asp:RequiredFieldValidator>
                    <asp:Label ID="lblCorreo" runat="server" Text="Correo o página" CssClass="lbl"></asp:Label>
                    <asp:TextBox ID="txtCorreoPaginaProveedor" runat="server" CssClass="textBoxProvedor mb-1 mt-1" onkeydown="return BloquearEnter(event);"></asp:TextBox>

                    <asp:RequiredFieldValidator ID="RFV7" runat="server" ErrorMessage="CAMPO OBLIGATORIO" ForeColor="Red" ControlToValidate="txtTelefonoProveedor" ValidationGroup="Grupo1"></asp:RequiredFieldValidator>
                    <asp:Label ID="lblTel" runat="server" Text="Teléfono" CssClass="lbl"></asp:Label>
                    <asp:TextBox ID="txtTelefonoProveedor" runat="server" CssClass="textBoxProvedor mb-1 mt-1" onkeydown="return BloquearEnter(event);"></asp:TextBox>

                    <asp:RequiredFieldValidator ID="RFV8" runat="server" ErrorMessage="CAMPO OBLIGATORIO" ForeColor="Red" ControlToValidate="txtDireccionProveedor" ValidationGroup="Grupo1"></asp:RequiredFieldValidator>
                    <asp:Label ID="lblDireccion" runat="server" Text="Dirección" CssClass="lbl"></asp:Label>
                    <asp:TextBox ID="txtDireccionProveedor" runat="server" CssClass="textBoxProvedor mb-1 mt-1" onkeydown="return BloquearEnter(event);"></asp:TextBox>

                    <asp:RequiredFieldValidator ID="RFV9" runat="server" ErrorMessage="CAMPO OBLIGATORIO" ForeColor="Red" ControlToValidate="ddlEstados" ValidationGroup="Grupo1"></asp:RequiredFieldValidator>
                    <asp:Label ID="lblEstado" runat="server" Text="Estado" CssClass="lbl"></asp:Label>
                    <asp:DropDownList ID="ddlEstados" AppendDataBoundItems="true" runat="server" Class="dd mt-1 rounded-3 mb-2 text-center">
                        <asp:ListItem Value="">&lt;Estado&gt;</asp:ListItem>
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="RFV10" runat="server" ErrorMessage="CAMPO OBLIGATORIO" ForeColor="Red" ControlToValidate="ddlCategoriaProveedor" ValidationGroup="Grupo1"></asp:RequiredFieldValidator>
                    <asp:Label ID="lblCategoria" runat="server" Text="Categoría" CssClass="lbl"></asp:Label>
                    <asp:DropDownList ID="ddlCategoriaProveedor" AppendDataBoundItems="true" runat="server" Class="dd mt-1  rounded-3 text-center">
                        <asp:ListItem Value="">&lt;Categoría&gt;</asp:ListItem>
                    </asp:DropDownList>

                </div>
                <div class="row p-3 px-5">
                    <%-- btn1 - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - --%>
                    <section class="ContenedorCancelar col align-items-center subir confi close">
                        <asp:ImageButton ID="CancelarActuBTN" runat="server" ImageUrl="~/Media/Resources/close.png" CssClass="confirmar rounded-circle p-2 " ValidationGroup="Grupo1"
                            OnClick="CancelarActuBTN_Click" />
                        <br />
                        <asp:Label ID="lblCancelar" runat="server" Text="Cancelar" CssClass="lbl"></asp:Label>
                    </section>
                    <%-- btn2 - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - --%>
                    <section class="ContenedorAgregar col align-items-center">
                        <asp:ImageButton ID="btnAgregarProveedor" runat="server" ImageUrl="~/Media/Resources/circle-plus-solid.svg" CssClass="confirmar rounded-circle p-2" ValidationGroup="Grupo1"
                            OnClick="btnAgregarProveedor_Click" />
                        <br />
                        <asp:Label ID="lblSubirProveedor" runat="server" Text="Registrar Proveedor" CssClass="lbl"></asp:Label>
                    </section>
                    <%-- btn3 - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - --%>
                    <section class="ContenedorActualizar col align-items-center close">
                        <asp:ImageButton ID="btnActualizarProveedor" runat="server" ImageUrl="~/Media/Resources/arrows-spin-solid.svg" CssClass="confirmar rounded-circle p-2" ValidationGroup="Grupo1"
                            OnClick="btnActualizarProveedor_Click" />
                        <br />
                        <asp:Label ID="lblActualizarProveedor" runat="server" Text="Actualizar Proveedor" CssClass="lbl"></asp:Label>
                    </section>
                </div>

            </article>

        </section>

    </main>

</asp:Content>
