<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FormInformeDerivacion.aspx.cs" Inherits="FormInformeDerivacion" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta name="viewport" content="width=device-width, initial-scale=1.0"/>
    <title>Cambur — Informe de Derivación</title>
    <link href="EstilosPaginas/Shared.css"                  rel="stylesheet" type="text/css"/>
    <link href="EstilosPaginas/FormInformeDerivacion.css"   rel="stylesheet" type="text/css"/>
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
                <a href="FormInformeDerivacion.aspx" class="nav-item active">📤 Derivaciones</a>
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
                    <span class="header-section">Derivaciones</span>
                    <span class="header-sep">/</span>
                    <asp:Label ID="lblHeaderTitulo" runat="server"
                        CssClass="header-page" Text="Generar informe" />
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

        
            <div class="page-content">

                <asp:Label ID="lblMensaje" runat="server"
                    Visible="false" CssClass="server-error" />

             
                <asp:Panel ID="pnlFormulario" runat="server">
                    <div class="derivacion-layout">

                  
                        <div class="derivacion-form-col">
                            <div class="content-card">

                    
                                <div class="paciente-header">
                                    <div class="paciente-header-avatar">
                                        <asp:Label ID="lblPacienteIniciales"
                                            runat="server" Text="MG" />
                                    </div>
                                    <div class="paciente-header-info">
                                        <asp:Label ID="lblPacienteNombre" runat="server"
                                            CssClass="paciente-header-nombre" Text="" />
                                        <div class="paciente-header-meta">
                                            <asp:Label ID="lblPacienteEdad"
                                                runat="server" CssClass="meta-item" Text="" />
                                            <span class="meta-sep">·</span>
                                            <asp:Label ID="lblPacienteConsultas"
                                                runat="server" CssClass="meta-item" Text="" />
                                        </div>
                                    </div>
                                </div>

                                <div class="section-sep">Datos de la derivación</div>

                                <div class="grid-2">
                                    <div class="field">
                                        <label for="ddlEspecialidad">Especialidad de derivación <sup>*</sup></label>
                                        <asp:DropDownList ID="ddlEspecialidad" runat="server"
                                            ClientIDMode="Static">
                                            <asp:ListItem Value=""    Text="Seleccioná..." />
                                            <asp:ListItem Value="PSI" Text="Psiquiatría" />
                                            <asp:ListItem Value="NEU" Text="Neurología" />
                                            <asp:ListItem Value="CAR" Text="Cardiología" />
                                            <asp:ListItem Value="NUT" Text="Nutrición" />
                                            <asp:ListItem Value="TRA" Text="Trabajo Social" />
                                            <asp:ListItem Value="MED" Text="Medicina General" />
                                            <asp:ListItem Value="FIS" Text="Fisiatría" />
                                            <asp:ListItem Value="OTR" Text="Otra especialidad" />
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="rfvEsp" runat="server"
                                            ControlToValidate="ddlEspecialidad"
                                            InitialValue=""
                                            ErrorMessage="Seleccioná la especialidad."
                                            CssClass="field-error" Display="Dynamic"
                                            ValidationGroup="vgDerivacion" />
                                    </div>

                                    <div class="field">
                                        <label for="txtProfDestino">Profesional destinatario <sup>*</sup></label>
                                        <asp:TextBox ID="txtProfDestino" runat="server"
                                            MaxLength="150"
                                            placeholder="Nombre del profesional que recibe"
                                            ClientIDMode="Static" />
                                        <asp:RequiredFieldValidator ID="rfvProf" runat="server"
                                            ControlToValidate="txtProfDestino"
                                            ErrorMessage="Ingresá el profesional destinatario."
                                            CssClass="field-error" Display="Dynamic"
                                            ValidationGroup="vgDerivacion" />
                                    </div>

                                    <div class="field full-col">
                                        <label for="txtInstitucion">Institución / Centro (opcional)</label>
                                        <asp:TextBox ID="txtInstitucion" runat="server"
                                            MaxLength="200"
                                            placeholder="Nombre del centro o institución"
                                            ClientIDMode="Static" />
                                    </div>

                                    <div class="field full-col">
                                        <label for="txtMotivo">Motivo de derivación <sup>*</sup></label>
                                        <asp:TextBox ID="txtMotivo" runat="server"
                                            TextMode="MultiLine" Rows="4"
                                            placeholder="Describí el motivo clínico por el que derivás al paciente..."
                                            ClientIDMode="Static" />
                                        <asp:RequiredFieldValidator ID="rfvMotivo" runat="server"
                                            ControlToValidate="txtMotivo"
                                            ErrorMessage="Ingresá el motivo de derivación."
                                            CssClass="field-error" Display="Dynamic"
                                            ValidationGroup="vgDerivacion" />
                                    </div>
                                </div>

                          
                                <div class="ia-aviso-derivacion">
                                    <div class="ia-aviso-icono">🤖</div>
                                    <div class="ia-aviso-texto">
                                        <strong>La IA utilizará para redactar el informe:</strong>
                                        <asp:Label ID="lblAvisoIA" runat="server"
                                            CssClass="ia-aviso-detalle" Text="" />
                                    </div>
                                </div>

                                <div class="form-actions">
                                    <a href="FormRegistroPaciente.aspx" class="btn-secondary">Cancelar</a>
                                    <asp:Button ID="btnGenerar" runat="server"
                                        Text="Generar informe con IA"
                                        CssClass="btn-primary btn-ia"
                                        ValidationGroup="vgDerivacion"
                                        OnClick="btnGenerar_Click"
                                        OnClientClick="mostrarCarga(); return true;" />
                                </div>

                            </div>
                        </div>

                  
                        <div class="derivacion-aside">
                            <div class="content-card info-card">
                                <p class="accesos-titulo">Información que se incluirá</p>
                                <div class="info-item">
                                    <span>📋</span>
                                    <asp:Label ID="lblInfoConsultas" runat="server"
                                        CssClass="info-item-texto" Text="" />
                                </div>
                                <div class="info-item">
                                    <span>🧬</span>
                                    <span class="info-item-texto">Antecedentes del historial clínico</span>
                                </div>
                                <div class="info-item">
                                    <span>📈</span>
                                    <span class="info-item-texto">Evolución observada en el tratamiento</span>
                                </div>
                                <div class="info-item">
                                    <span>🛠️</span>
                                    <span class="info-item-texto">Intervenciones y andamiajes implementados</span>
                                </div>
                            </div>

                            <div class="content-card aviso-card">
                                <div class="aviso-icon">🔒</div>
                                <p class="aviso-titulo">Revisión obligatoria</p>
                                <p class="aviso-texto">El informe generado deberá ser revisado y validado por el profesional antes de su uso externo.</p>
                            </div>
                        </div>

                    </div>
                </asp:Panel>

           
                <div class="carga-overlay" id="cargaOverlay" style="display:none;">
                    <div class="carga-card">
                        <div class="carga-spinner"></div>
                        <p class="carga-titulo">Generando informe con IA...</p>
                        <p class="carga-subtitulo">Consolidando información clínica y redactando el documento</p>
                    </div>
                </div>

              
                <asp:Panel ID="pnlAuditoria" runat="server"
                    CssClass="auditoria-layout" Visible="false">

                   
                    <div class="auditoria-form-col">
                        <div class="content-card">

                            <div class="auditoria-header">
                                <div>
                                    <h2 class="card-title">Informe de derivación generado</h2>
                                    <asp:Label ID="lblAuditoriaMeta" runat="server"
                                        CssClass="card-subtitle" Text="" />
                                </div>
                                <asp:Label runat="server"
                                    CssClass="badge-pendiente-revision"
                                    Text="Pendiente de revisión" />
                            </div>

                            <div class="ia-badge-resultado">
                                🤖 Generado por IA Asistiva · Revisá y ajustá el contenido antes de validar · No diagnóstico automático
                            </div>

                            <div class="section-sep">Contenido del informe</div>

                            <div class="auditoria-secciones">

                                <div class="field">
                                    <label for="txtSintesisDiagnostica">Síntesis diagnóstica</label>
                                    <asp:TextBox ID="txtSintesisDiagnostica" runat="server"
                                        TextMode="MultiLine" Rows="5"
                                        ClientIDMode="Static" />
                                </div>

                                <div class="field">
                                    <label for="txtAndamiajes">Andamiajes implementados</label>
                                    <asp:TextBox ID="txtAndamiajes" runat="server"
                                        TextMode="MultiLine" Rows="4"
                                        ClientIDMode="Static" />
                                </div>

                                <div class="field">
                                    <label for="txtObjetivos">Objetivos terapéuticos</label>
                                    <asp:TextBox ID="txtObjetivos" runat="server"
                                        TextMode="MultiLine" Rows="4"
                                        ClientIDMode="Static" />
                                </div>

                                <div class="grid-2">
                                    <div class="field">
                                        <label for="txtModalidadTrabajo">Modalidad de trabajo</label>
                                        <asp:TextBox ID="txtModalidadTrabajo" runat="server"
                                            TextMode="MultiLine" Rows="3"
                                            ClientIDMode="Static" />
                                    </div>
                                    <div class="field">
                                        <label for="txtMotivoDerivacion">Motivo de derivación</label>
                                        <asp:TextBox ID="txtMotivoDerivacion" runat="server"
                                            TextMode="MultiLine" Rows="3"
                                            ClientIDMode="Static" />
                                    </div>
                                </div>

                            </div>

                            <div class="section-sep">Firma del profesional</div>

                            <div class="field" style="max-width:400px;">
                                <label for="txtFirma">Firma digital <sup>*</sup></label>
                                <asp:TextBox ID="txtFirma" runat="server"
                                    placeholder="Nombre completo y número de matrícula"
                                    MaxLength="200"
                                    ClientIDMode="Static" />
                                <asp:RequiredFieldValidator ID="rfvFirma" runat="server"
                                    ControlToValidate="txtFirma"
                                    ErrorMessage="La firma es obligatoria para validar."
                                    CssClass="field-error" Display="Dynamic"
                                    ValidationGroup="vgAuditoria" />
                            </div>

                            <div class="form-actions">
                                <asp:Button ID="btnDescartar" runat="server"
                                    Text="🗑 Descartar informe"
                                    CssClass="btn-danger"
                                    OnClick="btnDescartar_Click"
                                    CausesValidation="false"
                                    OnClientClick="return confirm('¿Confirmar descarte? Esta acción es irreversible.');" />
                                <asp:Button ID="btnGuardarBorrador" runat="server"
                                    Text="Guardar borrador"
                                    CssClass="btn-secondary"
                                    OnClick="btnGuardarBorrador_Click"
                                    CausesValidation="false" />
                                <asp:Button ID="btnValidar" runat="server"
                                    Text="✓ Validar y firmar informe"
                                    CssClass="btn-success"
                                    ValidationGroup="vgAuditoria"
                                    OnClick="btnValidar_Click" />
                            </div>

                        </div>
                    </div>

          
                    <div class="auditoria-aside">

                        <div class="content-card meta-card">
                            <p class="accesos-titulo">Datos del informe</p>
                            <div class="meta-fila">
                                <span class="meta-label">Paciente</span>
                                <asp:Label ID="lblMetaPaciente" runat="server"
                                    CssClass="meta-valor" Text="" />
                            </div>
                            <div class="meta-fila">
                                <span class="meta-label">Especialidad destino</span>
                                <asp:Label ID="lblMetaEspecialidad" runat="server"
                                    CssClass="meta-valor" Text="" />
                            </div>
                            <div class="meta-fila">
                                <span class="meta-label">Profesional destinatario</span>
                                <asp:Label ID="lblMetaDestino" runat="server"
                                    CssClass="meta-valor" Text="" />
                            </div>
                            <div class="meta-fila" style="border-bottom:none;">
                                <span class="meta-label">Generado</span>
                                <asp:Label ID="lblMetaFecha" runat="server"
                                    CssClass="meta-valor" Text="" />
                            </div>
                        </div>

                        <div class="content-card aviso-card">
                            <div class="aviso-icon">⚠️</div>
                            <p class="aviso-titulo">Revisión profesional</p>
                            <p class="aviso-texto">Revisá y ajustá el contenido generado antes de validar. Solo vos podés firmar y habilitar este informe para uso externo.</p>
                        </div>

                    </div>

                </asp:Panel>

            </div>
        </div>

    </form>

    <script type="text/javascript">
        function mostrarCarga() {
            var o = document.getElementById('cargaOverlay');
            if (o) o.style.display = 'flex';
            return true;
        }
        window.addEventListener('DOMContentLoaded', function () {
            var o = document.getElementById('cargaOverlay');
            if (o) o.style.display = 'none';
        });
    </script>
</body>
</html>
