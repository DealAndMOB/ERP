<%@ Page Title="" Language="C#" MasterPageFile="~/Vistas/Master/plantilla.Master" AutoEventWireup="true" CodeBehind="CrearRemision.aspx.cs" Inherits="ERP.Vistas.Ventas.CrearRemision" %>

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
            <section class="orden text-center text-light justify-content-center align-content-center container-fluid pb-5">
                <section class="row p-2 text-start">
                    <article class="col-3 col-lg-5 text-start">
                        <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/Media/Resources/arrow-left-solid.svg" CssClass="atras rounded-circle p-2" OnClick="ImageButton1_Click" />
                        <br />
                        <asp:Label ID="Label1" runat="server" Text="Atrás" Font-Bold="True">
                        </asp:Label>
                    </article>
                    <article class="col-9 col-lg-7 text-start">
                        <h1 class="pt-4 mb-2">Crear Remisión</h1>
                    </article>
                </section>


                <section class="container border-Orange bg-dark rounded-2 p-4">
                    <article class="row justify-content-around">

                        <div class="col-6  col-lg-3 border-White">
                            <div class="row border bg-light text-dark">
                                <%--<asp:Label ID="lblTextFolio" runat="server" Text="Folio" Font-Bold="True"></asp:Label>--%>
                                <asp:Button ID="GenerarFolio" runat="server" Text="Generar Folio" OnClick="GenerarFolio_Click" CssClass="btnFolio"/>
                            </div>
                            <div class="row h4 pt-2 pb-2">
                                <asp:Label ID="lblFolio" runat="server" Text="FOLIO"></asp:Label>
                            </div>
                        </div>
                        <div class="col-6  col-lg-6 border-White">
                            <div class="row border bg-light text-dark">
                                <asp:Label ID="lblClienteTitle" runat="server" Font-Bold="true" Text="Cliente"></asp:Label>
                            </div>
                            <div class="row h4 pt-2 pb-2">
                                <asp:Label ID="lblCliente" runat="server" Text=""></asp:Label>
                            </div>
                        </div>
                        <div class="col-12 col-lg-3 border-White">
                            <div class="row border bg-light text-dark">
                                <asp:Label ID="lblTextFecha" runat="server" Text="Fecha de entrega" Font-Bold="True"></asp:Label>
                            </div>
                            <div class="row justify-content-center pt-2 pb-2">
                                <asp:TextBox ID="txtFechaEntrega" runat="server" TextMode="Date" Style="width: 80%;" CssClass="Date Orange" onkeydown="return BloquearEnter(event);"></asp:TextBox>
                            </div>

                        </div>
                    </article>

                    <article class="row mt-2 p-3">
                        <h2>Partidas</h2>
                        <div class="tablaRemision">
                            <asp:GridView ID="gvPartidas" AutoGenerateColumns="False" runat="server" Style="width: 100%; margin-top: 2rem;">
                                <Columns>
                                    <%--- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -  - - - - - - - - - - - - - - - - - - - - - - - --%>
                                    <asp:TemplateField HeaderText="Seleccionar">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chbxStatus" runat="server" CssClass="check" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <%--- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -  - - - - - - - - - - - - - - - - - - - - - - - --%>
                                    <asp:BoundField DataField="Partida" HeaderText="Partida" />
                                    <asp:BoundField DataField="Nombre" HeaderText="Producto" />
                                    <asp:BoundField DataField="Categoria" HeaderText="Categoría" />
                                    <%--- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -  - - - - - - - - - - - - - - - - - - - - - - - --%>
                                    <asp:ImageField DataImageUrlField="Imagen" DataImageUrlFormatString="~/Multimedia/{0}" HeaderText="Imagen"></asp:ImageField>
                                    <%--- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -  - - - - - - - - - - - - - - - - - - - - - - - --%>
                                    <asp:BoundField DataField="Restantes" HeaderText="Disponibles" />
                                    <%--- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -  - - - - - - - - - - - - - - - - - - - - - - - --%>
                                    <asp:TemplateField HeaderText="Unidades">
                                        <ItemTemplate>
                                            <asp:Label ID="lblErrorUnidades" runat="server" ForeColor="red"></asp:Label>
                                            <br />
                                            <asp:TextBox ID="txtCantidad" runat="server" Style="width: 70%; text-align: center;" TextMode="Number" onkeydown="return BloquearEnter(event);"></asp:TextBox>
                                            <br />
                                            <asp:RegularExpressionValidator ID="REV1" runat="server" ControlToValidate="txtCantidad" ErrorMessage="VALOR INVALIDO" ForeColor="Red"
                                                ValidationExpression="^(?!0)\d+$">
                                            </asp:RegularExpressionValidator>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </article>

                    <article class="row mt-3 align-items-center">
                        <div class="align-items-center">
                            <div class="col text-center">
                                <asp:ImageButton ID="btnAlertaConfirmacion" runat="server" CssClass="confirmar rounded-circle"
                                    ImageUrl="~/Media/Resources/check.svg" OnClick="btnAlertaConfirmacion_Click" /><br />
                                <asp:Label ID="lblConfirmar" runat="server" Text="Confirmar Remisión " Font-Bold="True">
                                </asp:Label>
                            </div>
                        </div>
                    </article>

                    <%-- - - - - - - - - - - - - - - - - - - - Incio alerta - - - - - - - - - - - - - - - - - - - --%>
                    <article class="alerta alert rounded close">
                        <div class="row justify-content-center text-dark text-center">
                            <img src="../../Media/Resources/exclamation-mark-svgrepo-com.svg" alt="" class="shake iconos" style="width: 5rem;">
                            <%--<i class="fa-solid fa-circle-exclamation fa-shake" style="color: #ee9510; font-size: ;"></i>--%>
                            <h1 class="text-dark">¿Seguro que deseas continuar?</h1>
                            <p>
                                <br>
                                Se te redirigirá a la remisión.
                            </p>
                            <div class="row mt-2">
                                <div class="col-5">
                                    <asp:Button ID="btnAlertcancel" runat="server" Text="Cancelar" class="cancelar" />
                                </div>
                                <div class="col-2"></div>
                                <div class="col-5">
                                    <asp:Button ID="BtnConfirmar" runat="server" Text="Aceptar" class="aceptar" OnClick="BtnConfirmar_Click" />
                                </div>
                            </div>
                        </div>
                    </article>
                    <%-- - - - - - - - - - - - - - - - - - - -Fin alerta - - - - - - - - - - - - - - - - - - - --%>
                </section>

            </section>
            <%-- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - --%>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
