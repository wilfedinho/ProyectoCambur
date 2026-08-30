using BE;
using BLL;
using SERVICIOS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web.UI.WebControls;
using GUI;

public partial class FormDashboard : PaginaBase
{
    private string PeriodoActivo
    {
        get { return ViewState["Periodo"] != null ? ViewState["Periodo"].ToString() : GestorDashboard.PERIODO_MES; }
        set { ViewState["Periodo"] = value; }
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
        if (!gestorPermiso.TienePermiso(psicologoActual.RolPermiso, "acceder_dashboard"))
        {
            DenegarAcceso();
            return;
        }

        AplicarTraducciones();

        if (!IsPostBack)
        {
            lblBienvenida.Text = string.Format(Traducir("lbl_bienvenida"), psicologoActual.Nombre);

            CultureInfo cultura = new CultureInfo("es-AR");
            string fecha = DateTime.Today.ToString("dddd d 'de' MMMM 'de' yyyy", cultura);
            lblFechaHoy.Text = char.ToUpper(fecha[0]) + fecha.Substring(1);

            try
            {
                CargarDashboard(GestorDashboard.PERIODO_MES);
            }
            catch (Exception)
            {
                MostrarError(Traducir("error_dashboard_recuperacion"));
            }
        }
    }

    private void AplicarTraducciones()
    {
        lblMenuCerrarSesionSidebar.Text = Traducir("menu_cerrar_sesion");
        lblHeaderPagina.Text = Traducir("nav_dashboard");

        btnSemana.Text = Traducir("periodo_semana");
        btnMes.Text = Traducir("periodo_mes");
        btnTrimestre.Text = Traducir("periodo_trimestre");
        btnAnio.Text = Traducir("periodo_anio");

        lblLabelTotalPacientes.Text = Traducir("kpi_total_pacientes");
        lblLabelNuevosPacientes.Text = Traducir("kpi_nuevos_pacientes");
        lblLabelConsultas.Text = Traducir("kpi_consultas_realizadas");
        lblLabelDerivaciones.Text = Traducir("kpi_derivaciones");
        lblLabelResumenes.Text = Traducir("kpi_resumenes_ia");
        lblLabelPerfilaciones.Text = Traducir("kpi_perfilaciones");
        lblLabelExportaciones.Text = Traducir("kpi_informes_exportados");

        lblGraficoTitulo.Text = Traducir("titulo_actividad_mensual");
        lblUltimasTitulo.Text = Traducir("titulo_ultimas_consultas");
        lnkVerTodasConsultas.Text = Traducir("lbl_ver_todas");
        gvUltimasConsultas.Columns[0].HeaderText = Traducir("th_paciente");
        gvUltimasConsultas.Columns[1].HeaderText = Traducir("th_fecha");
        gvUltimasConsultas.Columns[2].HeaderText = Traducir("th_duracion");
        gvUltimasConsultas.EmptyDataText = Traducir("msg_sin_consultas");

        lblPacientesActivosTitulo.Text = Traducir("titulo_pacientes_activos");
        lnkVerTodosPacientes.Text = Traducir("lbl_ver_todos");
    }

    private void CargarDashboard(string periodo)
    {
        PeriodoActivo = periodo;
        ActualizarBotonesPeriodo(periodo);

        Psicologo psicologoActual = GestorSesion.PsicologoActual;
        GestorDashboard gestorDashboard = new GestorDashboard();

        DatosDashboard datos = gestorDashboard.ObtenerIndicadores(psicologoActual.IdPsicologo, periodo);

        lblKpiTotalPacientes.Text = datos.TotalPacientes.ToString();
        lblKpiNuevosPacientes.Text = datos.NuevosPacientes.ToString();
        lblKpiConsultas.Text = datos.Consultas.ToString();
        lblKpiDerivaciones.Text = datos.Derivaciones.ToString();
        lblKpiDeltaPacientes.Text = datos.DeltaPacientes;
        lblKpiDeltaNuevos.Text = datos.DeltaNuevos;
        lblKpiDeltaConsultas.Text = datos.DeltaConsultas;
        lblKpiDeltaDeriv.Text = datos.DeltaDerivaciones;

        lblKpiResumenes.Text = datos.ResumenesIA.ToString();
        lblKpiPerfilaciones.Text = datos.Perfilaciones.ToString();
        lblKpiExportaciones.Text = datos.Exportaciones.ToString();
        lblNotaPerfilaciones.Text = datos.NotaPerfilaciones;
        lblNotaPerfilaciones.Visible = true;

        lblGraficoSubtitulo.Text = string.Format(Traducir("lbl_grafico_subtitulo"), 6);
        CargarGrafico(psicologoActual.IdPsicologo);
        CargarUltimasConsultas(psicologoActual.IdPsicologo);
        CargarPacientesActivos(psicologoActual.IdPsicologo);
    }

