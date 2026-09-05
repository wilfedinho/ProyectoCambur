using BE;
using BLL;
using SERVICIOS;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Script.Serialization;
using System.Web.UI.WebControls;
using GUI;

public partial class FormSuscripcion : PaginaBase
{
    private const string ACCION_CAMBIAR_PLAN = "CAMBIAR_PLAN";
    private const string ACCION_ACTUALIZAR_MEDIO_PAGO = "ACTUALIZAR_MEDIO_PAGO";

    public string JsonConfirmarCancelacion
    {
        get
        {
            JavaScriptSerializer serializador = new JavaScriptSerializer();
            return serializador.Serialize(Traducir("confirm_cancelar_suscripcion"));
        }
    }

    public string MensajesPagoJson
    {
        get
        {
            Dictionary<string, string> mensajes = new Dictionary<string, string>
            {
                { "tarjetaNoReconocida", Traducir("js_tarjeta_no_reconocida") },
                { "tarjetaInvalida", Traducir("js_tarjeta_invalida") },
                { "tarjetaNoIdentificada", Traducir("js_tarjeta_no_identificada") },
                { "procesando", Traducir("js_procesando_pago") }
            };

            JavaScriptSerializer serializador = new JavaScriptSerializer();
            return serializador.Serialize(mensajes);
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!GestorSesion.EstaAutenticado)
        {
            Response.Redirect("FormLogin.aspx");
            return;
        }

        Psicologo psicologoActual = GestorSesion.PsicologoActual;

        GestorPermiso gestorPermiso = new GestorPermiso();
        if (!gestorPermiso.TienePermiso(psicologoActual.RolPermiso, "acceder_gestionar_suscripcion"))
        {
            DenegarAcceso();
            return;
        }

        AplicarTraducciones();

        if (!IsPostBack)
        {
            CargarProfesional();
            CargarSuscripcion();
            CargarUso();
        }
    }

    private void AplicarTraducciones()
    {
        lblTaglineSidebar.Text = Traducir("tagline_panel_gestion");
        lblMenuCerrarSesionSidebar.Text = Traducir("menu_cerrar_sesion");
        lblHeaderTitulo.Text = Traducir("nav_gestionar_suscripcion");

        lblMsgSinSuscripcion.Text = Traducir("msg_sin_suscripcion_activa");
        lblEyebrowPlanActivo.Text = Traducir("eyebrow_plan_activo");

        lblEtiquetaActivaDesde.Text = Traducir("lbl_activa_desde");
        lblEtiquetaProxVencimiento.Text = Traducir("lbl_proximo_vencimiento");
        lblEtiquetaMedioPago.Text = Traducir("lbl_medio_pago");
        lblEtiquetaPrecio.Text = Traducir("lbl_precio_mensual");

        btnActualizarPago.Text = Traducir("btn_actualizar_medio_pago");
        btnCancelar.Text = Traducir("btn_cancelar_suscripcion");
        btnReactivar.Text = Traducir("btn_reactivar_suscripcion");

        lblPagoAviso.Text = Traducir("pago_aviso_seguro");
        lblEtiquetaNumeroTarjeta.Text = Traducir("lbl_numero_tarjeta");
        rfvTarjeta.ErrorMessage = Traducir("error_tarjeta_obligatoria");
        lblEtiquetaTitular.Text = Traducir("lbl_titular_tarjeta");
        rfvTitular.ErrorMessage = Traducir("error_campo_obligatorio");
        lblEtiquetaVencimiento.Text = Traducir("lbl_vencimiento_tarjeta");
        rfvVence.ErrorMessage = Traducir("error_campo_obligatorio");
        lblEtiquetaCVV.Text = Traducir("lbl_cvv");
        rfvCVV.ErrorMessage = Traducir("error_campo_obligatorio");
        btnCancelarPago.Text = Traducir("btn_cancelar");
        btnConfirmarPago.Text = Traducir("btn_confirmar_pago");

        lblCardTituloPlanes.Text = Traducir("titulo_cambiar_plan");
        lblCardSubtituloPlanes.Text = Traducir("subtitulo_cambiar_plan");

        lblPlanBasicoNombre.Text = Traducir("plan_basico_nombre");
        lblPlanProfesionalNombre.Text = Traducir("plan_profesional_nombre");
        lblPlanPremiumNombre.Text = Traducir("plan_premium_nombre");
        lblPorMes1.Text = lblPorMes2.Text = lblPorMes3.Text = Traducir("lbl_por_mes");
        lblBadgeMasElegido.Text = Traducir("badge_mas_elegido");

        lblFeaturePacientes.Text = Traducir("feature_pacientes");
        lblFeaturePacientesBasico.Text = Traducir("feature_pacientes_hasta20");
        lblFeaturePacientesProfesional.Text = Traducir("feature_pacientes_ilimitados");
        lblFeaturePacientesPremium.Text = Traducir("feature_pacientes_ilimitados");
        lblFeatureConsultasHistorial.Text = Traducir("feature_consultas_historial");
        lblFeatureResumenIA.Text = Traducir("feature_resumen_ia");
        lblFeatureInformeDerivacion.Text = Traducir("feature_informe_derivacion_ia");
        lblFeaturePerfilacion.Text = Traducir("feature_perfilacion");
        lblFeatureExportaciones.Text = Traducir("feature_exportaciones_pdf");
        lblFeatureExportacionesBasico.Text = Traducir("plan_basico_nombre");
        lblFeatureExportacionesAvanzado.Text = Traducir("feature_exportaciones_avanzado");
        lblFeatureSoporte.Text = Traducir("feature_soporte");
        lblFeatureSoporteBasico.Text = Traducir("feature_soporte_email");
        lblFeatureSoporteProfesional.Text = Traducir("feature_soporte_email_chat");
        lblFeatureSoportePremium.Text = Traducir("feature_soporte_prioritario");

        lblTituloUso.Text = Traducir("titulo_uso_periodo");
        lblEtiquetaUsoConsultas.Text = Traducir("lbl_uso_consultas");
        lblEtiquetaUsoResumenes.Text = Traducir("lbl_uso_resumenes");
        lblEtiquetaUsoDerivaciones.Text = Traducir("lbl_uso_derivaciones");
        lblEtiquetaUsoPerfiles.Text = Traducir("lbl_uso_perfiles");

        lblTituloPagoSeguro.Text = Traducir("titulo_pago_seguro");
        lblTextoPagoSeguro.Text = Traducir("texto_pago_seguro");

        lblTituloSuscripcionCancelada.Text = Traducir("titulo_suscripcion_cancelada");
        btnCerrarModal.Text = Traducir("btn_entendido");
    }

