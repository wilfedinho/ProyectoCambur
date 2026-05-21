using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
public partial class FormResumenIA : System.Web.UI.Page
{
    // =========================================================
    // PAGE LOAD
    // =========================================================
    protected void Page_Load(object sender, EventArgs e)
    {
        Page.UnobtrusiveValidationMode = UnobtrusiveValidationMode.None;
        if (!IsPostBack)
        {
            CargarProfesionalDemo();
            CargarDropdownPacientes();
            CargarFiltrosFechaDemo();
            MostrarEstado(1); // arrancar en Estado 1: filtros
        }
    }

    // =========================================================
    // CONTROL DE ESTADOS DE LA PANTALLA
    // Estado 1 = filtros | Estado 2 = consultas | Estado 3 = resumen
    // =========================================================
    private void MostrarEstado(int estado)
    {
        pnlFiltros.Visible = (estado == 1);
        pnlConsultas.Visible = (estado == 2);
        pnlResumen.Visible = (estado == 3);
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
    // DROPDOWN PACIENTES (demo)
    // TODO: reemplazar por BLL.PacienteBLL.ObtenerActivosPorProfesional()
    // =========================================================
    private void CargarDropdownPacientes()
    {
        ddlPaciente.Items.Clear();
        ddlPaciente.Items.Add(new ListItem("Seleccioná un paciente...", ""));
        ddlPaciente.Items.Add(new ListItem("Martín González", "1"));
        ddlPaciente.Items.Add(new ListItem("Sofía Ramírez", "2"));
        ddlPaciente.Items.Add(new ListItem("Carlos Ibáñez", "3"));
        ddlPaciente.Items.Add(new ListItem("Valentina Moreno", "4"));
        ddlPaciente.Items.Add(new ListItem("Facundo Pérez", "5"));

        // Preseleccionar el primero en demo
        ddlPaciente.Items[1].Selected = true;
    }

    // =========================================================
    // FECHAS POR DEFECTO (demo)
    // =========================================================
    private void CargarFiltrosFechaDemo()
    {
        txtFechaDesde.Text = new DateTime(2026, 1, 1).ToString("yyyy-MM-dd");
        txtFechaHasta.Text = DateTime.Today.ToString("yyyy-MM-dd");
    }

    // =========================================================
    // EVENTO: BUSCAR CONSULTAS (Estado 1 → Estado 2)
    // =========================================================
    protected void btnBuscar_Click(object sender, EventArgs e)
    {
        lblMensaje.Visible = false;

        if (!Page.IsValid) return;

        int idPaciente = 0;
        if (!int.TryParse(ddlPaciente.SelectedValue, out idPaciente) || idPaciente == 0)
        {
            MostrarError("Seleccioná un paciente válido.");
            return;
        }

        DateTime desde, hasta;
        if (!DateTime.TryParse(txtFechaDesde.Text, out desde) ||
            !DateTime.TryParse(txtFechaHasta.Text, out hasta))
        {
            MostrarError("Las fechas ingresadas no son válidas.");
            return;
        }

        if (desde > hasta)
        {
            MostrarError("La fecha de inicio debe ser anterior a la fecha de fin.");
            return;
        }

        // TODO: reemplazar por BLL.ConsultaBLL.ObtenerPorPacienteYPeriodo(idPaciente, desde, hasta)
        DataTable consultas = ObtenerConsultasDemo(idPaciente, desde, hasta);

        if (consultas.Rows.Count == 0)
        {
            MostrarError("No se encontraron consultas registradas para ese paciente en el período seleccionado.");
            return;
        }

        // Cargar repeater
        rptConsultas.DataSource = consultas;
        rptConsultas.DataBind();

        // Labels informativos
        lblCantConsultas.Text = consultas.Rows.Count + " consultas";
        lblCantConsultas.Visible = true;
        lblRangoBusqueda.Text = desde.ToString("dd/MM/yyyy") + " al " + hasta.ToString("dd/MM/yyyy");

        MostrarEstado(2);
    }

    // =========================================================
    // EVENTO: VOLVER A FILTROS (Estado 2 → Estado 1)
    // =========================================================
    protected void btnVolver_Click(object sender, EventArgs e)
    {
        lblMensaje.Visible = false;
        MostrarEstado(1);
    }

    // =========================================================
    // EVENTO: GENERAR RESUMEN (Estado 2 → Estado 3)
    // =========================================================
    protected void btnGenerar_Click(object sender, EventArgs e)
    {
        lblMensaje.Visible = false;

        string nombrePaciente = ddlPaciente.SelectedItem.Text;
        string periodoTexto = txtFechaDesde.Text != string.Empty
            ? DateTime.Parse(txtFechaDesde.Text).ToString("dd/MM/yyyy") + " al " +
              DateTime.Parse(txtFechaHasta.Text).ToString("dd/MM/yyyy")
            : "período seleccionado";

        // TODO: reemplazar este bloque por:
        //   1. Recopilar IDs de consultas seleccionadas del Repeater
        //   2. Llamar a BLL.ResumenBLL.GenerarConIA(idsConsultas, idPaciente, idProfesional)
        //      que internamente llama al módulo Python/FastAPI con OpenRouter
        //   3. Mostrar la respuesta estructurada en los Labels
        //   Ejemplo:
        //   List<int> idsSeleccionados = ObtenerIdsConsultasSeleccionadas();
        //   if (idsSeleccionados.Count == 0) { MostrarError("Seleccioná al menos una consulta."); return; }
        //   BE.ResumenIA resumen = BLL.ResumenBLL.GenerarConIA(idsSeleccionados);
        //   if (resumen == null) { MostrarError("El servicio de IA no pudo generar el resumen. Intentá nuevamente."); return; }
        //   lblContextoGeneral.Text   = resumen.ContextoGeneral;
        //   lblEvolucion.Text         = resumen.Evolucion;
        //   lblTemasRecurrentes.Text  = resumen.TemasRecurrentes;
        //   lblIntervenciones.Text    = resumen.Intervenciones;
        //   lblObservaciones.Text     = resumen.Observaciones;

        // DEMO: resumen hardcodeado
        CargarResumenDemo(nombrePaciente, periodoTexto);
        MostrarEstado(3);
    }

    // =========================================================
    // EVENTO: NUEVO RESUMEN (Estado 3 → Estado 1)
    // =========================================================
    protected void btnNuevoResumen_Click(object sender, EventArgs e)
    {
        lblMensaje.Visible = false;
        CargarFiltrosFechaDemo();
        MostrarEstado(1);
    }

    // =========================================================
    // EVENTO: GUARDAR RESUMEN
    // =========================================================
    protected void btnGuardarResumen_Click(object sender, EventArgs e)
    {
        lblMensaje.Visible = false;

        // TODO: reemplazar por:
        //   int idPaciente    = Convert.ToInt32(ddlPaciente.SelectedValue);
        //   int idProfesional = (int)Session["IdProfesional"];
        //   BE.ResumenIA resumen = new BE.ResumenIA();
        //   resumen.IdPaciente        = idPaciente;
        //   resumen.IdProfesional     = idProfesional;
        //   resumen.ContextoGeneral   = lblContextoGeneral.Text;  // BLL encripta con AES
        //   resumen.Evolucion         = lblEvolucion.Text;        // BLL encripta con AES
        //   resumen.TemasRecurrentes  = lblTemasRecurrentes.Text; // BLL encripta con AES
        //   resumen.Intervenciones    = lblIntervenciones.Text;   // BLL encripta con AES
        //   resumen.Observaciones     = lblObservaciones.Text;    // BLL encripta con AES
        //   resumen.FechaGeneracion   = DateTime.Now;
        //   bool ok = BLL.ResumenBLL.Guardar(resumen);
        //   if (ok) MostrarExito("Resumen guardado y encriptado correctamente.");
        //   else    MostrarError("No fue posible guardar el resumen.");

        MostrarExito("Resumen guardado y encriptado correctamente. Disponible para exportar en PDF.");
    }

    // =========================================================
    // DATOS DEMO — consultas por paciente y período
    // TODO: reemplazar por BLL.ConsultaBLL.ObtenerPorPacienteYPeriodo()
    // =========================================================
    private DataTable ObtenerConsultasDemo(int idPaciente, DateTime desde, DateTime hasta)
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("IdConsulta", typeof(int));
        dt.Columns.Add("Fecha", typeof(DateTime));
        dt.Columns.Add("Duracion", typeof(int));
        dt.Columns.Add("Modalidad", typeof(string));
        dt.Columns.Add("ModalidadCss", typeof(string));
        dt.Columns.Add("ResumenObjetivos", typeof(string));

        // Consultas demo del paciente 1
        var todasConsultas = new List<object[]>
        {
            new object[]{ 101, new DateTime(2026,4,15), 50, "Presencial", "presencial", "Regulación emocional ante conflicto laboral con supervisor." },
            new object[]{ 102, new DateTime(2026,3,28), 50, "Presencial", "presencial", "Trabajo sobre creencias nucleares de incompetencia." },
            new object[]{ 103, new DateTime(2026,3,10), 45, "Virtual",    "virtual",    "Psicoeducación sobre ansiedad y modelo TCC." },
            new object[]{ 104, new DateTime(2026,2,20), 50, "Presencial", "presencial", "Exploración de historia de apego con figura paterna." },
            new object[]{ 105, new DateTime(2026,1,30), 50, "Presencial", "presencial", "Registro de pensamientos automáticos negativos." },
            new object[]{ 106, new DateTime(2026,1,14), 40, "Telefónica", "telefonica", "Seguimiento breve post-crisis de ansiedad." },
        };

        foreach (var c in todasConsultas)
        {
            DateTime fechaConsulta = (DateTime)c[1];
            if (fechaConsulta >= desde && fechaConsulta <= hasta)
                dt.Rows.Add(c[0], c[1], c[2], c[3], c[4], c[5]);
        }

        return dt;
    }

