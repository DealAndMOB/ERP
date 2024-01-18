<%@ Page Title="" Language="C#" MasterPageFile="~/Vistas/Master/plantilla.Master" AutoEventWireup="true" CodeBehind="Venta.aspx.cs" Inherits="ERP.Vistas.Ventas.Venta" %>

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
    <main class="orden text-center text-light justify-content-center align-content-center container-fluid pb-5">
        <section class="row p-2 text-start">
            <article class="col-3 col-lg-5 text-start">
                <asp:ImageButton ID="atras" runat="server" ImageUrl="~/Media/Resources/arrow-left-solid.svg" CssClass="atras rounded-circle p-2" OnClick="atras_Click" />
                <br />
                <asp:Label ID="lblAtras" runat="server" Text="Atrás" Font-Bold="True">
                </asp:Label>
            </article>
            <article class="col-9 col-lg-7 text-start">
                <h1 class="p-3">Gestionar venta</h1>
            </article>
        </section>


        <section class="container position-relative justify-content-center align-items-center">
            <article class=" row contenedor mb-2 border-Orange bg-dark rounded-2 justify-content-center">

                <article class="row justify-content-center mt-2 p-2">
                    <div class="col-5 col-lg-3 border-White folio">
                        <div class="row bg-light text-dark">
                            <asp:Label ID="lblTextFolio" runat="server" Text="Folio" Font-Bold="True"></asp:Label>
                        </div>
                        <div class="row text-center mt-3 h3">
                            <asp:Label ID="lblFolio" runat="server" Text="" CssClass="folioTxt"></asp:Label>
                        </div>
                    </div>
                    <%-- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - --%>
                    <div class="col-5 col-lg-6 border-White folio">
                        <div class="row bg-light text-dark ">
                            <asp:Label ID="lblClienteTitle" runat="server" Text="Cliente" Font-Bold="True"></asp:Label>
                        </div>
                        <div class="row text-center h3 mt-3">
                            <asp:Label ID="lblCliente" runat="server" Text=""></asp:Label>
                        </div>
                    </div>
                    <%-- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - --%>
                    <div class="col-5 col-lg-2 folio border-White">
                        <div class="row text-dark bg-light text-center">
                            <asp:Label ID="lblTextTotal" runat="server" Text="Total" Font-Bold="True"></asp:Label>
                        </div>
                        <div class="row text-center pt-4 h4">
                            <asp:Label ID="lblTotal" runat="server" Text=""></asp:Label>
                        </div>
                    </div>
                    <%-- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - --%>
                    <div class="col-5 col-lg-1 folio border-White">
                        <div class="row  bg-light text-dark text-center">
                            <asp:Label ID="lblRemision" Class="px-1" runat="server" Text="Remisiones" Font-Bold="True"></asp:Label>
                        </div>
                        <div class="row justify-content-center p-2">
                            <asp:ImageButton ID="btnRemision" runat="server" CssClass="btnPlus rounded-circle"
                                ImageUrl="~/Media/Resources/circle-plus-solid.svg" OnClick="btnRemision_Click" /><br />
                        </div>
                    </div>
                    <%-- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - --%>
                </article>

                <article class="row mt-4 p-3">
                    <h2>Productos</h2>
                    <div class="tablaRemision">
                        <asp:GridView ID="gvPartidas" AutoGenerateColumns="False" runat="server" Style="width: 100%; margin-top: 2rem;">
                            <Columns>
                                <asp:BoundField DataField="Partida" HeaderText="Partida" />
                                <asp:BoundField DataField="NombreProducto" HeaderText="Producto" />
                                <asp:BoundField DataField="Categoria" HeaderText="Categoría" />
                                <%--- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -  - - - - - - - - - - - - - - - - - - - - - - - --%>
                                <asp:ImageField DataImageUrlField="Imagen" DataImageUrlFormatString="~/Multimedia/{0}" HeaderText="Imagen"></asp:ImageField>
                                <%--- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -  - - - - - - - - - - - - - - - - - - - - - - - --%>
                                <asp:BoundField DataField="Cantidad" HeaderText="Cantidad" />
                                <%--- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -  - - - - - - - - - - - - - - - - - - - - - - - --%>
                                <asp:BoundField DataField="PrecioFormato" HeaderText="Precio Unitario" />
                                <asp:BoundField DataField="TotalFormato" HeaderText="Total Partida" />
                                <%--- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -  - - - - - - - - - - - - - - - - - - - - - - - --%>
                            </Columns>
                        </asp:GridView>
                    </div>
                </article>

                <article class="row mt-3 p-3 align-items-center">

                    <div class="col-12 col-lg-6  p-1 mt-2 Condiciones-CotVen">
                        <div class="CondicionesVenta">
                            <asp:Label ID="lblVenta" runat="server" Text="Condiciones de venta" Font-Bold="True"></asp:Label>
                            <asp:TextBox ID="txtCondicion" runat="server" TextMode="MultiLine" CssClass="p-2" OnTextChanged="txtCondicion_TextChanged" onkeydown="return BloquearEnter(event);"></asp:TextBox>
                        </div>
                    </div>

                    <div class="col-12 col-lg-6 mt-3 ">
                        <div class="row justify-content-center align-items-center">
                            <div class="col text-center">
                                <asp:ImageButton ID="BtnFormularioPDF" runat="server" CssClass="confirmar rounded-circle"
                                    ImageUrl="~/Media/Resources/download.png" OnClick="BtnFormularioPDF_Click" /><br />
                                <asp:Label ID="lblDescarga" runat="server" Text="Descargar PDF" Font-Bold="True"></asp:Label>
                            </div>
                            <%--<div class="col text-center">
                                    <asp:ImageButton ID="BtnConfirmar" runat="server" CssClass="confirmar rounded-circle"
                                        ImageUrl="~/Media/Resources/check.svg"/><br />
                                    <asp:Label ID="lblConfirmar" runat="server" Text="Confirmar" Font-Bold="True">
                                    </asp:Label>
                                </div>--%>
                            <div class="col">
                                <asp:ImageButton ID="BtnReutilizar" runat="server" CssClass="confirmar rounded-circle p-2"
                                    ImageUrl="~/Media/Resources/recycle-solid.svg" OnClick="BtnReutilizar_Click" /><br />
                                <asp:Label ID="lblReciclar" runat="server" Text="Reutilizar Partidas " Font-Bold="True">
                                </asp:Label>
                            </div>
                            <div class="col text-center">
                                <asp:ImageButton ID="imgbtnReporte" runat="server" CssClass="confirmar rounded-circle p-2"
                                    ImageUrl="~/Media/Resources/file-solid.svg" OnClick="imgbtnReporte_Click" /><br />
                                <asp:Label ID="lblReporte" runat="server" Text="Consultar reporte " Font-Bold="True">
                                </asp:Label>
                            </div>
                        </div>
                    </div>
                </article>

            </article>
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
    </main>
    <%-- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - --%>
    <%--</ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="BtnDescargarPDF" />
        </Triggers>
    </asp:UpdatePanel>--%>
</asp:Content>
