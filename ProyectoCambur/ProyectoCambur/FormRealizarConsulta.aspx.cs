using BE;
using BLL;
using SERVICIOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using GUI;

public partial class FormRealizarConsulta : PaginaBase
{
    private class ItemHistorial
    {
        public DateTime Fecha { get; set; }
        public string Resumen { get; set; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        Page.UnobtrusiveValidationMode = System.Web.UI.UnobtrusiveValidationMode.None;

        if (!GestorSesion.EstaAutenticado)
        {
            Response.Redirect("FormLogin.aspx");
            return;
        }

        Psicologo psicologoActual = GestorSesion.PsicologoActual;

        GestorPermiso gestorPermiso = new GestorPermiso();
        if (!gestorPermiso.TienePermiso(psicologoActual.RolPermiso, "acceder_realizar_consulta"))
        {
            DenegarAcceso();
            return;
        }

        AplicarTraducciones();

        if (!IsPostBack)
        {
            lblTaglineSidebar.Text = Traducir("tagline_panel_gestion");
            CargarComboPacientes();
            LimpiarCardPaciente();
        }
    }

    private void AplicarTraducciones()
    {
        lblMenuCerrarSesionSidebar.Text = Traducir("menu_cerrar_sesion");
        lblHeaderSeccion.Text = Traducir("seccion_gestion_clinica");
        lblHeaderPagina.Text = Traducir("nav_realizar_consulta");

        lblFormTitulo.Text = Traducir("titulo_nueva_consulta");
        lblFormSubtitulo.Text = Traducir("subtitulo_realizar_consulta");

        lblSeccionPacienteFecha.Text = Traducir("seccion_vinculo_profesional");
        lblEtiquetaPaciente.Text = Traducir("lbl_paciente");
        rfvPaciente.ErrorMessage = Traducir("error_consulta_sin_paciente");
        lblEtiquetaFecha.Text = Traducir("lbl_fecha_consulta");
        rfvFecha.ErrorMessage = Traducir("error_fecha_consulta_obligatoria");
        lblEtiquetaDuracion.Text = Traducir("lbl_tiempo_consulta");
        rfvDuracion.ErrorMessage = Traducir("error_tiempo_consulta_invalido");
        cvDuracion.ErrorMessage = Traducir("error_tiempo_consulta_invalido");

        lblSeccionContenidoClinico.Text = Traducir("seccion_contenido_clinico");
        lblAvisoEncriptado.Text = Traducir("aviso_contenido_encriptado");
        lblEtiquetaObjetivos.Text = Traducir("lbl_objetivos");
        rfvObjetivos.ErrorMessage = Traducir("error_campo_obligatorio");
        lblEtiquetaObservaciones.Text = Traducir("lbl_observaciones");
        rfvObservaciones.ErrorMessage = Traducir("error_campo_obligatorio");
        lblEtiquetaHipotesis.Text = Traducir("lbl_hipotesis");
        lblEtiquetaIntervenciones.Text = Traducir("lbl_intervenciones");
        lblEtiquetaEvolucion.Text = Traducir("lbl_evolucion_observada");

        lblSeccionCierreClinico.Text = Traducir("seccion_cierre_clinico");
        lblEtiquetaDiagnostico.Text = Traducir("lbl_diagnostico");
        lblEtiquetaTratamiento.Text = Traducir("lbl_tratamiento");

        lblBtnCancelar.Text = Traducir("btn_cancelar");
        btnRegistrar.Text = Traducir("btn_registrar_consulta_form");

        lblEtiquetaTotalConsultas.Text = Traducir("lbl_total_consultas_paciente");
        lblEtiquetaUltimaSesion.Text = Traducir("lbl_ultima_sesion_paciente");
        lblAvisoCardTexto.Text = Traducir("aviso_contenido_encriptado");
        lblTituloUltimasConsultas.Text = Traducir("titulo_ultimas_consultas");
        lblSinConsultas.Text = Traducir("historial_sin_consultas");
    }

    protected void btnRegistrar_Click(object sender, EventArgs e)
    {
        lblMensaje.Visible = false;

        if (!Page.IsValid) return;

        int idPsicologo = GestorSesion.PsicologoActual.IdPsicologo;
        int idPaciente = Convert.ToInt32(ddlPaciente.SelectedValue);
        GestorPaciente gestorPaciente = new GestorPaciente();
        List<Paciente> propios = gestorPaciente.ObtenerPorPsicologo(idPsicologo);
        if (!propios.Any(p => p.IdPaciente == idPaciente))
        {
            MostrarError(Traducir("error_paciente_no_propio"));
            return;
        }

        DateTime fechaConsulta;
        DateTime.TryParse(txtFechaConsulta.Text, out fechaConsulta);
        int tiempoConsulta = Convert.ToInt32(txtDuracion.Text);

        Consulta nuevaConsulta = new Consulta();
        nuevaConsulta.IdPaciente = idPaciente;
        nuevaConsulta.IdPsicologo = idPsicologo;
        nuevaConsulta.FechaConsulta = fechaConsulta;
        nuevaConsulta.TiempoConsulta = tiempoConsulta;
        nuevaConsulta.Objetivos = txtObjetivos.Text.Trim();
        nuevaConsulta.Observaciones = txtObservaciones.Text.Trim();
        nuevaConsulta.Hipotesis = txtHipotesis.Text.Trim();
        nuevaConsulta.Intervenciones = txtIntervenciones.Text.Trim();
        nuevaConsulta.EvolucionObservada = txtEvolucion.Text.Trim();
        nuevaConsulta.Diagnostico = txtDiagnostico.Text.Trim();
        nuevaConsulta.Tratamiento = txtTratamiento.Text.Trim();

        GestorConsulta gestorConsulta = new GestorConsulta();
        try
        {
            gestorConsulta.Alta(nuevaConsulta);
            MostrarExito(Traducir("msg_consulta_registrada"));
            LimpiarFormulario();
            ActualizarCardPaciente(idPaciente);
            CargarUltimasConsultas(idPaciente);
        }
        catch (ExcepcionTraducible ex)
        {
            MostrarError(TraducirExcepcion(ex));
        }
    }