    private void CargarGrafico(int idPsicologo)
    {
        GestorDashboard gestorDashboard = new GestorDashboard();
        List<PuntoGrafico> puntos = gestorDashboard.ObtenerGraficoConsultasPorMes(idPsicologo);

        rptGrafico.DataSource = puntos;
        rptGrafico.DataBind();
    }

    private void CargarUltimasConsultas(int idPsicologo)
    {
        GestorDashboard gestorDashboard = new GestorDashboard();
        GestorPaciente gestorPaciente = new GestorPaciente();

        List<Consulta> ultimas = gestorDashboard.ObtenerUltimasConsultas(idPsicologo);
        Dictionary<int, string> nombresPaciente = gestorPaciente.ObtenerPorPsicologo(idPsicologo, soloActivos: false)
            .ToDictionary(p => p.IdPaciente, p => p.Nombre + " " + p.Apellido);

        DataTable dt = new DataTable();
        dt.Columns.Add("IdConsulta", typeof(int));
        dt.Columns.Add("Paciente", typeof(string));
        dt.Columns.Add("Fecha", typeof(DateTime));
        dt.Columns.Add("Duracion", typeof(int));

        foreach (Consulta c in ultimas)
        {
            string nombre = nombresPaciente.ContainsKey(c.IdPaciente) ? nombresPaciente[c.IdPaciente] : "—";
            dt.Rows.Add(c.IdConsulta, nombre, c.FechaConsulta, c.TiempoConsulta);
        }

        gvUltimasConsultas.DataSource = dt;
        gvUltimasConsultas.DataBind();
    }

    private void CargarPacientesActivos(int idPsicologo)
    {
        GestorDashboard gestorDashboard = new GestorDashboard();
        GestorConsulta gestorConsulta = new GestorConsulta();

        List<Paciente> activos = gestorDashboard.ObtenerPacientesActivos(idPsicologo);
        List<Consulta> todasConsultas = gestorConsulta.ObtenerPorPsicologo(idPsicologo);

        DataTable dt = new DataTable();
        dt.Columns.Add("IdPaciente", typeof(int));
        dt.Columns.Add("Nombre", typeof(string));
        dt.Columns.Add("Iniciales", typeof(string));
        dt.Columns.Add("UltimaConsulta", typeof(string));

        foreach (Paciente p in activos)
        {
            DateTime? ultima = gestorDashboard.ObtenerUltimaConsultaDe(p.IdPaciente, todasConsultas);
            string textoUltima = ultima.HasValue
                ? Traducir("lbl_ult_sesion") + ": " + ultima.Value.ToString("dd/MM/yyyy")
                : Traducir("lbl_sin_sesiones");
            dt.Rows.Add(p.IdPaciente, p.Nombre + " " + p.Apellido, ObtenerIniciales(p.Nombre, p.Apellido), textoUltima);
        }

        rptPacientesActivos.DataSource = dt;
        rptPacientesActivos.DataBind();

        lblBadgePacientesActivos.Text = dt.Rows.Count + " " + Traducir("lbl_activos");
        lblBadgePacientesActivos.Visible = true;
    }

    private void ActualizarBotonesPeriodo(string periodo)
    {
        btnSemana.CssClass = "periodo-btn" + (periodo == GestorDashboard.PERIODO_SEMANA ? " active" : "");
        btnMes.CssClass = "periodo-btn" + (periodo == GestorDashboard.PERIODO_MES ? " active" : "");
        btnTrimestre.CssClass = "periodo-btn" + (periodo == GestorDashboard.PERIODO_TRIMESTRE ? " active" : "");
        btnAnio.CssClass = "periodo-btn" + (periodo == GestorDashboard.PERIODO_ANIO ? " active" : "");
    }

    protected void btnPeriodo_Click(object sender, EventArgs e)
    {
        Button btn = (Button)sender;
        try
        {
            CargarDashboard(btn.CommandArgument);
        }
        catch (Exception)
        {
            MostrarError(Traducir("error_dashboard_recalculo"));
        }
    }

    private string ObtenerIniciales(string nombre, string apellido)
    {
        string i1 = !string.IsNullOrEmpty(nombre) ? nombre.Substring(0, 1) : "";
        string i2 = !string.IsNullOrEmpty(apellido) ? apellido.Substring(0, 1) : "";
        return (i1 + i2).ToUpper();
    }

    private void MostrarError(string mensaje)
    {
        lblMensaje.Text = mensaje;
        lblMensaje.CssClass = "server-error";
        lblMensaje.Visible = true;
    }
}