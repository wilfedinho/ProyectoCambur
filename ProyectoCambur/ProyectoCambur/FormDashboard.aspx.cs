using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Globalization;

public partial class FormDashboard : System.Web.UI.Page
{
    private string PeriodoActivo
    {
        get { return ViewState["Periodo"] != null ? ViewState["Periodo"].ToString() : "MES"; }
        set { ViewState["Periodo"] = value; }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            CargarProfesionalDemo();
            CargarDashboard("MES");
        }
    }
    private void CargarProfesionalDemo()
    {
        lblNombreProfesional.Text = "Lucía Martínez";
        lblIniciales.Text = "LM";
        lblBienvenida.Text = "Bienvenida, Lucía.";
        lblFechaHoy.Text = DateTime.Today.ToString("dddd d 'de' MMMM 'de' yyyy",
                                        new CultureInfo("es-AR"));
    }
    private void CargarDashboard(string periodo)
    {
        PeriodoActivo = periodo;
        ActualizarBotonesPeriodo(periodo);

        var datos = ObtenerDatosDemo(periodo);

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

        lblGraficoSubtitulo.Text = "Consultas por mes · últimos 6 meses";
        CargarGrafico();

        CargarUltimasConsultas();

        CargarPacientesActivos();
    }
    private void CargarGrafico()
    {
    
        var meses = new[]
        {
            new { Mes = "Diciembre",  MesCorto = "Dic", Valor = 8  },
            new { Mes = "Enero",      MesCorto = "Ene", Valor = 10 },
            new { Mes = "Febrero",    MesCorto = "Feb", Valor = 7  },
            new { Mes = "Marzo",      MesCorto = "Mar", Valor = 14 },
            new { Mes = "Abril",      MesCorto = "Abr", Valor = 12 },
            new { Mes = "Mayo",       MesCorto = "May", Valor = 9  },
        };

        int maxValor = 0;
        foreach (var m in meses) if (m.Valor > maxValor) maxValor = m.Valor;

        DataTable dt = new DataTable();
        dt.Columns.Add("Mes", typeof(string));
        dt.Columns.Add("MesCorto", typeof(string));
        dt.Columns.Add("Valor", typeof(int));
        dt.Columns.Add("PctAltura", typeof(int));
        dt.Columns.Add("EsActual", typeof(bool));

        string mesActual = DateTime.Today.ToString("MMMM", new CultureInfo("es-AR"));
       
        if (mesActual.Length > 0)
            mesActual = char.ToUpper(mesActual[0]) + mesActual.Substring(1);

        foreach (var m in meses)
        {
            int pct = maxValor > 0 ? (int)Math.Round((double)m.Valor / maxValor * 100) : 0;
            dt.Rows.Add(m.Mes, m.MesCorto, m.Valor, pct, m.Mes == mesActual);
        }

        rptGrafico.DataSource = dt;
        rptGrafico.DataBind();
    }

    private void CargarUltimasConsultas()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("IdConsulta", typeof(int));
        dt.Columns.Add("Paciente", typeof(string));
        dt.Columns.Add("Fecha", typeof(DateTime));
        dt.Columns.Add("Duracion", typeof(int));

        dt.Rows.Add(101, "Martín González", new DateTime(2026, 5, 8), 50);
        dt.Rows.Add(99, "Carlos Ibáñez", new DateTime(2026, 5, 6), 50);
        dt.Rows.Add(98, "Sofía Ramírez", new DateTime(2026, 5, 2), 50);
        dt.Rows.Add(97, "Facundo Pérez", new DateTime(2026, 4, 28), 45);
        dt.Rows.Add(96, "Valentina Moreno", new DateTime(2026, 4, 22), 50);

        gvUltimasConsultas.DataSource = dt;
        gvUltimasConsultas.DataBind();
    }

    private void CargarPacientesActivos()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("IdPaciente", typeof(int));
        dt.Columns.Add("Nombre", typeof(string));
        dt.Columns.Add("Iniciales", typeof(string));
        dt.Columns.Add("UltimaConsulta", typeof(string));

        dt.Rows.Add(1, "Martín González", "MG", "Últ. sesión: 08/05/2026");
        dt.Rows.Add(3, "Carlos Ibáñez", "CI", "Últ. sesión: 06/05/2026");
        dt.Rows.Add(2, "Sofía Ramírez", "SR", "Últ. sesión: 02/05/2026");
        dt.Rows.Add(5, "Facundo Pérez", "FP", "Últ. sesión: 28/04/2026");
        dt.Rows.Add(4, "Valentina Moreno", "VM", "Últ. sesión: 22/04/2026");

        rptPacientesActivos.DataSource = dt;
        rptPacientesActivos.DataBind();

        lblBadgePacientesActivos.Text = dt.Rows.Count + " activos";
        lblBadgePacientesActivos.Visible = true;
    }    
    private DatosDashboard ObtenerDatosDemo(string periodo)
    {
        switch (periodo)
        {
            case "SEMANA":
                return new DatosDashboard
                {
                    TotalPacientes = 18,
                    NuevosPacientes = 0,
                    Consultas = 4,
                    Derivaciones = 0,
                    ResumenesIA = 1,
                    Perfilaciones = 0,
                    Exportaciones = 1,
                    DeltaPacientes = "= Sin cambios",
                    DeltaNuevos = "Sin incorporaciones",
                    DeltaConsultas = "↑ 4 esta semana",
                    DeltaDerivaciones = "Sin derivaciones"
                };
            case "TRIMESTRE":
                return new DatosDashboard
                {
                    TotalPacientes = 18,
                    NuevosPacientes = 3,
                    Consultas = 38,
                    Derivaciones = 2,
                    ResumenesIA = 8,
                    Perfilaciones = 5,
                    Exportaciones = 6,
                    DeltaPacientes = "↑ 3 nuevos este trimestre",
                    DeltaNuevos = "↑ vs. 1 trimestre anterior",
                    DeltaConsultas = "↑ 12% vs. trimestre anterior",
                    DeltaDerivaciones = "= igual que trimestre anterior"
                };
            case "ANIO":
                return new DatosDashboard
                {
                    TotalPacientes = 18,
                    NuevosPacientes = 7,
                    Consultas = 142,
                    Derivaciones = 5,
                    ResumenesIA = 24,
                    Perfilaciones = 14,
                    Exportaciones = 18,
                    DeltaPacientes = "↑ 7 nuevos este año",
                    DeltaNuevos = "↑ vs. 4 del año anterior",
                    DeltaConsultas = "↑ 18% vs. año anterior",
                    DeltaDerivaciones = "↑ 2 más que el año pasado"
                };
            default: 
                return new DatosDashboard
                {
                    TotalPacientes = 18,
                    NuevosPacientes = 1,
                    Consultas = 14,
                    Derivaciones = 1,
                    ResumenesIA = 3,
                    Perfilaciones = 2,
                    Exportaciones = 2,
                    DeltaPacientes = "↑ 1 nuevo este mes",
                    DeltaNuevos = "↑ vs. 0 el mes pasado",
                    DeltaConsultas = "↑ 14% vs. mes anterior",
                    DeltaDerivaciones = "= igual que mes anterior"
                };
        }
    }

    private void ActualizarBotonesPeriodo(string periodo)
    {
        btnSemana.CssClass = "periodo-btn" + (periodo == "SEMANA" ? " active" : "");
        btnMes.CssClass = "periodo-btn" + (periodo == "MES" ? " active" : "");
        btnTrimestre.CssClass = "periodo-btn" + (periodo == "TRIMESTRE" ? " active" : "");
        btnAnio.CssClass = "periodo-btn" + (periodo == "ANIO" ? " active" : "");
    }

    protected void btnPeriodo_Click(object sender, EventArgs e)
    {
        System.Web.UI.WebControls.Button btn = (System.Web.UI.WebControls.Button)sender;
        CargarDashboard(btn.CommandArgument);
    }

    private class DatosDashboard
    {
        public int TotalPacientes { get; set; }
        public int NuevosPacientes { get; set; }
        public int Consultas { get; set; }
        public int Derivaciones { get; set; }
        public int ResumenesIA { get; set; }
        public int Perfilaciones { get; set; }
        public int Exportaciones { get; set; }
        public string DeltaPacientes { get; set; }
        public string DeltaNuevos { get; set; }
        public string DeltaConsultas { get; set; }
        public string DeltaDerivaciones { get; set; }
    }
}