    protected void ddlPaciente_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(ddlPaciente.SelectedValue))
        {
            LimpiarCardPaciente();
            return;
        }
        int idPaciente = Convert.ToInt32(ddlPaciente.SelectedValue);
        ActualizarCardPaciente(idPaciente);
        CargarUltimasConsultas(idPaciente);
    }

    private void CargarComboPacientes()
    {
        GestorPaciente gestorPaciente = new GestorPaciente();
        int idPropio = GestorSesion.PsicologoActual.IdPsicologo;
        List<Paciente> propios = gestorPaciente.ObtenerPorPsicologo(idPropio);
        ddlPaciente.Items.Clear();
        ddlPaciente.Items.Add(new ListItem(Traducir("opt_seleccionar"), ""));

        foreach (Paciente p in propios.OrderBy(x => x.Apellido))
        {
            ddlPaciente.Items.Add(new ListItem(p.Nombre + " " + p.Apellido, p.IdPaciente.ToString()));
        }
    }

    private void ActualizarCardPaciente(int idPaciente)
    {
        GestorPaciente gestorPaciente = new GestorPaciente();
        Paciente paciente = gestorPaciente.BuscarPorId(idPaciente);
        if (paciente == null)
        {
            LimpiarCardPaciente();
            return;
        }

        string inicialNombre = string.IsNullOrEmpty(paciente.Nombre) ? "" : paciente.Nombre.Substring(0, 1);
        string inicialApellido = string.IsNullOrEmpty(paciente.Apellido) ? "" : paciente.Apellido.Substring(0, 1);
        lblPacienteIniciales.Text = (inicialNombre + inicialApellido).ToUpper();
        lblPacienteNombre.Text = paciente.Nombre + " " + paciente.Apellido;
        lblPacienteEdad.Text = string.Format(Traducir("lbl_edad_anios"), CalcularEdad(paciente.FechaNacimiento));
        lblPacienteOcupacion.Text = paciente.Ocupacion;
        lblPacienteEstado.Text = paciente.EstadoCivil;
    }

    private void CargarUltimasConsultas(int idPaciente)
    {
        GestorConsulta gestorConsulta = new GestorConsulta();
        List<Consulta> consultas = gestorConsulta.ObtenerPorPaciente(idPaciente)
            .OrderByDescending(c => c.FechaConsulta)
            .ToList();

        lblTotalConsultas.Text = consultas.Count.ToString();
        lblUltimaConsulta.Text = consultas.Count > 0 ? consultas[0].FechaConsulta.ToString("dd/MM/yyyy") : "--";

        List<ItemHistorial> historial = consultas.Take(5).Select(c => new ItemHistorial
        {
            Fecha = c.FechaConsulta,
            Resumen = TruncarResumen(c.Objetivos)
        }).ToList();

        rptUltimasConsultas.DataSource = historial;
        rptUltimasConsultas.DataBind();

        rptUltimasConsultas.Visible = historial.Count > 0;
        lblSinConsultas.Visible = historial.Count == 0;
    }

    private string TruncarResumen(string objetivos)
    {
        if (string.IsNullOrWhiteSpace(objetivos)) return "--";
        return objetivos.Length > 60 ? objetivos.Substring(0, 60) + "…" : objetivos;
    }

    private int CalcularEdad(DateTime fechaNacimiento)
    {
        int edad = DateTime.Now.Year - fechaNacimiento.Year;
        if (fechaNacimiento.Date > DateTime.Now.AddYears(-edad)) edad--;
        return edad;
    }

    private void LimpiarCardPaciente()
    {
        lblPacienteIniciales.Text = "--";
        lblPacienteNombre.Text = Traducir("opt_seleccionar");
        lblPacienteEdad.Text = string.Empty;
        lblPacienteOcupacion.Text = string.Empty;
        lblPacienteEstado.Text = string.Empty;
        lblTotalConsultas.Text = "--";
        lblUltimaConsulta.Text = "--";
        rptUltimasConsultas.DataSource = null;
        rptUltimasConsultas.DataBind();
        rptUltimasConsultas.Visible = false;
        lblSinConsultas.Visible = false;
    }

    private void LimpiarFormulario()
    {
        txtFechaConsulta.Text = string.Empty;
        txtDuracion.Text = string.Empty;
        txtObjetivos.Text = string.Empty;
        txtObservaciones.Text = string.Empty;
        txtHipotesis.Text = string.Empty;
        txtIntervenciones.Text = string.Empty;
        txtEvolucion.Text = string.Empty;
        txtDiagnostico.Text = string.Empty;
        txtTratamiento.Text = string.Empty;
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