    protected string ObtenerPublicKeyMercadoPago()
    {
        string publicKey = ConfigurationManager.AppSettings["MercadoPagoPublicKey"];
        return publicKey ?? string.Empty;
    }

    private void CargarProfesional()
    {
        Psicologo psicologoActual = GestorSesion.PsicologoActual;
        hfDniPsicologo.Value = psicologoActual.Dni;
    }

    private void CargarSuscripcion()
    {
        Psicologo psicologoActual = GestorSesion.PsicologoActual;
        GestorSuscripcion gestorSuscripcion = new GestorSuscripcion();
        Suscripcion activa = gestorSuscripcion.ObtenerActivaDe(psicologoActual.IdPsicologo);

        pnlSinSuscripcion.Visible = activa == null;
        pnlPlanActivo.Visible = activa != null;

        if (activa == null)
        {
            btnReactivar.Visible = false;
            return;
        }

        InfoPlan plan = gestorSuscripcion.ObtenerPlanDe(activa) ?? CatalogoPlanes.Planes[0];

        lblPlanNombre.Text = TraducirNombrePlan(plan);
        lblPrecio.Text = "$" + plan.Precio.ToString("#,##0").Replace(",", ".") + " " + Traducir("lbl_por_mes");
        lblFechaInicio.Text = activa.FechaInicio.ToString("dd/MM/yyyy");
        lblProxVencimiento.Text = activa.FechaFin.HasValue ? activa.FechaFin.Value.ToString("dd/MM/yyyy") : "-";
        lblMedioPago.Text = string.IsNullOrWhiteSpace(activa.UltimosCuatroTarjeta)
            ? "-"
            : "**** **** **** " + activa.UltimosCuatroTarjeta;

        switch (activa.Estado)
        {
            case EstadoSuscripcion.Activa:
                lblPlanBadge.Text = "✓ " + Traducir("badge_suscripcion_activa");
                lblPlanBadge.CssClass = "badge-estado activo";
                btnCancelar.Visible = true;
                btnReactivar.Visible = false;
                break;
            case EstadoSuscripcion.Cancelada:
                lblPlanBadge.Text = Traducir("badge_suscripcion_cancelada");
                lblPlanBadge.CssClass = "badge-estado inactivo";
                btnCancelar.Visible = false;
                btnReactivar.Visible = true;
                break;
            default:
                lblPlanBadge.Text = Traducir("badge_suscripcion_vencida");
                lblPlanBadge.CssClass = "badge-estado inactivo";
                btnCancelar.Visible = false;
                btnReactivar.Visible = false;
                break;
        }

        MarcarPlanActual(plan.IdPlan);
    }

