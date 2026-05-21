<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FormExportarReporte.aspx.cs" Inherits="FormExportarReporte" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta name="viewport" content="width=device-width, initial-scale=1.0"/>
    <title>Cambur — Exportar Reporte</title>
    <link href="EstilosPaginas/Shared.css"               rel="stylesheet" type="text/css"/>
    <link href="EstilosPaginas/FormExportarReporte.css"  rel="stylesheet" type="text/css"/>
</head>
<body>
    <form id="form1" runat="server">

        <!-- SIDEBAR -->
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
                <a href="FormExportarReporte.aspx"   class="nav-item active">💾 Exportar</a>
            </nav>
            <div class="sidebar-footer">
                <a href="FormSuscripcion.aspx" class="nav-item">💳 Mi Suscripción</a>
                <a href="FormLogin.aspx"       class="nav-item nav-logout">🚪 Cerrar sesión</a>
            </div>
        </aside>

        <!-- ÁREA PRINCIPAL -->
        <div class="main-wrap">

            <!-- HEADER -->
            <header class="top-header">
                <div class="header-title">
                    <span class="header-section">Documentación</span>
                    <span class="header-sep">/</span>
                    <asp:Label ID="lblHeaderTitulo" runat="server"
                        CssClass="header-page" Text="Exportar reporte" />
                </div>
                <div class="header-user">
                    <div class="user-avatar">
                        <asp:Label ID="lblIniciales" runat="server" Text="LM" />
                    </div>
                    <div class="user-info">
                        <asp:Label ID="lblNombreProfesional" runat="server"
                            CssClass="user-name" Text="" />
                        <span class="user-role">Psicólogo/a</span>
                    </div>
                </div>
            </header>

            <!-- CONTENIDO -->
            <div class="page-content">

                <asp:Label ID="lblMensaje" runat="server"
                    Visible="false" CssClass="server-error" />

                <div class="exportar-layout">

                    <!-- COLUMNA PRINCIPAL -->
                    <div class="exportar-main">

                        <!-- Selector de paciente -->
                        <div class="content-card">
                            <div class="card-header">
                                <h2 class="card-title">Exportar reporte clínico</h2>
                                <p class="card-subtitle">Seleccioná el paciente y el tipo de documento que querés exportar en PDF.</p>
                            </div>

                            <div class="section-sep">Paciente</div>

                            <div class="grid-2" style="max-width:500px;">
                                <div class="field full">
                                    <label for="ddlPaciente">Paciente <sup>*</sup></label>
                                    <asp:DropDownList ID="ddlPaciente" runat="server"
                                        ClientIDMode="Static"
                                        AutoPostBack="true"
                                        OnSelectedIndexChanged="ddlPaciente_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </div>
                            </div>

                            <div class="section-sep">Tipo de documento</div>

                            <%-- Hidden field para el tipo seleccionado --%>
                            <asp:HiddenField ID="hfTipoSeleccionado" runat="server"
                                Value="" ClientIDMode="Static" />

                            <div class="documentos-grid">

                                <!-- Resumen Clínico -->
                                <div class="doc-card" id="docResumen"
                                     onclick="seleccionarDoc(this, 'RESUMEN')">
                                    <div class="doc-icono">🤖</div>
                                    <div class="doc-info">
                                        <div class="doc-tipo">Resumen Clínico IA</div>
                                        <asp:Label ID="lblFechaResumen" runat="server"
                                            CssClass="doc-meta" Text="" />
                                        <asp:Label ID="lblEstadoResumen" runat="server"
                                            CssClass="doc-badge doc-badge-ok" Text="" />
                                    </div>
                                    <div class="doc-check">○</div>
                                </div>

                                <!-- Informe de Derivación -->
                                <div class="doc-card" id="docDerivacion"
                                     onclick="seleccionarDoc(this, 'DERIVACION')">
                                    <div class="doc-icono">📤</div>
                                    <div class="doc-info">
                                        <div class="doc-tipo">Informe de Derivación</div>
                                        <asp:Label ID="lblFechaDerivacion" runat="server"
                                            CssClass="doc-meta" Text="" />
                                        <asp:Label ID="lblEstadoDerivacion" runat="server"
                                            CssClass="doc-badge" Text="" />
                                    </div>
                                    <div class="doc-check">○</div>
                                </div>

                                <!-- Perfil Evolutivo -->
                                <div class="doc-card" id="docPerfil"
                                     onclick="seleccionarDoc(this, 'PERFIL')">
                                    <div class="doc-icono">🧠</div>
                                    <div class="doc-info">
                                        <div class="doc-tipo">Perfil Evolutivo</div>
                                        <asp:Label ID="lblFechaPerfil" runat="server"
                                            CssClass="doc-meta" Text="" />
                                        <asp:Label ID="lblEstadoPerfil" runat="server"
                                            CssClass="doc-badge doc-badge-ok" Text="" />
                                    </div>
                                    <div class="doc-check">○</div>
                                </div>

                            </div>

                            <div class="form-actions">
                                <a href="FormDashboard.aspx" class="btn-secondary">Volver</a>
                                <asp:Button ID="btnExportar" runat="server"
                                    Text="📄 Exportar en PDF"
                                    CssClass="btn-primary btn-exportar"
                                    OnClick="btnExportar_Click"
                                    CausesValidation="false"
                                    OnClientClick="return validarSeleccion();" />
                            </div>
                        </div>

                        <!-- Preview del documento -->
                        <asp:Panel ID="pnlPreview" runat="server"
                            CssClass="content-card preview-card mt-24" Visible="false">

                            <div class="preview-header">
                                <div>
                                    <h3 class="grafico-titulo">Vista previa</h3>
                                    <asp:Label ID="lblPreviewMeta" runat="server"
                                        CssClass="card-subtitle" Text="" />
                                </div>
                                <asp:Label ID="lblPreviewBadge" runat="server"
                                    CssClass="doc-badge doc-badge-ok" Text="" />
                            </div>

                            <div class="preview-documento">
                                <!-- Membrete -->
                                <div class="prev-membrete">
                                    <div class="prev-logo">CAMBUR</div>
                                    <div class="prev-membrete-right">
                                        <asp:Label ID="lblPrevFechDoc" runat="server"
                                            CssClass="prev-meta-txt" Text="" />
                                        <asp:Label ID="lblPrevProfesional" runat="server"
                                            CssClass="prev-meta-txt" Text="" />
                                    </div>
                                </div>
                                <div class="prev-sep"></div>
                                <div class="prev-paciente-info">
                                    <asp:Label ID="lblPrevPaciente" runat="server"
                                        CssClass="prev-paciente-nombre" Text="" />
                                    <asp:Label ID="lblPrevDatosPaciente" runat="server"
                                        CssClass="prev-paciente-meta" Text="" />
                                </div>
                                <div class="prev-sep"></div>
                                <!-- Contenido encriptado / blur -->
                                <div class="prev-contenido">
                                    <div class="prev-seccion-titulo">
                                        <asp:Label ID="lblPrevTipoDoc" runat="server" Text="" />
                                    </div>
                                    <div class="prev-texto-blur">
                                        <div class="blur-linea bl-100"></div>
                                        <div class="blur-linea bl-85"></div>
                                        <div class="blur-linea bl-90"></div>
                                        <div class="blur-linea bl-70"></div>
                                        <div class="blur-linea bl-95"></div>
                                        <div class="blur-linea bl-60"></div>
                                    </div>
                                    <div class="prev-encriptado-aviso">
                                        🔒 Contenido encriptado — Solo visible para el profesional autenticado
                                    </div>
                                </div>
                            </div>

                        </asp:Panel>

                    </div>

                    <!-- COLUMNA LATERAL -->
                    <div class="exportar-aside">

                        <div class="content-card historial-exportaciones-card">
                            <p class="accesos-titulo">Exportaciones recientes</p>
                            <asp:Repeater ID="rptExportaciones" runat="server">
                                <ItemTemplate>
                                    <div class="exp-item">
                                        <span class="exp-icono"><%# Eval("Icono") %></span>
                                        <div class="exp-info">
                                            <span class="exp-tipo"><%# Eval("Tipo") %></span>
                                            <span class="exp-paciente"><%# Eval("Paciente") %></span>
                                            <span class="exp-fecha"><%# Eval("Fecha", "{0:dd/MM/yyyy}") %></span>
                                        </div>
                                    </div>
                                </ItemTemplate>
                            </asp:Repeater>
                            <asp:Label ID="lblSinExportaciones" runat="server"
                                CssClass="sin-perfiles-txt" Visible="false"
                                Text="Aún no realizaste exportaciones." />
                        </div>

                        <div class="content-card aviso-card">
                            <div class="aviso-icon">📄</div>
                            <p class="aviso-titulo">Formato PDF</p>
                            <p class="aviso-texto">Los reportes se generan en PDF con membrete profesional, datos del paciente y firma digital si corresponde.</p>
                        </div>

                        <div class="content-card aviso-card">
                            <div class="aviso-icon">🔒</div>
                            <p class="aviso-titulo">Protección de datos</p>
                            <p class="aviso-texto">El contenido se desencripta en el momento de la exportación y solo para el profesional autenticado. No queda copia sin encriptación.</p>
                        </div>

                    </div>

                </div>
            </div>
        </div>

    </form>

    <script type="text/javascript">
        function seleccionarDoc(card, tipo) {
            document.querySelectorAll('.doc-card').forEach(function (c) {
                c.classList.remove('seleccionado');
                var chk = c.querySelector('.doc-check');
                if (chk) chk.textContent = '○';
            });
            card.classList.add('seleccionado');
            var chk = card.querySelector('.doc-check');
            if (chk) chk.textContent = '●';
            var hf = document.getElementById('hfTipoSeleccionado');
            if (hf) hf.value = tipo;
        }

        function validarSeleccion() {
            var hf = document.getElementById('hfTipoSeleccionado');
            if (!hf || !hf.value) {
                alert('Seleccioná el tipo de documento a exportar.');
                return false;
            }
            return true;
        }

        window.addEventListener('DOMContentLoaded', function () {
            /* Restaurar selección visual si hay un tipo en el HiddenField (post postback) */
            var hf   = document.getElementById('hfTipoSeleccionado');
            var mapa = { 'RESUMEN': 'docResumen', 'DERIVACION': 'docDerivacion', 'PERFIL': 'docPerfil' };
            if (hf && hf.value && mapa[hf.value]) {
                var card = document.getElementById(mapa[hf.value]);
                if (card) seleccionarDoc(card, hf.value);
            }
        });
    </script>
</body>
</html>
