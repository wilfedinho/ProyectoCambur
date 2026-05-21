<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FormModificarConsulta.aspx.cs" Inherits="FormModificarConsulta" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta name="viewport" content="width=device-width, initial-scale=1.0"/>
    <title>Cambur — Modificar Consulta</title>
    <link href="EstilosPaginas/Shared.css"                 rel="stylesheet" type="text/css"/>
    <link href="EstilosPaginas/FormModificarConsulta.css"  rel="stylesheet" type="text/css"/>
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
                <a href="FormRealizarConsulta.aspx"  class="nav-item active">🗒️ Consultas</a>
                <a href="FormHistorialClinico.aspx"  class="nav-item">📋 Historial Clínico</a>
                <a href="FormResumenIA.aspx"         class="nav-item">🤖 Resumen IA</a>
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
                    <span class="header-section">Consultas</span>
                    <span class="header-sep">/</span>
                    <span class="header-page">Modificar consulta</span>
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

                <!-- PANEL: Vencimiento bloqueado -->
                <asp:Panel ID="pnlBloqueado" runat="server" CssClass="content-card plazo-vencido-card" Visible="false">
                    <div class="plazo-icono">🔒</div>
                    <div class="plazo-info">
                        <p class="plazo-titulo">Plazo de edición vencido</p>
                        <asp:Label ID="lblMensajeBloqueado" runat="server"
                            CssClass="plazo-subtitulo" Text="" />
                    </div>
                    <a href="FormRealizarConsulta.aspx" class="btn-secondary">Volver a consultas</a>
                </asp:Panel>

                <!-- LAYOUT DOS COLUMNAS -->
                <asp:Panel ID="pnlFormulario" runat="server" CssClass="modificar-layout">

                    <!-- COLUMNA IZQUIERDA: formulario editable -->
                    <div class="modificar-form-col">
                        <div class="content-card">

                            <!-- Header con datos de solo lectura -->
                            <div class="consulta-readonly-header">
                                <div class="consulta-readonly-paciente">
                                    <div class="cr-avatar">
                                        <asp:Label ID="lblPacienteIniciales" runat="server" Text="MG" />
                                    </div>
                                    <div class="cr-info">
                                        <asp:Label ID="lblPacienteNombre" runat="server"
                                            CssClass="cr-nombre" Text="" />
                                        <div class="cr-meta">
                                            <span class="cr-meta-item">📅</span>
                                            <asp:Label ID="lblFechaConsulta" runat="server"
                                                CssClass="cr-meta-item cr-fecha" Text="" />
                                            <span class="cr-meta-sep">·</span>
                                            <asp:Label ID="lblDuracionConsulta" runat="server"
                                                CssClass="cr-meta-item" Text="" />
                                            <span class="cr-meta-sep">·</span>
                                            <asp:Label ID="lblModalidadConsulta" runat="server"
                                                CssClass="cr-meta-item" Text="" />
                                        </div>
                                    </div>
                                </div>
                                <asp:Label ID="lblBadgePlazo" runat="server"
                                    CssClass="badge-plazo-ok" Text="" />
                            </div>

                            <!-- Aviso campos no editables -->
                            <div class="aviso-readonly">
                                ✏️ Solo podés modificar los campos clínicos. La fecha, duración, paciente y modalidad no son editables.
                            </div>

                            <div class="section-sep">Campos editables</div>

                            <div class="grid-1-col">

                                <div class="field">
                                    <label for="txtObjetivos">Objetivos de la consulta</label>
                                    <asp:TextBox ID="txtObjetivos" runat="server"
                                        TextMode="MultiLine" Rows="3"
                                        ClientIDMode="Static" />
                                </div>

                                <div class="field">
                                    <label for="txtObservaciones">Observaciones clínicas</label>
                                    <asp:TextBox ID="txtObservaciones" runat="server"
                                        TextMode="MultiLine" Rows="4"
                                        ClientIDMode="Static" />
                                </div>

                                <div class="field">
                                    <label for="txtHipotesis">Hipótesis</label>
                                    <asp:TextBox ID="txtHipotesis" runat="server"
                                        TextMode="MultiLine" Rows="3"
                                        ClientIDMode="Static" />
                                </div>

                                <div class="field">
                                    <label for="txtIntervenciones">Intervenciones realizadas</label>
                                    <asp:TextBox ID="txtIntervenciones" runat="server"
                                        TextMode="MultiLine" Rows="3"
                                        ClientIDMode="Static" />
                                </div>

                                <div class="field">
                                    <label for="txtEvolucion">Evolución observada</label>
                                    <asp:TextBox ID="txtEvolucion" runat="server"
                                        TextMode="MultiLine" Rows="3"
                                        ClientIDMode="Static" />
                                </div>

                            </div>

                            <div class="section-sep">Cierre clínico</div>

                            <div class="grid-2">
                                <div class="field">
                                    <label for="txtDiagnostico">Diagnóstico</label>
                                    <asp:TextBox ID="txtDiagnostico" runat="server"
                                        TextMode="MultiLine" Rows="3"
                                        ClientIDMode="Static" />
                                </div>
                                <div class="field">
                                    <label for="txtTratamiento">Tratamiento</label>
                                    <asp:TextBox ID="txtTratamiento" runat="server"
                                        TextMode="MultiLine" Rows="3"
                                        ClientIDMode="Static" />
                                </div>
                            </div>

                            <div class="form-actions">
                                <a href="FormRealizarConsulta.aspx" class="btn-secondary">Cancelar</a>
                                <asp:Button ID="btnGuardar" runat="server"
                                    Text="Guardar cambios"
                                    CssClass="btn-primary"
                                    OnClick="btnGuardar_Click"
                                    ValidationGroup="vgModificar" />
                            </div>

                        </div>
                    </div>

                    <!-- COLUMNA DERECHA: info y plazo -->
                    <div class="modificar-info-col">

                        <!-- Card plazo de edición -->
                        <div class="content-card plazo-card">
                            <p class="plazo-card-titulo">Plazo de edición</p>
                            <div class="plazo-dias-wrap">
                                <asp:Label ID="lblDiasRestantes" runat="server"
                                    CssClass="plazo-dias-num" Text="" />
                                <span class="plazo-dias-label">días restantes</span>
                            </div>
                            <div class="plazo-barra-wrap">
                                <div class="plazo-barra">
                                    <asp:Label ID="lblPlazoFill" runat="server"
                                        CssClass="plazo-fill" Text="" />
                                </div>
                            </div>
                            <asp:Label ID="lblFechaLimite" runat="server"
                                CssClass="plazo-fecha-limite" Text="" />
                        </div>

                        <!-- Card última modificación -->
                        <div class="content-card ultima-mod-card">
                            <p class="accesos-titulo">Historial de cambios</p>
                            <div class="mod-item">
                                <span class="mod-icono">📝</span>
                                <div class="mod-info">
                                    <span class="mod-label">Creada</span>
                                    <asp:Label ID="lblFechaCreacion" runat="server"
                                        CssClass="mod-valor" Text="" />
                                </div>
                            </div>
                            <div class="mod-item">
                                <span class="mod-icono">✏️</span>
                                <div class="mod-info">
                                    <span class="mod-label">Última modificación</span>
                                    <asp:Label ID="lblUltimaModificacion" runat="server"
                                        CssClass="mod-valor" Text="" />
                                </div>
                            </div>
                        </div>

                        <!-- Aviso encriptación -->
                        <div class="content-card aviso-card">
                            <div class="aviso-icon">🔒</div>
                            <p class="aviso-titulo">Datos encriptados</p>
                            <p class="aviso-texto">Los cambios se re-encriptan con AES al guardar, cumpliendo la Ley 25.326.</p>
                        </div>

                    </div>

                </asp:Panel>

            </div>
        </div>

    </form>
</body>
</html>
