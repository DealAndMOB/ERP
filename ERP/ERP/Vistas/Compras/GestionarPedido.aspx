<%@ Page Title="" Language="C#" MasterPageFile="~/Vistas/Master/plantilla.Master" AutoEventWireup="true" CodeBehind="GestionarPedido.aspx.cs" Inherits="ERP.Vistas.Compras.GestionarPedido" %>

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
    <%--<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>--%>
    <%-- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - --%>
    <section class="orden text-center text-light justify-content-center align-content-center container-fluid pb-5">

        <section class="row p-2 text-start">
            <article class="col-3 col-lg-5 text-start">
                <asp:ImageButton ID="imgbtnAtrasPedido" runat="server" ImageUrl="~/Media/Resources/arrow-left-solid.svg" CssClass="atras rounded-circle p-2" OnClick="imgbtnAtrasPedido_Click"  />
                <br />
                <asp:Label ID="Label1" runat="server" Text="Atrás" Font-Bold="True">
                </asp:Label>
            </article>
            <article class="col-9 col-lg-7 text-start">
                <h1 class="p-3">Gestionar pedido</h1>
            </article>
        </section>

        <section class="container border-Orange bg-dark rounded-2 p-4">

            <article class="row justify-content-around">

                <div class="col-6  col-lg-3 border-White">
                    <div class="row border bg-light text-dark">
                        <asp:Label ID="lblTextFolio" runat="server" Text="Folio" Font-Bold="True"></asp:Label>
                    </div>
                    <div class="row h4 pt-2 pb-2">
                        <asp:Label ID="lblFolio" runat="server" Text=""></asp:Label>
                    </div>
                </div>
                <div class="col-6  col-lg-6 border-White">
                    <div class="row border bg-light text-dark">
                        <asp:Label ID="lblProveedorTitle" runat="server" Font-Bold="true" Text="Proveedor"></asp:Label>
                    </div>
                    <div class="row h4 pt-2 pb-2">
                        <asp:Label ID="lblProveedor" runat="server" Text=""></asp:Label>
                    </div>
                </div>
                <div class="col-12 col-lg-3 border-White">
                    <div class="row border bg-light text-dark">
                        <asp:Label ID="lblTextTotal" runat="server" Text="Total" Font-Bold="True"></asp:Label>
                    </div>
                    <div class="row justify-content-center pt-2 pb-2 h4">
                        <asp:Label ID="lblTotal" runat="server" Text=""></asp:Label>
                    </div>

                </div>
            </article>

            <article class="row mt-3 p-3">
                <h2>Partidas</h2>
                <div class="tablaRemision">
                    <asp:GridView ID="gvPartidas" AutoGenerateColumns="False" runat="server" Style="width: 100%; margin-top: 2rem;">
                        <Columns>
                            <asp:BoundField DataField="Partida" HeaderText="Partida" />
                            <asp:BoundField DataField="Nombre" HeaderText="Producto" />
                            <asp:BoundField DataField="Categoria" HeaderText="Categoria" />
                            <%--- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -  - - - - - - - - - - - - - - - - - - - - - - - --%>
                            <asp:ImageField DataImageUrlField="Imagen" DataImageUrlFormatString="~/Multimedia/{0}" HeaderText="Imagen"></asp:ImageField>
                            <%--- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -  - - - - - - - - - - - - - - - - - - - - - - - --%>
                            <asp:BoundField DataField="Cantidad" HeaderText="Cantidad" />
                            <%--- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -  - - - - - - - - - - - - - - - - - - - - - - - --%>
                            <asp:BoundField DataField="CostoFormato" HeaderText="Costo Unitario" />
                            <asp:BoundField DataField="TotalFormato" HeaderText="Total Partida" />
                            <%--- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -  - - - - - - - - - - - - - - - - - - - - - - - --%>
                        </Columns>
                    </asp:GridView>
                </div>
            </article>

            <article class="row mt-3 align-items-center">

                <div class="col-12 col-lg-6 p-1 mt-2">
                    <div class="CondicionesVenta">
                        <asp:Label ID="lblVenta" runat="server" Text="Condiciones de compra" Font-Bold="True"></asp:Label>
                        <asp:TextBox ID="txtCondiciones" runat="server" TextMode="MultiLine" CssClass="Condiciones rounded-3" OnTextChanged="txtCondiciones_TextChanged" onkeydown="return BloquearEnter(event);">
                        </asp:TextBox>
                    </div>
                </div>

                <div class="col-12 col-lg-6 mt-3 ">
                    <div class="row justify-content-center align-items-center">
                        <div class="col text-center">
                            <asp:ImageButton ID="BtnFormularioPDF" runat="server" CssClass="confirmar rounded-circle"
                                ImageUrl="~/Media/Resources/download.png" OnClick="BtnFormularioPDF_Click" /><br />
                            <asp:Label ID="lblDescarga" runat="server" Text="Descargar PDF" Font-Bold="True"></asp:Label>
                        </div>
                        <div class="col">
                            <asp:ImageButton ID="BtnReutilizar" runat="server" CssClass="confirmar rounded-circle p-2"
                                ImageUrl="~/Media/Resources/recycle-solid.svg" OnClick="BtnReutilizar_Click" /><br />
                            <asp:Label ID="lblReciclar" runat="server" Text="Reutilizar Partidas " Font-Bold="True">
                            </asp:Label>
                        </div>
                    </div>
                </div>
            </article>
        </section>
        <%-- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - --%>
        <article class="alerta bg-dark border-White text-light rounded-2 Calendario close">
            <div class="row justify-content-end mb-4">
                <asp:ImageButton ID="imbtnClose" runat="server" ImageUrl="~/Media/Resources/close.png" CssClass="closeBtn" />
            </div>
            <div class="row">
                <h4>Indique con que fecha se creará el PDF</h4>
            </div>
            <div class="row justify-content-center mt-3 mb-2">
                <asp:RequiredFieldValidator ID="RFV1" runat="server" ErrorMessage="CAMPO OBLIGATORIO" ForeColor="Red" ControlToValidate="txtFechaEntrega" ValidationGroup="PDFAlterado"></asp:RequiredFieldValidator>
                <asp:TextBox ID="txtFechaEntrega" runat="server" TextMode="Date" CssClass="Date Orange" onkeydown="return BloquearEnter(event);"></asp:TextBox>
            </div>
            <div class="row mt-4">
                <div class="col-5">
                    <asp:Button ID="btnFechaCreación" runat="server" Text="Fecha de creación" class="cancelar" OnClick="btnFechaCreación_Click" />
                </div>
                <div class="col-2"></div>
                <div class="col-5">
                    <asp:Button ID="btnFechaAlterada" runat="server" Text="Actualizar fecha" class="aceptar" OnClick="btnFechaAlterada_Click" ValidationGroup="PDFAlterado" />
                </div>
            </div>
        </article>
        <%-- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - --%>
    </section>
    <%-- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - --%>
    <%-- </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="BtnDescargarPDF" />
        </Triggers>
    </asp:UpdatePanel>--%>
</asp:Content>
