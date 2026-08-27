using BE;
using BLL;
using SERVICIOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

public partial class FormMaestroConsulta : GUI.PaginaBase
{
    private class FilaConsulta
    {
        public int IdConsulta { get; set; }
        public string NombrePaciente { get; set; }
        public string NombrePsicologo { get; set; }
        public DateTime FechaConsulta { get; set; }
        public int TiempoConsulta { get; set; }
        public DateTime FechaRegistro { get; set; }
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
        if (!gestorPermiso.TienePermiso(psicologoActual.RolPermiso, "acceder_abm_consultas"))
        {
            DenegarAcceso();
            return;
        }

        AplicarTraducciones();

        if (!IsPostBack)
        {
            lblTaglineSidebar.Text = Traducir("tagline_panel_gestion");
            txtFechaConsulta.Attributes["max"] = DateTime.Now.ToString("yyyy-MM-dd");

            CargarComboPsicologos();
            ModoAlta();
            CargarGrilla();
        }
    }

    private void AplicarTraducciones()
    {
        lblMenuCerrarSesionSidebar.Text = Traducir("menu_cerrar_sesion");
        lblHeaderSeccion.Text = Traducir("header_administrador");
        lblHeaderPagina.Text = Traducir("nav_abm_consultas");

        lblSubtituloForm.Text = Traducir("subtitulo_abm_consulta");
        lblSeccionVinculo.Text = Traducir("seccion_vinculo_profesional");
        lblEtiquetaPsicologo.Text = Traducir("lbl_psicologo_asignado");
        rfvPsicologo.ErrorMessage = Traducir("error_paciente_sin_profesional");
        lblEtiquetaPaciente.Text = Traducir("lbl_paciente");
        rfvPaciente.ErrorMessage = Traducir("error_consulta_sin_paciente");
        lblHintPaciente.Text = Traducir("hint_paciente_por_psicologo");

        lblSeccionDatos.Text = Traducir("seccion_datos_consulta");
        lblEtiquetaFecha.Text = Traducir("lbl_fecha_consulta");
        rfvFecha.ErrorMessage = Traducir("error_fecha_consulta_obligatoria");
        lblEtiquetaTiempo.Text = Traducir("lbl_tiempo_consulta");
        rfvTiempo.ErrorMessage = Traducir("error_tiempo_consulta_invalido");
        cvTiempo.ErrorMessage = Traducir("error_tiempo_consulta_invalido");

        lblSeccionContenidoClinico.Text = Traducir("seccion_contenido_clinico");
        lblAvisoEncriptado.Text = Traducir("aviso_contenido_encriptado");
        lblEtiquetaObjetivos.Text = Traducir("lbl_objetivos");
        lblEtiquetaObservaciones.Text = Traducir("lbl_observaciones");
        lblEtiquetaHipotesis.Text = Traducir("lbl_hipotesis");
        lblEtiquetaIntervenciones.Text = Traducir("lbl_intervenciones");
        lblEtiquetaEvolucion.Text = Traducir("lbl_evolucion_observada");
        lblEtiquetaDiagnostico.Text = Traducir("lbl_diagnostico");
        lblEtiquetaTratamiento.Text = Traducir("lbl_tratamiento");

        lblTituloListado.Text = Traducir("titulo_consultas_registradas");
        lblSubtituloListado.Text = Traducir("subtitulo_consultas_registradas");
        gvConsultas.Columns[0].HeaderText = Traducir("lbl_paciente");
        gvConsultas.Columns[1].HeaderText = Traducir("lbl_psicologo_asignado");
        gvConsultas.Columns[2].HeaderText = Traducir("lbl_fecha_consulta");
        gvConsultas.Columns[3].HeaderText = Traducir("lbl_tiempo_consulta");
        gvConsultas.Columns[4].HeaderText = Traducir("col_acciones");
        gvConsultas.EmptyDataText = Traducir("empty_consultas");
    }
    private void CargarComboPsicologos()
    {
        GestorPaciente gestorPaciente = new GestorPaciente();
        List<Psicologo> clinicos = gestorPaciente.ObtenerPsicologosClinicos();

        ddlPsicologo.Items.Clear();
        ddlPsicologo.Items.Add(new ListItem(Traducir("opt_seleccionar"), ""));

        foreach (Psicologo p in clinicos.OrderBy(x => x.Apellido))
        {
            ddlPsicologo.Items.Add(new ListItem(p.Nombre + " " + p.Apellido, p.IdPsicologo.ToString()));
        }
    }

