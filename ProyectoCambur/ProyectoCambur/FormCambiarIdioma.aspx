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
                <div class="tagline">Gestión Clínica</div>
            </div>
            <nav class="sidebar-nav">
                <a href="FormDashboard.aspx"         class="nav-item">🏠 Dashboard</a>
                <a href="FormRegistroPaciente.aspx"  class="nav-item">👤 Pacientes</a>
                <a href="FormRealizarConsulta.aspx"  class="nav-item">🗒️ Consultas</a>
                <a href="FormHistorialClinico.aspx"  class="nav-item">📋 Historial Clínico</a>
                <a href="FormResumenIA.aspx"         class="nav-item">🤖 Resumen IA</a>
                <a href="FormLineaTemporal.aspx"     class="nav-item">📅 Línea Temporal</a>
                <a href="FormInformeDerivacion.aspx" class="nav-item">📤 Derivaciones</a>
                <a href="FormPerfilPaciente.aspx"    class="nav-item">🧠 Perfilación</a>
                <a href="FormExportarReporte.aspx"   class="nav-item">💾 Exportar</a>
            </nav>
            <div class="sidebar-footer">
                <a href="FormSuscripcion.aspx"  class="nav-item">💳 Mi Suscripción</a>
                <a href="FormLogout.aspx"       class="nav-item nav-logout">🚪 Cerrar sesión</a>
            </div>
        </aside>

     
        <div class="main-wrap">

         
            <header class="top-header">
                <div class="header-title">
                    <span class="header-section">Configuración</span>
                    <span class="header-sep">/</span>
                    <span class="header-page">Cambiar idioma</span>
                </div>
                <div class="header-user">
                    <div class="user-avatar">
                        <asp:Label ID="lblIniciales" runat="server" Text="LM" />
                    </div>
                    <div class="user-info">
                        <asp:Label ID="lblNombreProfesional" runat="server" CssClass="user-name" Text="" />
                        <span class="user-role">Psicólogo/a</span>
                    </div>
                </div>
            </header>

       
            <div class="page-content">

                <asp:Label ID="lblMensaje" runat="server" Visible="false" CssClass="server-error" />

                <div class="idioma-layout">

               
                    <div class="idioma-main">
                        <div class="content-card">

                            <div class="card-header">
                                <h2 class="card-title">Idioma de la interfaz</h2>
                                <p class="card-subtitle">Seleccioná el idioma en el que querés visualizar la plataforma. El cambio se aplica de forma inmediata.</p>
                            </div>

                            <div class="section-sep">Idioma actual</div>

                        
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
                                <asp:Label runat="server"
                                    CssClass="badge-activos" Text="Activo" />
                            </div>

                            <div class="section-sep">Idiomas disponibles</div>

                            <asp:HiddenField ID="hfIdiomaSeleccionado" runat="server"
                                Value="" ClientIDMode="Static" />

                      
                            <div class="idiomas-grid">
                                <asp:Repeater ID="rptIdiomas" runat="server">
                                    <ItemTemplate>
                                        <div class='<%# "idioma-card" + ((bool)Eval("EsActual") ? " idioma-card-activo" : "") %>'
                                             id='<%# "idiomaCard_" + Eval("Codigo") %>'
                                             onclick='<%# "seleccionarIdioma(this, \"" + Eval("Codigo") + "\")" %>'>
                                            <div class="idioma-flag"><%# Eval("Flag") %></div>
                                            <div class="idioma-card-info">
                                                <div class="idioma-card-nombre"><%# Eval("Nombre") %></div>
                                                <div class="idioma-card-codigo"><%# Eval("Codigo") %></div>
                                            </div>
                                            <div class="idioma-check">
                                                <%# (bool)Eval("EsActual") ? "●" : "○" %>
                                            </div>
                                          
                                            <asp:Panel runat="server"
                                                Visible='<%# !(bool)Eval("Activo") %>'
                                                CssClass="idioma-badge-inactivo">
                                                No disponible
                                            </asp:Panel>
                                        </div>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </div>

                            <div class="form-actions">
                                <a href="FormDashboard.aspx" class="btn-secondary">Cancelar</a>
                                <asp:Button ID="btnGuardar" runat="server"
                                    Text="Confirmar cambio de idioma"
                                    CssClass="btn-primary"
                                    OnClick="btnGuardar_Click"
                                    CausesValidation="false" />
                            </div>

                        </div>
                    </div>

                    <div class="idioma-aside">

                        <div class="content-card sel-card">
                            <p class="accesos-titulo">Idioma seleccionado</p>
                            <div id="seleccionadoInfo" class="sel-vacio">
                                Ningún idioma seleccionado todavía.
                            </div>
                        </div>

                        <div class="content-card aviso-card">
                            <div class="aviso-icon">🌐</div>
                            <p class="aviso-titulo">Aplicación inmediata</p>
                            <p class="aviso-texto">Al confirmar, la interfaz se recargará automáticamente en el idioma elegido. Las traducciones son gestionadas por el administrador del sistema.</p>
                        </div>

                        <div class="content-card aviso-card">
                            <div class="aviso-icon">📝</div>
                            <p class="aviso-titulo">Contenido clínico</p>
                            <p class="aviso-texto">El cambio de idioma afecta únicamente a la interfaz del sistema. El contenido clínico ingresado permanece en el idioma original.</p>
                        </div>

                    </div>
                </div>
            </div>
        </div>

    </form>

    <script type="text/javascript">
        var IDIOMAS_INFO = {
            'ES': { nombre: 'Español', flag: '🇦🇷' },
            'EN': { nombre: 'English', flag: '🇺🇸' },
            'PT': { nombre: 'Português', flag: '🇧🇷' },
            'FR': { nombre: 'Français', flag: '🇫🇷' }
        };

        function seleccionarIdioma(card, codigo) {
         
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
            if (hf) hf.value = codigo;

           
            var info   = IDIOMAS_INFO[codigo] || { nombre: codigo, flag: '🌐' };
            var panel  = document.getElementById('seleccionadoInfo');
            if (panel) {
                panel.className = 'sel-activo';
                panel.innerHTML =
                    '<span class="sel-flag">' + info.flag + '</span>' +
                    '<div><div class="sel-nombre">' + info.nombre + '</div>' +
                    '<div class="sel-codigo">' + codigo + '</div></div>';
            }
        }
    </script>
</body>
</html>
