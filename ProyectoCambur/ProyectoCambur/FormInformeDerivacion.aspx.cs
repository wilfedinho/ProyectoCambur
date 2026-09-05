using BE;
using BLL;
using SERVICIOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;
using GUI;

public partial class FormInformeDerivacion : PaginaBase
{
    public string JsonConfirmarDescarte
    {
        get
        {
            JavaScriptSerializer serializador = new JavaScriptSerializer();
            return serializador.Serialize(Traducir("confirm_descartar_informe"));
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        Page.UnobtrusiveValidationMode = UnobtrusiveValidationMode.None;

        if (!GestorSesion.EstaAutenticado)
        {
            Response.Redirect("FormLogin.aspx");
            return;
        }

        AplicarTraducciones();

        if (!IsPostBack)
        {
            CargarComboPacientes();
            if (!string.IsNullOrEmpty(ddlPacienteDerivacion.SelectedValue))
            {
                CargarInfoPaciente(Convert.ToInt32(ddlPacienteDerivacion.SelectedValue));
            }

            MostrarEstado(1);
        }
    }
    private void AplicarTraducciones()
    {
        lblTaglineSidebar.Text = Traducir("tagline_panel_gestion");
        lblMenuCerrarSesionSidebar.Text = Traducir("menu_cerrar_sesion");
        lblHeaderSeccion.Text = Traducir("seccion_modulo_ia");

        lblEtiquetaPacienteDerivacion.Text = Traducir("lbl_paciente");
        rfvPacienteDerivacion.ErrorMessage = Traducir("error_consulta_sin_paciente");

        lblSeccionDatosDerivacion.Text = Traducir("seccion_datos_derivacion");

        lblEtiquetaEspecialidad.Text = Traducir("lbl_especialidad_derivacion");
        ddlEspecialidad.Items[0].Text = Traducir("opt_seleccionar");
        ddlEspecialidad.Items[1].Text = Traducir("esp_psiquiatria");
        ddlEspecialidad.Items[2].Text = Traducir("esp_neurologia");
        ddlEspecialidad.Items[3].Text = Traducir("esp_cardiologia");
        ddlEspecialidad.Items[4].Text = Traducir("esp_nutricion");
        ddlEspecialidad.Items[5].Text = Traducir("esp_trabajo_social");
        ddlEspecialidad.Items[6].Text = Traducir("esp_medicina_general");
        ddlEspecialidad.Items[7].Text = Traducir("esp_fisiatria");
        ddlEspecialidad.Items[8].Text = Traducir("esp_otra_especialidad");
        rfvEsp.ErrorMessage = Traducir("error_especialidad_obligatoria");

        lblEtiquetaProfDestino.Text = Traducir("lbl_profesional_destinatario");
        txtProfDestino.Attributes["placeholder"] = Traducir("placeholder_prof_destino");
        rfvProf.ErrorMessage = Traducir("error_prof_destino_obligatorio");

        lblEtiquetaInstitucion.Text = Traducir("lbl_institucion_opcional");
        txtInstitucion.Attributes["placeholder"] = Traducir("placeholder_institucion");

        lblEtiquetaMotivo.Text = Traducir("lbl_motivo_derivacion");
        txtMotivo.Attributes["placeholder"] = Traducir("placeholder_motivo_derivacion");
        rfvMotivo.ErrorMessage = Traducir("error_motivo_derivacion_obligatorio");

        lblAvisoIATitulo.Text = Traducir("aviso_ia_informe_titulo");

        lnkCancelar.Text = Traducir("btn_cancelar");
        btnGenerar.Text = Traducir("btn_generar_informe_ia");

        lblTituloInfoIncluida.Text = Traducir("titulo_info_incluida");
        lblInfoAntecedentes.Text = Traducir("info_incluye_antecedentes");
        lblInfoEvolucionTexto.Text = Traducir("info_incluye_evolucion");
        lblInfoIntervenciones.Text = Traducir("info_incluye_intervenciones");

        lblAvisoRevisionObligatoriaTitulo.Text = Traducir("aviso_titulo_revision_obligatoria");
        lblAvisoRevisionObligatoriaTexto.Text = Traducir("aviso_texto_revision_obligatoria");

        lblCargaTitulo.Text = Traducir("carga_titulo_informe_ia");
        lblCargaSubtitulo.Text = Traducir("carga_subtitulo_informe_ia");

        lblTituloInformeGenerado.Text = Traducir("titulo_informe_generado");
        lblBadgePendienteRevision.Text = Traducir("badge_pendiente_revision");
        lblAvisoIABadgeInforme.Text = Traducir("aviso_ia_badge_informe");
        lblSeccionContenidoInforme.Text = Traducir("seccion_contenido_informe");

        lblEtiquetaSintesis.Text = Traducir("lbl_sintesis_diagnostica");
        lblEtiquetaAndamiajes.Text = Traducir("lbl_andamiajes_implementados");
        lblEtiquetaObjetivos.Text = Traducir("lbl_objetivos_terapeuticos");
        lblEtiquetaModalidad.Text = Traducir("lbl_modalidad_trabajo");
        lblEtiquetaMotivoAuditoria.Text = Traducir("lbl_motivo_derivacion");

        lblSeccionFirma.Text = Traducir("seccion_firma_profesional");
        lblEtiquetaFirma.Text = Traducir("lbl_firma_digital");
        txtFirma.Attributes["placeholder"] = Traducir("placeholder_firma");
        rfvFirma.ErrorMessage = Traducir("error_informe_firma_obligatoria");

        btnDescartar.Text = Traducir("btn_descartar_informe");
        btnGuardarBorrador.Text = Traducir("btn_guardar_borrador");
        btnValidar.Text = Traducir("btn_validar_firmar_informe");

        lblTituloDatosInforme.Text = Traducir("titulo_datos_informe");
        lblMetaLabelPaciente.Text = Traducir("lbl_paciente");
        lblMetaLabelEspecialidad.Text = Traducir("lbl_especialidad_destino");
        lblMetaLabelDestino.Text = Traducir("lbl_profesional_destinatario");
        lblMetaLabelFecha.Text = Traducir("lbl_generado");

        lblAvisoRevisionProfesionalTitulo.Text = Traducir("aviso_titulo_revision_profesional");
        lblAvisoRevisionProfesionalTexto.Text = Traducir("aviso_texto_revision_profesional");
    }
    private void MostrarEstado(int estado)
    {
        pnlFormulario.Visible = (estado == 1);
        pnlAuditoria.Visible = (estado == 2);
        lblHeaderTitulo.Text = estado == 1 ? Traducir("nav_informe_derivacion") : Traducir("nav_auditoria_informe");

        ucSidebarNavegacion.PaginaActual = estado == 1 ? "acceder_informe_derivacion" : "acceder_auditoria_informe";
        ucSidebarNavegacion.RenderizarNavegacion();
    }

    private void CargarComboPacientes()
    {
        GestorPaciente gestorPaciente = new GestorPaciente();
        int idPsicologo = GestorSesion.PsicologoActual.IdPsicologo;
        List<Paciente> propios = gestorPaciente.ObtenerPorPsicologo(idPsicologo);

        ddlPacienteDerivacion.Items.Clear();
        ddlPacienteDerivacion.Items.Add(new ListItem(Traducir("opt_seleccionar"), ""));
        foreach (Paciente p in propios.OrderBy(x => x.Apellido))
        {
            ddlPacienteDerivacion.Items.Add(new ListItem(p.Nombre + " " + p.Apellido, p.IdPaciente.ToString()));
        }
    }

    protected void ddlPacienteDerivacion_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblMensaje.Visible = false;
        if (!string.IsNullOrEmpty(ddlPacienteDerivacion.SelectedValue))
        {
            CargarInfoPaciente(Convert.ToInt32(ddlPacienteDerivacion.SelectedValue));
        }
    }

