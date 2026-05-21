using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
public partial class FormHistorialClinico : System.Web.UI.Page
{
    // =========================================================
    // PAGE LOAD
    // =========================================================
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            CargarProfesionalDemo();
            CargarHistorialDemo();
        }
    }

    // =========================================================
    // PROFESIONAL LOGUEADO (demo)
    // TODO: reemplazar por Session["Profesional"]
    // =========================================================
    private void CargarProfesionalDemo()
    {
        lblNombreProfesional.Text = "Lucía Martínez";
        lblIniciales.Text = "LM";
    }

    // =========================================================
    // DATOS DEL PACIENTE E HISTORIAL (demo)
    // En producción: obtener IdPaciente de Request.QueryString["id"]
    // y llamar a BLL.HistorialBLL.ObtenerPorPaciente(idPaciente)
    // TODO: reemplazar por:
    //   int idPaciente = Convert.ToInt32(Request.QueryString["id"]);
    //   BE.Paciente p = BLL.PacienteBLL.ObtenerPorId(idPaciente);
    //   BE.Historial h = BLL.HistorialBLL.ObtenerPorPaciente(idPaciente);
    //   (los campos vienen desencriptados desde la BLL)
    // =========================================================
    private void CargarHistorialDemo()
    {
        // -- Datos del paciente --
        lblHeaderPaciente.Text = "Historial Clínico — Martín González";
        lblPacienteIniciales.Text = "MG";
        lblPacienteNombre.Text = "Martín González";
        lblPacienteEdad.Text = "33 años";
        lblPacienteEstado.Text = "Soltero/a";
        lblPacienteOcup.Text = "Docente";

        // -- Contenido del historial (demo: datos desencriptados) --
        txtHabitosNocivos.Text = "Fumador ocasional (3-4 cigarrillos por semana). Refiere consumo moderado de alcohol los fines de semana. Dificultades para mantener rutina de sueño regular, se acuesta pasada la 1 AM habitualmente.";

        txtContextoFamiliar.Text = "Vive solo desde los 25 años. Relación distante con el padre (separado de la madre cuando tenía 8 años). Vínculo estrecho con la madre, con quien habla diariamente. Tiene un hermano mayor con quien tiene poca relación.";

        txtAntecedentesFamiliares.Text = "Madre con diagnóstico de trastorno de ansiedad generalizada. Abuelo paterno con alcoholismo crónico. Sin otros antecedentes psiquiátricos relevantes en la familia.";

        txtAntecedentesMedicos.Text = "Diagnóstico de hipertensión leve desde los 30 años, con seguimiento cardiológico anual. Sin medicación psiquiátrica actual. Alergia a la penicilina.";

        txtSituacionLaboral.Text = "Docente de nivel secundario desde hace 7 años. Refiere satisfacción con la tarea pero estrés crónico por conflictos institucionales. Ingresos estables pero tensión por bajo reconocimiento salarial.";

        // Dejar evento traumático vacío para demo (sección en "Pendiente")
        txtEventosTraumaticos.Text = string.Empty;

        // -- Badge de estado del historial --
        ActualizarBadgeEstadoHistorial();
    }

    // =========================================================
    // CALCULAR ESTADO GENERAL DEL HISTORIAL
    // =========================================================
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

        // Actualizar badges individuales desde server (para el estado inicial)
        ActualizarBadgeIndividual(lblBadgeHabitos, txtHabitosNocivos.Text);
        ActualizarBadgeIndividual(lblBadgeContexto, txtContextoFamiliar.Text);
        ActualizarBadgeIndividual(lblBadgeAntFam, txtAntecedentesFamiliares.Text);
        ActualizarBadgeIndividual(lblBadgeAntMed, txtAntecedentesMedicos.Text);
        ActualizarBadgeIndividual(lblBadgeLaboral, txtSituacionLaboral.Text);
        ActualizarBadgeIndividual(lblBadgeTrauma, txtEventosTraumaticos.Text);

        // Badge general
        if (completados == total)
        {
            lblEstadoHistorial.Text = "Historial completo";
            lblEstadoHistorial.CssClass = "badge-historial-completo";
        }
        else
        {
            lblEstadoHistorial.Text = completados + " de " + total + " secciones";
            lblEstadoHistorial.CssClass = "badge-historial-parcial";
        }
    }

    private void ActualizarBadgeIndividual(System.Web.UI.WebControls.Label badge, string contenido)
    {
        bool tieneContenido = !string.IsNullOrWhiteSpace(contenido);
        badge.Text = tieneContenido ? "Completado" : "Pendiente";
        badge.CssClass = tieneContenido ? "badge-seccion completado" : "badge-seccion";
    }

    // =========================================================
    // EVENTO BOTÓN GUARDAR
    // =========================================================
    protected void btnGuardar_Click(object sender, EventArgs e)
    {
        lblMensaje.Visible = false;

        // Validar que al menos una sección tenga contenido
        bool alguienCompleto =
            !string.IsNullOrWhiteSpace(txtHabitosNocivos.Text) ||
            !string.IsNullOrWhiteSpace(txtContextoFamiliar.Text) ||
            !string.IsNullOrWhiteSpace(txtAntecedentesFamiliares.Text) ||
            !string.IsNullOrWhiteSpace(txtAntecedentesMedicos.Text) ||
            !string.IsNullOrWhiteSpace(txtSituacionLaboral.Text) ||
            !string.IsNullOrWhiteSpace(txtEventosTraumaticos.Text);

        if (!alguienCompleto)
        {
            MostrarError("Completá al menos una sección del historial antes de guardar.");
            return;
        }

        // TODO: reemplazar por:
        //   int idPaciente    = Convert.ToInt32(Request.QueryString["id"]);
        //   int idProfesional = (int)Session["IdProfesional"];
        //   BE.Historial h = new BE.Historial();
        //   h.IdPaciente             = idPaciente;
        //   h.IdProfesional          = idProfesional;
        //   h.HabitosNocivos         = txtHabitosNocivos.Text.Trim();        // → BLL encripta con AES
        //   h.ContextoFamiliar       = txtContextoFamiliar.Text.Trim();      // → BLL encripta con AES
        //   h.AntecedentesFamiliares = txtAntecedentesFamiliares.Text.Trim();// → BLL encripta con AES
        //   h.AntecedentesMedicos    = txtAntecedentesMedicos.Text.Trim();   // → BLL encripta con AES
        //   h.SituacionLaboral       = txtSituacionLaboral.Text.Trim();      // → BLL encripta con AES
        //   h.EventosTraumaticos     = txtEventosTraumaticos.Text.Trim();    // → BLL encripta con AES
        //   bool ok = BLL.HistorialBLL.GuardarOActualizar(h);
        //   if (ok) MostrarExito("Historial guardado correctamente.");
        //   else    MostrarError("No fue posible guardar el historial.");

        // DEMO: simular guardado exitoso
        ActualizarBadgeEstadoHistorial();
        MostrarExito("Historial clínico guardado correctamente. Los datos fueron encriptados.");
    }

    // =========================================================
    // HELPERS
    // =========================================================
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
