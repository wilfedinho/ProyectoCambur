<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FormCambiarClave.aspx.cs" Inherits="FormCambiarClave" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta name="viewport" content="width=device-width, initial-scale=1.0"/>
    <title>Cambur — Cambiar Clave</title>
    <link href="EstilosPaginas/Shared.css"             rel="stylesheet" type="text/css"/>
    <link href="EstilosPaginas/FormCambiarClave.css"   rel="stylesheet" type="text/css"/>
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
                <a href="FormExportarReporte.aspx"   class="nav-item">💾 Exportar</a>
            </nav>
            <div class="sidebar-footer">
                <a href="FormSuscripcion.aspx"  class="nav-item">💳 Mi Suscripción</a>
                <a href="FormLogout.aspx"       class="nav-item nav-logout">🚪 Cerrar sesión</a>
            </div>
        </aside>

        <!-- ÁREA PRINCIPAL -->
        <div class="main-wrap">

            <!-- HEADER -->
            <header class="top-header">
                <div class="header-title">
                    <span class="header-section">Configuración</span>
                    <span class="header-sep">/</span>
                    <span class="header-page">Cambiar clave</span>
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

                <div class="clave-layout">

                    <!-- COLUMNA PRINCIPAL -->
                    <div class="clave-main">
                        <div class="content-card">

                            <div class="card-header">
                                <h2 class="card-title">Cambiar contraseña</h2>
                                <p class="card-subtitle">Actualizá tu contraseña de acceso. La nueva clave se hasheará con SHA-256 antes de almacenarse.</p>
                            </div>

                            <div class="section-sep">Verificación de identidad</div>

                            <!-- Clave actual -->
                            <div class="field">
                                <label for="txtClaveActual">Contraseña actual <sup>*</sup></label>
                                <div class="pass-wrap">
                                    <asp:TextBox ID="txtClaveActual" runat="server"
                                        TextMode="Password" MaxLength="100"
                                        placeholder="Tu contraseña actual"
                                        ClientIDMode="Static" />
                                    <button type="button" class="pass-toggle"
                                            onclick="toggleField('txtClaveActual', this)">👁</button>
                                </div>
                                <asp:RequiredFieldValidator ID="rfvClaveActual" runat="server"
                                    ControlToValidate="txtClaveActual"
                                    ErrorMessage="La contraseña actual es obligatoria."
                                    CssClass="field-error" Display="Dynamic"
                                    ValidationGroup="vgClave" />
                            </div>

                            <div class="section-sep">Nueva contraseña</div>

                            <!-- Nueva clave -->
                            <div class="field">
                                <label for="txtClaveNueva">Nueva contraseña <sup>*</sup></label>
                                <div class="pass-wrap">
                                    <asp:TextBox ID="txtClaveNueva" runat="server"
                                        TextMode="Password" MaxLength="100"
                                        placeholder="Mín. 7 car., mayúscula y símbolo"
                                        ClientIDMode="Static"
                                        oninput="checkStrength(this.value)" />
                                    <button type="button" class="pass-toggle"
                                            onclick="toggleField('txtClaveNueva', this)">👁</button>
                                </div>
                                <!-- Indicador de fortaleza -->
                                <div class="strength-bars">
                                    <div class="strength-bar" id="bar1"></div>
                                    <div class="strength-bar" id="bar2"></div>
                                    <div class="strength-bar" id="bar3"></div>
                                </div>
                                <span class="strength-label" id="lblStrength"></span>
                                <asp:RequiredFieldValidator ID="rfvClaveNueva" runat="server"
                                    ControlToValidate="txtClaveNueva"
                                    ErrorMessage="La nueva contraseña es obligatoria."
                                    CssClass="field-error" Display="Dynamic"
                                    ValidationGroup="vgClave" />
                            </div>

                            <!-- Confirmar nueva clave -->
                            <div class="field" style="margin-top:4px;">
                                <label for="txtClaveConfirmacion">Confirmar nueva contraseña <sup>*</sup></label>
                                <div class="pass-wrap">
                                    <asp:TextBox ID="txtClaveConfirmacion" runat="server"
                                        TextMode="Password" MaxLength="100"
                                        placeholder="Repetir nueva contraseña"
                                        ClientIDMode="Static" />
                                    <button type="button" class="pass-toggle"
                                            onclick="toggleField('txtClaveConfirmacion', this)">👁</button>
                                </div>
                                <asp:RequiredFieldValidator ID="rfvConfirmacion" runat="server"
                                    ControlToValidate="txtClaveConfirmacion"
                                    ErrorMessage="La confirmación es obligatoria."
                                    CssClass="field-error" Display="Dynamic"
                                    ValidationGroup="vgClave" />
                                <asp:CompareValidator ID="cvClaves" runat="server"
                                    ControlToValidate="txtClaveConfirmacion"
                                    ControlToCompare="txtClaveNueva"
                                    ErrorMessage="La nueva contraseña y su confirmación no coinciden."
                                    CssClass="field-error" Display="Dynamic"
                                    ValidationGroup="vgClave" />
                            </div>

                            <div class="form-actions">
                                <a href="FormDashboard.aspx" class="btn-secondary">Cancelar</a>
                                <asp:Button ID="btnConfirmar" runat="server"
                                    Text="Confirmar cambio"
                                    CssClass="btn-primary"
                                    ValidationGroup="vgClave"
                                    OnClick="btnConfirmar_Click" />
                            </div>

                        </div>
                    </div>

                    <!-- COLUMNA LATERAL -->
                    <div class="clave-aside">

                        <!-- Política de contraseña -->
                        <div class="content-card politica-card">
                            <p class="accesos-titulo">Política de contraseña</p>
                            <div class="politica-item" id="polLong">
                                <span class="pol-icono pol-pendiente">○</span>
                                <span class="pol-texto">Mínimo 7 caracteres</span>
                            </div>
                            <div class="politica-item" id="polMay">
                                <span class="pol-icono pol-pendiente">○</span>
                                <span class="pol-texto">Al menos una mayúscula</span>
                            </div>
                            <div class="politica-item" id="polMin">
                                <span class="pol-icono pol-pendiente">○</span>
                                <span class="pol-texto">Al menos una minúscula</span>
                            </div>
                            <div class="politica-item" id="polEsp">
                                <span class="pol-icono pol-pendiente">○</span>
                                <span class="pol-texto">Al menos un carácter especial</span>
                            </div>
                            <div class="politica-item" id="polDif">
                                <span class="pol-icono pol-pendiente">○</span>
                                <span class="pol-texto">Distinta a la contraseña actual</span>
                            </div>
                        </div>

                        <div class="content-card aviso-card">
                            <div class="aviso-icon">🔒</div>
                            <p class="aviso-titulo">Almacenamiento seguro</p>
                            <p class="aviso-texto">Tu contraseña se almacena como hash SHA-256 irreversible. Nadie, incluido el administrador, puede ver tu clave en texto plano.</p>
                        </div>

                        <div class="content-card aviso-card">
                            <div class="aviso-icon">⚠️</div>
                            <p class="aviso-titulo">Al cambiar la clave</p>
                            <p class="aviso-texto">No se cerrará tu sesión activa. Si sospechás accesos no autorizados, cerrá sesión desde todos tus dispositivos.</p>
                        </div>

                    </div>

                </div>
            </div>
        </div>

    </form>

    <script type="text/javascript">
        /* Mostrar/ocultar campo de contraseña */
        function toggleField(id, btn) {
            var input = document.getElementById(id);
            if (!input) return;
            input.type = input.type === 'password' ? 'text' : 'password';
        }

        /* Indicador de fortaleza + política */
        function checkStrength(val) {
            var b1  = document.getElementById('bar1');
            var b2  = document.getElementById('bar2');
            var b3  = document.getElementById('bar3');
            var lbl = document.getElementById('lblStrength');
            if (!b1) return;

            [b1, b2, b3].forEach(function (b) { b.style.background = '#C8C2B8'; });
            lbl.style.display = 'none';

            if (!val) { actualizarPolitica(val); return; }

            var score = 0;
            if (val.length >= 7)            score++;
            if (/[A-Z]/.test(val))          score++;
            if (/[^a-zA-Z0-9]/.test(val))  score++;

            lbl.style.display = 'block';
            if (score === 1) {
                b1.style.background = '#E8455A';
                lbl.textContent = 'Contraseña débil';
                lbl.style.color = '#E8455A';
            } else if (score === 2) {
                b1.style.background = b2.style.background = '#F4A261';
                lbl.textContent = 'Contraseña regular';
                lbl.style.color = '#F4A261';
            } else {
                b1.style.background = b2.style.background = b3.style.background = '#2A9D8F';
                lbl.textContent = 'Contraseña segura';
                lbl.style.color = '#2A9D8F';
            }

            actualizarPolitica(val);
        }

        /* Actualizar checklist de política */
        function actualizarPolitica(val) {
            marcarPol('polLong', val && val.length >= 7);
            marcarPol('polMay',  val && /[A-Z]/.test(val));
            marcarPol('polMin',  val && /[a-z]/.test(val));
            marcarPol('polEsp',  val && /[^a-zA-Z0-9]/.test(val));
            /* polDif solo se puede verificar server-side */
        }

        function marcarPol(id, ok) {
            var el    = document.getElementById(id);
            if (!el) return;
            var icono = el.querySelector('.pol-icono');
            if (!icono) return;
            icono.textContent = ok ? '●' : '○';
            icono.className   = ok ? 'pol-icono pol-ok' : 'pol-icono pol-pendiente';
        }
    </script>
</body>
</html>
