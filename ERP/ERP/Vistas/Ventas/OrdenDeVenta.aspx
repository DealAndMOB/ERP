<%@ Page Title="" Language="C#" MasterPageFile="~/Vistas/Master/plantilla.Master" AutoEventWireup="true" CodeBehind="OrdenDeVenta.aspx.cs" Inherits="ERP.Vistas.Ventas.OrdenDeVenta" %>

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

                <h1 class="pt-3 mb-3">Historial de cotizaciones</h1>
                <section class="row position-relative justify-content-center align-items-center">
                    <article class="contenedor mb-2 border-Orange bg-dark rounded-2 justify-content-center">
                        <div class="row pt-2">
                            <%--- - - - - - - - - - - - - - - - - - - - - - - -- - - - - - - - - - - - - - - - - - - - - - - - - - - - - --%>
                            <div class="col-12 col-lg-6">
                                <div class="row mt-3 mb-2">
                                    <div class="col align-self-center">

                                        <h4>Estatus</h4>
                                    </div>
                                    <div class="col text-start">
                                        <asp:DropDownList ID="ddlEstado" runat="server" Class="Drop bg-dark text-light text-center" AutoPostBack="true" OnSelectedIndexChanged="ddlEstado_SelectedIndexChanged">
                                            <asp:ListItem Selected="True" disabled="True" Value="">&lt; Estado de Venta &gt;</asp:ListItem>
                                            <asp:ListItem Selected="False" Enabled="true" Value="1">Confirmadas</asp:ListItem>
                                            <asp:ListItem Selected="False" Enabled="true" Value="0">Pendientes</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                            <%--- - - - - - - - - - - - - - - - - - - - - - - -- - - - - - - - - - - - - - - - - - - - - - - - - - - - - --%>
                            <div class="col-12 col-lg-6">
                                <div class="row mt-1">

                                    <div class="col-4 align-self-center mt-1 text-end">
                                        <h4>Folio</h4>
                                    </div>

                                    <div class="col-8 text-start ">
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

                        <%--- - - - - - - - - - - - - - - - - - - - - - - -- - - - - - - - - - - - - - - - - - - - - - - - - - - - - --%>
                    </article>
                </section>
                <section class="row contenedor">
                    <article class="descripOrden-center tablaOrdenVenta">
                        <asp:GridView ID="gvCotizacionVenta" runat="server" AutoGenerateColumns="False" Style="width: 100%;">
                            <Columns>
                                <asp:BoundField DataField="Folio" HeaderText="Folio" />
                                <asp:BoundField DataField="Cliente" HeaderText="Cliente" />
                                <asp:BoundField DataField="TotalFormato" HeaderText="Total" />
                                <asp:BoundField DataField="FechaCorta" HeaderText="Fecha" />
                                <asp:BoundField DataField="Estatus" HeaderText="Estado" />
                                <asp:TemplateField HeaderText="Editar Cotización Venta">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="btnEditCotizacion" runat="server" CssClass="editar p-1 rounded-2" ImageUrl="~/Media/Resources/pen-to-square-solid.svg" OnClick="btnEditCotizacion_Click" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </article>
                </section>
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

