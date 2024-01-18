<%@ Page Title="" Language="C#" MasterPageFile="~/Vistas/Master/plantilla.Master" AutoEventWireup="true" CodeBehind="Remision.aspx.cs" Inherits="ERP.Vistas.Ventas.Remision" %>

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
                <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/Media/Resources/arrow-left-solid.svg" CssClass="atras rounded-circle p-2" OnClick="ImageButton1_Click" />
                <br />
                <asp:Label ID="Label1" runat="server" Text="Atrás" Font-Bold="True">
                </asp:Label>
            </article>
            <article class="col-9 col-lg-7 text-start">
                <h1 class="pt-2 mb-2">Consultar remisión</h1>
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
                        <asp:Label ID="lblClienteTitle" runat="server" Text="Cliente" Font-Bold="true"></asp:Label>
                    </div>
                    <div class="row h4 pt-2 pb-2">
                        <asp:Label ID="lblCliente" runat="server" Text=""></asp:Label>
                    </div>
                </div>
                <div class="col-12 col-lg-3 border-White">
                    <div class="row border bg-light text-dark">
                        <asp:Label ID="lblTextFecha" runat="server" Text="Fecha de entrega" Font-Bold="True"></asp:Label>
                    </div>
                    <div class="row justify-content-center pt-2 pb-2 h4">
                        <asp:Label ID="lblFecha" runat="server" Text=""></asp:Label>
                    </div>
                </div>
            </article>

            <article class="row mt-4 p-3 descriPartida-center ">
                <h2>Partidas</h2>
                <div class="tablaRemision">
                    <asp:GridView ID="gvPartidas" AutoGenerateColumns="False" runat="server" Style="width: 100%; margin-top: 2rem;">
                        <Columns>
                            <asp:BoundField DataField="Partida" HeaderText="Partida" />
                            <asp:BoundField DataField="Descripcion" HeaderText="Descripcion" />
                            <%--- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -  - - - - - - - - - - - - - - - - - - - - - - - --%>
                            <asp:ImageField DataImageUrlField="Imagen" DataImageUrlFormatString="~/Multimedia/{0}" HeaderText="Imagen"></asp:ImageField>
                            <%--- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -  - - - - - - - - - - - - - - - - - - - - - - - --%>
                            <asp:BoundField DataField="Cantidad" HeaderText="Cantidad" />
                            <%--- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -  - - - - - - - - - - - - - - - - - - - - - - - --%>
                        </Columns>
                    </asp:GridView>
                </div>
            </article>

            <article class="row mt-3 align-items-center">
                <div class="col text-center">
                    <asp:ImageButton ID="BtnFormularioPDF" runat="server" CssClass="confirmar rounded-circle"
                        ImageUrl="~/Media/Resources/download.png" OnClick="BtnFormularioPDF_Click" /><br />
                    <asp:Label ID="lblDescarga" runat="server" Text="Descargar PDF" Font-Bold="True"></asp:Label>
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
    </main>
    <%-- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - --%>
    <%--</ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="BtnDescargarPDF" />
        </Triggers>
    </asp:UpdatePanel>--%>
</asp:Content>
