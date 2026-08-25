<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FormAccesoDenegado.aspx.cs" Inherits="FormAccesoDenegado" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta name="viewport" content="width=device-width, initial-scale=1.0"/>
    <title>Cambur — Acceso no autorizado</title>
    <link href="EstilosPaginas/FormAccesoDenegado.css" rel="stylesheet" type="text/css"/>
</head>
<body class="pagina-centrada">
    <form id="form1" runat="server">
        <div class="denegado-card">
            <div class="denegado-icono">🚫</div>
            <div class="logotype">CAM<span>BUR</span></div>

            <p class="denegado-titulo"><asp:Label ID="lblTitulo" runat="server" Text="" /></p>
            <p class="denegado-texto"><asp:Label ID="lblTexto" runat="server" Text="" /></p>

            <div class="denegado-countdown">
                <asp:Label ID="lblCountdownTexto" runat="server" Text="" />
                <span id="segundosRestantes">10</span>
            </div>
        </div>
    </form>

    <script type="text/javascript">
        var segundos = 10;
        var span = document.getElementById('segundosRestantes');

        var intervalo = setInterval(function () {
            segundos--;
            if (span) span.textContent = segundos;

            if (segundos <= 0) {
                clearInterval(intervalo);
                window.location.href = 'FormLogin.aspx?acceso_denegado=ok';
            }
        }, 1000);
    </script>
</body>
</html>
