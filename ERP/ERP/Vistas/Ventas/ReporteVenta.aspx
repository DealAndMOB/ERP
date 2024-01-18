<%@ Page Title="" Language="C#" MasterPageFile="~/Vistas/Master/plantilla.Master" AutoEventWireup="true" CodeBehind="ReporteVenta.aspx.cs" Inherits="ERP.Vistas.Ventas.ReporteVenta" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <section class="RFinanciero text-center text-light justify-content-center align-content-center container-fluid">
        <div class="pb-4"></div>
        <section class="row p-2 text-start">
            <article class="col-3 col-lg-4 text-start">
                <asp:ImageButton ID="imgbtnAtrasVenta" runat="server" ImageUrl="~/Media/Resources/arrow-left-solid.svg" CssClass="atras rounded-circle p-2" OnClick="imgbtnAtrasVenta_Click" />
                <br />
                <asp:Label ID="Label2" runat="server" Text="Atrás" Font-Bold="True">
                </asp:Label>
            </article>
            <article class="col-3 col-lg-7 position-absolute mt-3 start-50 translate-middle d-flex flex-column align-items-center">
                <h1 class="pt-2 mb-2">UTILIDADES</h1>
            </article>
        </section>

        <article class="descrip-center-Reporte row p-2 tabla" style="width: 90%; margin: auto;">

            <asp:GridView ID="GvDatosCotizacion" runat="server" AutoGenerateColumns="false" Style="width: 100%; margin-bottom: .5rem">
                <Columns>
                    <asp:BoundField DataField="Partida" HeaderText="Partida" />
                    <asp:BoundField DataField="NombreProducto" HeaderText="Producto" />
                    <asp:BoundField DataField="Unidades" HeaderText="Unidades" />
                    <asp:BoundField DataField="CostoFormato" HeaderText="Costo" />
                    <asp:BoundField DataField="TotalPartidaCostoFormato" HeaderText="Gasto Total" />
                    <asp:BoundField DataField="PrecioFormato" HeaderText="Precio" />
                    <asp:BoundField DataField="TotalPartidaVentaFormato" HeaderText="Venta Total" />
                    <asp:BoundField DataField="MargenBruto" HeaderText="Utilidad Bruto" />
                </Columns>
            </asp:GridView>


        </article>

        <article class="row p-2 mt-5" style="width: 90%; margin: auto;">

            <div class="col-12 col-lg-6 mt-3 descrip-center-Reporte">
                <h1 class="">Aumentos sobre precios de venta</h1>
                <div class="tabla">
                    <asp:GridView ID="GvGastosExtra" runat="server" AutoGenerateColumns="false" Style="width: 100%; margin-bottom: .5rem">
                        <Columns>
                            <asp:BoundField DataField="Partida" HeaderText="Partida" />
                            <asp:BoundField DataField="NombreProducto" HeaderText="Producto" />
                            <asp:BoundField DataField="CriterioAumento" HeaderText="Nombre" />
                            <asp:BoundField DataField="BaseFormato" HeaderText="Precio Base" />
                            <asp:BoundField DataField="Porcentaje" HeaderText="PA" />
                            <asp:BoundField DataField="MontoAumento" HeaderText="Monto Agregado" />
                            <asp:BoundField DataField="PrecioFormato" HeaderText="Precio Final" />
                        </Columns>

                    </asp:GridView>
                </div>
            </div>

            <div class="col row mt-3">
                <h1 class="">IVA a favor y por pagar</h1>
                <%-- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -  --%>
                <div class="col col-lg-6 texto">
                    <article class="iva">
                        <%-- Encabezado --%>
                        <div class="d-flex Orange text-dark p-2 rounded" style="border-bottom: solid 2px #212529;">
                            <div class="col text-end">
                                <asp:Label ID="lblSubtotalGastos" runat="server" Text="Subtotal : $" Font-Bold="True"></asp:Label>
                            </div>

                            <div class="col text-end">
                                <asp:Label ID="lblSubGastos" runat="server" Text="0.00"></asp:Label>
                            </div>

                        </div>

                        <%-- Cuerpo --%>
                        <div class="d-flex White p-2 text-dark">

                            <div class="col mt-2 text-end">
                                <asp:Label ID="lblIVA" runat="server" Text="IVA : $" Font-Bold="True"></asp:Label>
                            </div>

                            <div class="col mt-2 text-end">
                                <asp:Label ID="lblValorIVA" runat="server" Text="0.00"></asp:Label>
                            </div>

                        </div>

                        <div class="d-flex p-2 text-white Green rounded" style="border-top: solid 2px #212529;">

                            <div class="col text-end">
                                <asp:Label ID="Gastos" runat="server" Text="Gasto : $" Font-Bold="True"></asp:Label>
                            </div>
                            <div class="col text-end">
                                <asp:Label ID="TotalGastos" runat="server" Text="0.00"></asp:Label>

                            </div>
                        </div>
                    </article>
                </div>
                <%-- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -  --%>
                <div class="col col-lg-6 texto">
                    <article class="iva">
                        <%-- Encabezado --%>
                        <div class="d-flex Orange text-dark p-2 rounded" style="border-bottom: solid 2px #212529;">
                            <div class="col text-end">
                                <asp:Label ID="lblSubtotalTitle" runat="server" Text="SubTotal : $" Font-Bold="True"></asp:Label>
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
                                <asp:Label ID="lblIVA2" runat="server" Text="0.00"></asp:Label>
                            </div>

                        </div>

                        <div class="d-flex p-2 text-white Green rounded" style="border-top: solid 2px #212529;">

                            <div class="col text-end">
                                <asp:Label ID="lblTotalTitle" runat="server" Text="Venta : $" Font-Bold="True"></asp:Label>
                            </div>
                            <div class="col text-end">
                                <asp:Label ID="lblTotal" runat="server" Text="0.00"></asp:Label>

                            </div>
                        </div>
                    </article>
                </div>
                <%-- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -  --%>
                <div class="mt-3">
                    <div class="iva d-flex White text-dark p-2 rounded gap-3" style="border-bottom: solid 2px #212529;">

                        <div class="col text-end">
                            <asp:Label ID="Label3" runat="server" Text="Utilidad Bruta Total sin IVA : $ " Font-Bold="True"></asp:Label>
                        </div>

                        <div class="col text-start">
                            <asp:Label ID="MargenSinIVAtotal" runat="server" Text=" 0.00"></asp:Label>
                        </div>
                    </div>

                    <div class="mt-1 iva d-flex Orange text-dark p-2 rounded gap-3" style="border-bottom: solid 2px #212529;">

                        <div class="col text-end">
                            <asp:Label ID="Label5" runat="server" Text="Utilidad Bruta Total con  IVA : $ " Font-Bold="True"></asp:Label>
                        </div>

                        <div class="col text-start">
                            <asp:Label ID="MargenConIVAtotal" runat="server" Text=" 0.00"></asp:Label>
                        </div>
                    </div>
                </div>

            </div>
        </article>

        <br />
        <article class="mt-4">

            <asp:ImageButton ID="ImgDescargarPDF" ImageUrl="~/Media/Resources/download.png" CssClass="confirmar rounded-circle p-2" runat="server" OnClick="ImgDescargarPDF_Click" /><br />
            <asp:Label ID="Label1" runat="server" Text="Descargar Reporte"></asp:Label>


        </article>



    </section>

</asp:Content>
