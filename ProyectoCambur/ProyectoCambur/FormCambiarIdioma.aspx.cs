using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;


public partial class FormCambiarIdioma : System.Web.UI.Page
{
    // =========================================================
    // MODELO INTERNO
    // =========================================================
    private class IdiomaDemo
    {
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public string Flag { get; set; }
        public bool Activo { get; set; }  // si el admin lo habilitó
        public bool EsActual { get; set; }  // si es el del profesional logueado
    }

    // =========================================================
    // PAGE LOAD
    // =========================================================
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            CargarProfesionalDemo();
            CargarIdiomaActivoDemo();
            CargarGrillaIdiomas();
        }
    }

    // =========================================================
    // PROFESIONAL (demo)
    // TODO: reemplazar por Session["Profesional"]
    // =========================================================
    private void CargarProfesionalDemo()
    {
        lblNombreProfesional.Text = "Lucía Martínez";
        lblIniciales.Text = "LM";
    }

    // =========================================================
    // IDIOMA ACTIVO DEL PROFESIONAL (demo)
    // TODO: reemplazar por:
    //   int idProfesional = (int)Session["IdProfesional"];
    //   BE.Profesional prof = BLL.ProfesionalBLL.ObtenerPorId(idProfesional);
    //   string codigoActual = prof.CodigoIdioma;
    // =========================================================
    private void CargarIdiomaActivoDemo()
    {
        lblIdiomaActivoFlag.Text = "🇦🇷";
        lblIdiomaActivoNombre.Text = "Español";
        lblIdiomaActivoCodigo.Text = "ES";
    }

    // =========================================================
    // GRILLA DE IDIOMAS DISPONIBLES (demo)
    // TODO: reemplazar por BLL.IdiomaBLL.ObtenerTodos()
    // =========================================================
    private void CargarGrillaIdiomas()
    {
        string idiomaActual = "ES"; // TODO: leer de Session o BD

        var idiomas = new List<IdiomaDemo>
        {
            new IdiomaDemo { Codigo="ES", Nombre="Español",    Flag="🇦🇷", Activo=true,  EsActual=true  },
            new IdiomaDemo { Codigo="EN", Nombre="English",    Flag="🇺🇸", Activo=true,  EsActual=false },
            new IdiomaDemo { Codigo="PT", Nombre="Português",  Flag="🇧🇷", Activo=true,  EsActual=false },
            new IdiomaDemo { Codigo="FR", Nombre="Français",   Flag="🇫🇷", Activo=false, EsActual=false },
        };

        DataTable dt = new DataTable();
        dt.Columns.Add("Codigo", typeof(string));
        dt.Columns.Add("Nombre", typeof(string));
        dt.Columns.Add("Flag", typeof(string));
        dt.Columns.Add("Activo", typeof(bool));
        dt.Columns.Add("EsActual", typeof(bool));

        foreach (var i in idiomas)
            dt.Rows.Add(i.Codigo, i.Nombre, i.Flag, i.Activo, i.EsActual);

        rptIdiomas.DataSource = dt;
        rptIdiomas.DataBind();
    }

    // =========================================================
    // EVENTO: CONFIRMAR CAMBIO DE IDIOMA (CUS03)
    // =========================================================
    protected void btnGuardar_Click(object sender, EventArgs e)
    {
        lblMensaje.Visible = false;

        string nuevoCodigo = hfIdiomaSeleccionado.Value;

        if (string.IsNullOrEmpty(nuevoCodigo))
        {
            MostrarError("Seleccioná un idioma antes de confirmar.");
            return;
        }

        string idiomaActual = "ES"; // TODO: leer de BD

        if (nuevoCodigo == idiomaActual)
        {
            MostrarError("El idioma seleccionado ya es el que tenés configurado actualmente.");
            return;
        }

        // ── Validar que el idioma siga activo (flujo 4.1) ─────
        // TODO: reemplazar por:
        //   BE.Idioma idioma = BLL.IdiomaBLL.ObtenerPorCodigo(nuevoCodigo);
        //   if (idioma == null || !idioma.Activo)
        //   { MostrarError("El idioma seleccionado no se encuentra disponible."); return; }
        bool idiomaActivo = ValidarIdiomaActivoDemo(nuevoCodigo);
        if (!idiomaActivo)
        {
            MostrarError("El idioma seleccionado no se encuentra disponible. Seleccioná otro idioma.");
            CargarGrillaIdiomas();
            return;
        }

        // ── Persistir preferencia (paso 5) ────────────────────
        // TODO: reemplazar por:
        //   int idProfesional = (int)Session["IdProfesional"];
        //   bool ok = BLL.ProfesionalBLL.ActualizarIdioma(idProfesional, nuevoCodigo);
        //   if (!ok) { MostrarError("No fue posible guardar el cambio. Intentá nuevamente."); return; }
        //   BLL.DigitoVerificadorBLL.RecalcularPorProfesional(idProfesional);
        //   BLL.BitacoraBLL.Registrar(idProfesional, "Configuración", "Cambio de idioma a " + nuevoCodigo, criticidad: 3);
        //   Session["Idioma"] = nuevoCodigo;
        //   Response.Redirect(Request.RawUrl + "?idioma=ok"); // recarga con nuevo idioma

        // DEMO: actualizar el label de idioma activo y recargar grilla
        ActualizarIdiomaActivoDemo(nuevoCodigo);
        MostrarExito("Idioma actualizado correctamente a " + ObtenerNombreIdiomaDemo(nuevoCodigo) +
                     ". En producción la interfaz se recargará automáticamente.");
        CargarGrillaIdiomas();
    }

    // =========================================================
    // HELPERS DEMO
    // =========================================================
    private bool ValidarIdiomaActivoDemo(string codigo)
    {
        return codigo != "FR"; // FR está desactivado en demo
    }

    private string ObtenerNombreIdiomaDemo(string codigo)
    {
        switch (codigo)
        {
            case "ES": return "Español";
            case "EN": return "English";
            case "PT": return "Português";
            case "FR": return "Français";
            default: return codigo;
        }
    }

    private void ActualizarIdiomaActivoDemo(string codigo)
    {
        string flag = codigo == "EN" ? "🇺🇸"
                    : codigo == "PT" ? "🇧🇷"
                    : codigo == "FR" ? "🇫🇷"
                    : "🇦🇷";
        lblIdiomaActivoFlag.Text = flag;
        lblIdiomaActivoNombre.Text = ObtenerNombreIdiomaDemo(codigo);
        lblIdiomaActivoCodigo.Text = codigo;
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