    private string TraducirNombrePlan(InfoPlan plan)
    {
        switch (plan.IdPlan)
        {
            case 1: return Traducir("plan_basico_nombre");
            case 3: return Traducir("plan_premium_nombre");
            default: return Traducir("plan_profesional_nombre");
        }
    }

    private void CargarUso()
    {
        Psicologo psicologoActual = GestorSesion.PsicologoActual;
        GestorSuscripcion gestorSuscripcion = new GestorSuscripcion();
        Suscripcion activa = gestorSuscripcion.ObtenerActivaDe(psicologoActual.IdPsicologo);

        DateTime desde = activa != null ? activa.FechaInicio : DateTime.Now.AddMonths(-1);
        UsoPeriodo uso = gestorSuscripcion.ObtenerUso(psicologoActual.IdPsicologo, desde);

        lblUsoConsultas.Text = uso.Consultas.ToString();
        lblUsoResumenes.Text = uso.ResumenesIA.ToString();
        lblUsoDerivaciones.Text = uso.Derivaciones.ToString();
        lblUsoPerfiles.Text = uso.Perfiles.ToString();
    }

    private void MarcarPlanActual(int idPlan)
    {
        string textoSeleccionar = Traducir("btn_plan_seleccionar");
        string textoPlanActual = Traducir("btn_plan_actual");

        btnSelBasico.Text = textoSeleccionar;
        btnSelBasico.CssClass = "btn-plan-sel";
        btnSelBasico.Enabled = true;

        btnSelProfesional.Text = textoSeleccionar;
        btnSelProfesional.CssClass = "btn-plan-sel";
        btnSelProfesional.Enabled = true;

        btnSelPremium.Text = textoSeleccionar;
        btnSelPremium.CssClass = "btn-plan-sel";
        btnSelPremium.Enabled = true;

        switch (idPlan)
        {
            case 1:
                btnSelBasico.Text = textoPlanActual;
                btnSelBasico.CssClass = "btn-plan-actual";
                btnSelBasico.Enabled = false;
                break;
            case 2:
                btnSelProfesional.Text = textoPlanActual;
                btnSelProfesional.CssClass = "btn-plan-actual";
                btnSelProfesional.Enabled = false;
                break;
            case 3:
                btnSelPremium.Text = textoPlanActual;
                btnSelPremium.CssClass = "btn-plan-actual";
                btnSelPremium.Enabled = false;
                break;
        }
    }

    protected void btnActualizarPago_Click(object sender, EventArgs e)
    {
        lblMensaje.Visible = false;
        pnlModalCancelacion.Visible = false;

        Psicologo psicologoActual = GestorSesion.PsicologoActual;
        GestorSuscripcion gestorSuscripcion = new GestorSuscripcion();
        Suscripcion activa = gestorSuscripcion.ObtenerActivaDe(psicologoActual.IdPsicologo);
        InfoPlan planActual = gestorSuscripcion.ObtenerPlanDe(activa) ?? CatalogoPlanes.Planes[1];

        AbrirModalPago(ACCION_ACTUALIZAR_MEDIO_PAGO, planActual.IdPlan, Traducir("titulo_actualizar_medio_pago"));
    }

    protected void btnCambiarPlan_Click(object sender, EventArgs e)
    {
        lblMensaje.Visible = false;
        pnlModalCancelacion.Visible = false;

        Button btn = (Button)sender;
        int idPlanDestino = Convert.ToInt32(btn.CommandArgument);
        InfoPlan plan = CatalogoPlanes.ObtenerPorId(idPlanDestino);
        string nombrePlan = plan != null ? TraducirNombrePlan(plan) : "";

        AbrirModalPago(ACCION_CAMBIAR_PLAN, idPlanDestino, string.Format(Traducir("titulo_cambiar_a_plan"), nombrePlan));
    }

