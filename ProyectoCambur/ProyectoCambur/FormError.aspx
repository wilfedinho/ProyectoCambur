<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FormError.aspx.cs" Inherits="FormError" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta name="viewport" content="width=device-width, initial-scale=1.0"/>
    <title>Cambur</title>
    <link href="EstilosPaginas/Shared.css"    rel="stylesheet" type="text/css"/>
    <link href="EstilosPaginas/FormError.css" rel="stylesheet" type="text/css"/>
</head>
<body class="pagina-centrada">
    <form id="form1" runat="server">
        <div class="error-card">
            <div class="error-icono">⚠️</div>
            <div class="logotype">CAM<span>BUR</span></div>
            <asp:Label ID="lblMensaje" runat="server" CssClass="error-mensaje" Text="" />
            <a href="FormLogin.aspx" class="btn-primary error-volver">Volver al inicio</a>
        </div>
    </form>
</body>
</html>