    private void CargarInfoPaciente(int idPaciente)
    {
        GestorPaciente gestorPaciente = new GestorPaciente();
        Paciente paciente = gestorPaciente.BuscarPorId(idPaciente);
        if (paciente == null) return;

        GestorConsulta gestorConsulta = new GestorConsulta();
        int cantidadConsultas = gestorConsulta.ObtenerPorPaciente(idPaciente).Count;

        lblPacienteIniciales.Text = Iniciales(paciente.Nombre, paciente.Apellido);
        lblPacienteNombre.Text = paciente.Nombre + " " + paciente.Apellido;
        lblPacienteEdad.Text = CalcularEdad(paciente.FechaNacimiento) + " " + Traducir("lbl_anios");
        lblPacienteConsultas.Text = TextoCantidadConsultas(cantidadConsultas);

        GestorHistorialClinico gestorHistorial = new GestorHistorialClinico();
        bool tieneHistorial = gestorHistorial.BuscarPorPaciente(idPaciente) != null;

        lblAvisoIA.Text = (cantidadConsultas > 0 ? TextoCantidadConsultas(cantidadConsultas) : Traducir("txt_sin_consultas_registradas")) +
                           " · " + (tieneHistorial ? Traducir("txt_historial_clinico_completo") : Traducir("txt_sin_historial_clinico")) +
                           " · " + Traducir("txt_evolucion_observada");
        lblInfoConsultas.Text = TextoCantidadConsultas(cantidadConsultas);
    }

