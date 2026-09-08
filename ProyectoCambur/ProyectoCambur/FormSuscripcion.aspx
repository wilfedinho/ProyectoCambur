<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FormSuscripcion.aspx.cs" Inherits="FormSuscripcion" %>
<%@ Register Src="~/UserControls/HeaderUsuario.ascx" TagPrefix="uc" TagName="HeaderUsuario" %>
<%@ Register Src="~/UserControls/SidebarNavegacion.ascx" TagPrefix="uc" TagName="SidebarNavegacion" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta name="viewport" content="width=device-width, initial-scale=1.0"/>
    <title>Cambur — Mi Suscripción</title>
    <link href="EstilosPaginas/Shared.css"           rel="stylesheet" type="text/css"/>
    <link href="EstilosPaginas/HeaderUsuario.css"    rel="stylesheet" type="text/css"/>
    <link href="EstilosPaginas/SidebarNavegacion.css" rel="stylesheet" type="text/css"/>
    <link href="EstilosPaginas/FormSuscripcion.css"  rel="stylesheet" type="text/css"/>
    <script src="https://js.stripe.com/v3/"></script>
</head>
<body>
    <form id="form1" runat="server">

        <aside class="sidebar">
            <div class="sidebar-logo">
                <div class="logotype">CAM<span>BUR</span></div>
                <div class="tagline"><asp:Label ID="lblTaglineSidebar" runat="server" Text="" /></div>
            </div>
            <uc:SidebarNavegacion ID="ucSidebarNavegacion" runat="server" PaginaActual="acceder_gestionar_suscripcion" />
            <div class="sidebar-footer">
                <a href="FormLogout.aspx" class="nav-item nav-logout"><span>🚪</span> <asp:Label ID="lblMenuCerrarSesionSidebar" runat="server" Text="" /></a>
            </div>
        </aside>

        <div class="main-wrap">

            <header class="top-header">
                <div class="header-title">
                    <span class="header-section">Cuenta</span>
                    <span class="header-sep">/</span>
                    <asp:Label ID="lblHeaderTitulo" runat="server" CssClass="header-page" Text="" />
                </div>
                <uc:HeaderUsuario ID="ucHeaderUsuario" runat="server" />
            </header>

            <div class="page-content">

                <asp:Label ID="lblMensaje" runat="server" Visible="false" CssClass="server-error" />

                <div class="suscripcion-layout">

                    <div class="suscripcion-main">

                        <asp:Panel ID="pnlSinSuscripcion" runat="server" CssClass="content-card" Visible="false">
                            <p class="card-subtitle"><asp:Label ID="lblMsgSinSuscripcion" runat="server" Text="" /></p>
                        </asp:Panel>

                        <asp:Panel ID="pnlPlanActivo" runat="server" CssClass="content-card plan-activo-card">
                            <div class="plan-activo-header">
                                <div>
                                    <p class="plan-eyebrow"><asp:Label ID="lblEyebrowPlanActivo" runat="server" Text="" /></p>
                                    <asp:Label ID="lblPlanNombre" runat="server"
                                        CssClass="plan-nombre" Text="" />
                                </div>
                                <asp:Label ID="lblPlanBadge" runat="server"
                                    CssClass="badge-estado activo" Text="" />
                            </div>

                            <div class="plan-detalles-grid">
                                <div class="plan-detalle-item">
                                    <span class="pd-label"><asp:Label ID="lblEtiquetaActivaDesde" runat="server" Text="" /></span>
                                    <asp:Label ID="lblFechaInicio" runat="server"
                                        CssClass="pd-valor" Text="" />
                                </div>
                                <div class="plan-detalle-item">
                                    <span class="pd-label"><asp:Label ID="lblEtiquetaProxVencimiento" runat="server" Text="" /></span>
                                    <asp:Label ID="lblProxVencimiento" runat="server"
                                        CssClass="pd-valor" Text="" />
                                </div>
                                <div class="plan-detalle-item">
                                    <span class="pd-label"><asp:Label ID="lblEtiquetaMedioPago" runat="server" Text="" /></span>
                                    <asp:Label ID="lblMedioPago" runat="server"
                                        CssClass="pd-valor" Text="" />
                                </div>
                                <div class="plan-detalle-item">
                                    <span class="pd-label"><asp:Label ID="lblEtiquetaPrecio" runat="server" Text="" /></span>
                                    <asp:Label ID="lblPrecio" runat="server"
                                        CssClass="pd-valor pd-precio" Text="" />
                                </div>
                            </div>

                            <div class="plan-acciones">
                                <asp:Button ID="btnActualizarPago" runat="server"
                                    Text=""
                                    CssClass="btn-secondary"
                                    OnClick="btnActualizarPago_Click"
                                    CausesValidation="false" />
                                <asp:Button ID="btnCancelar" runat="server"
                                    Text=""
                                    CssClass="btn-cancelar"
                                    ClientIDMode="Static"
                                    OnClick="btnCancelar_Click"
                                    CausesValidation="false"
                                    OnClientClick="return confirm(<%= JsonConfirmarCancelacion %>);" />
                                <asp:Button ID="btnReactivar" runat="server"
                                    Text=""
                                    CssClass="btn-success"
                                    Visible="false"
                                    OnClick="btnReactivar_Click"
                                    CausesValidation="false" />
                            </div>
                        </asp:Panel>


                        <asp:Panel ID="pnlPago" runat="server"
                            CssClass="content-card modal-pago-card mt-24" Visible="false">
                            <h3 class="modal-titulo"><asp:Label ID="lblModalPagoTitulo" runat="server" Text="" /></h3>
                            <p class="pago-aviso"><asp:Label ID="lblPagoAviso" runat="server" Text="" /></p>

                            <asp:HiddenField ID="hfDniPsicologo" runat="server" ClientIDMode="Static" />
                            <asp:HiddenField ID="hfAccionPago" runat="server" ClientIDMode="Static" />
                            <asp:HiddenField ID="hfPlanSeleccionadoPago" runat="server" ClientIDMode="Static" />
                            <asp:HiddenField ID="hfTokenTarjetaPago" runat="server" ClientIDMode="Static" />

                            <div class="grid-2" style="max-width:460px; margin-top:16px;">
                                <div class="field full">
                                    <asp:Label ID="lblEtiquetaTitular" runat="server" AssociatedControlID="txtNuevoTitular" Text="" />
                                    <sup>*</sup>
                                    <asp:TextBox ID="txtNuevoTitular" runat="server"
                                        MaxLength="100" placeholder="Nombre en la tarjeta"
                                        ClientIDMode="Static" autocomplete="off" />
                                    <asp:RequiredFieldValidator ID="rfvTitular" runat="server"
                                        ControlToValidate="txtNuevoTitular"
                                        ErrorMessage=""
                                        CssClass="field-error" Display="Dynamic"
                                        ValidationGroup="vgPago" />
                                </div>
                                <div class="field full">
                                    <asp:Label ID="lblEtiquetaNumeroTarjeta" runat="server" Text="" />
                                    <sup>*</sup>
                                    <div id="stripeCardElementPago" class="stripe-card-element"></div>
                                    <div id="stripeCardErrorsPago" class="field-error"></div>
                                </div>
                            </div>
                            <div class="form-actions">
                                <asp:Button ID="btnCancelarPago" runat="server"
                                    Text="" CssClass="btn-secondary"
                                    OnClick="btnCancelarPago_Click" CausesValidation="false" />
                                <asp:Button ID="btnConfirmarPago" runat="server"
                                    Text=""
                                    CssClass="btn-primary"
                                    ClientIDMode="Static"
                                    ValidationGroup="vgPago"
                                    OnClientClick="return confirmarPago();"
                                    OnClick="btnConfirmarPago_Click" />
                            </div>
                        </asp:Panel>


                        <div class="content-card mt-24">
                            <div class="card-header">
                                <h2 class="card-title"><asp:Label ID="lblCardTituloPlanes" runat="server" Text="" /></h2>
                                <p class="card-subtitle"><asp:Label ID="lblCardSubtituloPlanes" runat="server" Text="" /></p>
                            </div>

                            <div class="planes-comparativa">

                                <div class="comp-fila comp-header">
                                    <div class="comp-feature"></div>
                                    <div class="comp-plan">
                                        <div class="comp-plan-nombre"><asp:Label ID="lblPlanBasicoNombre" runat="server" Text="" /></div>
                                        <div class="comp-plan-precio">USD $4.99<span><asp:Label ID="lblPorMes1" runat="server" Text="" /></span></div>
                                    </div>
                                    <div class="comp-plan comp-plan-destacado">
                                        <div class="comp-plan-badge"><asp:Label ID="lblBadgeMasElegido" runat="server" Text="" /></div>
                                        <div class="comp-plan-nombre"><asp:Label ID="lblPlanProfesionalNombre" runat="server" Text="" /></div>
                                        <div class="comp-plan-precio">USD $14.99<span><asp:Label ID="lblPorMes2" runat="server" Text="" /></span></div>
                                    </div>
                                    <div class="comp-plan">
                                        <div class="comp-plan-nombre"><asp:Label ID="lblPlanPremiumNombre" runat="server" Text="" /></div>
                                        <div class="comp-plan-precio">USD $21.99<span><asp:Label ID="lblPorMes3" runat="server" Text="" /></span></div>
                                    </div>
                                </div>

                                <div class="comp-fila">
                                    <div class="comp-feature"><asp:Label ID="lblFeaturePacientes" runat="server" Text="" /></div>
                                    <div class="comp-valor"><asp:Label ID="lblFeaturePacientesBasico" runat="server" Text="" /></div>
                                    <div class="comp-valor comp-destacado"><asp:Label ID="lblFeaturePacientesProfesional" runat="server" Text="" /></div>
                                    <div class="comp-valor"><asp:Label ID="lblFeaturePacientesPremium" runat="server" Text="" /></div>
                                </div>
                                <div class="comp-fila">
                                    <div class="comp-feature"><asp:Label ID="lblFeatureConsultasHistorial" runat="server" Text="" /></div>
                                    <div class="comp-valor">✓</div>
                                    <div class="comp-valor comp-destacado">✓</div>
                                    <div class="comp-valor">✓</div>
                                </div>
                                <div class="comp-fila">
                                    <div class="comp-feature"><asp:Label ID="lblFeatureResumenIA" runat="server" Text="" /></div>
                                    <div class="comp-valor comp-no">✗</div>
                                    <div class="comp-valor comp-destacado">✓</div>
                                    <div class="comp-valor">✓</div>
                                </div>
                                <div class="comp-fila">
                                    <div class="comp-feature"><asp:Label ID="lblFeatureInformeDerivacion" runat="server" Text="" /></div>
                                    <div class="comp-valor comp-no">✗</div>
                                    <div class="comp-valor comp-destacado">✓</div>
                                    <div class="comp-valor">✓</div>
                                </div>
                                <div class="comp-fila">
                                    <div class="comp-feature"><asp:Label ID="lblFeaturePerfilacion" runat="server" Text="" /></div>
                                    <div class="comp-valor comp-no">✗</div>
                                    <div class="comp-valor comp-destacado">✓</div>
                                    <div class="comp-valor">✓</div>
                                </div>
                                <div class="comp-fila">
                                    <div class="comp-feature"><asp:Label ID="lblFeatureExportaciones" runat="server" Text="" /></div>
                                    <div class="comp-valor comp-no">✗</div>
                                    <div class="comp-valor comp-destacado"><asp:Label ID="lblFeatureExportacionesBasico" runat="server" Text="" /></div>
                                    <div class="comp-valor"><asp:Label ID="lblFeatureExportacionesAvanzado" runat="server" Text="" /></div>
                                </div>
                                <div class="comp-fila">
                                    <div class="comp-feature"><asp:Label ID="lblFeatureSoporte" runat="server" Text="" /></div>
                                    <div class="comp-valor"><asp:Label ID="lblFeatureSoporteBasico" runat="server" Text="" /></div>
                                    <div class="comp-valor comp-destacado"><asp:Label ID="lblFeatureSoporteProfesional" runat="server" Text="" /></div>
                                    <div class="comp-valor"><asp:Label ID="lblFeatureSoportePremium" runat="server" Text="" /></div>
                                </div>

                                <div class="comp-fila comp-footer">
                                    <div class="comp-feature"></div>
                                    <div class="comp-action">
                                        <asp:Button ID="btnSelBasico" runat="server"
                                            Text=""
                                            CssClass="btn-plan-sel"
                                            CommandArgument="1"
                                            OnClick="btnCambiarPlan_Click"
                                            CausesValidation="false" />
                                    </div>
                                    <div class="comp-action comp-destacado">
                                        <asp:Button ID="btnSelProfesional" runat="server"
                                            Text=""
                                            CssClass="btn-plan-sel"
                                            CommandArgument="2"
                                            OnClick="btnCambiarPlan_Click"
                                            CausesValidation="false" />
                                    </div>
                                    <div class="comp-action">
                                        <asp:Button ID="btnSelPremium" runat="server"
                                            Text=""
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
                            <p class="accesos-titulo"><asp:Label ID="lblTituloUso" runat="server" Text="" /></p>
                            <div class="uso-item">
                                <span class="uso-label"><asp:Label ID="lblEtiquetaUsoConsultas" runat="server" Text="" /></span>
                                <span class="uso-valor">
                                    <asp:Label ID="lblUsoConsultas" runat="server" Text="0" />
                                </span>
                            </div>
                            <div class="uso-item">
                                <span class="uso-label"><asp:Label ID="lblEtiquetaUsoResumenes" runat="server" Text="" /></span>
                                <span class="uso-valor">
                                    <asp:Label ID="lblUsoResumenes" runat="server" Text="0" />
                                </span>
                            </div>
                            <div class="uso-item">
                                <span class="uso-label"><asp:Label ID="lblEtiquetaUsoDerivaciones" runat="server" Text="" /></span>
                                <span class="uso-valor">
                                    <asp:Label ID="lblUsoDerivaciones" runat="server" Text="0" />
                                </span>
                            </div>
                            <div class="uso-item" style="border-bottom:none;">
                                <span class="uso-label"><asp:Label ID="lblEtiquetaUsoPerfiles" runat="server" Text="" /></span>
                                <span class="uso-valor">
                                    <asp:Label ID="lblUsoPerfiles" runat="server" Text="0" />
                                </span>
                            </div>
                        </div>

                        <div class="content-card aviso-card">
                            <div class="aviso-icon">🔒</div>
                            <p class="aviso-titulo"><asp:Label ID="lblTituloPagoSeguro" runat="server" Text="" /></p>
                            <p class="aviso-texto"><asp:Label ID="lblTextoPagoSeguro" runat="server" Text="" /></p>
                        </div>

                    </div>

                </div>
            </div>
        </div>

        <asp:Panel ID="pnlModalCancelacion" runat="server"
            CssClass="modal-overlay" Visible="false">
            <div class="modal-card">
                <div class="modal-icono">⚠️</div>
                <h3 class="modal-card-titulo"><asp:Label ID="lblTituloSuscripcionCancelada" runat="server" Text="" /></h3>
                <asp:Label ID="lblMensajeCancelacion" runat="server"
                    CssClass="modal-card-texto" Text="" />
                <asp:Button ID="btnCerrarModal" runat="server"
                    Text=""
                    CssClass="btn-primary"
                    OnClick="btnCerrarModal_Click"
                    CausesValidation="false" />
            </div>
        </asp:Panel>

    </form>

    <script type="text/javascript">
        var stripe = Stripe('<%= ObtenerPublicKeyStripe() %>');
        var stripeElementsPago = stripe.elements();
        var stripeCardElementPago = stripeElementsPago.create('card', {
            style: {
                base: { fontSize: '15px', color: '#1a1a1a', fontFamily: 'inherit', '::placeholder': { color: '#9a9a9a' } },
                invalid: { color: '#E8455A' }
            },
            hidePostalCode: true
        });
        stripeCardElementPago.mount('#stripeCardElementPago');
        stripeCardElementPago.on('change', function (evento) {
            var lblErrorTarjeta = document.getElementById('stripeCardErrorsPago');
            lblErrorTarjeta.textContent = evento.error ? evento.error.message : '';
        });

        var MSG_PAGO = <%= MensajesPagoJson %>;

        function confirmarPago() {
            if (typeof Page_ClientValidate === 'function') {
                if (!Page_ClientValidate('vgPago')) return false;
            }

            var btn = document.getElementById('btnConfirmarPago');
            var textoOriginal = btn.value;
            btn.disabled = true;
            btn.value = MSG_PAGO.procesando;

            function mostrarErrorTarjeta(mensaje) {
                btn.disabled = false;
                btn.value = textoOriginal;
                var lbl = document.getElementById('<%= lblMensaje.ClientID %>');
                if (lbl) {
                    lbl.textContent = mensaje;
                    lbl.className = 'server-error';
                    lbl.style.display = 'block';
                }
            }

            stripe.createPaymentMethod({
                type: 'card',
                card: stripeCardElementPago,
                billing_details: {
                    name: document.getElementById('txtNuevoTitular').value
                }
            }).then(function (resultado) {
                if (resultado.error) {
                    console.log('Error al tokenizar la tarjeta con Stripe:', resultado.error);
                    mostrarErrorTarjeta(resultado.error.message || MSG_PAGO.tarjetaInvalida);
                    return;
                }
                document.getElementById('hfTokenTarjetaPago').value = resultado.paymentMethod.id;
                __doPostBack('btnConfirmarPago', '');
            }).catch(function (error) {
                console.log('Error al procesar la tarjeta con Stripe:', error);
                mostrarErrorTarjeta(MSG_PAGO.tarjetaNoIdentificada);
            });

            return false;
        }
    </script>
</body>
</html>