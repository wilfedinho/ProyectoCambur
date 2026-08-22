<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FormCambiarIdioma.aspx.cs" Inherits="FormCambiarIdioma" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta name="viewport" content="width=device-width, initial-scale=1.0"/>
    <title>Cambur — Cambiar Idioma</title>
    <link href="EstilosPaginas/Shared.css"              rel="stylesheet" type="text/css"/>
    <link href="EstilosPaginas/FormCambiarIdioma.css"   rel="stylesheet" type="text/css"/>
</head>
<body>
    <form id="form1" runat="server">

        <aside class="sidebar">
            <div class="sidebar-logo">
                <div class="logotype">CAM<span>BUR</span></div>
                <div class="tagline"><asp:Label ID="lblTaglineSidebar" runat="server" Text="" /></div>
            </div>
            <nav class="sidebar-nav">
                <asp:HyperLink ID="lnkVolverMenu" runat="server" CssClass="nav-item" Text="🏠 Menú" NavigateUrl="~/FormLogin.aspx" />
            </nav>
            <div class="sidebar-footer">
                <a href="FormLogout.aspx" class="nav-item nav-logout">🚪 <asp:Label ID="lblMenuCerrarSesion" runat="server" Text="" /></a>
            </div>
        </aside>

        <div class="main-wrap">

            <header class="top-header">
                <div class="header-title">
                    <asp:Label ID="lblHeaderSeccion" runat="server" CssClass="header-section" Text="" />
                    <span class="header-sep">/</span>
                    <asp:Label ID="lblHeaderPagina" runat="server" CssClass="header-page" Text="" />
                </div>
                <div class="header-user">
                    <div class="user-avatar">
                        <asp:Label ID="lblIniciales" runat="server" Text="" />
                    </div>
                    <div class="user-info">
                        <asp:Label ID="lblNombreProfesional" runat="server" CssClass="user-name" Text="" />
                        <asp:Label ID="lblRolActual" runat="server" CssClass="user-role" Text="" />
                    </div>
                </div>
            </header>

            <div class="page-content">

                <asp:Label ID="lblMensaje" runat="server" Visible="false" />

                <div class="idioma-layout">

                    <div class="idioma-main">
                        <div class="content-card">

                            <div class="card-header">
                                <h2 class="card-title"><asp:Label ID="lblTituloCard" runat="server" Text="" /></h2>
                                <p class="card-subtitle"><asp:Label ID="lblSubtituloCard" runat="server" Text="" /></p>
                            </div>

                            <asp:Label ID="lblSeccionActual" runat="server" CssClass="section-sep" Text="" />

                            <div class="idioma-activo-card">
                                <div class="ia-flag">
                                    <asp:Label ID="lblIdiomaActivoFlag" runat="server" Text="" />
                                </div>
                                <div class="ia-info">
                                    <asp:Label ID="lblIdiomaActivoNombre" runat="server"
                                        CssClass="ia-nombre" Text="" />
                                    <asp:Label ID="lblIdiomaActivoCodigo" runat="server"
                                        CssClass="ia-codigo" Text="" />
                                </div>
                                <asp:Label ID="lblBadgeActivo" runat="server"
                                    CssClass="badge-activos" Text="" />
                            </div>

                            <asp:Label ID="lblSeccionDisponibles" runat="server" CssClass="section-sep" Text="" />

                            <asp:HiddenField ID="hfIdiomaSeleccionado" runat="server"
                                Value="" ClientIDMode="Static" />

                            <div class="idiomas-grid">
                                <asp:Repeater ID="rptIdiomas" runat="server">
                                    <ItemTemplate>
                                        <div class='<%# "idioma-card" + ((bool)Eval("EsActual") ? " idioma-card-activo" : "") %>'
                                             id='<%# "idiomaCard_" + Eval("CodigoIso") %>'
                                             onclick='<%# "seleccionarIdioma(this, \"" + System.Web.HttpUtility.JavaScriptStringEncode(Eval("NombreIdioma").ToString()) + "\")" %>'>
                                            <div class="idioma-flag"><%# Eval("Flag") %></div>
                                            <div class="idioma-card-info">
                                                <div class="idioma-card-nombre"><%# Eval("NombreIdioma") %></div>
                                                <div class="idioma-card-codigo"><%# Eval("CodigoIso") %></div>
                                            </div>
                                            <div class="idioma-check">
                                                <%# (bool)Eval("EsActual") ? "●" : "○" %>
                                            </div>

                                            <asp:Panel runat="server"
                                                Visible='<%# !(bool)Eval("Disponible") %>'
                                                CssClass="idioma-badge-inactivo">
                                                <%# Eval("TextoNoDisponible") %>
                                            </asp:Panel>
                                        </div>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </div>

                            <div class="form-actions">
                                <asp:HyperLink ID="lnkCancelar" runat="server" CssClass="btn-secondary" Text="" NavigateUrl="~/FormLogin.aspx" />
                                <asp:Button ID="btnGuardar" runat="server"
                                    Text=""
                                    CssClass="btn-primary"
                                    OnClick="btnGuardar_Click"
                                    CausesValidation="false" />
                            </div>

                        </div>
                    </div>

                    <div class="idioma-aside">

                        <div class="content-card sel-card">
                            <p class="accesos-titulo"><asp:Label ID="lblTituloSeleccion" runat="server" Text="" /></p>
                            <div id="seleccionadoInfo" class="sel-vacio">
                                <asp:Label ID="lblSinSeleccion" runat="server" Text="" />
                            </div>
                        </div>

                        <div class="content-card aviso-card">
                            <div class="aviso-icon">🌐</div>
                            <p class="aviso-titulo"><asp:Label ID="lblAvisoInmediatoTitulo" runat="server" Text="" /></p>
                            <p class="aviso-texto"><asp:Label ID="lblAvisoInmediatoTexto" runat="server" Text="" /></p>
                        </div>

                        <div class="content-card aviso-card">
                            <div class="aviso-icon">📝</div>
                            <p class="aviso-titulo"><asp:Label ID="lblAvisoClinicoTitulo" runat="server" Text="" /></p>
                            <p class="aviso-texto"><asp:Label ID="lblAvisoClinicoTexto" runat="server" Text="" /></p>
                        </div>

                    </div>
                </div>
            </div>
        </div>

    </form>

    <script type="text/javascript">
        function seleccionarIdioma(card, nombreIdioma) {
            if (card.querySelector('.idioma-badge-inactivo')) return;

            document.querySelectorAll('.idioma-card').forEach(function (c) {
                c.classList.remove('idioma-card-seleccionado');
                var chk = c.querySelector('.idioma-check');
                if (chk && !c.classList.contains('idioma-card-activo'))
                    chk.textContent = '○';
            });

            card.classList.add('idioma-card-seleccionado');
            var chk = card.querySelector('.idioma-check');
            if (chk) chk.textContent = '●';

            var hf = document.getElementById('hfIdiomaSeleccionado');
            if (hf) hf.value = nombreIdioma;

            var nombre = card.querySelector('.idioma-card-nombre').textContent;
            var flag = card.querySelector('.idioma-flag').textContent;
            var codigo = card.querySelector('.idioma-card-codigo').textContent;

            var panel = document.getElementById('seleccionadoInfo');
            if (panel) {
                panel.className = 'sel-activo';
                panel.innerHTML =
                    '<span class="sel-flag">' + flag + '</span>' +
                    '<div><div class="sel-nombre">' + nombre + '</div>' +
                    '<div class="sel-codigo">' + codigo + '</div></div>';
            }
        }
    </script>
</body>
</html>