    private string TextoCantidadConsultas(int cantidad)
    {
        return cantidad + " " + (cantidad == 1 ? Traducir("txt_consulta_registrada_singular") : Traducir("txt_consultas_registradas_plural"));
    }

    protected void btnGenerar_Click(object sender, EventArgs e)
    {
        lblMensaje.Visible = false;

        if (!Page.IsValid) return;

        int idPsicologo = GestorSesion.PsicologoActual.IdPsicologo;
        int idPaciente = Convert.ToInt32(ddlPacienteDerivacion.SelectedValue);
        string especialidad = ddlEspecialidad.SelectedItem.Text;
        string profDestino = txtProfDestino.Text.Trim();
        string institucion = txtInstitucion.Text.Trim();
        string motivo = txtMotivo.Text.Trim();

        GestorInformeDerivacion gestorInforme = new GestorInformeDerivacion();
        try
        {
            int idGenerado = gestorInforme.Generar(idPsicologo, idPaciente, especialidad, profDestino, institucion, motivo);
            hdnIdInforme.Value = idGenerado.ToString();
            MostrarInformeGenerado(idGenerado);
            MostrarEstado(2);
        }
        catch (ExcepcionTraducible ex)
        {
            MostrarError(TraducirExcepcion(ex));
        }
    }

    private void MostrarInformeGenerado(int idInforme)
    {
        GestorInformeDerivacion gestorInforme = new GestorInformeDerivacion();
        InformeDerivacion informe = gestorInforme.BuscarPorId(idInforme);
        SeccionesInformeDerivacion secciones = gestorInforme.ObtenerSecciones(informe);

        txtSintesisDiagnostica.Text = secciones.SintesisDiagnostica;
        txtAndamiajes.Text = secciones.Andamiajes;
        txtObjetivos.Text = secciones.Objetivos;
        txtModalidadTrabajo.Text = secciones.ModalidadTrabajo;
        txtMotivoDerivacion.Text = secciones.MotivoDerivacion;
        txtFirma.Text = string.Empty;

        lblMetaPaciente.Text = ddlPacienteDerivacion.SelectedItem.Text;
        lblMetaEspecialidad.Text = secciones.EspecialidadDerivacion;
        lblMetaDestino.Text = secciones.ProfesionalDestinatario + (!string.IsNullOrEmpty(secciones.Institucion) ? " — " + secciones.Institucion : "");
        lblMetaFecha.Text = informe.FechaGeneracion.ToString("dd/MM/yyyy HH:mm");

        lblAuditoriaMeta.Text = secciones.EspecialidadDerivacion + " · " + secciones.ProfesionalDestinatario;
    }