    // =========================================================
    // RESUMEN DEMO (simula respuesta del módulo IA)
    // =========================================================
    private void CargarResumenDemo(string nombrePaciente, string periodo)
    {
        // Meta del resumen
        lblResumenMeta.Text = "Paciente: " + nombrePaciente + " · " + periodo;
        lblMetaPaciente.Text = nombrePaciente;
        lblMetaPeriodo.Text = periodo;
        lblMetaConsultas.Text = "6 consultas analizadas";
        lblMetaFecha.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm");

        // Contenido generado por IA (demo hardcodeado)
        lblContextoGeneral.Text =
            "Durante el período analizado, el paciente atravesó una etapa de alta carga emocional " +
            "vinculada principalmente a conflictos en su entorno laboral y a la activación de patrones " +
            "relacionales establecidos en la infancia. Las sesiones mostraron un trabajo consistente " +
            "en el marco de la Terapia Cognitivo Conductual, con foco en la identificación y " +
            "reestructuración de pensamientos automáticos negativos.";

        lblEvolucion.Text =
            "Se observa una progresión positiva en la capacidad del paciente para identificar " +
            "sus pensamientos automáticos sin actuar impulsivamente sobre ellos. En las primeras " +
            "sesiones del período predominaba la reactividad emocional; hacia el cierre del período " +
            "el paciente logra mayor distancia cognitiva ante situaciones de conflicto. " +
            "La tolerancia a la frustración mostró mejoras moderadas pero sostenidas.";

        lblTemasRecurrentes.Text =
            "• Conflicto con figuras de autoridad (supervisor laboral, padre).\n" +
            "• Creencias nucleares de incompetencia e insuficiencia.\n" +
            "• Dificultades en la regulación emocional ante críticas externas.\n" +
            "• Patrón de hipervigilancia en contextos evaluativos.\n" +
            "• Comparación negativa con pares en entornos laborales.";

        lblIntervenciones.Text =
            "• Reestructuración cognitiva sobre creencias de incompetencia (3 sesiones).\n" +
            "• Registro de pensamientos automáticos como tarea entre sesiones.\n" +
            "• Ejercicios de respiración diafragmática al inicio de cada sesión.\n" +
            "• Psicoeducación sobre el modelo ABC del pensamiento (sesión 3).\n" +
            "• Técnica de la flecha descendente para identificación de creencias nucleares.";

        lblObservaciones.Text =
            "El vínculo terapéutico se consolida de manera positiva. El paciente muestra " +
            "adherencia a las tareas entre sesiones y refiere percibir cambios concretos en " +
            "su vida cotidiana. Se sugiere continuar profundizando el trabajo sobre los esquemas " +
            "relacionales de apego en las próximas sesiones, dado que emergen como sustrato " +
            "de los conflictos actuales con figuras de autoridad.";
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
