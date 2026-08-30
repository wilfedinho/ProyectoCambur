using BE;
using BLL;
using SERVICIOS;
using System;
using System.Linq;
using System.Web.UI.WebControls;
using GUI;

public partial class FormHistorialClinico : PaginaBase
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
        if (!gestorPermiso.TienePermiso(psicologoActual.RolPermiso, "acceder_generar_historial"))
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
        lblHeaderPagina.Text = Traducir("nav_generar_historial");
        lblFormTituloSeleccion.Text = Traducir("titulo_historial_clinico");
        lblFormSubtituloSeleccion.Text = Traducir("subtitulo_seleccion_paciente_historial");
        lblEtiquetaPacienteSeleccion.Text = Traducir("lbl_paciente");
        rfvPacienteSeleccion.ErrorMessage = Traducir("error_consulta_sin_paciente");
        btnContinuar.Text = Traducir("btn_continuar");
        lblSeccionInfoClinica.Text = Traducir("seccion_info_clinica_persistente");
        lblHintHistorial.Text = Traducir("hint_historial_clinico");
        lblTituloHabitos.Text = Traducir("seccion_habitos_nocivos");
        txtHabitosNocivos.Attributes["placeholder"] = Traducir("placeholder_habitos_nocivos");
        lblTituloContexto.Text = Traducir("seccion_contexto_familiar");
        txtContextoFamiliar.Attributes["placeholder"] = Traducir("placeholder_contexto_familiar");
        lblTituloAntFam.Text = Traducir("seccion_antecedentes_familiares");
        txtAntecedentesFamiliares.Attributes["placeholder"] = Traducir("placeholder_antecedentes_familiares");
        lblTituloAntMed.Text = Traducir("seccion_antecedentes_medicos");
        txtAntecedentesMedicos.Attributes["placeholder"] = Traducir("placeholder_antecedentes_medicos");
        lblTituloLaboral.Text = Traducir("seccion_situacion_laboral");
        txtSituacionLaboral.Attributes["placeholder"] = Traducir("placeholder_situacion_laboral");
        lblTituloTrauma.Text = Traducir("seccion_eventos_traumaticos");
        txtEventosTraumaticos.Attributes["placeholder"] = Traducir("placeholder_eventos_traumaticos");
        btnVolverSeleccion.Text = Traducir("btn_elegir_otro_paciente");
        btnGuardar.Text = Traducir("btn_guardar_historial");
        lblAvisoEncriptadoTitulo.Text = Traducir("titulo_datos_encriptados");
        lblAvisoEncriptadoTexto.Text = Traducir("aviso_historial_encriptado");
        lblProgresoTitulo.Text = Traducir("titulo_completitud_historial");
    }

    protected void btnContinuar_Click(object sender, EventArgs e)
    {
        lblMensaje.Visible = false;

        if (!Page.IsValid) return;

        int idPaciente = Convert.ToInt32(ddlPacienteSeleccion.SelectedValue);
        int idPsicologo = GestorSesion.PsicologoActual.IdPsicologo;

        GestorPaciente gestorPaciente = new GestorPaciente();
        Paciente paciente = gestorPaciente.BuscarPorId(idPaciente);
        if (paciente == null || paciente.IdPsicologo != idPsicologo)
        {
            MostrarError(Traducir("error_paciente_no_propio"));
            return;
        }

        CargarFormularioParaPaciente(paciente);
        MostrarEstado(2);
    }

    protected void btnVolverSeleccion_Click(object sender, EventArgs e)
    {
        lblMensaje.Visible = false;
        MostrarEstado(1);
    }

    protected void btnGuardar_Click(object sender, EventArgs e)
    {
        lblMensaje.Visible = false;

        int idPaciente = Convert.ToInt32(hdnIdPaciente.Value);
        int idPsicologo = GestorSesion.PsicologoActual.IdPsicologo;

        GestorPaciente gestorPaciente = new GestorPaciente();
        Paciente paciente = gestorPaciente.BuscarPorId(idPaciente);
        if (paciente == null || paciente.IdPsicologo != idPsicologo)
        {
            MostrarError(Traducir("error_paciente_no_propio"));
            return;
        }

        HistorialClinico historial = new HistorialClinico();
        historial.IdPaciente = idPaciente;
        historial.HabitosNocivos = txtHabitosNocivos.Text.Trim();
        historial.ContextoFamiliar = txtContextoFamiliar.Text.Trim();
        historial.AntecedentesFamiliares = txtAntecedentesFamiliares.Text.Trim();
        historial.AntecedentesMedicos = txtAntecedentesMedicos.Text.Trim();
        historial.SituacionLaboral = txtSituacionLaboral.Text.Trim();
        historial.EventosTraumaticos = txtEventosTraumaticos.Text.Trim();

        GestorHistorialClinico gestorHistorial = new GestorHistorialClinico();

        try
        {
            bool esAlta = hdnModo.Value != "modificar";

            if (esAlta)
            {
                gestorHistorial.Alta(historial);
                MostrarExito(Traducir("msg_historial_guardado"));
            }
            else
            {
                historial.IdHistorial = Convert.ToInt32(hdnIdHistorial.Value);
                gestorHistorial.Modificar(historial);
                MostrarExito(Traducir("msg_historial_actualizado"));
            }
            CargarFormularioParaPaciente(paciente);
        }
        catch (ExcepcionTraducible ex)
        {
            if (ex.Clave == "error_historial_ya_existe")
            {
                CargarFormularioParaPaciente(paciente);
                MostrarError(Traducir("error_historial_ya_existe"));
            }
            else
            {
                MostrarError(TraducirExcepcion(ex));
            }
        }
    }

    private void CargarFormularioParaPaciente(Paciente paciente)
    {
        hdnIdPaciente.Value = paciente.IdPaciente.ToString();

        lblPacienteNombre.Text = paciente.Nombre + " " + paciente.Apellido;
        lblPacienteIniciales.Text = ObtenerIniciales(paciente.Nombre, paciente.Apellido);
        lblPacienteEdad.Text = CalcularEdad(paciente.FechaNacimiento) + " " + Traducir("lbl_anios");
        lblPacienteEstado.Text = paciente.EstadoCivil;
        lblPacienteOcup.Text = paciente.Ocupacion;

        GestorHistorialClinico gestorHistorial = new GestorHistorialClinico();
        HistorialClinico historialExistente = gestorHistorial.BuscarPorPaciente(paciente.IdPaciente);

        if (historialExistente != null)
        {
            hdnModo.Value = "modificar";
            hdnIdHistorial.Value = historialExistente.IdHistorial.ToString();
            lblFormTituloFormulario.Text = Traducir("titulo_modificar_historial");
            btnGuardar.Text = Traducir("btn_guardar_cambios");

            txtHabitosNocivos.Text = historialExistente.HabitosNocivos;
            txtContextoFamiliar.Text = historialExistente.ContextoFamiliar;
            txtAntecedentesFamiliares.Text = historialExistente.AntecedentesFamiliares;
            txtAntecedentesMedicos.Text = historialExistente.AntecedentesMedicos;
            txtSituacionLaboral.Text = historialExistente.SituacionLaboral;
            txtEventosTraumaticos.Text = historialExistente.EventosTraumaticos;
        }
        else
        {
            hdnModo.Value = "alta";
            hdnIdHistorial.Value = "0";
            lblFormTituloFormulario.Text = Traducir("titulo_generar_historial");
            btnGuardar.Text = Traducir("btn_guardar_historial");

            txtHabitosNocivos.Text = string.Empty;
            txtContextoFamiliar.Text = string.Empty;
            txtAntecedentesFamiliares.Text = string.Empty;
            txtAntecedentesMedicos.Text = string.Empty;
            txtSituacionLaboral.Text = string.Empty;
            txtEventosTraumaticos.Text = string.Empty;
        }

        ActualizarBadgeEstadoHistorial();
    }

    private void ActualizarBadgeEstadoHistorial()
    {
        int completados = 0;
        int total = 6;

        if (!string.IsNullOrWhiteSpace(txtHabitosNocivos.Text)) completados++;
        if (!string.IsNullOrWhiteSpace(txtContextoFamiliar.Text)) completados++;
        if (!string.IsNullOrWhiteSpace(txtAntecedentesFamiliares.Text)) completados++;
        if (!string.IsNullOrWhiteSpace(txtAntecedentesMedicos.Text)) completados++;
        if (!string.IsNullOrWhiteSpace(txtSituacionLaboral.Text)) completados++;
        if (!string.IsNullOrWhiteSpace(txtEventosTraumaticos.Text)) completados++;

        ActualizarBadgeIndividual(lblBadgeHabitos, txtHabitosNocivos.Text);
        ActualizarBadgeIndividual(lblBadgeContexto, txtContextoFamiliar.Text);
        ActualizarBadgeIndividual(lblBadgeAntFam, txtAntecedentesFamiliares.Text);
        ActualizarBadgeIndividual(lblBadgeAntMed, txtAntecedentesMedicos.Text);
        ActualizarBadgeIndividual(lblBadgeLaboral, txtSituacionLaboral.Text);
        ActualizarBadgeIndividual(lblBadgeTrauma, txtEventosTraumaticos.Text);

        if (completados == total)
        {
            lblEstadoHistorial.Text = Traducir("badge_historial_completo");
            lblEstadoHistorial.CssClass = "badge-historial-completo";
        }
        else
        {
            lblEstadoHistorial.Text = completados + " " + Traducir("lbl_de") + " " + total + " " + Traducir("lbl_secciones");
            lblEstadoHistorial.CssClass = "badge-historial-parcial";
        }
    }

    private void ActualizarBadgeIndividual(Label badge, string contenido)
    {
        bool tieneContenido = !string.IsNullOrWhiteSpace(contenido);
        badge.Text = tieneContenido ? Traducir("badge_completado") : Traducir("badge_pendiente");
        badge.CssClass = tieneContenido ? "badge-seccion completado" : "badge-seccion";
    }

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

    private void MostrarEstado(int estado)
    {
        pnlSeleccionPaciente.Visible = (estado == 1);
        pnlFormulario.Visible = (estado == 2);
    }

    private string ObtenerIniciales(string nombre, string apellido)
    {
        string i1 = !string.IsNullOrEmpty(nombre) ? nombre.Substring(0, 1) : "";
        string i2 = !string.IsNullOrEmpty(apellido) ? apellido.Substring(0, 1) : "";
        return (i1 + i2).ToUpper();
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