    protected void btnValidar_Click(object sender, EventArgs e)
    {
        lblMensaje.Visible = false;

        if (!Page.IsValid) return;

        if (string.IsNullOrWhiteSpace(txtSintesisDiagnostica.Text) &&
            string.IsNullOrWhiteSpace(txtAndamiajes.Text))
        {
            MostrarError(Traducir("error_informe_vacio"));
            return;
        }

        int idPsicologo = GestorSesion.PsicologoActual.IdPsicologo;
        int idInforme = Convert.ToInt32(hdnIdInforme.Value);

        GestorInformeDerivacion gestorInforme = new GestorInformeDerivacion();
        try
        {
            gestorInforme.Auditar(idPsicologo, idInforme,
                txtSintesisDiagnostica.Text.Trim(), txtAndamiajes.Text.Trim(), txtObjetivos.Text.Trim(),
                txtModalidadTrabajo.Text.Trim(), txtMotivoDerivacion.Text.Trim(), txtFirma.Text.Trim());

            MostrarExito(string.Format(Traducir("exito_informe_validado"), txtFirma.Text.Trim()));
        }
        catch (ExcepcionTraducible ex)
        {
            MostrarError(TraducirExcepcion(ex));
        }
    }

    protected void btnGuardarBorrador_Click(object sender, EventArgs e)
    {
        lblMensaje.Visible = false;

        int idPsicologo = GestorSesion.PsicologoActual.IdPsicologo;
        int idInforme = Convert.ToInt32(hdnIdInforme.Value);

        GestorInformeDerivacion gestorInforme = new GestorInformeDerivacion();
        try
        {
            gestorInforme.GuardarBorrador(idPsicologo, idInforme,
                txtSintesisDiagnostica.Text.Trim(), txtAndamiajes.Text.Trim(), txtObjetivos.Text.Trim(),
                txtModalidadTrabajo.Text.Trim(), txtMotivoDerivacion.Text.Trim());

            MostrarExito(Traducir("exito_borrador_guardado"));
        }
        catch (ExcepcionTraducible ex)
        {
            MostrarError(TraducirExcepcion(ex));
        }
    }

    protected void btnDescartar_Click(object sender, EventArgs e)
    {
        lblMensaje.Visible = false;

        int idPsicologo = GestorSesion.PsicologoActual.IdPsicologo;
        int idInforme = Convert.ToInt32(hdnIdInforme.Value);

        GestorInformeDerivacion gestorInforme = new GestorInformeDerivacion();
        try
        {
            gestorInforme.Descartar(idPsicologo, idInforme);
            LimpiarFormulario();
            MostrarEstado(1);
            MostrarExito(Traducir("exito_informe_descartado"));
        }
        catch (ExcepcionTraducible ex)
        {
            MostrarError(TraducirExcepcion(ex));
        }
    }

    private void LimpiarFormulario()
    {
        ddlEspecialidad.SelectedIndex = 0;
        txtProfDestino.Text = string.Empty;
        txtInstitucion.Text = string.Empty;
        txtMotivo.Text = string.Empty;
        hdnIdInforme.Value = string.Empty;
    }

    private string Iniciales(string nombre, string apellido)
    {
        string i1 = !string.IsNullOrWhiteSpace(nombre) ? nombre.Trim().Substring(0, 1).ToUpper() : "";
        string i2 = !string.IsNullOrWhiteSpace(apellido) ? apellido.Trim().Substring(0, 1).ToUpper() : "";
        return i1 + i2;
    }

    private int CalcularEdad(DateTime fechaNacimiento)
    {
        int edad = DateTime.Today.Year - fechaNacimiento.Year;
        if (fechaNacimiento.Date > DateTime.Today.AddYears(-edad)) edad--;
        return edad;
    }

    private void MostrarError(string msg)
    {
        lblMensaje.Text = msg;
        lblMensaje.CssClass = "server-error";
        lblMensaje.Visible = true;
    }

    private void MostrarExito(string msg)
    {
        lblMensaje.Text = msg;
        lblMensaje.CssClass = "server-success";
        lblMensaje.Visible = true;
    }
}