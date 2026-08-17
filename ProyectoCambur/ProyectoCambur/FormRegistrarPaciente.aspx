<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FormRegistrarPaciente.aspx.cs" Inherits="FormRegistrarPaciente" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta name="viewport" content="width=device-width, initial-scale=1.0"/>
    <title>Cambur — Registrar Paciente</title>
    <link href="EstilosPaginas/Shared.css"               rel="stylesheet" type="text/css"/>
    <link href="EstilosPaginas/FormRegistroPaciente.css"  rel="stylesheet" type="text/css"/>
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
                <a href="FormRegistroPaciente.aspx"  class="nav-item active">👤 Pacientes</a>
                <a href="FormRealizarConsulta.aspx"  class="nav-item">🗒️ Consultas</a>
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
                    <span class="header-section">Pacientes</span>
                    <span class="header-sep">/</span>
                    <span class="header-page">Registrar paciente</span>
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

                <div class="content-card">
                    <div class="card-header">
                        <h2 class="card-title">Nuevo paciente</h2>
                        <p class="card-subtitle">Completá los datos para registrar un nuevo paciente en tu entorno clínico.</p>
                    </div>

                    <div class="section-sep">Datos personales</div>

                    <div class="grid-3">
                        <div class="field">
                            <label for="txtNombre">Nombre <sup>*</sup></label>
                            <asp:TextBox ID="txtNombre" runat="server" MaxLength="100" placeholder="Ej: Martín" ClientIDMode="Static" />
                            <asp:RequiredFieldValidator ID="rfvNombre" runat="server"
                                ControlToValidate="txtNombre" ErrorMessage="El nombre es obligatorio."
                                CssClass="field-error" Display="Dynamic" ValidationGroup="vgPaciente" />
                        </div>

                        <div class="field">
                            <label for="txtApellido">Apellido <sup>*</sup></label>
                            <asp:TextBox ID="txtApellido" runat="server" MaxLength="100" placeholder="Ej: González" ClientIDMode="Static" />
                            <asp:RequiredFieldValidator ID="rfvApellido" runat="server"
                                ControlToValidate="txtApellido" ErrorMessage="El apellido es obligatorio."
                                CssClass="field-error" Display="Dynamic" ValidationGroup="vgPaciente" />
                        </div>

                        <div class="field">
                            <label for="txtFechaNacimiento">Fecha de nacimiento <sup>*</sup></label>
                            <asp:TextBox ID="txtFechaNacimiento" runat="server" TextMode="Date" ClientIDMode="Static" />
                            <asp:RequiredFieldValidator ID="rfvFecha" runat="server"
                                ControlToValidate="txtFechaNacimiento" ErrorMessage="La fecha es obligatoria."
                                CssClass="field-error" Display="Dynamic" ValidationGroup="vgPaciente" />
                        </div>

                        <div class="field">
                            <label for="txtOcupacion">Ocupación</label>
                            <asp:TextBox ID="txtOcupacion" runat="server" MaxLength="150" placeholder="Ej: Docente" ClientIDMode="Static" />
                        </div>

                        <div class="field">
                            <label for="ddlEstadoCivil">Estado civil <sup>*</sup></label>
                            <asp:DropDownList ID="ddlEstadoCivil" runat="server" ClientIDMode="Static">
                                <asp:ListItem Value=""    Text="Seleccioná..." />
                                <asp:ListItem Value="SOL" Text="Soltero/a" />
                                <asp:ListItem Value="CAS" Text="Casado/a" />
                                <asp:ListItem Value="DIV" Text="Divorciado/a" />
                                <asp:ListItem Value="VIU" Text="Viudo/a" />
                                <asp:ListItem Value="PAR" Text="En pareja" />
                                <asp:ListItem Value="SEP" Text="Separado/a" />
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="rfvEstado" runat="server"
                                ControlToValidate="ddlEstadoCivil" InitialValue=""
                                ErrorMessage="Seleccioná el estado civil."
                                CssClass="field-error" Display="Dynamic" ValidationGroup="vgPaciente" />
                        </div>

                        <div class="field">
                            <label>Sexo <sup>*</sup></label>
                            <div class="radio-group">
                                <label class="radio-label">
                                    <asp:RadioButton ID="rbMasculino"   runat="server" GroupName="Sexo" Text="Masculino" />
                                </label>
                                <label class="radio-label">
                                    <asp:RadioButton ID="rbFemenino"    runat="server" GroupName="Sexo" Text="Femenino" />
                                </label>
                                <label class="radio-label">
                                    <asp:RadioButton ID="rbNoEspecifica" runat="server" GroupName="Sexo" Text="No especifica" />
                                </label>
                            </div>
                        </div>
                    </div>

                    <div class="section-sep">Contacto</div>

                    <div class="grid-2">
                        <div class="field">
                            <label for="txtEmail">Correo electrónico</label>
                            <asp:TextBox ID="txtEmail" runat="server" TextMode="Email" MaxLength="150"
                                placeholder="martin@email.com" ClientIDMode="Static" />
                            <asp:RegularExpressionValidator ID="revEmail" runat="server"
                                ControlToValidate="txtEmail"
                                ValidationExpression="^[\w\.\-]+@[\w\-]+\.[a-zA-Z]{2,}$"
                                ErrorMessage="Formato de correo inválido."
                                CssClass="field-error" Display="Dynamic" ValidationGroup="vgPaciente" />
                        </div>

                        <div class="field">
                            <label for="txtTelefono">Teléfono</label>
                            <asp:TextBox ID="txtTelefono" runat="server" MaxLength="20"
                                placeholder="Ej: 11-2345-6789" ClientIDMode="Static" />
                        </div>
                    </div>

                    <div class="form-actions">
                        <a href="FormDashboard.aspx" class="btn-secondary">Cancelar</a>
                        <asp:Button ID="btnRegistrar" runat="server"
                            Text="Registrar paciente"
                            CssClass="btn-primary"
                            ValidationGroup="vgPaciente"
                            OnClick="btnRegistrar_Click" />
                    </div>
                </div>

               
                <div class="content-card mt-24">

                    <div class="card-header-row">
                        <div class="card-header-left">
                            <h2 class="card-title">Pacientes registrados</h2>
                            <div class="badges-row">
                                <asp:Label ID="lblBadgeActivos"   runat="server" CssClass="badge-activos"   Text="" />
                                <asp:Label ID="lblBadgeInactivos" runat="server" CssClass="badge-inactivos" Text="" />
                            </div>
                        </div>
                    </div>

                    <div class="table-wrap">
                        <asp:GridView ID="gvPacientes" runat="server"
                            CssClass="data-table"
                            AutoGenerateColumns="false"
                            GridLines="None"
                            EmptyDataText="Todavía no registraste ningún paciente."
                            OnRowCommand="gvPacientes_RowCommand">

                            <EmptyDataRowStyle CssClass="empty-row" />
                            <HeaderStyle      CssClass="table-header" />
                            <RowStyle         CssClass="table-row" />
                            <AlternatingRowStyle CssClass="table-row table-row-alt" />

                            <Columns>
                                <asp:BoundField  DataField="NombreCompleto" HeaderText="Paciente"      HeaderStyle-CssClass="th-left" />
                                <asp:BoundField  DataField="Edad"           HeaderText="Edad"          HeaderStyle-CssClass="th-centro" ItemStyle-CssClass="td-centro" />
                                <asp:BoundField  DataField="EstadoCivil"    HeaderText="Estado civil"  HeaderStyle-CssClass="th-centro" ItemStyle-CssClass="td-centro" />
                                <asp:BoundField  DataField="FechaRegistro"  HeaderText="Registrado"    HeaderStyle-CssClass="th-centro" ItemStyle-CssClass="td-centro"
                                    DataFormatString="{0:dd/MM/yyyy}" />

                             
                                <asp:TemplateField HeaderText="Estado" HeaderStyle-CssClass="th-centro" ItemStyle-CssClass="td-centro">
                                    <ItemTemplate>
                                        <span class='<%# (bool)Eval("Activo") ? "badge-estado activo" : "badge-estado inactivo" %>'>
                                            <%# (bool)Eval("Activo") ? "Activo" : "Inactivo" %>
                                        </span>
                                    </ItemTemplate>
                                </asp:TemplateField>

                            
                                <asp:TemplateField HeaderText="Acciones" HeaderStyle-CssClass="th-centro" ItemStyle-CssClass="td-acciones">
                                    <ItemTemplate>

                                    
                                        <a href='FormHistorialClinico.aspx?id=<%# Eval("IdPaciente") %>' class="tbl-btn tbl-btn-ver" title="Ver detalle">
                                            👁️ Ver
                                        </a>

                                        <asp:LinkButton ID="lbModificar" runat="server"
                                            CommandName="Modificar"
                                            CommandArgument='<%# Eval("IdPaciente") %>'
                                            CssClass="tbl-btn tbl-btn-mod"
                                            Text="✏️ Modificar" />

                                       
                                        <asp:LinkButton ID="lbBaja" runat="server"
                                            CommandName="DarBaja"
                                            CommandArgument='<%# Eval("IdPaciente") %>'
                                            CssClass='<%# (bool)Eval("Activo") ? "tbl-btn tbl-btn-baja" : "tbl-btn-hidden" %>'
                                            Text="🚫 Dar de baja"
                                            OnClientClick="return confirm('¿Confirmás dar de baja a este paciente?');" />

                                      
                                        <asp:LinkButton ID="lbReactivar" runat="server"
                                            CommandName="Reactivar"
                                            CommandArgument='<%# Eval("IdPaciente") %>'
                                            CssClass='<%# (bool)Eval("Activo") ? "tbl-btn-hidden" : "tbl-btn tbl-btn-reactivar" %>'
                                            Text="✅ Reactivar" />

                                    </ItemTemplate>
                                </asp:TemplateField>

                            </Columns>
                        </asp:GridView>
                    </div>
                </div>

            </div>
        </div>

    </form>
</body>
</html>
