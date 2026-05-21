<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FormResumenIA.aspx.cs" Inherits="FormResumenIA" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta name="viewport" content="width=device-width, initial-scale=1.0"/>
    <title>Cambur — Resumen Clínico IA</title>
    <link href="EstilosPaginas/Shared.css"           rel="stylesheet" type="text/css"/>
    <link href="EstilosPaginas/FormResumenIA.css"    rel="stylesheet" type="text/css"/>
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
                <a href="FormResumenIA.aspx"         class="nav-item active">🤖 Resumen IA</a>
                <a href="FormLineaTemporal.aspx"     class="nav-item">📅 Línea Temporal</a>
                <a href="FormInformeDerivacion.aspx" class="nav-item">📤 Derivaciones</a>
                <a href="FormPerfilPaciente.aspx"    class="nav-item">🧠 Perfilación</a>
                <a href="FormExportarReporte.aspx"   class="nav-item">💾 Exportar</a>
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
                    <span class="header-section">IA Asistiva</span>
                    <span class="header-sep">/</span>
                    <span class="header-page">Resumen Clínico</span>
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

            <!-- CONTENIDO -->
            <div class="page-content">

                <asp:Label ID="lblMensaje" runat="server" Visible="false" CssClass="server-error" />

                <!-- ================================================
                     ESTADO 1 — FILTROS
                     ================================================ -->
                <asp:Panel ID="pnlFiltros" runat="server" CssClass="content-card">

                    <div class="card-header">
                        <h2 class="card-title">Resumen clínico asistido por IA</h2>
                        <p class="card-subtitle">Seleccioná el paciente y el período para generar una síntesis estructurada de sus consultas.</p>
                    </div>

                    <div class="ia-badge-aviso">
                        🤖 La IA organiza y sintetiza la información registrada. No emite diagnósticos ni reemplaza el criterio profesional.
                    </div>

                    <div class="section-sep">Filtros de búsqueda</div>

                    <div class="grid-3">
                        <div class="field">
                            <label for="ddlPaciente">Paciente <sup>*</sup></label>
                            <asp:DropDownList ID="ddlPaciente" runat="server" ClientIDMode="Static">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="rfvPaciente" runat="server"
                                ControlToValidate="ddlPaciente" InitialValue=""
                                ErrorMessage="Seleccioná un paciente."
                                CssClass="field-error" Display="Dynamic"
                                ValidationGroup="vgFiltro" />
                        </div>

                        <div class="field">
                            <label for="txtFechaDesde">Fecha desde <sup>*</sup></label>
                            <asp:TextBox ID="txtFechaDesde" runat="server"
                                TextMode="Date" ClientIDMode="Static" />
                            <asp:RequiredFieldValidator ID="rfvDesde" runat="server"
                                ControlToValidate="txtFechaDesde"
                                ErrorMessage="Ingresá la fecha de inicio."
                                CssClass="field-error" Display="Dynamic"
                                ValidationGroup="vgFiltro" />
                        </div>

                        <div class="field">
                            <label for="txtFechaHasta">Fecha hasta <sup>*</sup></label>
                            <asp:TextBox ID="txtFechaHasta" runat="server"
                                TextMode="Date" ClientIDMode="Static" />
                            <asp:RequiredFieldValidator ID="rfvHasta" runat="server"
                                ControlToValidate="txtFechaHasta"
                                ErrorMessage="Ingresá la fecha de fin."
                                CssClass="field-error" Display="Dynamic"
                                ValidationGroup="vgFiltro" />
                        </div>
                    </div>

                    <div class="form-actions">
                        <asp:Button ID="btnBuscar" runat="server"
                            Text="Buscar consultas"
                            CssClass="btn-secondary"
                            ValidationGroup="vgFiltro"
                            OnClick="btnBuscar_Click" />
                    </div>

                </asp:Panel>

                <!-- ================================================
                     ESTADO 2 — LISTA DE CONSULTAS ENCONTRADAS
                     ================================================ -->
                <asp:Panel ID="pnlConsultas" runat="server" CssClass="content-card mt-24" Visible="false">

                    <div class="card-header-row">
                        <div class="card-header-left">
                            <h2 class="card-title">Consultas encontradas</h2>
                            <asp:Label ID="lblCantConsultas" runat="server" CssClass="badge-activos" Text="" />
                        </div>
                        <asp:Label ID="lblRangoBusqueda" runat="server" CssClass="rango-label" Text="" />
                    </div>

                    <p class="hint-text" style="margin-top:6px;">
                        Seleccioná las consultas que querés incluir en el resumen. Podés usar el check de la cabecera para seleccionar todas.
                    </p>

                    <div class="table-wrap">
                        <table class="data-table" id="tblConsultas">
                            <thead>
                                <tr>
                                    <th class="th-check">
                                        <input type="checkbox" id="chkTodas" onclick="toggleTodas(this)" title="Seleccionar todas" />
                                    </th>
                                    <th class="th-left">Fecha</th>
                                    <th class="th-left">Duración</th>
                                    <th class="th-left">Modalidad</th>
                                    <th class="th-left">Resumen de objetivos</th>
                                </tr>
                            </thead>
                        </table>

                        <%-- Repeater que llena el tbody --%>
                        <table class="data-table" style="margin-top:-1px; border-top:none;">
                            <asp:Repeater ID="rptConsultas" runat="server">
                                <ItemTemplate>
                                    <tr class="table-row">
                                        <td class="td-check">
                                            <asp:CheckBox ID="chkConsulta" runat="server"
                                                CssClass="chk-consulta"
                                                Checked="true" />
                                            <asp:HiddenField ID="hfIdConsulta" runat="server"
                                                Value='<%# Eval("IdConsulta") %>' />
                                        </td>
                                        <td class="td-fecha"><%# Eval("Fecha", "{0:dd/MM/yyyy}") %></td>
                                        <td class="td-dur"><%# Eval("Duracion") %> min</td>
                                        <td class="td-mod">
                                            <span class='<%# "badge-modalidad " + Eval("ModalidadCss") %>'>
                                                <%# Eval("Modalidad") %>
                                            </span>
                                        </td>
                                        <td class="td-resumen"><%# Eval("ResumenObjetivos") %></td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                        </table>
                    </div>

                    <div class="form-actions">
                        <asp:Button ID="btnVolver" runat="server"
                            Text="← Cambiar filtros"
                            CssClass="btn-secondary"
                            OnClick="btnVolver_Click"
                            CausesValidation="false" />
                        <asp:Button ID="btnGenerar" runat="server"
                            Text="Generar resumen con IA"
                            CssClass="btn-primary btn-ia"
                            OnClick="btnGenerar_Click"
                            CausesValidation="false"
                            OnClientClick="mostrarCarga(); return true;" />
                    </div>

                </asp:Panel>

                <!-- ================================================
                     ESTADO 2.5 — ANIMACIÓN DE CARGA
                     ================================================ -->
                <div class="carga-overlay" id="cargaOverlay" style="display:none;">
                    <div class="carga-card">
                        <div class="carga-spinner"></div>
                        <p class="carga-titulo">Procesando con IA Asistiva...</p>
                        <p class="carga-subtitulo">Analizando consultas y generando síntesis estructurada</p>
                    </div>
                </div>

                <!-- ================================================
                     ESTADO 3 — RESUMEN GENERADO
                     ================================================ -->
                <asp:Panel ID="pnlResumen" runat="server" CssClass="resumen-layout" Visible="false">

                    <!-- Columna principal: resumen -->
                    <div class="resumen-main">
                        <div class="content-card">

                            <div class="resumen-header">
                                <div>
                                    <h2 class="card-title">Resumen clínico generado</h2>
                                    <asp:Label ID="lblResumenMeta" runat="server" CssClass="card-subtitle" Text="" />
                                </div>
                                <div class="resumen-header-actions">
                                    <asp:Button ID="btnNuevoResumen" runat="server"
                                        Text="← Nuevo resumen"
                                        CssClass="btn-secondary"
                                        OnClick="btnNuevoResumen_Click"
                                        CausesValidation="false" />
                                    <asp:Button ID="btnGuardarResumen" runat="server"
                                        Text="💾 Guardar"
                                        CssClass="btn-primary"
                                        OnClick="btnGuardarResumen_Click"
                                        CausesValidation="false" />
                                </div>
                            </div>

                            <div class="ia-badge-resultado">
                                🤖 Generado por IA Asistiva · Solo organizativo · No diagnóstico · Revisión profesional recomendada
                            </div>

                            <!-- Sección 1: Contexto general -->
                            <div class="resumen-seccion">
                                <div class="resumen-seccion-titulo">
                                    <span class="resumen-seccion-icono">📌</span> Contexto general del período
                                </div>
                                <asp:Label ID="lblContextoGeneral" runat="server"
                                    CssClass="resumen-seccion-texto" Text="" />
                            </div>

                            <!-- Sección 2: Evolución observada -->
                            <div class="resumen-seccion">
                                <div class="resumen-seccion-titulo">
                                    <span class="resumen-seccion-icono">📈</span> Evolución observada
                                </div>
                                <asp:Label ID="lblEvolucion" runat="server"
                                    CssClass="resumen-seccion-texto" Text="" />
                            </div>

                            <!-- Sección 3: Temas recurrentes -->
                            <div class="resumen-seccion">
                                <div class="resumen-seccion-titulo">
                                    <span class="resumen-seccion-icono">🔁</span> Temas recurrentes
                                </div>
                                <asp:Label ID="lblTemasRecurrentes" runat="server"
                                    CssClass="resumen-seccion-texto" Text="" />
                            </div>

                            <!-- Sección 4: Intervenciones destacadas -->
                            <div class="resumen-seccion">
                                <div class="resumen-seccion-titulo">
                                    <span class="resumen-seccion-icono">🛠️</span> Intervenciones destacadas
                                </div>
                                <asp:Label ID="lblIntervenciones" runat="server"
                                    CssClass="resumen-seccion-texto" Text="" />
                            </div>

                            <!-- Sección 5: Observaciones del período -->
                            <div class="resumen-seccion" style="border-bottom:none; margin-bottom:0; padding-bottom:0;">
                                <div class="resumen-seccion-titulo">
                                    <span class="resumen-seccion-icono">💡</span> Observaciones del período
                                </div>
                                <asp:Label ID="lblObservaciones" runat="server"
                                    CssClass="resumen-seccion-texto" Text="" />
                            </div>

                        </div>
                    </div>

                    <!-- Columna lateral: info -->
                    <div class="resumen-aside">

                        <div class="content-card resumen-meta-card">
                            <p class="accesos-titulo">Detalles del resumen</p>
                            <div class="meta-fila">
                                <span class="meta-label">Paciente</span>
                                <asp:Label ID="lblMetaPaciente" runat="server" CssClass="meta-valor" Text="" />
                            </div>
                            <div class="meta-fila">
                                <span class="meta-label">Período</span>
                                <asp:Label ID="lblMetaPeriodo"  runat="server" CssClass="meta-valor" Text="" />
                            </div>
                            <div class="meta-fila">
                                <span class="meta-label">Consultas analizadas</span>
                                <asp:Label ID="lblMetaConsultas" runat="server" CssClass="meta-valor" Text="" />
                            </div>
                            <div class="meta-fila">
                                <span class="meta-label">Generado</span>
                                <asp:Label ID="lblMetaFecha" runat="server" CssClass="meta-valor" Text="" />
                            </div>
                        </div>

                        <div class="content-card aviso-card">
                            <div class="aviso-icon">🔒</div>
                            <p class="aviso-titulo">Resumen encriptado</p>
                            <p class="aviso-texto">El contenido generado se encripta con AES al guardarse, cumpliendo la Ley 25.326.</p>
                        </div>

                        <div class="content-card accesos-card">
                            <p class="accesos-titulo">Acciones relacionadas</p>
                            <a href="FormExportarReporte.aspx" class="acceso-item">💾 <span>Exportar en PDF</span></a>
                            <a href="FormInformeDerivacion.aspx" class="acceso-item">📤 <span>Generar derivación</span></a>
                            <a href="FormLineaTemporal.aspx" class="acceso-item">📅 <span>Ver línea temporal</span></a>
                        </div>

                    </div>

                </asp:Panel>

            </div>
        </div>

    </form>

    <script type="text/javascript">
        /* Seleccionar / deseleccionar todas las consultas */
        function toggleTodas(chkTodas) {
            var checks = document.querySelectorAll('.chk-consulta input[type="checkbox"]');
            checks.forEach(function (c) { c.checked = chkTodas.checked; });
        }

        /* Mostrar overlay de carga antes del postback */
        function mostrarCarga() {
            var overlay = document.getElementById('cargaOverlay');
            if (overlay) overlay.style.display = 'flex';
            return true;
        }

        /* Ocultar overlay al cargar la página (por si volvió de postback) */
        window.addEventListener('DOMContentLoaded', function () {
            var overlay = document.getElementById('cargaOverlay');
            if (overlay) overlay.style.display = 'none';
        });
    </script>
</body>
</html>
