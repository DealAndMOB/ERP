<%@ Page Title="" Language="C#" MasterPageFile="~/Vistas/Master/plantilla.Master" AutoEventWireup="true" CodeBehind="Clientes.aspx.cs" Inherits="ERP.Vistas.Catalogos.Clientes" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%--<script type="text/javascript">
        function Search_Gridview(strKey, strGV) {
            var strData = strKey.value.toLowerCase().split(" ");
            var tblData = document.getElementById("<%= gvClientes.ClientID %>");
            var rowData;
            for (var i = 1; i < tblData.rows.length; i++) {
                rowData = tblData.rows[i].innerHTML;
                var styleDisplay = 'none';
                for (var j = 0; j < strData.length; j++) {
                    if (rowData.toLowerCase().indexOf(strData[j]) >= 0)
                        styleDisplay = '';
                    else {
                        styleDisplay = 'none';
                        break;
                    }
                }
                tblData.rows[i].style.display = styleDisplay;
            }
        }
    </script>--%>
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

    <div class="Clientes text-center text-light justify-content-center align-content-center container-fluid">

        <h1 class="p-4">Catálogo de clientes</h1>

        <section class="row justify-content-center">

            <article class="col-12 col-lg-8 border-Orange gv mb-2 rounded-3 bg-dark">

                <h2 class="p-4">Clientes registrados</h2>

                <div class="row align-items-center justify-content-center">

                    <div class="col text-end">
                        <asp:Label ID="lblSearchCliente" runat="server" Text="Buscar cliente por RFC"></asp:Label>
                    </div>

                    <div class="col">
                        <asp:Label ID="lblErrorBusqueda" runat="server" ForeColor="Red" Class="mb-2 h5"></asp:Label>
                        <asp:TextBox ID="txtBusquedaCliente" runat="server" CssClass="txtSeacr" onkeydown="return BloquearEnter(event);"></asp:TextBox>
                        <%--<asp:TextBox ID="txtBusquedaCliente" runat="server" CssClass="txtSeacr" onkeyup="Search_Gridview(this, 'gvClientes')"></asp:TextBox>--%>
                    </div>

                    <div class="col text-start">
                        <asp:ImageButton ID="imbtnSearchRFC" runat="server" ImageUrl="~/Media/Resources/Buscar.svg" CssClass="searchBtn rounded-circle p-2" OnClick="btnBuscarCliente_Click" />
                    </div>
                </div>

                <div class="row px-4">

                    <h3 class="p-4">Resultados</h3>

                    <div class="tabla">
                        <asp:GridView ID="gvClientes" Style="width: 100%" runat="server" OnRowUpdating="gvClientes_RowUpdating" AutoGenerateColumns="False" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="4" ForeColor="Black" GridLines="Horizontal">
                        <Columns>
                            <asp:BoundField DataField="RFC" HeaderText="RFC" />
                            <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
                            <asp:BoundField DataField="Direccion" HeaderText="Dirección" />
                            <asp:BoundField DataField="Telefono" HeaderText="Teléfono" />
                            <asp:BoundField DataField="Correo" HeaderText="Correo" />
                            <%-- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - --%>
                            <asp:ButtonField ControlStyle-CssClass="btn btn-success" CommandName="Update" Text="Actualizar" ButtonType="Button" ShowHeader="True" HeaderText="Actualizar"></asp:ButtonField>
                            <%-- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - --%>
                        </Columns>
                    </asp:GridView>
                    </div>
                </div>
            </article>

            <article class="col-12 col-lg-3 border-Green rounded-3 form bg-dark mb-2">

                <h2 class="pt-3 mb-3">Nuevo cliente</h2>

                <div class="row justify-content-center">
                    <%-- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - --%>
                    <asp:Label ID="lblErrorInterno" runat="server" ForeColor="Red" Class="mb-2 h5"></asp:Label>
                    <%-- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - --%>

                    <asp:RequiredFieldValidator ID="RFV1" runat="server" ErrorMessage="CAMPO OBLIGATORIO" ForeColor="Red" ControlToValidate="txtRFCCliente" ValidationGroup="Grupo1"></asp:RequiredFieldValidator>
                    <asp:Label ID="lblRFC" runat="server" Text="RFC"></asp:Label>
                    <asp:TextBox ID="txtRFCCliente" runat="server" CssClass="textBox mb-3 mt-1" onkeydown="return BloquearEnter(event);"></asp:TextBox>

                    <asp:RequiredFieldValidator ID="RFV2" runat="server" ErrorMessage="CAMPO OBLIGATORIO" ForeColor="Red" ControlToValidate="txtNombreCliente" ValidationGroup="Grupo1"></asp:RequiredFieldValidator>
                    <asp:Label ID="lblNombre" runat="server" Text="Nombre del cliente"></asp:Label>
                    <asp:TextBox ID="txtNombreCliente" runat="server" CssClass="textBox mb-3 mt-1" onkeydown="return BloquearEnter(event);"></asp:TextBox>

                    <asp:RequiredFieldValidator ID="RFV3" runat="server" ErrorMessage="CAMPO OBLIGATORIO" ForeColor="Red" ControlToValidate="txtDireccionCliente" ValidationGroup="Grupo1"></asp:RequiredFieldValidator>
                    <asp:Label ID="lblDireccion" runat="server" Text="Dirección del cliente"></asp:Label>
                    <asp:TextBox ID="txtDireccionCliente" runat="server" CssClass="textBox mb-3 mt-1" onkeydown="return BloquearEnter(event);"></asp:TextBox>

                    <asp:RequiredFieldValidator ID="RFV4" runat="server" ErrorMessage="CAMPO OBLIGATORIO" ForeColor="Red" ControlToValidate="txtTelefonoCliente" ValidationGroup="Grupo1"></asp:RequiredFieldValidator>
                    <asp:Label ID="lblTel" runat="server" Text="Teléfono del cliente"></asp:Label>
                    <asp:TextBox ID="txtTelefonoCliente" runat="server" CssClass="textBox mb-3 mt-1" onkeydown="return BloquearEnter(event);"></asp:TextBox>

                    <asp:RequiredFieldValidator ID="RFV5" runat="server" ErrorMessage="CAMPO OBLIGATORIO" ForeColor="Red" ControlToValidate="txtCorreo" ValidationGroup="Grupo1"></asp:RequiredFieldValidator>
                    <asp:Label ID="lblCorreo" runat="server" Text="Correo del cliente"></asp:Label>
                    <asp:TextBox ID="txtCorreo" runat="server" CssClass="textBox mb-3 mt-1" onkeydown="return BloquearEnter(event);"></asp:TextBox>
                </div>

                <%--<div class="row mt-1 mb-4 justify-content-center">
                    <asp:ImageButton ID="imgbtnConfirmar" runat="server" ImageUrl="~/Media/Resources/circle-plus-solid.svg" CssClass="confirmar rounded-circle" OnClick="btnSubirCliente_Click" ValidationGroup="Grupo1" />
                    <asp:Label ID="lblConfirmar" runat="server" Text="Agregar cliente"></asp:Label>
                </div>--%>

                <div class="row p-3 px-5">
                    <%-- btn1 - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - --%>
                    <section class="ContenedorCancelar col align-items-center subir confi close">
                        <asp:ImageButton ID="CancelarActuBTN" runat="server" ImageUrl="~/Media/Resources/close.png" CssClass="confirmar rounded-circle p-2 " ValidationGroup="Grupo1" 
                            OnClick="CancelarActuBTN_Click"/>
                        <br />
                        <asp:Label ID="lblCancelarCliente" runat="server" Text="Cancelar"></asp:Label>
                    </section>
                    <%-- btn2 - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - --%>
                    <section class="ContenedorAgregar col align-items-center">
                        <asp:ImageButton ID="btnSubirCliente" runat="server" ImageUrl="~/Media/Resources/circle-plus-solid.svg" CssClass="confirmar rounded-circle p-2" ValidationGroup="Grupo1"  
                            OnClick="btnSubirCliente_Click" />
                        <br />
                        <asp:Label ID="lblSubirCliente" runat="server" Text="Registrar Cliente"></asp:Label>
                    </section>
                    <%-- btn3 - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - --%>
                    <section class="ContenedorActualizar col align-items-center close">
                        <asp:ImageButton ID="btnActualizarCliente" runat="server" ImageUrl="~/Media/Resources/arrows-spin-solid.svg" CssClass="confirmar rounded-circle p-2" ValidationGroup="Grupo1" 
                            OnClick="btnActualizarCliente_Click"/>
                        <br />
                        <asp:Label ID="lblActualizarCliente" runat="server" Text="Actualizar Cliente"></asp:Label>
                    </section>
                </div>
            </article>

        </section>

    </div>

</asp:Content>
