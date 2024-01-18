<%@ Page Title="" Language="C#" MasterPageFile="~/Vistas/Master/plantilla.Master" AutoEventWireup="true" CodeBehind="Remisiones.aspx.cs" Inherits="ERP.Vistas.Ventas.Remisiones" %>

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
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <%--- - - - - - - - - - - - - - - - - - - - - - - START UpdatePanel - - - - - - - - - - - - - - - - - - - - - - --%>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <%--- - - - - - - - - - - - - - - - - - - - - - - -- - - - - - - - - - - - - - - - - - - - - - - - - - - - - --%>
            <main class="orden text-center text-light justify-content-center align-content-center container-fluid pb-5">
                <section class="row p-2 text-start">
                    <article class="col-3 col-lg-5 text-start">
                        <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/Media/Resources/arrow-left-solid.svg" CssClass="atras rounded-circle p-2" OnClick="ImageButton1_Click"/>
                        <br />
                        <asp:Label ID="lblAtras" runat="server" Text="Atrás" Font-Bold="True">
                        </asp:Label>
                    </article>
                    <article class="col-9 col-lg-7 text-start">
                        <h1 class="pt-4 mb-5">Remisiones</h1>
                    </article>
                </section>


                <article class="contenedor mb-2 border-Orange bg-dark rounded-2 justify-content-center p-2">

                    <div class="row">
                        <%--- - - - - - - - - - - - - - - - - - - - - - - -- - - - - - - - - - - - - - - - - - - - - - - - - - - - - --%>
                        <div class="col-12 col-lg-6">
                            <div class="row mt-3 mb-2">
                                <div class="col align-self-center text-end">
                                    <h4>Crear Remisión:</h4>
                                </div>
                                <div class="col text-start">
                                    <asp:ImageButton ID="btnCrearRemision" runat="server" CssClass="btnPlus rounded-circle"
                                        ImageUrl="~/Media/Resources/circle-plus-solid.svg" OnClick="btnCrearRemision_Click" /><br />
                                </div>
                            </div>
                        </div>
                        <%--- - - - - - - - - - - - - - - - - - - - - - - -- - - - - - - - - - - - - - - - - - - - - - - - - - - - - --%>
                        <div class="col-12 col-lg-6">
                            <div class="row mt-3">

                                <div class="col align-self-center mt-1 text-end">
                                    <h4>Folio</h4>
                                </div>

                                <div class="col text-start ">
                                    <div class="row ">

                                        <div class="col m-1 align-self-center text-start">
                                            <asp:TextBox ID="txtBusquedaFolio" runat="server" CssClass="txtSeacr" onkeydown="return BloquearEnter(event);"></asp:TextBox>
                                        </div>

                                        <div class="col-4 text-start align-self-center">
                                            <asp:ImageButton ID="btnBuscarFolio" runat="server" ImageUrl="~/Media/Resources/Buscar.svg" CssClass="searchBtn rounded-circle p-2" OnClick="btnBuscarFolio_Click"/>
                                        </div>

                                    </div>
                                </div>
                            </div>
                        </div>
                        <%--- - - - - - - - - - - - - - - - - - - - - - - -- - - - - - - - - - - - - - - - - - - - - - - - - - - - - --%>
                    </div>
                </article>

                <article class="row contenedor descripOrden-center">
                    <div class="tablaRemision">
                         <asp:GridView ID="gvRemisiones" runat="server" AutoGenerateColumns="False" Style="width: 100%;">
                        <Columns>
                            <asp:BoundField DataField="Folio" HeaderText="Folio" />
                            <asp:BoundField DataField="FolioVenta" HeaderText="FolioVenta" />
                            <asp:BoundField DataField="Cliente" HeaderText="Cliente" />
                            <asp:BoundField DataField="FechaFormato" HeaderText="FechaEntrega" />
                            <%--- - - - - - - - - - - - - - - - - - - - - - - -- - - - - - - - - - - - - - - - - - - - - - - - - - - - - --%>
                            <asp:TemplateField HeaderText="Consultar Remision">
                                <ItemTemplate>
                                    <asp:ImageButton ID="btnRemision" runat="server" CssClass="editar p-1 rounded-2" ImageUrl="~/Media/Resources/pen-to-square-solid.svg" OnClick="btnRemision_Click" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <%--- - - - - - - - - - - - - - - - - - - - - - - -- - - - - - - - - - - - - - - - - - - - - - - - - - - - - --%>
                        </Columns>
                    </asp:GridView>
                    </div>
                </article>

            </main>
            <%--- - - - - - - - - - - - - - - - - - - - - - - -- - - - - - - - - - - - - - - - - - - - - - - - - - - - - --%>
        </ContentTemplate>
    </asp:UpdatePanel>
    <%--- - - - - - - - - - - - - - - - - - - - - - - FIN UpdatePanel - - - - - - - - - - - - - - - - - - - - - - --%>
    <%-- Pantalla de carga - - - - - -  - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - --%>
    <asp:UpdateProgress ID="UpdateProgress" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
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
</asp:Content>

