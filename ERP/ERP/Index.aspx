<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="ERP.Index" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Login</title>
    <link href="Media/Resources/LOGO CUBO.png" rel="Icon" />
    <link href="Content/bootstrap.min.css" rel="stylesheet" />
    <script src="https://kit.fontawesome.com/b0cb49c5f4.js" crossorigin="anonymous"></script>
    <%-- sweetalert - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - --%>
    <script src="https://unpkg.com/sweetalert/dist/sweetalert.min.js"></script>
    <script src="Scripts/ScriptsAsp/Alertas.js"></script>
</head>
<body class="sesion text-center" onload="bloquearNavegacion();">
    <form id="form1" runat="server">
        <header class=" bg-dark p-2 text-light text-center">
            <a class="navbar-brand mb-5" href="Index.aspx">
                <asp:Image ID="Logo" runat="server" ImageUrl="~/Media/Resources/LOGO CUBO.png" alt="Logo" Width="58" Height="48" class="d-inline-block align-text-center" />
                <font clas="font">AGC COMERCIAL</font>
            </a>
        </header>
        <br/>
        <br/>
        <section class="container justify-content-center border mt-5 login text-center text-light mb-3">
            <h2 class="fw-bold mb-2 text-uppercase mb-3 mt-5 text-center">Iniciar sesión</h2>
            <p class="text-white-50 mb-5 text-center">Por favor ingrese usuario y contraseña!</p>
            <asp:TextBox ID="txtCorreo" runat="server" class="textBox mb-2"></asp:TextBox>
            <p>Correo</p>
            <asp:TextBox ID="txtContraseña" runat="server" class="textBox mt-2 mb-2" TextMode="Password"></asp:TextBox> 
            <p class="mb-2">Contraseña</p>
            <br />
            <asp:Button ID="btnIniciar" runat="server" Text="Iniciar sesión" Class="btnSesion mt-3 mb-5" OnClick="btnIniciar_Click" />
        </section>
    </form>

    <script src="Scripts/bootstrap.min.js"></script>
</body>
</html>