    private void AbrirModalPago(string accion, int idPlan, string titulo)
    {
        hfAccionPago.Value = accion;
        hfPlanSeleccionadoPago.Value = idPlan.ToString();
        hfTokenTarjetaPago.Value = "";
        hfPaymentMethodIdPago.Value = "";
        lblModalPagoTitulo.Text = titulo;
        LimpiarFormPago();
        pnlPago.Visible = true;
    }

    protected void btnCancelarPago_Click(object sender, EventArgs e)
    {
        pnlPago.Visible = false;
        LimpiarFormPago();
    }

    protected void btnConfirmarPago_Click(object sender, EventArgs e)
    {
        lblMensaje.Visible = false;

        if (!Page.IsValid) return;

        string tokenTarjeta = hfTokenTarjetaPago.Value;
        string paymentMethodId = hfPaymentMethodIdPago.Value;

        if (string.IsNullOrWhiteSpace(tokenTarjeta) || string.IsNullOrWhiteSpace(paymentMethodId))
        {
            MostrarError(Traducir("error_tarjeta_no_validada"));
            return;
        }

        int idPlan;
        int.TryParse(hfPlanSeleccionadoPago.Value, out idPlan);

        Psicologo psicologoActual = GestorSesion.PsicologoActual;
        GestorSuscripcion gestorSuscripcion = new GestorSuscripcion();

        try
        {
            Psicologo actualizado;
            string mensajeExito;

            if (hfAccionPago.Value == ACCION_ACTUALIZAR_MEDIO_PAGO)
            {
                actualizado = gestorSuscripcion.ActualizarMedioPago(psicologoActual.IdPsicologo, tokenTarjeta, paymentMethodId);
                mensajeExito = Traducir("exito_medio_pago_actualizado");
            }
            else
            {
                actualizado = gestorSuscripcion.CambiarPlan(psicologoActual.IdPsicologo, idPlan, tokenTarjeta, paymentMethodId);
                mensajeExito = Traducir("exito_plan_actualizado");
            }

            psicologoActual.RolPermiso = actualizado.RolPermiso;
            GestorSesion.PsicologoActual = psicologoActual;

            pnlPago.Visible = false;
            LimpiarFormPago();
            CargarSuscripcion();
            CargarUso();
            MostrarExito(mensajeExito);
        }
        catch (ExcepcionTraducible ex)
        {
            MostrarError(TraducirExcepcion(ex));
        }
        catch (Exception)
        {
            MostrarError(Traducir("error_pago_no_procesado"));
        }
    }

    protected void btnCancelar_Click(object sender, EventArgs e)
    {
        lblMensaje.Visible = false;

        Psicologo psicologoActual = GestorSesion.PsicologoActual;
        GestorSuscripcion gestorSuscripcion = new GestorSuscripcion();

        try
        {
            gestorSuscripcion.Cancelar(psicologoActual.IdPsicologo);
            CargarSuscripcion();

            lblMensajeCancelacion.Text = string.Format(Traducir("msg_suscripcion_cancelada"), lblProxVencimiento.Text);
            pnlModalCancelacion.Visible = true;
        }
        catch (ExcepcionTraducible ex)
        {
            MostrarError(TraducirExcepcion(ex));
        }
    }

    protected void btnReactivar_Click(object sender, EventArgs e)
    {
        lblMensaje.Visible = false;

        Psicologo psicologoActual = GestorSesion.PsicologoActual;
        GestorSuscripcion gestorSuscripcion = new GestorSuscripcion();

        try
        {
            gestorSuscripcion.Reactivar(psicologoActual.IdPsicologo);
            CargarSuscripcion();
            MostrarExito(Traducir("exito_suscripcion_reactivada"));
        }
        catch (ExcepcionTraducible ex)
        {
            MostrarError(TraducirExcepcion(ex));
        }
    }

    protected void btnCerrarModal_Click(object sender, EventArgs e)
    {
        pnlModalCancelacion.Visible = false;
    }

    private void LimpiarFormPago()
    {
        txtNuevaTarjeta.Text = string.Empty;
        txtNuevoTitular.Text = string.Empty;
        txtNuevoVence.Text = string.Empty;
        txtNuevoCVV.Text = string.Empty;
    }

    private void MostrarError(string mensaje)
    {
        lblMensaje.Text = mensaje;
        lblMensaje.CssClass = "server-error";
        lblMensaje.Visible = true;
    }

    private void MostrarExito(string mensaje)
    {
        lblMensaje.Text = mensaje;
        lblMensaje.CssClass = "server-success";
        lblMensaje.Visible = true;
    }
}