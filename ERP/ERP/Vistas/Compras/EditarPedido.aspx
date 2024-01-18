<%@ Page Title="" Language="C#" MasterPageFile="~/Vistas/Master/plantilla.Master" AutoEventWireup="true" CodeBehind="EditarPedido.aspx.cs" Inherits="ERP.Vistas.Compras.EditarPedido" %>

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
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <%-- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - --%>
            <section class="ECotizacion text-center text-light justify-content-center align-content-center container-fluid pb-5">

                <section class="row p-2 text-start">
                    <article class="col-3 col-lg-5 text-start">
                        <asp:ImageButton ID="imgbtnAtras" runat="server" ImageUrl="~/Media/Resources/arrow-left-solid.svg" CssClass="atras rounded-circle p-2" OnClick="imgbtnAtras_Click" />
                        <br />
                        <asp:Label ID="Label1" runat="server" Text="Atrás" Font-Bold="True">
                        </asp:Label>
                    </article>
                    <article class="col-9 col-lg-7 text-start">
                        <h1 class="p-3">Editar pedido</h1>
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
                        <div class="col-6 col-lg-2 border-White">
                            <div class="row border bg-light text-dark">
                                <asp:Label ID="lblTextTotal" runat="server" Text="Total" Font-Bold="True"></asp:Label>
                            </div>
                            <div class="row justify-content-center pt-2 pb-2 h4">
                                <asp:Label ID="lblTotal" runat="server" Text=""></asp:Label>
                            </div>
                        </div>
                        <div class="col-6 col-lg-1 border-White">
                            <div class="row border bg-light text-dark">
                                <asp:Label ID="lblStatus" runat="server" Text="Estatus" Font-Bold="True"></asp:Label>
                            </div>
                            <div class="row justify-content-center pt-2 pb-2">
                                <asp:CheckBox ID="chbxStatus" runat="server" CssClass="mt-2 check" />
                            </div>
                        </div>
                    </article>


                    <article class="mt-4">
                        <h2>Partidas</h2>
                        <%--OnTextChanged="txtCantidad_TextChanged"--%>
                        <div class="tablaRemision">
                            <asp:GridView ID="gvPartidas" AutoGenerateColumns="False" runat="server" Style="width: 100%; margin-top: 2rem;"
                                OnRowDataBound="gvPartidas_RowDataBound" OnRowUpdating="gvPartidas_RowUpdating" OnRowDeleting="gvPartidas_RowDeleting">
                                <Columns>
                                    <asp:BoundField DataField="Partida" HeaderText="Partida" />
                                    <asp:BoundField DataField="Nombre" HeaderText="Producto" />
                                    <asp:BoundField DataField="Categoria" HeaderText="Categoría" />
                                    <%--- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -  - - - - - - - - - - - - - - - - - - - - - - - --%>
                                    <asp:ImageField DataImageUrlField="Imagen" DataImageUrlFormatString="~/Multimedia/{0}" HeaderText="Imagen"></asp:ImageField>
                                    <%--- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -  - - - - - - - - - - - - - - - - - - - - - - - --%>
                                    <%--- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -  - - - - - - - - - - - - - - - - - - - - - - - --%>
                                    <asp:TemplateField HeaderText="Unidades">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtCantidad" runat="server" Style="width: 70%; text-align: center;" TextMode="Number" onkeydown="return BloquearEnter(event);"></asp:TextBox>
                                            <br />
                                            <asp:RegularExpressionValidator ID="REV1" runat="server"
                                                ControlToValidate="txtCantidad" ErrorMessage="Valor Invalido"
                                                ForeColor="Red" ValidationExpression="^(?!0)\d+$">
                                            </asp:RegularExpressionValidator>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <%--- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -  - - - - - - - - - - - - - - - - - - - - - - - --%>
                                    <asp:BoundField DataField="CostoFormato" HeaderText="Costo Unitario" />
                                    <asp:BoundField DataField="TotalFormato" HeaderText="Total Partida" />
                                    <%--- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -  - - - - - - - - - - - - - - - - - - - - - - - --%>
                                    <asp:ButtonField CommandName="Update" Text="Actualizar" ButtonType="Button" ShowHeader="True" HeaderText="Calcular" ControlStyle-CssClass="btnC rounded-3 m-2 p-2"></asp:ButtonField>
                                    <asp:ButtonField CommandName="Delete" Text="Eliminar" ButtonType="Button" ShowHeader="True" HeaderText="Eliminar Partida" ControlStyle-CssClass="btnC rounded-3 m-2 p-2"></asp:ButtonField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </article>

                    <article class="row mt-3">
                        <div class="col-12 col-lg-6 p-1 mt-2">
                            <asp:Label ID="lblCatalogosNone" runat="server">Condiciones de compra</asp:Label>
                            <asp:TextBox ID="txtCondiciones" runat="server" TextMode="MultiLine" CssClass="Condiciones rounded-3" onkeydown="return BloquearEnter(event);">
                            </asp:TextBox>
                        </div>

                        <div class="col-12 col-lg-6 mt-3 ">
                            <div class="row justify-content-center align-items-center">
                                <div class="col text-center">
                                    <asp:ImageButton ID="BtnDescargarPDF" runat="server" CssClass="confirmar rounded-circle"
                                        ImageUrl="~/Media/Resources/download.png" OnClick="BtnDescargarPDF_Click" /><br />
                                    <asp:Label ID="lblDescarga" runat="server" Text="Descargar" Font-Bold="True"></asp:Label>
                                </div>
                                <div class="col text-center">
                                    <asp:ImageButton ID="btnAlertaConfirmacion" runat="server" CssClass="confirmar rounded-circle"
                                        ImageUrl="~/Media/Resources/check.svg" OnClick="btnAlertaConfirmacion_Click" /><br />
                                    <asp:Label ID="lblConfirmar" runat="server" Text="Confirmar cambios " Font-Bold="True">
                                    </asp:Label>
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

                    <%-- ALERTA FECHA PDF - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - --%>
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

                    <%-- - - - - - - - - - - - - - - - - - - - Incio alerta - - - - - - - - - - - - - - - - - - - --%>
                    <article class="alerta alert rounded close">
                        <div class="row justify-content-center text-dark text-center">
                            <img src="../../Media/Resources/exclamation-mark-svgrepo-com.svg" alt="" class="shake iconos" style="width: 5rem;">
                            <%--<i class="fa-solid fa-circle-exclamation fa-shake" style="color: #ee9510; font-size: ;"></i>--%>
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
                                    <asp:Button ID="BTNConfirmar" runat="server" Text="Aceptar" class="aceptar" OnClick="BTNConfirmar_Click" />
                                </div>
                            </div>
                        </div>
                    </article>
                    <%-- - - - - - - - - - - - - - - - - - - -Fin alerta - - - - - - - - - - - - - - - - - - - --%>

                    <%-- - - - - - - - - - - - - - - - - - - - Incio alerta 2- - - - - - - - - - - - - - - - - - - --%>
                <article class="alerta alert2 rounded close">
                    <div class="row justify-content-center text-dark text-center">
                        <img src="../../Media/Resources/exclamation-mark-svgrepo-com.svg" alt="" class="shake iconos" style="width: 5rem;">
                        <%--<i class="fa-solid fa-circle-exclamation fa-shake" style="color: #ee9510; font-size: ;"></i>--%>
                        <h1 class="text-dark">¿Quieres confirmar esta compra? </h1>
                        <p>
                            <br>
                            Se te redirigirá al historial de pedidos.
                        </p>
                        <div class="row mt-2">
                            <div class="col-5">
                                <asp:Button ID="btnAlertcancel2" runat="server" Text="Cancelar" class="cancelar" />
                            </div>
                            <div class="col-2"></div>
                            <div class="col-5">
                                <asp:Button ID="BTNConfirmarVenta" runat="server" Text="Aceptar" class="aceptar" OnClick="BTNConfirmar_Click" />
                            </div>
                        </div>
                    </div>
                </article>
                <%-- - - - - - - - - - - - - - - - - - - -Fin alerta 2- - - - - - - - - - - - - - - - - - - --%>
                </section>
            </section>
            <%-- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - --%>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnFechaCreación" />
            <asp:PostBackTrigger ControlID="btnFechaAlterada" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
