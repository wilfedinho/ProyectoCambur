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
                    <p class="logout-titulo"><asp:Label ID="lblTituloCerrando" runat="server" Text="" /></p>
                    <p class="logout-subtitulo"><asp:Label ID="lblSubtituloCerrando" runat="server" Text="" /></p>
                </asp:Panel>

                <asp:Panel ID="pnlError" runat="server" CssClass="logout-estado" Visible="false">
                    <div class="logout-icono-error">⚠️</div>
                    <p class="logout-titulo"><asp:Label ID="lblTituloError" runat="server" Text="" /></p>
                    <asp:Label ID="lblErrorLogout" runat="server"
                        CssClass="logout-subtitulo" Text="" />
                    <asp:HyperLink ID="lnkIrLogin" runat="server" CssClass="btn-ir-login" Text="" NavigateUrl="~/FormLogin.aspx" />
                </asp:Panel>

                <asp:Panel ID="pnlExito" runat="server" CssClass="logout-estado" Visible="false">
                    <div class="logout-icono-ok">✓</div>
                    <p class="logout-titulo"><asp:Label ID="lblTituloExito" runat="server" Text="" /></p>
                    <p class="logout-subtitulo"><asp:Label ID="lblSubtituloExito" runat="server" Text="" /></p>
                </asp:Panel>

            </div>
        </div>
    </form>
</body>
</html>
