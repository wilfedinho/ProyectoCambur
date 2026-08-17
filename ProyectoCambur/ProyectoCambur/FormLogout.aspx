<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FormLogout.aspx.cs" Inherits="FormLogout" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta name="viewport" content="width=device-width, initial-scale=1.0"/>
    <title>Cambur — Cerrando sesión...</title>
    <link href="EstilosPaginas/FormLogout.css" rel="stylesheet" type="text/css"/>
</head>
<body>
    <form id="form1" runat="server">
        <div class="logout-wrap">
            <div class="logout-card">
                <div class="logout-logo">CAM<span>BUR</span></div>

           
                <asp:Panel ID="pnlCerrando" runat="server" CssClass="logout-estado">
                    <div class="logout-spinner"></div>
                    <p class="logout-titulo">Cerrando sesión...</p>
                    <p class="logout-subtitulo">Limpiando datos de sesión de forma segura.</p>
                </asp:Panel>

         
                <asp:Panel ID="pnlError" runat="server" CssClass="logout-estado" Visible="false">
                    <div class="logout-icono-error">⚠️</div>
                    <p class="logout-titulo">Error al cerrar sesión</p>
                    <asp:Label ID="lblErrorLogout" runat="server"
                        CssClass="logout-subtitulo" Text="" />
                    <a href="FormLogin.aspx" class="btn-ir-login">Ir al inicio de sesión</a>
                </asp:Panel>

             
                <asp:Panel ID="pnlExito" runat="server" CssClass="logout-estado" Visible="false">
                    <div class="logout-icono-ok">✓</div>
                    <p class="logout-titulo">Sesión cerrada</p>
                    <p class="logout-subtitulo">Serás redirigido al inicio de sesión...</p>
                </asp:Panel>

            </div>
        </div>
    </form>
</body>
</html>
