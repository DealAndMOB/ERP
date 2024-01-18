<%@ Page Title="" Language="C#" MasterPageFile="~/Vistas/Master/plantilla.Master" AutoEventWireup="true" CodeBehind="Usuarios.aspx.cs" Inherits="ERP.Vistas.AdminSistema.Usuarios" %>

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
    <main class="usuarios  text-center text-light justify-content-center align-content-center container-fluid ">
        <h1 class="p-1">Administración de usuarios</h1>
        <section class="container text-start justify-content-center align-items-center mt-3">
            <article class="row">
                <div class="col-6 col-lg-1">
                    <div class="row text-center justify-content-center">
                        <asp:ImageButton ID="imbtnNuevoUsuario" runat="server" ImageUrl="~/Media/Resources/Agregar ususario.svg" CssClass="imgBtn rounded-circle p-3 mb-2 mr" OnClick="imbtnNuevoUsuario_Click" />
                        <p>Nuevo usuario</p>
                    </div>
                </div>
                <div class="col-6 col-lg-1">
                    <div class="row text-center justify-content-center">
                        <asp:ImageButton ID="imbtnNuevoNivel" runat="server" ImageUrl="~/Media/Resources/user-lock-solid.svg" CssClass="imgBtn rounded-circle p-3 mb-2 mr" OnClick="imbtnNuevoNivel_Click" />
                        <p>Nuevo perfil</p>
                    </div>
                </div>
                <div class="col-12 col-lg-10 "></div>
            </article>
        </section>
        <section class="container text-center justify-content-center align-items-center position-relative">
            <article class="usuario bg-dark border-Green text-light position-absolute rounded">

                <div class="row justify-content-end m-2">
                    <asp:ImageButton ID="imbtnClose" runat="server" ImageUrl="~/Media/Resources/close.png" CssClass="closeBtn" OnClick="imbtnClose_Click" />
                </div>

                <h4 class="mb-2">Nuevo usuario</h4>

                <div class="row justify-content-center text-center mt-2">
                    <%--- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -  ---%>
                    <asp:RequiredFieldValidator ID="RFV1" runat="server" ErrorMessage="CAMPO OBLIGATORIO" ForeColor="Red" ControlToValidate="txtNombre" ValidationGroup="Grupo1"></asp:RequiredFieldValidator>
                    <asp:Label ID="lblNombre" runat="server" Text="Nombre del usuario"></asp:Label>
                    <asp:TextBox ID="txtNombre" runat="server" CssClass="textBox mb-3 mt-3" onkeydown="return BloquearEnter(event);"></asp:TextBox>

                    <%--- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -  ---%>
                    <asp:RequiredFieldValidator ID="RFV2" runat="server" ErrorMessage="CAMPO OBLIGATORIO" ForeColor="Red" ControlToValidate="txtCorreo" ValidationGroup="Grupo1"></asp:RequiredFieldValidator>
                    <asp:Label ID="lblCorreo" runat="server" Text="Correo del usuario "></asp:Label>
                    <asp:TextBox ID="txtCorreo" runat="server" CssClass="textBox mb-3 mt-3" onkeydown="return BloquearEnter(event);"></asp:TextBox>

                    <%--- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -  ---%>
                    <asp:RequiredFieldValidator ID="RFV3" runat="server" ErrorMessage="CAMPO OBLIGATORIO" ForeColor="Red" ControlToValidate="txtContraseña" ValidationGroup="Grupo1"></asp:RequiredFieldValidator>
                    <asp:Label ID="lblPass" runat="server" Text="Contraseña del usuario "></asp:Label>
                    <asp:TextBox ID="txtContraseña" runat="server" CssClass="textBox mb-3 mt-3" TextMode="Password" onkeydown="return BloquearEnter(event);"></asp:TextBox>

                    <%--- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -  ---%>
                    <asp:RequiredFieldValidator ID="RFV4" runat="server" ErrorMessage="CAMPO OBLIGATORIO" ForeColor="Red" ControlToValidate="ddlPerfiles" ValidationGroup="Grupo1"></asp:RequiredFieldValidator>
                    <asp:Label ID="lblNivel" runat="server" Text="Nivel del usuario " CssClass="mt-1 mb-2"></asp:Label>
                    <asp:DropDownList ID="ddlPerfiles" runat="server" AppendDataBoundItems="true" Class="ddRegistro text-center p-2 mt-2 mb-3 rounded-2">
                        <asp:ListItem Selected="true" Value="">&lt; Perfiles &gt;</asp:ListItem>
                    </asp:DropDownList>

                </div>

                <div class="row text-center justify-content-center mt-2 mb-3">
                    <asp:ImageButton ID="btnSubirUsuario" runat="server" ImageUrl="~/Media/Resources/circle-plus-solid.svg" CssClass="confirmar rounded-circle" OnClick="btnSubirUsuario_Click" ValidationGroup="Grupo1" />
                    <asp:Label ID="lblConfirmarCliente" runat="server" Text="Registar usuario"></asp:Label>
                </div>

            </article>
            <%--Actualizar Usuario - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - --%>
            <article class="nivel mt-2 bg-dark border-Green text-light position-absolute rounded">

                <div class="row justify-content-end m-2">
                    <asp:ImageButton ID="imgbtnCloseNivel" runat="server" ImageUrl="~/Media/Resources/close.png" CssClass="closeBtn" OnClick="imgbtnCloseNivel_Click" />
                </div>

                <h4 class="mb-2">Perfiles</h4>

                <%-- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - --%>
                <div class="row justify-content-center text-center mt-2">
                    <asp:RequiredFieldValidator ID="RFV9" runat="server" ErrorMessage="CAMPO OBLIGATORIO" ForeColor="Red" ControlToValidate="txtPerfil" ValidationGroup="Grupo3"></asp:RequiredFieldValidator>
                    <asp:Label ID="lblNomNivel" runat="server" Text="Nombre del nivel"></asp:Label>
                    <asp:TextBox ID="txtPerfil" runat="server" CssClass="textBox mb-3 mt-3" onkeydown="return BloquearEnter(event);"></asp:TextBox>
                </div>
                <%-- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - --%>
                <h4 class="mt-3 mb-2">Módulos</h4>
                <div class="row mt-3 text-center">
                    <div class="col text-end">
                        <%-- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - --%>
                        <div class=" row mb-2">
                            <div class="col text-end">
                                <asp:Label ID="lblCompras" runat="server" Text="Compras"></asp:Label>
                            </div>
                            <div class="col text-start">
                                <asp:CheckBox ID="ChBoxCompras" runat="server" />
                            </div>
                        </div>
                        <%-- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - --%>
                        <div class="col mb-2">
                            <div class=" row">
                                <div class="col text-end">
                                    <asp:Label ID="lblCatalogos" runat="server" Text="Catálogos"></asp:Label>
                                </div>
                                <div class="col text-start">
                                    <asp:CheckBox ID="chBoxCatalogos" runat="server" />
                                </div>
                            </div>
                        </div>
                        <%-- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - --%>
                    </div>

                    <div class="col text-start">
                        <%-- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - --%>
                        <div class=" row">
                            <div class="col text-end">
                                <asp:Label ID="lblVentas" runat="server" Text="Ventas"></asp:Label>
                            </div>
                            <div class="col text-start">
                                <asp:CheckBox ID="ChBoxVentas" runat="server" />
                            </div>
                        </div>
                        <%-- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - --%>

                        <div class="col">
                            <div class=" row">
                                <div class="col text-end">
                                    <asp:Label ID="lblSistema" runat="server" Text="Sistema"></asp:Label>
                                </div>
                                <div class="col text-start">
                                    <asp:CheckBox ID="ChBoxSistema" runat="server" />
                                </div>
                            </div>
                        </div>
                        <%-- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - --%>
                    </div>
                </div>

                <div class="row text-center justify-content-center mt-2 mb-3">
                    <asp:ImageButton ID="btnSubirPerfil" runat="server" ImageUrl="~/Media/Resources/circle-plus-solid.svg" CssClass="confirmar rounded-circle" OnClick="btnPerfil_Click" ValidationGroup="Grupo3" />
                    <asp:Label ID="lblNuevoNivel" runat="server" Text="Registrar Perfil" CssClass="mt-3"></asp:Label>
                </div>
            </article>

            <%--Actualizar Perfil - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - --%>
            <article class="UpdateNivel mt-2  bg-dark border-Green text-light position-absolute rounded">

                <div class="row justify-content-end m-2">
                    <asp:ImageButton ID="btnUpdateCloseNivel" runat="server" ImageUrl="~/Media/Resources/close.png" CssClass="closeBtn" OnClick="imgbtnCloseNivel_Click" />
                </div>

                <h4 class="mb-2">Perfiles</h4>

                <%-- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - --%>
                <div class="row justify-content-center text-center mt-2">
                    <asp:RequiredFieldValidator ID="RFV10" runat="server" ErrorMessage="CAMPO OBLIGATORIO" ForeColor="Red" ControlToValidate="txtUpdatePerfil" ValidationGroup="Grupo4"></asp:RequiredFieldValidator>
                    <asp:Label ID="Label1" runat="server" Text="Nombre del nivel"></asp:Label>
                    <asp:TextBox ID="txtUpdatePerfil" runat="server" CssClass="textBox mb-3 mt-3" onkeydown="return BloquearEnter(event);"></asp:TextBox>
                </div>

                <h4 class="mt-3 mb-2">Módulos</h4>
                <%-- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - --%>
                <div class="row mt-3 text-center">
                    <div class="col text-end">
                        <%-- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - --%>
                        <div class=" row mb-2">
                            <div class="col text-end">
                                <asp:Label ID="Label2" runat="server" Text="Compras"></asp:Label>
                            </div>
                            <div class="col text-start">
                                <asp:CheckBox ID="ChBoxUpdateCompras" runat="server" />
                            </div>
                        </div>
                        <%-- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - --%>
                        <div class="col mb-2">
                            <div class=" row">
                                <div class="col text-end">
                                    <asp:Label ID="Label3" runat="server" Text="Catálogos"></asp:Label>
                                </div>
                                <div class="col text-start">
                                    <asp:CheckBox ID="ChBoxUpdateCatalogos" runat="server" />
                                </div>
                            </div>
                        </div>
                        <%-- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - --%>
                    </div>

                    <div class="col text-start">
                        <%-- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - --%>
                        <div class=" row">
                            <div class="col text-end">
                                <asp:Label ID="Label4" runat="server" Text="Ventas"></asp:Label>
                            </div>
                            <div class="col text-start">
                                <asp:CheckBox ID="ChBoxUpdateVentas" runat="server" />
                            </div>
                        </div>
                        <%-- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - --%>

                        <div class="col">
                            <div class=" row">
                                <div class="col text-end">
                                    <asp:Label ID="Label6" runat="server" Text="Sistema"></asp:Label>
                                </div>
                                <div class="col text-start">
                                    <asp:CheckBox ID="ChBoxUpdateSistema" runat="server" />
                                </div>
                            </div>
                        </div>
                        <%-- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - --%>
                    </div>
                </div>
                <%-- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - --%>

                <div class="row text-center justify-content-center mt-2 mb-3">
                    <asp:ImageButton ID="btnActualizarPerfil" runat="server" ImageUrl="~/Media/Resources/circle-plus-solid.svg" CssClass="confirmar rounded-circle" OnClick="btnActualizarPerfil_Click" ValidationGroup="Grupo4" />
                    <asp:Label ID="Label5" runat="server" Text="Actualizar Nivel" CssClass="mt-3"></asp:Label>
                </div>
            </article>
            <%-- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - --%>
            <article class="UpdateUsuario bg-dark border-Green text-light position-absolute rounded">

                <div class="row justify-content-end m-2">
                    <asp:ImageButton ID="imbtnClose2" runat="server" ImageUrl="~/Media/Resources/close.png" CssClass="closeBtn" OnClick="imbtnClose_Click" />
                </div>

                <h4 class="mb-2">Nuevo usuario</h4>

                <div class="row justify-content-center text-center mt-2">
                    <%--- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -  ---%>
                    <asp:RequiredFieldValidator ID="RFV5" runat="server" ErrorMessage="CAMPO OBLIGATORIO" ForeColor="Red" ControlToValidate="txtUpdateNombre" ValidationGroup="Grupo2"></asp:RequiredFieldValidator>
                    <asp:Label ID="lblUpdateNombre" runat="server" Text="Nombre del usuario"></asp:Label>
                    <asp:TextBox ID="txtUpdateNombre" runat="server" CssClass="textBox mb-3 mt-3" onkeydown="return BloquearEnter(event);"></asp:TextBox>

                    <%--- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -  ---%>
                    <asp:RequiredFieldValidator ID="RFV6" runat="server" ErrorMessage="CAMPO OBLIGATORIO" ForeColor="Red" ControlToValidate="txtUpdateCorreo" ValidationGroup="Grupo2"></asp:RequiredFieldValidator>
                    <asp:Label ID="lblUpdateCorreo" runat="server" Text="Correo del usuario "></asp:Label>
                    <asp:TextBox ID="txtUpdateCorreo" runat="server" CssClass="textBox mb-3 mt-3" onkeydown="return BloquearEnter(event);"></asp:TextBox>

                    <%--- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -  ---%>
                    <asp:RequiredFieldValidator ID="RFV7" runat="server" ErrorMessage="CAMPO OBLIGATORIO" ForeColor="Red" ControlToValidate="txtUpdateContraseña" ValidationGroup="Grupo2"></asp:RequiredFieldValidator>
                    <asp:Label ID="lblUpdatePass" runat="server" Text="Contraseña del usuario "></asp:Label>
                    <asp:TextBox ID="txtUpdateContraseña" runat="server" CssClass="textBox mb-3 mt-3" onkeydown="return BloquearEnter(event);"></asp:TextBox>

                    <%--- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -  ---%>
                    <asp:RequiredFieldValidator ID="RFV8" runat="server" ErrorMessage="CAMPO OBLIGATORIO" ForeColor="Red" ControlToValidate="ddlUpdatePerfiles" ValidationGroup="Grupo2"></asp:RequiredFieldValidator>
                    <asp:Label ID="lblUpdateNivel" runat="server" Text="Nivel del usuario " CssClass="mt-1 mb-2"></asp:Label>
                    <asp:DropDownList ID="ddlUpdatePerfiles" runat="server" AppendDataBoundItems="true" Class="ddRegistro text-center p-2 mt-2 mb-3 rounded-2">
                        <asp:ListItem Selected="true" Value="">&lt; Perfiles &gt;</asp:ListItem>
                    </asp:DropDownList>

                </div>

                <div class="row text-center justify-content-center mt-2 mb-3">
                    <asp:ImageButton ID="btnActualizarUsuario" runat="server" ImageUrl="~/Media/Resources/circle-plus-solid.svg" CssClass="confirmar rounded-circle" OnClick="btnActualizarUsuario_Click" ValidationGroup="Grupo2" />
                    <asp:Label ID="lblUpdateConfirmarCliente" runat="server" Text="Actualizar usuario"></asp:Label>
                </div>

            </article>
        </section>
        <section class="container text-center justify-content-center align-items-center position-relative  p-3">



            <%-- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - --%>

            <%-- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - --%>
            <article class="border-Orange p-1 rounded-3 bg-dark flex-column align-items-center mb-4">
                <h3 class="mt-1 mb-3">Usuarios en el sistema </h3>
            </article>
            <%-- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - --%>
            <article>
                <div class="tabla">
                    <asp:GridView ID="gvUsuarios" Style="width: 100%" runat="server" OnRowDeleting="gvUsuarios_RowDeleting" OnRowUpdating="gvUsuarios_RowUpdating" AutoGenerateColumns="False" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="4" ForeColor="Black" GridLines="Horizontal">
                        <Columns>
                            <asp:BoundField DataField="ID" HeaderText="ID"></asp:BoundField>
                            <asp:BoundField DataField="Nombre" HeaderText="Nombre"></asp:BoundField>
                            <asp:BoundField DataField="Correo" HeaderText="Correo"></asp:BoundField>
                            <%--<asp:BoundField DataField="Contraseña"  HeaderText="Contraseña" Visible="false"></asp:BoundField>--%>
                            <asp:BoundField DataField="Perfil" HeaderText="Perfil"></asp:BoundField>
                            <%-- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - --%>
                            <asp:ButtonField ControlStyle-CssClass="btn btn-success" CommandName="Update" Text="Actualizar" ButtonType="Button" ShowHeader="True" HeaderText="Actualizar"></asp:ButtonField>
                            <%-- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - --%>
                            <asp:ButtonField ControlStyle-CssClass="btn btn-danger" CommandName="Delete" Text="Eliminar" ButtonType="Button" ShowHeader="True" HeaderText="Eliminar"></asp:ButtonField>
                            <%-- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - --%>
                        </Columns>
                    </asp:GridView>
                </div>

                <%-- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - --%>
                <article class="border-Orange p-1 rounded-3 bg-dark flex-column align-items-center mb-4 mt-4">
                    <h3 class="mt-1 mb-3">Perfiles en el sistema </h3>
                </article>
                <%-- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - --%>

                <div class="tabla">
                    <asp:GridView ID="gvPerfiles" Style="width: 100%" runat="server" OnRowDeleting="gvPerfiles_RowDeleting" OnRowUpdating="gvPerfiles_RowUpdating" AutoGenerateColumns="False" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="4" ForeColor="Black" GridLines="Horizontal">
                        <Columns>
                            <asp:BoundField DataField="ID" HeaderText="ID"></asp:BoundField>
                            <asp:BoundField DataField="Nombre" HeaderText="Nombre"></asp:BoundField>
                            <%-- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - --%>
                            <asp:ButtonField ControlStyle-CssClass="btn btn-success" CommandName="Update" Text="Actualizar" ButtonType="Button" ShowHeader="True" HeaderText="Actualizar"></asp:ButtonField>
                            <%-- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - --%>
                            <asp:ButtonField ControlStyle-CssClass="btn btn-danger" CommandName="Delete" Text="Eliminar" ButtonType="Button" ShowHeader="True" HeaderText="Eliminar"></asp:ButtonField>
                            <%-- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - --%>
                        </Columns>
                    </asp:GridView>
                </div>

            </article>
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
                            <asp:Button ID="btnEliminarUsuario" runat="server" Text="Aceptar" class="aceptar" OnClick="btnEliminarUsuario_Click" />
                        </div>
                    </div>
                </div>
            </article>

            <article class="alerta alert2 rounded close">
                <div class="row justify-content-center text-dark text-center">
                    <img src="../../Media/Resources/exclamation-mark-svgrepo-com.svg" alt="" class="shake iconos" style="width: 5rem;">
                    <h1 class="text-dark">¿Seguro que deseas continuar?</h1>
                    <p>
                        <br>
                        No podras deshacer este paso.
                    </p>
                    <div class="row mt-2">
                        <div class="col-5">
                            <asp:Button ID="btnCancel" runat="server" Text="Cancelar" class="cancelar" />
                        </div>
                        <div class="col-2"></div>
                        <div class="col-5">
                            <asp:Button ID="btnEliminarPerfil" runat="server" Text="Aceptar" class="aceptar" OnClick="btnEliminarPerfil_Click" />
                        </div>
                    </div>
                </div>
            </article>
            <%-- Fin alerta --%>
        </section>
    </main>
</asp:Content>

