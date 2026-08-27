using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class FormLineaTemporal : System.Web.UI.Page
{
    private string FiltroTipo
    {
        get { return ViewState["FiltroTipo"] != null ? ViewState["FiltroTipo"].ToString() : "TODOS"; }
        set { ViewState["FiltroTipo"] = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            CargarProfesionalDemo();
            CargarPacienteDemo();
            txtDesde.Text = new DateTime(2025, 1, 1).ToString("yyyy-MM-dd");
            txtHasta.Text = DateTime.Today.ToString("yyyy-MM-dd");
            CargarTimeline("TODOS", null, null);
        }
    }

    private void CargarProfesionalDemo()
    {
        lblNombreProfesional.Text = "Lucía Martínez";
        lblIniciales.Text = "LM";
    }

    private void CargarPacienteDemo()
    {
        lblHeaderPaciente.Text = "Línea Temporal — Martín González";
        lblPacienteIniciales.Text = "MG";
        lblPacienteNombre.Text = "Martín González";
        lblPacienteEdad.Text = "33 años";
        lblPacienteEstado.Text = "Soltero/a";
        lblPacienteOcup.Text = "Docente";
    }

    private List<EventoTimeline> ObtenerEventosDemo()
    {
        return new List<EventoTimeline>
        {
            new EventoTimeline {
                IdEvento  = 101,
                Tipo      = "CONSULTA",
                TipoLabel = "Consulta",
                TipoCss   = "consulta",
                Icono     = "🗒️",
                Fecha     = new DateTime(2026, 5, 8),
                Resumen   = "Cierre de ciclo y evaluación de avances terapéuticos.",
                Detalle   = "El paciente refiere sentirse más estable emocionalmente. " +
                            "Se realizó una revisión de los objetivos planteados al inicio del tratamiento. " +
                            "Se evidencian mejoras en la regulación emocional y en la identificación de pensamientos automáticos.",
                Duracion  = 50,
                Modalidad = "Presencial"
            },
            new EventoTimeline {
                IdEvento  = 100,
                Tipo      = "EVENTO",
                TipoLabel = "Evento clínico",
                TipoCss   = "evento",
                Icono     = "⚡",
                Fecha     = new DateTime(2026, 4, 20),
                Resumen   = "Crisis de ansiedad reportada por el paciente entre sesiones.",
                Detalle   = "El paciente contactó por mensaje refiriendo un episodio de ansiedad aguda " +
                            "luego de una reunión conflictiva con su supervisor. " +
                            "Se acordó una sesión de seguimiento telefónico al día siguiente.",
                Duracion  = 0,
                Modalidad = ""
            },
            new EventoTimeline {
                IdEvento  = 99,
                Tipo      = "CONSULTA",
                TipoLabel = "Consulta",
                TipoCss   = "consulta",
                Icono     = "🗒️",
                Fecha     = new DateTime(2026, 4, 15),
                Resumen   = "Regulación emocional ante conflicto laboral con supervisor.",
                Detalle   = "El paciente llega visiblemente ansioso. Se trabajó reestructuración cognitiva " +
                            "sobre la interpretación del conflicto. Ejercicio de respiración diafragmática al inicio. " +
                            "Al cierre refiere sentirse más tranquilo.",
                Duracion  = 50,
                Modalidad = "Presencial"
            },
            new EventoTimeline {
                IdEvento  = 98,
                Tipo      = "HISTORIAL",
                TipoLabel = "Historial clínico",
                TipoCss   = "historial",
                Icono     = "📋",
                Fecha     = new DateTime(2026, 3, 15),
                Resumen   = "Registro de antecedentes médicos: hipertensión leve y alergia a la penicilina.",
                Detalle   = "Se completó la sección de antecedentes médicos del historial clínico. " +
                            "El paciente refiere diagnóstico de hipertensión leve desde los 30 años " +
                            "con seguimiento cardiológico anual. Sin medicación psiquiátrica actual.",
                Duracion  = 0,
                Modalidad = ""
            },
            new EventoTimeline {
                IdEvento  = 97,
                Tipo      = "CONSULTA",
                TipoLabel = "Consulta",
                TipoCss   = "consulta",
                Icono     = "🗒️",
                Fecha     = new DateTime(2026, 3, 10),
                Resumen   = "Psicoeducación sobre el modelo TCC y ansiedad.",
                Detalle   = "Sesión dedicada a la psicoeducación. Se explicó el modelo ABC del pensamiento " +
                            "y la relación entre situaciones, pensamientos, emociones y conductas. " +
                            "El paciente mostró buena comprensión y actitud receptiva.",
                Duracion  = 45,
                Modalidad = "Virtual"
            },
            new EventoTimeline {
                IdEvento  = 96,
                Tipo      = "CONSULTA",
                TipoLabel = "Consulta",
                TipoCss   = "consulta",
                Icono     = "🗒️",
                Fecha     = new DateTime(2026, 2, 20),
                Resumen   = "Exploración de historia de apego con figura paterna.",
                Detalle   = "Se inició la exploración de los vínculos tempranos. El paciente refiere una " +
                            "relación distante con el padre desde la separación. Se identificó un patrón " +
                            "de activación del esquema de abandono ante situaciones de conflicto con figuras masculinas.",
                Duracion  = 50,
                Modalidad = "Presencial"
            },
            new EventoTimeline {
                IdEvento  = 95,
                Tipo      = "HISTORIAL",
                TipoLabel = "Historial clínico",
                TipoCss   = "historial",
                Icono     = "📋",
                Fecha     = new DateTime(2026, 1, 20),
                Resumen   = "Registro de contexto familiar: padres separados, vínculo estrecho con la madre.",
                Detalle   = "Se completó la sección de contexto familiar. Padres separados cuando tenía 8 años. " +
                            "Vive solo desde los 25 años. Relación distante con el padre. " +
                            "Contacto diario con la madre. Hermano mayor con poco contacto.",
                Duracion  = 0,
                Modalidad = ""
            },
            new EventoTimeline {
                IdEvento  = 94,
                Tipo      = "CONSULTA",
                TipoLabel = "Consulta",
                TipoCss   = "consulta",
                Icono     = "🗒️",
                Fecha     = new DateTime(2025, 12, 10),
                Resumen   = "Primera sesión — motivo de consulta y encuadre terapéutico.",
                Detalle   = "Primera sesión. El paciente refiere angustia y dificultades en el ámbito laboral. " +
                            "Se estableció el encuadre terapéutico y se acordaron los objetivos iniciales del tratamiento. " +
                            "Se propuso abordaje desde TCC.",
                Duracion  = 60,
                Modalidad = "Presencial"
            },
        };
    }

    private void CargarTimeline(string tipo, DateTime? desde, DateTime? hasta)
    {
        var todos = ObtenerEventosDemo();
        var filtrados = new List<EventoTimeline>();

        foreach (var ev in todos)
        {
            bool pasaTipo = (tipo == "TODOS" || ev.Tipo == tipo);
            bool pasaDesde = !desde.HasValue || ev.Fecha >= desde.Value;
            bool pasaHasta = !hasta.HasValue || ev.Fecha <= hasta.Value;
            if (pasaTipo && pasaDesde && pasaHasta)
                filtrados.Add(ev);
        }
        var consultas = todos.FindAll(x => x.Tipo == "CONSULTA");
        lblStatConsultas.Text = consultas.Count.ToString();

        if (consultas.Count > 0)
        {
            var primera = consultas[consultas.Count - 1].Fecha;
            var ultima = consultas[0].Fecha;
            lblStatInicio.Text = primera.ToString("dd/MM/yyyy");
            lblStatUltima.Text = ultima.ToString("dd/MM/yyyy");
            int meses = ((ultima.Year - primera.Year) * 12) + ultima.Month - primera.Month;
            lblStatMeses.Text = meses.ToString();
        }

        if (filtrados.Count == 0)
        {
            lblSinRegistros.Visible = true;
            rptTimeline.DataSource = null;
            rptTimeline.DataBind();
            lblTotalEventos.Text = "Sin registros para los filtros seleccionados.";
            return;
        }

        lblSinRegistros.Visible = false;
        lblTotalEventos.Text = filtrados.Count + " registro" + (filtrados.Count != 1 ? "s" : "") + " encontrado" + (filtrados.Count != 1 ? "s" : "");
        for (int i = 0; i < filtrados.Count; i++)
            filtrados[i].LadoCss = (i % 2 == 0) ? "der" : "izq";
        DataTable dt = new DataTable();
        dt.Columns.Add("IdEvento", typeof(int));
        dt.Columns.Add("Tipo", typeof(string));
        dt.Columns.Add("TipoLabel", typeof(string));
        dt.Columns.Add("TipoCss", typeof(string));
        dt.Columns.Add("Icono", typeof(string));
        dt.Columns.Add("Fecha", typeof(DateTime));
        dt.Columns.Add("Resumen", typeof(string));
        dt.Columns.Add("Detalle", typeof(string));
        dt.Columns.Add("Duracion", typeof(int));
        dt.Columns.Add("Modalidad", typeof(string));
        dt.Columns.Add("LadoCss", typeof(string));

        foreach (var ev in filtrados)
            dt.Rows.Add(ev.IdEvento, ev.Tipo, ev.TipoLabel, ev.TipoCss,
                        ev.Icono, ev.Fecha, ev.Resumen, ev.Detalle,
                        ev.Duracion, ev.Modalidad, ev.LadoCss);

        rptTimeline.DataSource = dt;
        rptTimeline.DataBind();

        ActualizarBotonesFiltro(tipo);
    }

    private void ActualizarBotonesFiltro(string tipo)
    {
        btnFiltroTodos.CssClass = "filtro-btn" + (tipo == "TODOS" ? " active" : "");
        btnFiltroConsulta.CssClass = "filtro-btn" + (tipo == "CONSULTA" ? " active" : "");
        btnFiltroHistorial.CssClass = "filtro-btn" + (tipo == "HISTORIAL" ? " active" : "");
        btnFiltroEvento.CssClass = "filtro-btn" + (tipo == "EVENTO" ? " active" : "");
    }
    protected void btnFiltro_Click(object sender, EventArgs e)
    {
        Button btn = (Button)sender;
        string tipo = btn.CommandArgument;
        FiltroTipo = tipo;

        DateTime? desde = null, hasta = null;
        DateTime d, h;
        if (DateTime.TryParse(txtDesde.Text, out d)) desde = d;
        if (DateTime.TryParse(txtHasta.Text, out h)) hasta = h;

        CargarTimeline(tipo, desde, hasta);
    }
    protected void btnAplicarFecha_Click(object sender, EventArgs e)
    {
        DateTime? desde = null, hasta = null;
        DateTime d, h;
        if (DateTime.TryParse(txtDesde.Text, out d)) desde = d;
        if (DateTime.TryParse(txtHasta.Text, out h)) hasta = h;

        if (desde.HasValue && hasta.HasValue && desde.Value > hasta.Value)
        {
            MostrarError("La fecha de inicio debe ser anterior a la fecha de fin.");
            return;
        }

        CargarTimeline(FiltroTipo, desde, hasta);
    }
    protected void rptTimeline_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
    }

    private void MostrarError(string msg)
    {
        lblMensaje.Text = msg;
        lblMensaje.CssClass = "server-error";
        lblMensaje.Visible = true;
    }

    private class EventoTimeline
    {
        public int IdEvento { get; set; }
        public string Tipo { get; set; } 
        public string TipoLabel { get; set; }
        public string TipoCss { get; set; }  
        public string Icono { get; set; }
        public DateTime Fecha { get; set; }
        public string Resumen { get; set; }
        public string Detalle { get; set; }
        public int Duracion { get; set; }
        public string Modalidad { get; set; }
        public string LadoCss { get; set; }  
    }
}
