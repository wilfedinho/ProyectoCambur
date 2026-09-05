<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FormHistorialClinico.aspx.cs" Inherits="FormHistorialClinico" %>
<%@ Register Src="~/UserControls/HeaderUsuario.ascx" TagPrefix="uc" TagName="HeaderUsuario" %>
<%@ Register Src="~/UserControls/SidebarNavegacion.ascx" TagPrefix="uc" TagName="SidebarNavegacion" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta name="viewport" content="width=device-width, initial-scale=1.0"/>
    <title>Cambur — Historial Clínico</title>
    <link href="EstilosPaginas/Shared.css"               rel="stylesheet" type="text/css"/>
    <link href="EstilosPaginas/HeaderUsuario.css"        rel="stylesheet" type="text/css"/>
    <link href="EstilosPaginas/SidebarNavegacion.css"    rel="stylesheet" type="text/css"/>
    <link href="EstilosPaginas/FormHistorialClinico.css" rel="stylesheet" type="text/css"/>
</head>
<body>
    <form id="form1" runat="server">

        <aside class="sidebar">
            <div class="sidebar-logo">
                <div class="logotype">CAM<span>BUR</span></div>
                <asp:Label ID="lblTaglineSidebar" runat="server" CssClass="tagline" Text="" />
            </div>
            <uc:SidebarNavegacion ID="ucSidebarNavegacion" runat="server" PaginaActual="acceder_generar_historial" />
            <div class="sidebar-footer">
                <a href="FormLogout.aspx" class="nav-item nav-logout"><span>🚪</span> <asp:Label ID="lblMenuCerrarSesionSidebar" runat="server" Text="" /></a>
            </div>
        </aside>

        <div class="main-wrap">

            <header class="top-header">
                <div class="header-title">
                    <asp:Label ID="lblHeaderSeccion" runat="server" CssClass="header-section" Text="" />
                    <span class="header-sep">/</span>
                    <asp:Label ID="lblHeaderPagina" runat="server" CssClass="header-page" Text="" />
                </div>
                <uc:HeaderUsuario ID="ucHeaderUsuario" runat="server" />
            </header>

            <div class="page-content">

                <asp:Label ID="lblMensaje" runat="server" Visible="false" CssClass="server-error" />

                <asp:HiddenField ID="hdnIdPaciente" runat="server" Value="0" />
                <asp:HiddenField ID="hdnModo" runat="server" Value="alta" />
                <asp:HiddenField ID="hdnIdHistorial" runat="server" Value="0" />

                <asp:Panel ID="pnlSeleccionPaciente" runat="server" CssClass="content-card">
                    <div class="card-header">
                        <h2 class="card-title"><asp:Label ID="lblFormTituloSeleccion" runat="server" Text="" /></h2>
                        <p class="card-subtitle"><asp:Label ID="lblFormSubtituloSeleccion" runat="server" Text="" /></p>
                    </div>

                    <div class="grid-1-col">
                        <div class="field">
                            <asp:Label ID="lblEtiquetaPacienteSeleccion" runat="server" AssociatedControlID="ddlPacienteSeleccion" Text="" />
                            <asp:DropDownList ID="ddlPacienteSeleccion" runat="server" ClientIDMode="Static" />
                            <asp:RequiredFieldValidator ID="rfvPacienteSeleccion" runat="server"
                                ControlToValidate="ddlPacienteSeleccion" InitialValue=""
                                ErrorMessage="." CssClass="field-error" Display="Dynamic"
                                ValidationGroup="vgSeleccion" />
                        </div>
                    </div>

                    <div class="form-actions">
                        <asp:Button ID="btnContinuar" runat="server"
                            Text="Continuar" CssClass="btn-primary"
                            OnClick="btnContinuar_Click"
                            ValidationGroup="vgSeleccion" />
                    </div>
                </asp:Panel>

                <asp:Panel ID="pnlFormulario" runat="server" CssClass="historial-layout">

                    <div class="historial-form-col">

                        <div class="content-card">

                            <div class="paciente-header">
                                <div class="paciente-header-avatar">
                                    <asp:Label ID="lblPacienteIniciales" runat="server" Text="" />
                                </div>
                                <div class="paciente-header-info">
                                    <asp:Label ID="lblPacienteNombre" runat="server"
                                        CssClass="paciente-header-nombre" Text="" />
                                    <div class="paciente-header-meta">
                                        <asp:Label ID="lblPacienteEdad"    runat="server" CssClass="meta-item" Text="" />
                                        <span class="meta-sep">·</span>
                                        <asp:Label ID="lblPacienteEstado"  runat="server" CssClass="meta-item" Text="" />
                                        <span class="meta-sep">·</span>
                                        <asp:Label ID="lblPacienteOcup"    runat="server" CssClass="meta-item" Text="" />
                                    </div>
                                </div>
                                <div class="paciente-header-actions">
                                    <asp:Label ID="lblEstadoHistorial" runat="server"
                                        CssClass="badge-historial-completo" Text="" />
                                </div>
                            </div>

                            <h3 class="form-subtitulo"><asp:Label ID="lblFormTituloFormulario" runat="server" Text="" /></h3>

                            <asp:Label ID="lblSeccionInfoClinica" runat="server" CssClass="section-sep" Text="" style="margin-top:20px;display:block;" />
                            <p class="hint-text"><asp:Label ID="lblHintHistorial" runat="server" Text="" /></p>

                            <div class="campo-historial">
                                <div class="campo-historial-header">
                                    <div class="seccion-header-left">
                                        <span class="seccion-icono">🚬</span>
                                        <asp:Label ID="lblTituloHabitos" runat="server" CssClass="seccion-titulo" Text="" />
                                    </div>
                                    <asp:Label ID="lblBadgeHabitos" runat="server" CssClass="badge-seccion" Text="" />
                                </div>
                                <div class="field">
                                    <asp:TextBox ID="txtHabitosNocivos" runat="server"
                                        TextMode="MultiLine" Rows="4"
                                        ClientIDMode="Static"
                                        oninput="actualizarBadge(this, 'lblBadgeHabitos')" />
                                </div>
                            </div>

                            <div class="campo-historial">
                                <div class="campo-historial-header">
                                    <div class="seccion-header-left">
                                        <span class="seccion-icono">👨‍👩‍👧</span>
                                        <asp:Label ID="lblTituloContexto" runat="server" CssClass="seccion-titulo" Text="" />
                                    </div>
                                    <asp:Label ID="lblBadgeContexto" runat="server" CssClass="badge-seccion" Text="" />
                                </div>
                                <div class="field">
                                    <asp:TextBox ID="txtContextoFamiliar" runat="server"
                                        TextMode="MultiLine" Rows="4"
                                        ClientIDMode="Static"
                                        oninput="actualizarBadge(this, 'lblBadgeContexto')" />
                                </div>
                            </div>

                            <div class="campo-historial">
                                <div class="campo-historial-header">
                                    <div class="seccion-header-left">
                                        <span class="seccion-icono">🧬</span>
                                        <asp:Label ID="lblTituloAntFam" runat="server" CssClass="seccion-titulo" Text="" />
                                    </div>
                                    <asp:Label ID="lblBadgeAntFam" runat="server" CssClass="badge-seccion" Text="" />
                                </div>
                                <div class="field">
                                    <asp:TextBox ID="txtAntecedentesFamiliares" runat="server"
                                        TextMode="MultiLine" Rows="4"
                                        ClientIDMode="Static"
                                        oninput="actualizarBadge(this, 'lblBadgeAntFam')" />
                                </div>
                            </div>

                            <div class="campo-historial">
                                <div class="campo-historial-header">
                                    <div class="seccion-header-left">
                                        <span class="seccion-icono">🏥</span>
                                        <asp:Label ID="lblTituloAntMed" runat="server" CssClass="seccion-titulo" Text="" />
                                    </div>
                                    <asp:Label ID="lblBadgeAntMed" runat="server" CssClass="badge-seccion" Text="" />
                                </div>
                                <div class="field">
                                    <asp:TextBox ID="txtAntecedentesMedicos" runat="server"
                                        TextMode="MultiLine" Rows="4"
                                        ClientIDMode="Static"
                                        oninput="actualizarBadge(this, 'lblBadgeAntMed')" />
                                </div>
                            </div>

                            <div class="campo-historial">
                                <div class="campo-historial-header">
                                    <div class="seccion-header-left">
                                        <span class="seccion-icono">💼</span>
                                        <asp:Label ID="lblTituloLaboral" runat="server" CssClass="seccion-titulo" Text="" />
                                    </div>
                                    <asp:Label ID="lblBadgeLaboral" runat="server" CssClass="badge-seccion" Text="" />
                                </div>
                                <div class="field">
                                    <asp:TextBox ID="txtSituacionLaboral" runat="server"
                                        TextMode="MultiLine" Rows="4"
                                        ClientIDMode="Static"
                                        oninput="actualizarBadge(this, 'lblBadgeLaboral')" />
                                </div>
                            </div>

                            <div class="campo-historial">
                                <div class="campo-historial-header">
                                    <div class="seccion-header-left">
                                        <span class="seccion-icono">⚡</span>
                                        <asp:Label ID="lblTituloTrauma" runat="server" CssClass="seccion-titulo" Text="" />
                                    </div>
                                    <asp:Label ID="lblBadgeTrauma" runat="server" CssClass="badge-seccion" Text="" />
                                </div>
                                <div class="field">
                                    <asp:TextBox ID="txtEventosTraumaticos" runat="server"
                                        TextMode="MultiLine" Rows="4"
                                        ClientIDMode="Static"
                                        oninput="actualizarBadge(this, 'lblBadgeTrauma')" />
                                </div>
                            </div>

                            <div class="form-actions">
                                <asp:Button ID="btnVolverSeleccion" runat="server"
                                    Text="" CssClass="btn-secondary"
                                    OnClick="btnVolverSeleccion_Click"
                                    CausesValidation="false" />
                                <asp:Button ID="btnGuardar" runat="server"
                                    Text="" CssClass="btn-primary"
                                    OnClick="btnGuardar_Click" />
                            </div>

                        </div>
                    </div>

                    <div class="historial-info-col">

                        <div class="content-card progreso-card">
                            <p class="progreso-titulo"><asp:Label ID="lblProgresoTitulo" runat="server" Text="" /></p>
                            <div class="progreso-barra-wrap">
                                <div class="progreso-barra">
                                    <div class="progreso-fill" id="progresoFill" style="width: 0%"></div>
                                </div>
                                <span class="progreso-pct" id="progresoPct">0%</span>
                            </div>
                            <div class="progreso-items">
                                <div class="progreso-item" id="pi-habitos">
                                    <span class="pi-icono pi-pendiente">○</span>
                                    <span class="pi-label">🚬</span>
                                </div>
                                <div class="progreso-item" id="pi-contexto">
                                    <span class="pi-icono pi-pendiente">○</span>
                                    <span class="pi-label">👨‍👩‍👧</span>
                                </div>
                                <div class="progreso-item" id="pi-antfam">
                                    <span class="pi-icono pi-pendiente">○</span>
                                    <span class="pi-label">🧬</span>
                                </div>
                                <div class="progreso-item" id="pi-antmed">
                                    <span class="pi-icono pi-pendiente">○</span>
                                    <span class="pi-label">🏥</span>
                                </div>
                                <div class="progreso-item" id="pi-laboral">
                                    <span class="pi-icono pi-pendiente">○</span>
                                    <span class="pi-label">💼</span>
                                </div>
                                <div class="progreso-item" id="pi-trauma">
                                    <span class="pi-icono pi-pendiente">○</span>
                                    <span class="pi-label">⚡</span>
                                </div>
                            </div>
                        </div>

                        <div class="content-card aviso-card">
                            <div class="aviso-icon">🔒</div>
                            <p class="aviso-titulo"><asp:Label ID="lblAvisoEncriptadoTitulo" runat="server" Text="" /></p>
                            <p class="aviso-texto"><asp:Label ID="lblAvisoEncriptadoTexto" runat="server" Text="" /></p>
                        </div>

                    </div>

                </asp:Panel>
            </div>
        </div>

    </form>

    <script type="text/javascript">

        function actualizarBadge(textarea, badgeId) {
            var badge = document.getElementById(badgeId);
            if (!badge) return;
            var tieneContenido = textarea.value.trim().length > 0;
            badge.className = tieneContenido ? 'badge-seccion completado' : 'badge-seccion';
            actualizarProgreso();
        }

        function actualizarProgreso() {
            var campos = [
                { id: 'txtHabitosNocivos', piId: 'pi-habitos' },
                { id: 'txtContextoFamiliar', piId: 'pi-contexto' },
                { id: 'txtAntecedentesFamiliares', piId: 'pi-antfam' },
                { id: 'txtAntecedentesMedicos', piId: 'pi-antmed' },
                { id: 'txtSituacionLaboral', piId: 'pi-laboral' },
                { id: 'txtEventosTraumaticos', piId: 'pi-trauma' }
            ];

            var completados = 0;
            campos.forEach(function (c) {
                var el = document.getElementById(c.id);
                var pi = document.getElementById(c.piId);
                if (!el || !pi) return;
                var ok = el.value.trim().length > 0;
                if (ok) completados++;
                var icono = pi.querySelector('.pi-icono');
                if (icono) {
                    icono.textContent = ok ? '●' : '○';
                    icono.className = ok ? 'pi-icono pi-ok' : 'pi-icono pi-pendiente';
                }
            });

            var pct = Math.round((completados / campos.length) * 100);
            var fill = document.getElementById('progresoFill');
            var pctEl = document.getElementById('progresoPct');
            if (fill) fill.style.width = pct + '%';
            if (pctEl) pctEl.textContent = pct + '%';
        }

        window.addEventListener('DOMContentLoaded', function () {
            actualizarProgreso();
        });
    </script>
</body>
</html>