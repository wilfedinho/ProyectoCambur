using BE;
using BLL;
using SERVICIOS;
using System;
using System.Linq;
using System.Web.UI.WebControls;
using GUI;

public partial class FormModificarConsulta : PaginaBase
{
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
        if (!gestorPermiso.TienePermiso(psicologoActual.RolPermiso, "acceder_modificar_consulta"))
        {
            DenegarAcceso();
            return;
        }

        AplicarTraducciones();

        if (!IsPostBack)
        {
            lblTaglineSidebar.Text = Traducir("tagline_panel_gestion");
            CargarComboPacientes();
            MostrarEstado(1);
        }
    }

    private void AplicarTraducciones()
    {
        lblMenuCerrarSesionSidebar.Text = Traducir("menu_cerrar_sesion");
        lblHeaderSeccion.Text = Traducir("seccion_gestion_clinica");
        lblHeaderPagina.Text = Traducir("nav_modificar_consulta");

    
        lblFormTituloSeleccion.Text = Traducir("titulo_modificar_consulta");
        lblFormSubtituloSeleccion.Text = Traducir("subtitulo_seleccion_consulta");
        lblEtiquetaPacienteSeleccion.Text = Traducir("lbl_paciente");
        rfvPacienteSeleccion.ErrorMessage = Traducir("error_consulta_sin_paciente");
        btnBuscarConsultas.Text = Traducir("btn_buscar_consultas");

        lblTituloConsultasEncontradas.Text = Traducir("titulo_consultas_editables");
        lblHintConsultas.Text = Traducir("hint_seleccionar_consulta_modificar");
        lblThFecha.Text = Traducir("th_fecha");
        lblThDuracion.Text = Traducir("th_duracion");
        lblThResumenObjetivos.Text = Traducir("th_resumen_objetivos");
        lblThPlazo.Text = Traducir("th_plazo_restante");

        lblPlazoVencidoTitulo.Text = Traducir("titulo_plazo_vencido");
        btnVolverDesdeBloqueado.Text = Traducir("btn_volver");

        
        lblAvisoReadonly.Text = Traducir("aviso_campos_no_editables");
        lblSeccionEditables.Text = Traducir("seccion_campos_editables");
        lblEtiquetaObjetivos.Text = Traducir("lbl_objetivos");
        lblEtiquetaObservaciones.Text = Traducir("lbl_observaciones");
        lblEtiquetaHipotesis.Text = Traducir("lbl_hipotesis");
        lblEtiquetaIntervenciones.Text = Traducir("lbl_intervenciones");
        lblEtiquetaEvolucion.Text = Traducir("lbl_evolucion_observada");
        lblSeccionCierre.Text = Traducir("seccion_cierre_clinico");
        lblEtiquetaDiagnostico.Text = Traducir("lbl_diagnostico");
        lblEtiquetaTratamiento.Text = Traducir("lbl_tratamiento");

        rfvObjetivos.ErrorMessage = Traducir("error_campo_obligatorio");
        rfvObservaciones.ErrorMessage = Traducir("error_campo_obligatorio");
        rfvHipotesis.ErrorMessage = Traducir("error_campo_obligatorio");
        rfvIntervenciones.ErrorMessage = Traducir("error_campo_obligatorio");
        rfvEvolucion.ErrorMessage = Traducir("error_campo_obligatorio");
        rfvDiagnostico.ErrorMessage = Traducir("error_campo_obligatorio");
        rfvTratamiento.ErrorMessage = Traducir("error_campo_obligatorio");

        btnVolverFormulario.Text = Traducir("btn_cancelar_edicion");
        btnGuardar.Text = Traducir("btn_guardar_cambios");

        lblPlazoCardTitulo.Text = Traducir("titulo_plazo_edicion");
        lblDiasRestantesLabel.Text = Traducir("lbl_dias_restantes");
        lblHistorialCambiosTitulo.Text = Traducir("titulo_historial_cambios");
        lblCreadaLabel.Text = Traducir("lbl_creada");
        lblUltimaModLabel.Text = Traducir("lbl_ultima_modificacion");
        lblAvisoEncriptadoTitulo.Text = Traducir("titulo_datos_encriptados");
        lblAvisoEncriptadoTexto.Text = Traducir("aviso_consulta_reencriptada");
    }

    #region Estado 1 - Selección

    private void CargarComboPacientes()
    {
        GestorPaciente gestorPaciente = new GestorPaciente();
        int idPropio = GestorSesion.PsicologoActual.IdPsicologo;
        var propios = gestorPaciente.ObtenerPorPsicologo(idPropio);

        ddlPacienteSeleccion.Items.Clear();
        ddlPacienteSeleccion.Items.Add(new ListItem(Traducir("opt_seleccionar"), ""));

        foreach (Paciente p in propios.OrderBy(x => x.Apellido))
        {
            ddlPacienteSeleccion.Items.Add(new ListItem(p.Nombre + " " + p.Apellido, p.IdPaciente.ToString()));
        }
    }

    protected void btnBuscarConsultas_Click(object sender, EventArgs e)
    {
        lblMensaje.Visible = false;

        if (!Page.IsValid) return;

        int idPaciente = Convert.ToInt32(ddlPacienteSeleccion.SelectedValue);
        int idPsicologo = GestorSesion.PsicologoActual.IdPsicologo;

        GestorPaciente gestorPaciente = new GestorPaciente();
        var propios = gestorPaciente.ObtenerPorPsicologo(idPsicologo);
        if (!propios.Any(p => p.IdPaciente == idPaciente))
        {
            MostrarError(Traducir("error_paciente_no_propio"));
            return;
        }

        GestorConsulta gestorConsulta = new GestorConsulta();
        var editables = gestorConsulta.ObtenerPorPaciente(idPaciente)
            .Where(c => (DateTime.Now - c.FechaRegistro).TotalDays <= GestorConsulta.DIAS_LIMITE_MODIFICACION)
            .OrderByDescending(c => c.FechaConsulta)
            .ToList();

        if (editables.Count == 0)
        {
            MostrarError(Traducir("error_sin_consultas_editables"));
            rptConsultas.DataSource = null;
            rptConsultas.DataBind();
            lblCantConsultas.Visible = false;
            return;
        }

        rptConsultas.DataSource = editables.Select(c => new
        {
            IdConsulta = c.IdConsulta,
            Fecha = c.FechaConsulta,
            Duracion = c.TiempoConsulta,
            ResumenObjetivos = TruncarResumen(c.Objetivos),
            DiasRestantes = Math.Max(0, GestorConsulta.DIAS_LIMITE_MODIFICACION - (int)(DateTime.Now - c.FechaRegistro).TotalDays)
        }).ToList();
        rptConsultas.DataBind();

        lblCantConsultas.Text = editables.Count + " " + Traducir("lbl_consultas_plural");
        lblCantConsultas.Visible = true;

        MostrarEstado(1);
        pnlListaConsultas.Visible = true;
    }

    protected void rptConsultas_ItemCommand(object sender, RepeaterCommandEventArgs e)
    {
        if (e.CommandName != "Modificar") return;

        int idConsulta = Convert.ToInt32(e.CommandArgument);
        CargarFormularioParaEdicion(idConsulta);
    }

    #endregion

    #region Estado 3 - Edición

    private void CargarFormularioParaEdicion(int idConsulta)
    {
        int idPsicologo = GestorSesion.PsicologoActual.IdPsicologo;

        GestorConsulta gestorConsulta = new GestorConsulta();
        Consulta consulta = gestorConsulta.BuscarPorId(idConsulta);
        if (consulta == null || consulta.IdPsicologo != idPsicologo)
        {
            MostrarError(Traducir("error_paciente_no_propio"));
            return;
        }

        double diasTranscurridos = (DateTime.Now - consulta.FechaRegistro).TotalDays;
        if (diasTranscurridos > GestorConsulta.DIAS_LIMITE_MODIFICACION)
        {
            lblMensajeBloqueado.Text = string.Format(Traducir("msg_consulta_plazo_vencido"),
                consulta.FechaConsulta.ToString("dd/MM/yyyy"), GestorConsulta.DIAS_LIMITE_MODIFICACION);
            MostrarEstado(2);
            return;
        }

        hdnIdConsulta.Value = consulta.IdConsulta.ToString();

        GestorPaciente gestorPaciente = new GestorPaciente();
        Paciente paciente = gestorPaciente.BuscarPorId(consulta.IdPaciente);

        lblPacienteNombre.Text = paciente != null ? paciente.Nombre + " " + paciente.Apellido : "—";
        lblPacienteIniciales.Text = paciente != null ? ObtenerIniciales(paciente.Nombre, paciente.Apellido) : "—";
        lblFechaConsulta.Text = consulta.FechaConsulta.ToString("dd/MM/yyyy");
        lblDuracionConsulta.Text = consulta.TiempoConsulta + " " + Traducir("lbl_minutos");

        int diasRestantes = Math.Max(0, GestorConsulta.DIAS_LIMITE_MODIFICACION - (int)diasTranscurridos);
        DateTime fechaLimite = consulta.FechaRegistro.AddDays(GestorConsulta.DIAS_LIMITE_MODIFICACION);

        if (diasRestantes <= 1)
        {
            lblBadgePlazo.Text = "⚠️ " + Traducir("badge_ultimo_dia_editar");
            lblBadgePlazo.CssClass = "badge-plazo-urgente";
        }
        else
        {
            lblBadgePlazo.Text = "✓ " + Traducir("badge_editable_hasta") + " " + fechaLimite.ToString("dd/MM/yyyy");
            lblBadgePlazo.CssClass = "badge-plazo-ok";
        }

        lblDiasRestantes.Text = diasRestantes.ToString();
        lblFechaLimite.Text = Traducir("lbl_limite") + ": " + fechaLimite.ToString("dddd dd/MM/yyyy");

        double pctRestante = Math.Round(((double)diasRestantes / GestorConsulta.DIAS_LIMITE_MODIFICACION) * 100, 0);
        lblPlazoFill.Style["width"] = pctRestante + "%";
        lblPlazoFill.Style["background"] = diasRestantes <= 1 ? "#F4A261" : "#2A9D8F";

        lblFechaCreacion.Text = consulta.FechaRegistro.ToString("dd/MM/yyyy HH:mm");
        lblUltimaModificacion.Text = consulta.FechaModificacion.HasValue
            ? consulta.FechaModificacion.Value.ToString("dd/MM/yyyy HH:mm")
            : Traducir("lbl_sin_modificaciones");

        txtObjetivos.Text = consulta.Objetivos;
        txtObservaciones.Text = consulta.Observaciones;
        txtHipotesis.Text = consulta.Hipotesis;
        txtIntervenciones.Text = consulta.Intervenciones;
        txtEvolucion.Text = consulta.EvolucionObservada;
        txtDiagnostico.Text = consulta.Diagnostico;
        txtTratamiento.Text = consulta.Tratamiento;

        MostrarEstado(3);
    }

    protected void btnGuardar_Click(object sender, EventArgs e)
    {
        lblMensaje.Visible = false;

        if (!Page.IsValid) return;

        int idConsulta = Convert.ToInt32(hdnIdConsulta.Value);
        int idPsicologo = GestorSesion.PsicologoActual.IdPsicologo;

        GestorConsulta gestorConsulta = new GestorConsulta();
        Consulta consulta = gestorConsulta.BuscarPorId(idConsulta);

        if (consulta == null || consulta.IdPsicologo != idPsicologo)
        {
            MostrarError(Traducir("error_paciente_no_propio"));
            return;
        }

        consulta.Objetivos = txtObjetivos.Text.Trim();
        consulta.Observaciones = txtObservaciones.Text.Trim();
        consulta.Hipotesis = txtHipotesis.Text.Trim();
        consulta.Intervenciones = txtIntervenciones.Text.Trim();
        consulta.EvolucionObservada = txtEvolucion.Text.Trim();
        consulta.Diagnostico = txtDiagnostico.Text.Trim();
        consulta.Tratamiento = txtTratamiento.Text.Trim();

        try
        {
            gestorConsulta.Modificar(consulta);
            MostrarExito(Traducir("msg_consulta_modificada"));
            lblUltimaModificacion.Text = consulta.FechaModificacion.Value.ToString("dd/MM/yyyy HH:mm");
        }
        catch (ExcepcionTraducible ex)
        {
            if (ex.Clave == "error_consulta_fuera_de_plazo_modificacion")
            {
                lblMensajeBloqueado.Text = string.Format(Traducir("msg_consulta_plazo_vencido"),
                    consulta.FechaConsulta.ToString("dd/MM/yyyy"), GestorConsulta.DIAS_LIMITE_MODIFICACION);
                MostrarEstado(2);
            }
            else
            {
                MostrarError(TraducirExcepcion(ex));
            }
        }
    }

    protected void btnVolverFormulario_Click(object sender, EventArgs e)
    {
        lblMensaje.Visible = false;
        VolverASeleccion();
    }

    protected void btnVolverDesdeBloqueado_Click(object sender, EventArgs e)
    {
        lblMensaje.Visible = false;
        VolverASeleccion();
    }

    private void VolverASeleccion()
    {
        pnlListaConsultas.Visible = false;
        MostrarEstado(1);
    }

    #endregion

    private void MostrarEstado(int estado)
    {
        pnlSeleccion.Visible = (estado == 1);
        pnlBloqueado.Visible = (estado == 2);
        pnlFormulario.Visible = (estado == 3);
    }

    private string TruncarResumen(string objetivos)
    {
        if (string.IsNullOrWhiteSpace(objetivos)) return "--";
        return objetivos.Length > 60 ? objetivos.Substring(0, 60) + "…" : objetivos;
    }

    private string ObtenerIniciales(string nombre, string apellido)
    {
        string i1 = !string.IsNullOrEmpty(nombre) ? nombre.Substring(0, 1) : "";
        string i2 = !string.IsNullOrEmpty(apellido) ? apellido.Substring(0, 1) : "";
        return (i1 + i2).ToUpper();
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