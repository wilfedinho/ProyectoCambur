<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FormRealizarConsulta.aspx.cs" Inherits="FormRealizarConsulta" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta name="viewport" content="width=device-width, initial-scale=1.0"/>
    <title>Cambur — Realizar Consulta</title>
    <link href="EstilosPaginas/Shared.css"              rel="stylesheet" type="text/css"/>
    <link href="EstilosPaginas/FormRealizarConsulta.css" rel="stylesheet" type="text/css"/>
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

     
        <div class="main-wrap">

      
            <header class="top-header">
                <div class="header-title">
                    <span class="header-section">Consultas</span>
                    <span class="header-sep">/</span>
                    <span class="header-page">Registrar consulta</span>
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

              
                <div class="consulta-layout">

                  
                    <div class="consulta-form-col">

                    
                        <div class="content-card">
                            <div class="card-header">
                                <h2 class="card-title">Nueva consulta</h2>
                                <p class="card-subtitle">Seleccioná el paciente y completá los datos clínicos de la sesión.</p>
                            </div>

                            <div class="section-sep">Paciente y fecha</div>

                            <div class="grid-2">
                                <div class="field">
                                    <label for="ddlPaciente">Paciente <sup>*</sup></label>
                                    <asp:DropDownList ID="ddlPaciente" runat="server"
                                        ClientIDMode="Static"
                                        AutoPostBack="true"
                                        OnSelectedIndexChanged="ddlPaciente_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="rfvPaciente" runat="server"
                                        ControlToValidate="ddlPaciente"
                                        InitialValue=""
                                        ErrorMessage="Seleccioná un paciente."
                                        CssClass="field-error" Display="Dynamic"
                                        ValidationGroup="vgConsulta" />
                                </div>

                                <div class="field">
                                    <label for="txtFechaConsulta">Fecha de la consulta <sup>*</sup></label>
                                    <asp:TextBox ID="txtFechaConsulta" runat="server"
                                        TextMode="Date" ClientIDMode="Static" />
                                    <asp:RequiredFieldValidator ID="rfvFecha" runat="server"
                                        ControlToValidate="txtFechaConsulta"
                                        ErrorMessage="La fecha es obligatoria."
                                        CssClass="field-error" Display="Dynamic"
                                        ValidationGroup="vgConsulta" />
                                </div>

                                <div class="field">
                                    <label for="txtDuracion">Duración (minutos) <sup>*</sup></label>
                                    <asp:TextBox ID="txtDuracion" runat="server"
                                        TextMode="Number" MaxLength="3"
                                        placeholder="Ej: 50"
                                        ClientIDMode="Static" />
                                    <asp:RequiredFieldValidator ID="rfvDuracion" runat="server"
                                        ControlToValidate="txtDuracion"
                                        ErrorMessage="La duración es obligatoria."
                                        CssClass="field-error" Display="Dynamic"
                                        ValidationGroup="vgConsulta" />
                                </div>

                                <div class="field">
                                    <label for="ddlModalidad">Modalidad</label>
                                    <asp:DropDownList ID="ddlModalidad" runat="server" ClientIDMode="Static">
                                        <asp:ListItem Value="PRE" Text="Presencial" />
                                        <asp:ListItem Value="VIR" Text="Virtual" />
                                        <asp:ListItem Value="TEL" Text="Telefónica" />
                                    </asp:DropDownList>
                                </div>
                            </div>

                            <div class="section-sep">Contenido clínico</div>

                            <div class="grid-1">

                                <div class="field">
                                    <label for="txtObjetivos">Objetivos de la consulta <sup>*</sup></label>
                                    <asp:TextBox ID="txtObjetivos" runat="server"
                                        TextMode="MultiLine" Rows="3"
                                        placeholder="¿Qué se buscó trabajar en esta sesión?"
                                        ClientIDMode="Static" />
                                    <asp:RequiredFieldValidator ID="rfvObjetivos" runat="server"
                                        ControlToValidate="txtObjetivos"
                                        ErrorMessage="Los objetivos son obligatorios."
                                        CssClass="field-error" Display="Dynamic"
                                        ValidationGroup="vgConsulta" />
                                </div>

                                <div class="field">
                                    <label for="txtObservaciones">Observaciones clínicas <sup>*</sup></label>
                                    <asp:TextBox ID="txtObservaciones" runat="server"
                                        TextMode="MultiLine" Rows="4"
                                        placeholder="Descripción de lo observado durante la sesión..."
                                        ClientIDMode="Static" />
                                    <asp:RequiredFieldValidator ID="rfvObservaciones" runat="server"
                                        ControlToValidate="txtObservaciones"
                                        ErrorMessage="Las observaciones son obligatorias."
                                        CssClass="field-error" Display="Dynamic"
                                        ValidationGroup="vgConsulta" />
                                </div>

                                <div class="field">
                                    <label for="txtHipotesis">Hipótesis</label>
                                    <asp:TextBox ID="txtHipotesis" runat="server"
                                        TextMode="MultiLine" Rows="3"
                                        placeholder="Hipótesis clínicas planteadas..."
                                        ClientIDMode="Static" />
                                </div>

                                <div class="field">
                                    <label for="txtIntervenciones">Intervenciones realizadas</label>
                                    <asp:TextBox ID="txtIntervenciones" runat="server"
                                        TextMode="MultiLine" Rows="3"
                                        placeholder="Técnicas e intervenciones aplicadas..."
                                        ClientIDMode="Static" />
                                </div>

                                <div class="field">
                                    <label for="txtEvolucion">Evolución observada</label>
                                    <asp:TextBox ID="txtEvolucion" runat="server"
                                        TextMode="MultiLine" Rows="3"
                                        placeholder="¿Cómo evolucionó el paciente en esta sesión?"
                                        ClientIDMode="Static" />
                                </div>

                            </div>

                            <div class="section-sep">Cierre clínico</div>

                            <div class="grid-2">
                                <div class="field">
                                    <label for="txtDiagnostico">Diagnóstico</label>
                                    <asp:TextBox ID="txtDiagnostico" runat="server"
                                        TextMode="MultiLine" Rows="3"
                                        placeholder="Impresión diagnóstica de la sesión..."
                                        ClientIDMode="Static" />
                                </div>

                                <div class="field">
                                    <label for="txtTratamiento">Tratamiento</label>
                                    <asp:TextBox ID="txtTratamiento" runat="server"
                                        TextMode="MultiLine" Rows="3"
                                        placeholder="Líneas de tratamiento planificadas..."
                                        ClientIDMode="Static" />
                                </div>
                            </div>

                      
                            <div class="form-actions">
                                <a href="FormDashboard.aspx" class="btn-secondary">Cancelar</a>
                                <asp:Button ID="btnRegistrar" runat="server"
                                    Text="Registrar consulta"
                                    CssClass="btn-primary"
                                    ValidationGroup="vgConsulta"
                                    OnClick="btnRegistrar_Click" />
                            </div>

                        </div>
                    </div>

                   
                    <div class="consulta-info-col">

                   
                        <div class="content-card paciente-card">
                            <div class="paciente-avatar-grande">
                                <asp:Label ID="lblPacienteIniciales" runat="server" Text="--" CssClass="avatar-circulo" />
                            </div>
                            <asp:Label ID="lblPacienteNombre"   runat="server" CssClass="paciente-card-nombre" Text="Seleccioná un paciente" />
                            <asp:Label ID="lblPacienteEdad"     runat="server" CssClass="paciente-card-dato"   Text="" />
                            <asp:Label ID="lblPacienteOcupacion" runat="server" CssClass="paciente-card-dato"  Text="" />
                            <asp:Label ID="lblPacienteEstado"   runat="server" CssClass="paciente-card-dato"   Text="" />

                            <div class="paciente-card-sep"></div>

                            <div class="paciente-stat-row">
                                <div class="paciente-stat">
                                    <asp:Label ID="lblTotalConsultas" runat="server" CssClass="stat-num" Text="--" />
                                    <span class="stat-label">Consultas</span>
                                </div>
                                <div class="paciente-stat">
                                    <asp:Label ID="lblUltimaConsulta" runat="server" CssClass="stat-fecha" Text="--" />
                                    <span class="stat-label">Última sesión</span>
                                </div>
                            </div>
                        </div>

                     
                        <div class="content-card aviso-card">
                            <div class="aviso-icon">🔒</div>
                            <p class="aviso-titulo">Datos encriptados</p>
                            <p class="aviso-texto">El contenido clínico se encripta con AES antes de persistirse, cumpliendo la Ley 25.326.</p>
                        </div>

                     
                        <div class="content-card historial-card">
                            <p class="historial-titulo">Últimas consultas</p>
                            <asp:Repeater ID="rptUltimasConsultas" runat="server">
                                <ItemTemplate>
                                    <div class="historial-item">
                                        <span class="historial-fecha"><%# Eval("Fecha", "{0:dd/MM/yyyy}") %></span>
                                        <span class="historial-resumen"><%# Eval("Resumen") %></span>
                                    </div>
                                </ItemTemplate>
                            </asp:Repeater>
                            <asp:Label ID="lblSinConsultas" runat="server"
                                CssClass="historial-vacio"
                                Text="Este paciente aún no tiene consultas registradas."
                                Visible="false" />
                        </div>

                    </div>

                </div>
            </div>
        </div>

    </form>
</body>
</html>