    protected void ddlPsicologo_SelectedIndexChanged(object sender, EventArgs e)
    {
        CargarComboPacientes();
    }
    private void CargarComboPacientes()
    {
        ddlPaciente.Items.Clear();

        if (string.IsNullOrEmpty(ddlPsicologo.SelectedValue))
        {
            ddlPaciente.Enabled = false;
            ddlPaciente.Items.Add(new ListItem(Traducir("opt_seleccionar_psicologo_primero"), ""));
            return;
        }

        int idPsicologo = Convert.ToInt32(ddlPsicologo.SelectedValue);
        GestorPaciente gestorPaciente = new GestorPaciente();
        List<Paciente> pacientes = gestorPaciente.ObtenerPorPsicologo(idPsicologo);

        ddlPaciente.Enabled = true;
        ddlPaciente.Items.Add(new ListItem(Traducir("opt_seleccionar"), ""));

        foreach (Paciente p in pacientes.OrderBy(x => x.Apellido))
        {
            ddlPaciente.Items.Add(new ListItem(p.Nombre + " " + p.Apellido, p.IdPaciente.ToString()));
        }
    }

    protected void btnGuardar_Click(object sender, EventArgs e)
    {
        lblMensaje.Visible = false;

        bool esAlta = hdnIdConsulta.Value == "0";

        if (!Page.IsValid) return;

        GestorConsulta gestorConsulta = new GestorConsulta();

        try
        {
            DateTime fechaConsulta;
            DateTime.TryParse(txtFechaConsulta.Text, out fechaConsulta);
            int tiempoConsulta = Convert.ToInt32(txtTiempoConsulta.Text);

            if (esAlta)
            {
                Consulta nuevaConsulta = new Consulta();
                nuevaConsulta.IdPaciente = Convert.ToInt32(ddlPaciente.SelectedValue);
                nuevaConsulta.IdPsicologo = Convert.ToInt32(ddlPsicologo.SelectedValue);
                nuevaConsulta.FechaConsulta = fechaConsulta;
                nuevaConsulta.TiempoConsulta = tiempoConsulta;
                nuevaConsulta.Objetivos = txtObjetivos.Text.Trim();
                nuevaConsulta.Observaciones = txtObservaciones.Text.Trim();
                nuevaConsulta.Hipotesis = txtHipotesis.Text.Trim();
                nuevaConsulta.Intervenciones = txtIntervenciones.Text.Trim();
                nuevaConsulta.EvolucionObservada = txtEvolucionObservada.Text.Trim();
                nuevaConsulta.Diagnostico = txtDiagnostico.Text.Trim();
                nuevaConsulta.Tratamiento = txtTratamiento.Text.Trim();

                gestorConsulta.Alta(nuevaConsulta);
                MostrarExito(Traducir("msg_consulta_registrada"));
            }
            else
            {
                int idConsulta = Convert.ToInt32(hdnIdConsulta.Value);
                Consulta consultaModificada = gestorConsulta.BuscarPorId(idConsulta);
                if (consultaModificada == null)
                {
                    MostrarError(Traducir("msg_consulta_no_existe"));
                    ModoAlta();
                    CargarGrilla();
                    return;
                }

                consultaModificada.IdPaciente = Convert.ToInt32(ddlPaciente.SelectedValue);
                consultaModificada.IdPsicologo = Convert.ToInt32(ddlPsicologo.SelectedValue);
                consultaModificada.FechaConsulta = fechaConsulta;
                consultaModificada.TiempoConsulta = tiempoConsulta;
                consultaModificada.Objetivos = txtObjetivos.Text.Trim();
                consultaModificada.Observaciones = txtObservaciones.Text.Trim();
                consultaModificada.Hipotesis = txtHipotesis.Text.Trim();
                consultaModificada.Intervenciones = txtIntervenciones.Text.Trim();
                consultaModificada.EvolucionObservada = txtEvolucionObservada.Text.Trim();
                consultaModificada.Diagnostico = txtDiagnostico.Text.Trim();
                consultaModificada.Tratamiento = txtTratamiento.Text.Trim();

                gestorConsulta.Modificar(consultaModificada);
                MostrarExito(Traducir("msg_consulta_modificada"));
            }

            ModoAlta();
            CargarGrilla();
        }
        catch (ExcepcionTraducible ex)
        {
            MostrarError(TraducirExcepcion(ex));
        }
    }

    protected void btnCancelarEdicion_Click(object sender, EventArgs e)
    {
        ModoAlta();
    }

