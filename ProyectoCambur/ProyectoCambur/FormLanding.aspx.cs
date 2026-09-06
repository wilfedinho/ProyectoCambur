using BE;
using BLL;
using SERVICIOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUI;

public partial class FormLanding : PaginaBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        AplicarTraducciones();

        if (!IsPostBack)
        {
            CargarSesion();
            CargarTestimonios();
        }
    }

    #region Traducciones

    private void AplicarTraducciones()
    {
        lblNavInicio.Text = Traducir("landing_nav_inicio");
        lblNavNosotros.Text = Traducir("landing_nav_nosotros");
        lblNavServicios.Text = Traducir("landing_nav_servicios");
        lblNavTestimonios.Text = Traducir("landing_nav_testimonios");
        lblNavFaq.Text = Traducir("landing_nav_faq");
        lblNavRse.Text = Traducir("landing_nav_rse");
        lblNavContacto.Text = Traducir("landing_nav_contacto");

        lblBtnIniciarSesion.Text = Traducir("landing_btn_iniciar_sesion");
        lblBtnCrearCuenta.Text = Traducir("landing_btn_crear_cuenta");
        lblBtnCerrarSesion.Text = Traducir("menu_cerrar_sesion");
        lblBtnIrPanel.Text = Traducir("landing_btn_ir_panel");

        lblHeroEyebrow.Text = Traducir("landing_hero_eyebrow");
        lblHeroTitulo.Text = Traducir("landing_hero_titulo");
        lblHeroTexto.Text = Traducir("landing_hero_texto");
        lblHeroCtaServicios.Text = Traducir("landing_hero_cta_servicios");
        lblHeroCtaCambur.Text = Traducir("landing_hero_cta_cambur");

        lblNosotrosTitulo.Text = Traducir("landing_nosotros_titulo");
        lblNosotrosTexto.Text = Traducir("landing_nosotros_texto");
        lblNosotrosDato1Num.Text = Traducir("landing_nosotros_dato_1_num");
        lblNosotrosDato1Texto.Text = Traducir("landing_nosotros_dato_1_texto");
        lblNosotrosDato2Num.Text = Traducir("landing_nosotros_dato_2_num");
        lblNosotrosDato2Texto.Text = Traducir("landing_nosotros_dato_2_texto");
        lblNosotrosDato3Num.Text = Traducir("landing_nosotros_dato_3_num");
        lblNosotrosDato3Texto.Text = Traducir("landing_nosotros_dato_3_texto");

        lblServiciosTitulo.Text = Traducir("landing_servicios_titulo");
        lblServiciosSubtitulo.Text = Traducir("landing_servicios_subtitulo");
        lblServicio1Titulo.Text = Traducir("landing_servicio_1_titulo");
        lblServicio1Texto.Text = Traducir("landing_servicio_1_texto");
        lblServicio2Titulo.Text = Traducir("landing_servicio_2_titulo");
        lblServicio2Texto.Text = Traducir("landing_servicio_2_texto");
        lblServicio3Badge.Text = Traducir("landing_servicio_3_badge");
        lblServicio3Titulo.Text = Traducir("landing_servicio_3_titulo");
        lblServicio3Texto.Text = Traducir("landing_servicio_3_texto");

        lblTestimoniosTitulo.Text = Traducir("landing_testimonios_titulo");
        lblSinTestimonios.Text = Traducir("landing_testimonios_sin_datos");

        lblValorarTitulo.Text = Traducir("landing_valorar_titulo");
        lblValorarTexto.Text = Traducir("landing_valorar_texto");
        lblValorarSinSuscripcion.Text = Traducir("landing_valorar_sin_suscripcion");
        lblValorarPlanActualPrefijo.Text = Traducir("landing_valorar_plan_actual_prefijo");
        lblValorarYaExistenteAviso.Text = Traducir("landing_valorar_ya_existente_aviso");
        txtComentario.Attributes["placeholder"] = Traducir("landing_valorar_comentario_placeholder");
        btnEnviarValoracion.Text = Traducir("landing_btn_enviar_valoracion");

        lblFaqTitulo.Text = Traducir("landing_faq_titulo");
        lblFaqPregunta1.Text = Traducir("landing_faq_pregunta_1");
        lblFaqRespuesta1.Text = Traducir("landing_faq_respuesta_1");
        lblFaqPregunta2.Text = Traducir("landing_faq_pregunta_2");
        lblFaqRespuesta2.Text = Traducir("landing_faq_respuesta_2");
        lblFaqPregunta3.Text = Traducir("landing_faq_pregunta_3");
        lblFaqRespuesta3.Text = Traducir("landing_faq_respuesta_3");
        lblFaqPregunta4.Text = Traducir("landing_faq_pregunta_4");
        lblFaqRespuesta4.Text = Traducir("landing_faq_respuesta_4");
        lblFaqPregunta5.Text = Traducir("landing_faq_pregunta_5");
        lblFaqRespuesta5.Text = Traducir("landing_faq_respuesta_5");
        lblFaqPregunta6.Text = Traducir("landing_faq_pregunta_6");
        lblFaqRespuesta6.Text = Traducir("landing_faq_respuesta_6");

        lblRseTitulo.Text = Traducir("landing_rse_titulo");
        lblRseTexto.Text = Traducir("landing_rse_texto");
        lblRsePilar1Titulo.Text = Traducir("landing_rse_pilar_1_titulo");
        lblRsePilar1Texto.Text = Traducir("landing_rse_pilar_1_texto");
        lblRsePilar2Titulo.Text = Traducir("landing_rse_pilar_2_titulo");
        lblRsePilar2Texto.Text = Traducir("landing_rse_pilar_2_texto");
        lblRsePilar3Titulo.Text = Traducir("landing_rse_pilar_3_titulo");
        lblRsePilar3Texto.Text = Traducir("landing_rse_pilar_3_texto");

        lblContactoTitulo.Text = Traducir("landing_contacto_titulo");
        lblContactoTexto.Text = Traducir("landing_contacto_texto");
        lblContactoLblNombre.Text = Traducir("landing_contacto_lbl_nombre");
        lblContactoLblEmail.Text = Traducir("landing_contacto_lbl_email");
        lblContactoLblAsunto.Text = Traducir("landing_contacto_lbl_asunto");
        lblContactoLblMensaje.Text = Traducir("landing_contacto_lbl_mensaje");
        btnEnviarContacto.Text = Traducir("landing_btn_enviar_contacto");
        lblContactoExito.Text = Traducir("landing_contacto_enviado");

        lblFooterTexto.Text = Traducir("landing_footer_texto");
    }

    #endregion

    #region Sesión / panel de valoración

    private void CargarSesion()
    {
        bool autenticado = GestorSesion.EstaAutenticado;

        pnlAccionesAnonimo.Visible = !autenticado;
        pnlAccionesAutenticado.Visible = autenticado;
        pnlValorarWrap.Visible = autenticado;

        if (!autenticado)
        {
            return;
        }

        Psicologo psicologoActual = GestorSesion.PsicologoActual;
        GestorSuscripcion gestorSuscripcion = new GestorSuscripcion();
        Suscripcion activa = gestorSuscripcion.ObtenerActivaDe(psicologoActual.IdPsicologo);

        if (activa == null)
        {
            pnlValorarSinSuscripcion.Visible = true;
            pnlValorarFormulario.Visible = false;
            return;
        }

        pnlValorarSinSuscripcion.Visible = false;
        pnlValorarFormulario.Visible = true;

        InfoPlan plan = gestorSuscripcion.ObtenerPlanDe(activa);
        lblValorarPlanActualNombre.Text = plan != null ? plan.NombreComercial : activa.Plan.ToString();

        GestorValoracion gestorValoracion = new GestorValoracion();
        ValoracionServicio existente = gestorValoracion.ObtenerValoracionDe(psicologoActual.IdPsicologo);

        if (existente != null)
        {
            lblValorarYaExistenteAviso.Visible = true;
            hfPuntuacion.Value = existente.Puntuacion.ToString();
            txtComentario.Text = existente.Comentario;
        }
        else
        {
            lblValorarYaExistenteAviso.Visible = false;
            hfPuntuacion.Value = "0";
            txtComentario.Text = string.Empty;
        }
    }

    #endregion

    #region Testimonios

    private void CargarTestimonios()
    {
        GestorValoracion gestorValoracion = new GestorValoracion();

        BE.ResumenValoraciones resumen = gestorValoracion.ObtenerResumen();
        lblResumenEstrellas.Text = ConstruirEstrellasHtml((int)Math.Round(resumen.Promedio, MidpointRounding.AwayFromZero));
        lblResumenPromedio.Text = resumen.Cantidad > 0
            ? resumen.Promedio.ToString("0.0") + " " + Traducir("landing_testimonios_promedio_sufijo")
            : string.Empty;
        lblResumenCantidad.Text = resumen.Cantidad > 0
            ? "· " + resumen.Cantidad + " " + Traducir("landing_testimonios_cantidad_sufijo")
            : string.Empty;

        List<ValoracionServicio> testimonios = gestorValoracion.ObtenerTestimonios(12);

        List<TestimonioVM> vistaModelo = testimonios.Select(v => new TestimonioVM
        {
            Comentario = v.Comentario,
            NombreProfesional = v.NombreProfesional,
            ApellidoProfesional = v.ApellidoProfesional,
            EstrellasHtml = ConstruirEstrellasHtml(v.Puntuacion),
            PlanTexto = ObtenerNombreComercialPlan(v.Plan)
        }).ToList();

        rptTestimonios.DataSource = vistaModelo;
        rptTestimonios.DataBind();

        lblSinTestimonios.Visible = vistaModelo.Count == 0;
    }

    private string ObtenerNombreComercialPlan(PlanSuscripcion plan)
    {
        InfoPlan info = CatalogoPlanes.Planes.FirstOrDefault(p => p.Plan == plan);
        return info != null ? info.NombreComercial : plan.ToString();
    }

    private string ConstruirEstrellasHtml(int puntuacion)
    {
        StringBuilder sb = new StringBuilder();
        for (int i = 1; i <= 5; i++)
        {
            sb.Append(i <= puntuacion
                ? "<span class=\"estrella-llena\">★</span>"
                : "<span class=\"estrella-vacia\">★</span>");
        }
        return sb.ToString();
    }

    private class TestimonioVM
    {
        public string Comentario { get; set; }
        public string NombreProfesional { get; set; }
        public string ApellidoProfesional { get; set; }
        public string EstrellasHtml { get; set; }
        public string PlanTexto { get; set; }
    }

    #endregion

    #region Envío de valoración

    protected void btnEnviarValoracion_Click(object sender, EventArgs e)
    {
        lblMensajeValoracion.Visible = false;

        if (!GestorSesion.EstaAutenticado)
        {
            Response.Redirect("FormLogin.aspx");
            return;
        }

        int puntuacion;
        int.TryParse(hfPuntuacion.Value, out puntuacion);

        GestorValoracion gestorValoracion = new GestorValoracion();

        try
        {
            gestorValoracion.EnviarValoracion(GestorSesion.PsicologoActual.IdPsicologo, puntuacion, txtComentario.Text);

            CargarTestimonios();
            CargarSesion();

            MostrarMensajeValoracion(Traducir("landing_valoracion_enviada"), esError: false);
        }
        catch (ExcepcionTraducible ex)
        {
            MostrarMensajeValoracion(TraducirExcepcion(ex), esError: true);
        }
        catch (Exception)
        {
            MostrarMensajeValoracion(Traducir("error_valoracion_puntuacion_invalida"), esError: true);
        }
    }

    private void MostrarMensajeValoracion(string mensaje, bool esError)
    {
        lblMensajeValoracion.Text = mensaje;
        lblMensajeValoracion.CssClass = esError ? "server-error" : "server-success";
        lblMensajeValoracion.Visible = true;
    }

    #endregion

    #region Formulario de contacto

    protected void btnEnviarContacto_Click(object sender, EventArgs e)
    {
        lblMensajeContacto.Visible = false;

        GestorContacto gestorContacto = new GestorContacto();

        try
        {
            gestorContacto.EnviarMensaje(txtContactoNombre.Text, txtContactoEmail.Text, txtContactoAsunto.Text, txtContactoMensaje.Text);

            pnlContactoForm.Visible = false;
            pnlContactoExito.Visible = true;
        }
        catch (ExcepcionTraducible ex)
        {
            lblMensajeContacto.Text = TraducirExcepcion(ex);
            lblMensajeContacto.CssClass = "server-error";
            lblMensajeContacto.Visible = true;
        }
    }

    #endregion
}