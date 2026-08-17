<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FormSuscripcion.aspx.cs" Inherits="FormSuscripcion" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta name="viewport" content="width=device-width, initial-scale=1.0"/>
    <title>Cambur — Mi Suscripción</title>
    <link href="EstilosPaginas/Shared.css"           rel="stylesheet" type="text/css"/>
    <link href="EstilosPaginas/FormSuscripcion.css"  rel="stylesheet" type="text/css"/>
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
                <a href="FormSuscripcion.aspx" class="nav-item active">💳 Mi Suscripción</a>
                <a href="FormLogin.aspx"       class="nav-item nav-logout">🚪 Cerrar sesión</a>
            </div>
        </aside>

    
        <div class="main-wrap">

    
            <header class="top-header">
                <div class="header-title">
                    <span class="header-section">Cuenta</span>
                    <span class="header-sep">/</span>
                    <span class="header-page">Mi suscripción</span>
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

                <div class="suscripcion-layout">

        
                    <div class="suscripcion-main">


                        <div class="content-card plan-activo-card">
                            <div class="plan-activo-header">
                                <div>
                                    <p class="plan-eyebrow">Plan activo</p>
                                    <asp:Label ID="lblPlanNombre" runat="server"
                                        CssClass="plan-nombre" Text="" />
                                </div>
                                <asp:Label ID="lblPlanBadge" runat="server"
                                    CssClass="badge-estado activo" Text="Activa" />
                            </div>

                            <div class="plan-detalles-grid">
                                <div class="plan-detalle-item">
                                    <span class="pd-label">Activa desde</span>
                                    <asp:Label ID="lblFechaInicio" runat="server"
                                        CssClass="pd-valor" Text="" />
                                </div>
                                <div class="plan-detalle-item">
                                    <span class="pd-label">Próximo vencimiento</span>
                                    <asp:Label ID="lblProxVencimiento" runat="server"
                                        CssClass="pd-valor" Text="" />
                                </div>
                                <div class="plan-detalle-item">
                                    <span class="pd-label">Medio de pago</span>
                                    <asp:Label ID="lblMedioPago" runat="server"
                                        CssClass="pd-valor" Text="" />
                                </div>
                                <div class="plan-detalle-item">
                                    <span class="pd-label">Precio mensual</span>
                                    <asp:Label ID="lblPrecio" runat="server"
                                        CssClass="pd-valor pd-precio" Text="" />
                                </div>
                            </div>

                            <div class="plan-acciones">
                                <asp:Button ID="btnActualizarPago" runat="server"
                                    Text="💳 Actualizar medio de pago"
                                    CssClass="btn-secondary"
                                    OnClick="btnActualizarPago_Click"
                                    CausesValidation="false" />
                                <asp:Button ID="btnCancelar" runat="server"
                                    Text="Cancelar suscripción"
                                    CssClass="btn-cancelar"
                                    OnClick="btnCancelar_Click"
                                    CausesValidation="false"
                                    OnClientClick="return confirm('¿Confirmás la cancelación? La suscripción permanecerá activa hasta el vencimiento del período en curso.');" />
                            </div>
                        </div>

                    
                        <asp:Panel ID="pnlActualizarPago" runat="server"
                            CssClass="content-card modal-pago-card mt-24" Visible="false">
                            <h3 class="modal-titulo">Actualizar medio de pago</h3>
                            <div class="grid-2" style="max-width:460px; margin-top:16px;">
                                <div class="field full">
                                    <label for="txtNuevaTarjeta">Número de tarjeta <sup>*</sup></label>
                                    <asp:TextBox ID="txtNuevaTarjeta" runat="server"
                                        MaxLength="19" placeholder="0000 0000 0000 0000"
                                        ClientIDMode="Static"
                                        oninput="formatCard(this)" />
                                    <asp:RequiredFieldValidator ID="rfvTarjeta" runat="server"
                                        ControlToValidate="txtNuevaTarjeta"
                                        ErrorMessage="Ingresá el número de tarjeta."
                                        CssClass="field-error" Display="Dynamic"
                                        ValidationGroup="vgPago" />
                                </div>
                                <div class="field">
                                    <label for="txtNuevoVence">Vencimiento <sup>*</sup></label>
                                    <asp:TextBox ID="txtNuevoVence" runat="server"
                                        MaxLength="5" placeholder="MM/AA"
                                        ClientIDMode="Static"
                                        oninput="formatExpiry(this)" />
                                    <asp:RequiredFieldValidator ID="rfvVence" runat="server"
                                        ControlToValidate="txtNuevoVence"
                                        ErrorMessage="Obligatorio."
                                        CssClass="field-error" Display="Dynamic"
                                        ValidationGroup="vgPago" />
                                </div>
                                <div class="field">
                                    <label for="txtNuevoCVV">CVV <sup>*</sup></label>
                                    <asp:TextBox ID="txtNuevoCVV" runat="server"
                                        TextMode="Password" MaxLength="4"
                                        placeholder="CVV" ClientIDMode="Static" />
                                    <asp:RequiredFieldValidator ID="rfvCVV" runat="server"
                                        ControlToValidate="txtNuevoCVV"
                                        ErrorMessage="Obligatorio."
                                        CssClass="field-error" Display="Dynamic"
                                        ValidationGroup="vgPago" />
                                </div>
                            </div>
                            <div class="form-actions">
                                <asp:Button ID="btnCancelarPago" runat="server"
                                    Text="Cancelar" CssClass="btn-secondary"
                                    OnClick="btnCancelarPago_Click" CausesValidation="false" />
                                <asp:Button ID="btnGuardarPago" runat="server"
                                    Text="Guardar nuevo medio de pago"
                                    CssClass="btn-primary"
                                    ValidationGroup="vgPago"
                                    OnClick="btnGuardarPago_Click" />
                            </div>
                        </asp:Panel>

                 
                        <div class="content-card mt-24">
                            <div class="card-header">
                                <h2 class="card-title">Cambiar de plan</h2>
                                <p class="card-subtitle">Compará las características de cada plan y seleccioná el que mejor se adapte a tu práctica.</p>
                            </div>

                            <div class="planes-comparativa">

            
                                <div class="comp-fila comp-header">
                                    <div class="comp-feature"></div>
                                    <div class="comp-plan">
                                        <div class="comp-plan-nombre">Básico</div>
                                        <div class="comp-plan-precio">$4.990<span>/mes</span></div>
                                    </div>
                                    <div class="comp-plan comp-plan-destacado">
                                        <div class="comp-plan-badge">Más elegido</div>
                                        <div class="comp-plan-nombre">Profesional</div>
                                        <div class="comp-plan-precio">$9.990<span>/mes</span></div>
                                    </div>
                                    <div class="comp-plan">
                                        <div class="comp-plan-nombre">Premium</div>
                                        <div class="comp-plan-precio">$14.990<span>/mes</span></div>
                                    </div>
                                </div>

                     
                                <div class="comp-fila">
                                    <div class="comp-feature">Pacientes</div>
                                    <div class="comp-valor">Hasta 20</div>
                                    <div class="comp-valor comp-destacado">Ilimitados</div>
                                    <div class="comp-valor">Ilimitados</div>
                                </div>
                                <div class="comp-fila">
                                    <div class="comp-feature">Consultas y Historial</div>
                                    <div class="comp-valor">✓</div>
                                    <div class="comp-valor comp-destacado">✓</div>
                                    <div class="comp-valor">✓</div>
                                </div>
                                <div class="comp-fila">
                                    <div class="comp-feature">Resumen Clínico IA</div>
                                    <div class="comp-valor comp-no">✗</div>
                                    <div class="comp-valor comp-destacado">✓</div>
                                    <div class="comp-valor">✓</div>
                                </div>
                                <div class="comp-fila">
                                    <div class="comp-feature">Informe de Derivación IA</div>
                                    <div class="comp-valor comp-no">✗</div>
                                    <div class="comp-valor comp-destacado">✓</div>
                                    <div class="comp-valor">✓</div>
                                </div>
                                <div class="comp-fila">
                                    <div class="comp-feature">Perfilación del Paciente</div>
                                    <div class="comp-valor comp-no">✗</div>
                                    <div class="comp-valor comp-destacado">✓</div>
                                    <div class="comp-valor">✓</div>
                                </div>
                                <div class="comp-fila">
                                    <div class="comp-feature">Exportaciones PDF</div>
                                    <div class="comp-valor comp-no">✗</div>
                                    <div class="comp-valor comp-destacado">Básico</div>
                                    <div class="comp-valor">Avanzado</div>
                                </div>
                                <div class="comp-fila">
                                    <div class="comp-feature">Soporte</div>
                                    <div class="comp-valor">Email</div>
                                    <div class="comp-valor comp-destacado">Email + Chat</div>
                                    <div class="comp-valor">Prioritario</div>
                                </div>

                    
                                <div class="comp-fila comp-footer">
                                    <div class="comp-feature"></div>
                                    <div class="comp-action">
                                        <asp:Button ID="btnSelBasico" runat="server"
                                            Text="Seleccionar"
                                            CssClass="btn-plan-sel"
                                            CommandArgument="1"
                                            OnClick="btnCambiarPlan_Click"
                                            CausesValidation="false" />
                                    </div>
                                    <div class="comp-action comp-destacado">
                                        <asp:Button ID="btnSelProfesional" runat="server"
                                            Text="Plan actual"
                                            CssClass="btn-plan-actual"
                                            Enabled="false"
                                            CommandArgument="2"
                                            OnClick="btnCambiarPlan_Click"
                                            CausesValidation="false" />
                                    </div>
                                    <div class="comp-action">
                                        <asp:Button ID="btnSelPremium" runat="server"
                                            Text="Seleccionar"
                                            CssClass="btn-plan-sel"
                                            CommandArgument="3"
                                            OnClick="btnCambiarPlan_Click"
                                            CausesValidation="false" />
                                    </div>
                                </div>

                            </div>
                        </div>
                    </div>

           
                    <div class="suscripcion-aside">

                        <div class="content-card uso-card">
                            <p class="accesos-titulo">Uso del período</p>
                            <div class="uso-item">
                                <span class="uso-label">Consultas este mes</span>
                                <span class="uso-valor">
                                    <asp:Label ID="lblUsoConsultas" runat="server" Text="14" />
                                </span>
                            </div>
                            <div class="uso-item">
                                <span class="uso-label">Resúmenes IA generados</span>
                                <span class="uso-valor">
                                    <asp:Label ID="lblUsoResumenes" runat="server" Text="3" />
                                </span>
                            </div>
                            <div class="uso-item">
                                <span class="uso-label">Derivaciones generadas</span>
                                <span class="uso-valor">
                                    <asp:Label ID="lblUsoDerivaciones" runat="server" Text="1" />
                                </span>
                            </div>
                            <div class="uso-item" style="border-bottom:none;">
                                <span class="uso-label">Perfilaciones</span>
                                <span class="uso-valor">
                                    <asp:Label ID="lblUsoPerfiles" runat="server" Text="2" />
                                </span>
                            </div>
                        </div>

                        <div class="content-card aviso-card">
                            <div class="aviso-icon">🔒</div>
                            <p class="aviso-titulo">Pago seguro</p>
                            <p class="aviso-texto">Los datos bancarios se transmiten encriptados y no se almacenan en nuestros servidores.</p>
                        </div>

                    </div>

                </div>
            </div>
        </div>

    
        <asp:Panel ID="pnlModalCancelacion" runat="server"
            CssClass="modal-overlay" Visible="false">
            <div class="modal-card">
                <div class="modal-icono">⚠️</div>
                <h3 class="modal-card-titulo">Suscripción cancelada</h3>
                <asp:Label ID="lblMensajeCancelacion" runat="server"
                    CssClass="modal-card-texto" Text="" />
                <asp:Button ID="btnCerrarModal" runat="server"
                    Text="Entendido"
                    CssClass="btn-primary"
                    OnClick="btnCerrarModal_Click"
                    CausesValidation="false" />
            </div>
        </asp:Panel>

    </form>

    <script type="text/javascript">
        function formatCard(input) {
            var v = input.value.replace(/\D/g,'').substring(0,16);
            input.value = v.replace(/(.{4})/g,'$1 ').trim();
        }
        function formatExpiry(input) {
            var v = input.value.replace(/\D/g,'').substring(0,4);
            input.value = v.length >= 3 ? v.substring(0,2)+'/'+v.substring(2) : v;
        }
    </script>
</body>
</html>