    protected void gvConsultas_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvConsultas.PageIndex = e.NewPageIndex;
        CargarGrilla();
    }

    protected void gvConsultas_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName != "Modificar") return;

        int idConsulta = Convert.ToInt32(e.CommandArgument);
        CargarFormularioParaEdicion(idConsulta);
    }

    protected void gvConsultas_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType != DataControlRowType.DataRow) return;

        FilaConsulta fila = e.Row.DataItem as FilaConsulta;
        if (fila == null) return;

        LinkButton lbModificar = e.Row.FindControl("lbModificar") as LinkButton;
        if (lbModificar == null) return;
        bool dentroDePlazo = (DateTime.Now - fila.FechaRegistro).TotalDays <= GestorConsulta.DIAS_LIMITE_MODIFICACION;

        if (dentroDePlazo)
        {
            lbModificar.Text = "✏️ " + Traducir("btn_modificar");
            lbModificar.Visible = true;
        }
        else
        {
            lbModificar.Visible = false;
        }
    }
    private void CargarGrilla()
    {
        GestorConsulta gestorConsulta = new GestorConsulta();
        GestorPaciente gestorPaciente = new GestorPaciente();
        GestorPsicologo gestorPsicologo = new GestorPsicologo();

        List<Consulta> todas = gestorConsulta.ObtenerTodas();
        Dictionary<int, string> pacientesPorId = gestorPaciente.ObtenerTodos().ToDictionary(p => p.IdPaciente, p => p.Nombre + " " + p.Apellido);
        Dictionary<int, string> psicologosPorId = gestorPsicologo.ObtenerTodos().ToDictionary(p => p.IdPsicologo, p => p.Nombre + " " + p.Apellido);

        List<FilaConsulta> filas = todas.Select(c => new FilaConsulta
        {
            IdConsulta = c.IdConsulta,
            NombrePaciente = pacientesPorId.ContainsKey(c.IdPaciente) ? pacientesPorId[c.IdPaciente] : "—",
            NombrePsicologo = psicologosPorId.ContainsKey(c.IdPsicologo) ? psicologosPorId[c.IdPsicologo] : "—",
            FechaConsulta = c.FechaConsulta,
            TiempoConsulta = c.TiempoConsulta,
            FechaRegistro = c.FechaRegistro
        }).ToList();

        gvConsultas.DataSource = filas;
        gvConsultas.DataBind();
    }

    private void ModoAlta()
    {
        hdnIdConsulta.Value = "0";
        lblFormTitulo.Text = Traducir("titulo_nueva_consulta");
        btnGuardar.Text = Traducir("btn_registrar_consulta_form");
        btnCancelarEdicion.Visible = false;

        ddlPsicologo.SelectedIndex = 0;
        CargarComboPacientes();
        txtFechaConsulta.Text = string.Empty;
        txtTiempoConsulta.Text = string.Empty;
        txtObjetivos.Text = string.Empty;
        txtObservaciones.Text = string.Empty;
        txtHipotesis.Text = string.Empty;
        txtIntervenciones.Text = string.Empty;
        txtEvolucionObservada.Text = string.Empty;
        txtDiagnostico.Text = string.Empty;
        txtTratamiento.Text = string.Empty;
    }

    private void CargarFormularioParaEdicion(int idConsulta)
    {
        GestorConsulta gestorConsulta = new GestorConsulta();
        Consulta consulta = gestorConsulta.BuscarPorId(idConsulta);
        if (consulta == null)
        {
            MostrarError(Traducir("msg_consulta_no_existe"));
            CargarGrilla();
            return;
        }

        hdnIdConsulta.Value = consulta.IdConsulta.ToString();
        lblFormTitulo.Text = Traducir("titulo_modificar_consulta_form");
        btnGuardar.Text = Traducir("btn_guardar_cambios");
        btnCancelarEdicion.Text = Traducir("btn_cancelar_edicion");
        btnCancelarEdicion.Visible = true;
        if (ddlPsicologo.Items.FindByValue(consulta.IdPsicologo.ToString()) == null)
        {
            GestorPsicologo gestorPsicologo = new GestorPsicologo();
            Psicologo psicologoDeLaConsulta = gestorPsicologo.BuscarPorId(consulta.IdPsicologo);
            if (psicologoDeLaConsulta != null)
            {
                ddlPsicologo.Items.Add(new ListItem(
                    psicologoDeLaConsulta.Nombre + " " + psicologoDeLaConsulta.Apellido + " ⚠",
                    psicologoDeLaConsulta.IdPsicologo.ToString()));
            }
        }
        ddlPsicologo.SelectedValue = consulta.IdPsicologo.ToString();
        CargarComboPacientes();
        if (ddlPaciente.Items.FindByValue(consulta.IdPaciente.ToString()) == null)
        {
            GestorPaciente gestorPaciente = new GestorPaciente();
            Paciente pacienteDeLaConsulta = gestorPaciente.BuscarPorId(consulta.IdPaciente);
            if (pacienteDeLaConsulta != null)
            {
                ddlPaciente.Items.Add(new ListItem(
                    pacienteDeLaConsulta.Nombre + " " + pacienteDeLaConsulta.Apellido + " ⚠",
                    pacienteDeLaConsulta.IdPaciente.ToString()));
            }
        }
        ddlPaciente.SelectedValue = consulta.IdPaciente.ToString();

        txtFechaConsulta.Text = consulta.FechaConsulta.ToString("yyyy-MM-dd");
        txtTiempoConsulta.Text = consulta.TiempoConsulta.ToString();
        txtObjetivos.Text = consulta.Objetivos;
        txtObservaciones.Text = consulta.Observaciones;
        txtHipotesis.Text = consulta.Hipotesis;
        txtIntervenciones.Text = consulta.Intervenciones;
        txtEvolucionObservada.Text = consulta.EvolucionObservada;
        txtDiagnostico.Text = consulta.Diagnostico;
        txtTratamiento.Text = consulta.Tratamiento;
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