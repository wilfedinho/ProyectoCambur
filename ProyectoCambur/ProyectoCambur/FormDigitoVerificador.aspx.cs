using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class FormDigitoVerificador : System.Web.UI.Page
{
    // =========================================================
    // MODELOS INTERNOS
    // =========================================================
    private class TablaInfo
    {
        public string Nombre { get; set; }
        public int Registros { get; set; }
    }

    private class Inconsistencia
    {
        public string Tabla { get; set; }
        public string Tipo { get; set; }
        public string TipoCss { get; set; }
        public string IdRegistro { get; set; }
        public string Detalle { get; set; }
    }

    private class HistorialDV
    {
        public DateTime Fecha { get; set; }
        public string Resultado { get; set; }
        public string ResultadoIcono { get; set; }
        public string ResultadoCss { get; set; }
    }

    // =========================================================
    // PAGE LOAD
    // =========================================================
    protected void Page_Load(object sender, EventArgs e)
    {
        Page.UnobtrusiveValidationMode = UnobtrusiveValidationMode.None;
        if (!IsPostBack)
        {
            CargarAdminDemo();
            CargarTablasDemo();
            CargarHistorialDVDemo();
            MostrarEstado("inicial");
        }
    }

    // =========================================================
    // ADMIN (demo)
    // TODO: reemplazar por Session["Administrador"]
    // =========================================================
    private void CargarAdminDemo()
    {
        lblNombreAdmin.Text = "Web Master";
        lblIniciales.Text = "WM";
    }

    // =========================================================
    // CONTROL DE ESTADOS
    // =========================================================
    private void MostrarEstado(string estado)
    {
        pnlInicial.Visible = estado == "inicial";
        pnlResultadoOk.Visible = estado == "ok";
        pnlResultadoError.Visible = estado == "error";
    }

    // =========================================================
    // TABLAS DEL SISTEMA (demo)
    // TODO: reemplazar por BLL.DigitoVerificadorBLL.ObtenerTablas()
    // =========================================================
    private void CargarTablasDemo()
    {
        var tablas = new List<TablaInfo>
        {
            new TablaInfo { Nombre = "Profesionales",  Registros = 18  },
            new TablaInfo { Nombre = "Pacientes",       Registros = 87  },
            new TablaInfo { Nombre = "Consultas",       Registros = 342 },
            new TablaInfo { Nombre = "Historiales",     Registros = 84  },
            new TablaInfo { Nombre = "ResumenesIA",     Registros = 56  },
            new TablaInfo { Nombre = "Derivaciones",    Registros = 12  },
            new TablaInfo { Nombre = "Perfilaciones",   Registros = 28  },
            new TablaInfo { Nombre = "Suscripciones",   Registros = 18  },
            new TablaInfo { Nombre = "Idiomas",         Registros = 4   },
            new TablaInfo { Nombre = "Traducciones",    Registros = 240 },
            new TablaInfo { Nombre = "Bitacora",        Registros = 892 },
            new TablaInfo { Nombre = "Exportaciones",   Registros = 74  },
        };

        DataTable dt = new DataTable();
        dt.Columns.Add("Nombre", typeof(string));
        dt.Columns.Add("Registros", typeof(int));
        foreach (var t in tablas)
            dt.Rows.Add(t.Nombre, t.Registros);

        rptTablas.DataSource = dt;
        rptTablas.DataBind();
    }

    // =========================================================
    // HISTORIAL DE VERIFICACIONES (demo)
    // TODO: reemplazar por BLL.BitacoraBLL.ObtenerPorModulo("DigitoVerificador", 5)
    // =========================================================
    private void CargarHistorialDVDemo()
    {
        var historial = new List<HistorialDV>
        {
            new HistorialDV { Fecha=new DateTime(2026,5,19,8,35,0),  Resultado="Sin inconsistencias", ResultadoIcono="●", ResultadoCss="ok"  },
            new HistorialDV { Fecha=new DateTime(2026,5,15,9,00,0),  Resultado="Sin inconsistencias", ResultadoIcono="●", ResultadoCss="ok"  },
            new HistorialDV { Fecha=new DateTime(2026,5,10,11,20,0), Resultado="2 inconsistencias",   ResultadoIcono="●", ResultadoCss="err" },
            new HistorialDV { Fecha=new DateTime(2026,5,08,8,45,0),  Resultado="Sin inconsistencias", ResultadoIcono="●", ResultadoCss="ok"  },
        };

        DataTable dt = new DataTable();
        dt.Columns.Add("Fecha", typeof(DateTime));
        dt.Columns.Add("Resultado", typeof(string));
        dt.Columns.Add("ResultadoIcono", typeof(string));
        dt.Columns.Add("ResultadoCss", typeof(string));

        foreach (var h in historial)
            dt.Rows.Add(h.Fecha, h.Resultado, h.ResultadoIcono, h.ResultadoCss);

        rptHistorialDV.DataSource = dt;
        rptHistorialDV.DataBind();
    }

    // =========================================================
    // EVENTO: INICIAR VERIFICACIÓN (CUS07)
    // =========================================================
    protected void btnRecalcular_Click(object sender, EventArgs e)
    {
        lblMensaje.Visible = false;

        DateTime inicio = DateTime.Now;

        try
        {
            // TODO: reemplazar todo este bloque por:
            //   BLL.DigitoVerificadorBLL.ResultadoVerificacion resultado =
            //       BLL.DigitoVerificadorBLL.VerificarSistema();
            //
            //   BLL.BitacoraBLL.Registrar(idAdmin, "DigitoVerificador",
            //       "Verificación de integridad ejecutada. " +
            //       (resultado.Inconsistencias.Count == 0
            //           ? "Sin inconsistencias."
            //           : resultado.Inconsistencias.Count + " inconsistencias detectadas."),
            //       criticidad: resultado.Inconsistencias.Count > 0 ? 1 : 3);
            //
            //   if (resultado.Inconsistencias.Count == 0)
            //       MostrarResultadoOk(resultado.TablasVerificadas, resultado.RegistrosAnalizados, resultado.TiempoMs);
            //   else
            //       MostrarResultadoError(resultado.Inconsistencias);

            // DEMO: simular verificación (alterna entre OK y con error según el toggle de Session)
            bool simularError = Session["DV_SimularError"] != null && (bool)Session["DV_SimularError"];
            Session["DV_SimularError"] = !simularError;

            // ── FLUJO ALTERNATIVO 2.1: Error al recuperar registros ──
            // TODO: si BLL lanza excepción → catch abajo

            TimeSpan duracion = DateTime.Now - inicio;
            int tiempoMs = (int)duracion.TotalMilliseconds + 850; // simular tiempo real

            if (simularError)
            {
                // Simular inconsistencias detectadas (flujos 3.1, 4.1, 4.2)
                MostrarResultadoErrorDemo(tiempoMs);
            }
            else
            {
                // Sin inconsistencias
                MostrarResultadoOk(12, 1855, tiempoMs);
            }

            CargarHistorialDVDemo();
        }
        catch (Exception ex)
        {
            // Flujo 2.1: fallo al recuperar registros de una tabla
            MostrarError("No fue posible completar la verificación debido a un error técnico. " +
                         "Verificá la conectividad con la base de datos y reintentá. " +
                         "Detalle: " + ex.Message);
            MostrarEstado("inicial");
        }
    }

    // =========================================================
    // MOSTRAR RESULTADO: SIN INCONSISTENCIAS
    // =========================================================
    private void MostrarResultadoOk(int tablas, int registros, int tiempoMs)
    {
        MostrarEstado("ok");
        lblStatTablas.Text = tablas.ToString();
        lblStatRegistros.Text = registros.ToString("N0").Replace(",", ".");
        lblStatTiempo.Text = (tiempoMs / 1000.0).ToString("0.0") + "s";
    }

    // =========================================================
    // MOSTRAR RESULTADO: CON INCONSISTENCIAS (demo)
    // =========================================================
    private void MostrarResultadoErrorDemo(int tiempoMs)
    {
        MostrarEstado("error");

        var inconsistencias = new List<Inconsistencia>
        {
            // Flujo 3.1: dígito horizontal alterado
            new Inconsistencia
            {
                Tabla      = "Consultas",
                Tipo       = "DV Horizontal",
                TipoCss    = "horizontal",
                IdRegistro = "142",
                Detalle    = "El hash recalculado del registro no coincide con el almacenado. Posible modificación directa en BD."
            },
            // Flujo 4.1: registros faltantes (dígito vertical)
            new Inconsistencia
            {
                Tabla      = "Pacientes",
                Tipo       = "DV Vertical (falta)",
                TipoCss    = "vertical-falta",
                IdRegistro = "—",
                Detalle    = "Se esperaban 87 registros pero se encontraron 86. Posible eliminación fuera del flujo controlado."
            },
            // Flujo 4.2: registros no esperados (dígito vertical)
            new Inconsistencia
            {
                Tabla      = "Bitacora",
                Tipo       = "DV Vertical (extra)",
                TipoCss    = "vertical-extra",
                IdRegistro = "—",
                Detalle    = "Se esperaban 891 registros pero se encontraron 892. Posible inserción directa en BD."
            },
        };

        lblResumenError.Text = inconsistencias.Count +
            " inconsistencia" + (inconsistencias.Count != 1 ? "s" : "") +
            " detectada" + (inconsistencias.Count != 1 ? "s" : "") +
            " en " + "2 tablas afectadas.";

        DataTable dt = new DataTable();
        dt.Columns.Add("Tabla", typeof(string));
        dt.Columns.Add("Tipo", typeof(string));
        dt.Columns.Add("TipoCss", typeof(string));
        dt.Columns.Add("IdRegistro", typeof(string));
        dt.Columns.Add("Detalle", typeof(string));

        foreach (var inc in inconsistencias)
            dt.Rows.Add(inc.Tabla, inc.Tipo, inc.TipoCss, inc.IdRegistro, inc.Detalle);

        gvInconsistencias.DataSource = dt;
        gvInconsistencias.DataBind();
    }

    // =========================================================
    // EVENTO: NUEVA VERIFICACIÓN
    // =========================================================
    protected void btnNuevaVerificacion_Click(object sender, EventArgs e)
    {
        lblMensaje.Visible = false;
        MostrarEstado("inicial");
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
}
