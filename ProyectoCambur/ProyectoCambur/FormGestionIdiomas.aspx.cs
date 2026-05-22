using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;

public partial class FormGestionIdiomas : System.Web.UI.Page
{
    // =========================================================
    // ESTADO: código del idioma en edición de traducciones
    // =========================================================
    private string CodigoIdiomaActual
    {
        get { return ViewState["IdiomaActual"] != null ? ViewState["IdiomaActual"].ToString() : ""; }
        set { ViewState["IdiomaActual"] = value; }
    }

    // =========================================================
    // PAGE LOAD
    // =========================================================
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            CargarAdminDemo();
            CargarGrillaIdiomas();
        }
    }

    // =========================================================
    // ADMIN (demo)
    // TODO: reemplazar por Session["Administrador"]
    // =========================================================
    private void CargarAdminDemo()
    {
        lblNombreAdmin.Text = "Admin Sistema";
        lblIniciales.Text = "AD";
    }

    // =========================================================
    // GRILLA DE IDIOMAS
    // TODO: reemplazar por BLL.IdiomaBLL.ObtenerTodos()
    // =========================================================
    private void CargarGrillaIdiomas()
    {
        var idiomas = ObtenerIdiomasDemo();

        DataTable dt = new DataTable();
        dt.Columns.Add("Codigo", typeof(string));
        dt.Columns.Add("Nombre", typeof(string));
        dt.Columns.Add("Flag", typeof(string));
        dt.Columns.Add("Traducciones", typeof(int));
        dt.Columns.Add("Activo", typeof(bool));

        foreach (var i in idiomas)
            dt.Rows.Add(i.Codigo, i.Nombre, i.Flag, i.Traducciones, i.Activo);

        gvIdiomas.DataSource = dt;
        gvIdiomas.DataBind();

        lblTotalIdiomas.Text = idiomas.Count + " idiomas";
        lblTotalIdiomas.Visible = true;
    }

    // =========================================================
    // EVENTO: MOSTRAR PANEL DE ALTA
    // =========================================================
    protected void btnMostrarAlta_Click(object sender, EventArgs e)
    {
        lblMensaje.Visible = false;
        pnlAltaIdioma.Visible = true;
        pnlTraducciones.Visible = false;
        txtNombreIdioma.Text = string.Empty;
        txtCodigoIdioma.Text = string.Empty;
        txtFlagIdioma.Text = string.Empty;
    }

    protected void btnCancelarAlta_Click(object sender, EventArgs e)
    {
        pnlAltaIdioma.Visible = false;
    }

    // =========================================================
    // EVENTO: GUARDAR NUEVO IDIOMA (escenario principal CUS11)
    // =========================================================
    protected void btnGuardarAlta_Click(object sender, EventArgs e)
    {
        lblMensaje.Visible = false;

        if (!Page.IsValid) return;

        string nombre = txtNombreIdioma.Text.Trim();
        string codigo = txtCodigoIdioma.Text.Trim().ToUpper();
        string flag = txtFlagIdioma.Text.Trim();

        // ── Validar duplicado (flujo 7.1) ─────────────────────
        // TODO: reemplazar por:
        //   if (BLL.IdiomaBLL.ExistePorNombreOCodigo(nombre, codigo))
        bool duplicado = ValidarDuplicadoDemo(nombre, codigo);
        if (duplicado)
        {
            MostrarError("Ya existe un idioma con ese nombre o código en el sistema. " +
                         "Ingresá un idioma diferente.");
            return;
        }

        // ── Persistir el nuevo idioma con traducciones default ──
        // TODO: reemplazar por:
        //   BE.Idioma nuevoIdioma = new BE.Idioma();
        //   nuevoIdioma.Nombre = nombre;
        //   nuevoIdioma.Codigo = codigo;
        //   nuevoIdioma.Flag   = flag;
        //   nuevoIdioma.Activo = true;
        //   bool ok = BLL.IdiomaBLL.Agregar(nuevoIdioma);
        //   // BLL internamente genera todas las traducciones con clave = valor (default)
        //   // BLL.DigitoVerificadorBLL.RecalcularTabla("Idiomas");
        //   // BLL.DigitoVerificadorBLL.RecalcularTabla("Traducciones");
        //   // BLL.BitacoraBLL.Registrar(idAdmin, "Administración", "Idioma agregado: " + codigo, criticidad: 2);
        //   if (!ok) { MostrarError("No fue posible guardar el idioma. Intentá nuevamente."); return; }

        // DEMO: simular alta exitosa
        pnlAltaIdioma.Visible = false;
        CargarGrillaIdiomas();
        MostrarExito("Idioma \"" + nombre + "\" (" + codigo + ") agregado correctamente. " +
                     "Se generaron las traducciones por defecto. " +
                     "Podés editarlas desde la columna Traducciones.");
    }

    // =========================================================
    // EVENTO: COMANDOS DE LA GRILLA DE IDIOMAS
    // =========================================================
    protected void gvIdiomas_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        lblMensaje.Visible = false;

        if (e.CommandName == "VerTraducciones")
        {
            string codigo = e.CommandArgument.ToString();
            CodigoIdiomaActual = codigo;
            CargarTraducciones(codigo);
            pnlTraducciones.Visible = true;
            pnlAltaIdioma.Visible = false;
        }
        else if (e.CommandName == "ToggleActivo")
        {
            string[] args = e.CommandArgument.ToString().Split('|');
            string codigo = args[0];
            bool activo = bool.Parse(args[1]);

            // TODO: reemplazar por BLL.IdiomaBLL.CambiarEstado(codigo, !activo)
            // DEMO: actualizar grilla
            CargarGrillaIdiomas();
            string accion = activo ? "desactivado" : "activado";
            MostrarExito("Idioma \"" + codigo + "\" " + accion + " correctamente.");
        }
    }

    // =========================================================
    // TRADUCCIONES DEL IDIOMA SELECCIONADO
    // TODO: reemplazar por BLL.IdiomaBLL.ObtenerTraducciones(codigo)
    // =========================================================
    private void CargarTraducciones(string codigo)
    {
        lblIdiomaEditar.Text = ObtenerNombreIdiomaDemo(codigo) + " (" + codigo + ")";

        DataTable dt = new DataTable();
        dt.Columns.Add("Clave", typeof(string));
        dt.Columns.Add("Valor", typeof(string));

        // Traducciones demo
        var claves = new Dictionary<string, string>
        {
            { "BT_Login",          codigo == "EN" ? "Log In"          : codigo == "PT" ? "Entrar"         : "Iniciar sesión"       },
            { "BT_Logout",         codigo == "EN" ? "Log Out"         : codigo == "PT" ? "Sair"           : "Cerrar sesión"        },
            { "BT_Registrar",      codigo == "EN" ? "Register"        : codigo == "PT" ? "Registrar"      : "Registrar"            },
            { "BT_Guardar",        codigo == "EN" ? "Save"            : codigo == "PT" ? "Salvar"         : "Guardar"              },
            { "BT_Cancelar",       codigo == "EN" ? "Cancel"          : codigo == "PT" ? "Cancelar"       : "Cancelar"             },
            { "BT_Confirmar",      codigo == "EN" ? "Confirm"         : codigo == "PT" ? "Confirmar"      : "Confirmar"            },
            { "LBL_Pacientes",     codigo == "EN" ? "Patients"        : codigo == "PT" ? "Pacientes"      : "Pacientes"            },
            { "LBL_Consultas",     codigo == "EN" ? "Consultations"   : codigo == "PT" ? "Consultas"      : "Consultas"            },
            { "LBL_Historial",     codigo == "EN" ? "Clinical History": codigo == "PT" ? "Histórico"      : "Historial Clínico"    },
            { "LBL_Dashboard",     codigo == "EN" ? "Dashboard"       : codigo == "PT" ? "Painel"         : "Dashboard"            },
            { "LBL_ResumenIA",     codigo == "EN" ? "AI Summary"      : codigo == "PT" ? "Resumo IA"      : "Resumen IA"           },
            { "MSG_ErrorGeneral",  codigo == "EN" ? "An error occurred": codigo == "PT" ? "Ocorreu um erro": "Ocurrió un error"    },
            { "MSG_ExitoGuardado", codigo == "EN" ? "Saved successfully": codigo == "PT" ? "Salvo com sucesso": "Guardado correctamente" },
            { "LBL_Nombre",        codigo == "EN" ? "Name"            : codigo == "PT" ? "Nome"           : "Nombre"               },
            { "LBL_Email",         codigo == "EN" ? "Email"           : codigo == "PT" ? "Email"          : "Correo electrónico"   },
        };

        foreach (var kv in claves)
            dt.Rows.Add(kv.Key, kv.Value);

        gvTraducciones.DataSource = dt;
        gvTraducciones.DataBind();
    }

    // =========================================================
    // EVENTO: GUARDAR TRADUCCIÓN INDIVIDUAL (flujo 4.1 CUS11)
    // =========================================================
    protected void gvTraducciones_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        lblMensaje.Visible = false;

        if (e.CommandName != "GuardarTrad") return;

        int rowIndex = Convert.ToInt32(e.CommandArgument);
        GridViewRow fila = gvTraducciones.Rows[rowIndex];

        TextBox txt = (TextBox)fila.FindControl("txtTraduccion");
        HiddenField hf = (HiddenField)fila.FindControl("hfClave");
        if (txt == null || hf == null) return;

        string clave = hf.Value;
        string valor = txt.Text.Trim();

        // Validar que no esté vacía (flujo 4.1)
        if (string.IsNullOrEmpty(valor))
        {
            MostrarError("El valor de la traducción no puede estar vacío. Ingresá un texto válido.");
            return;
        }

        // TODO: reemplazar por:
        //   bool ok = BLL.IdiomaBLL.ActualizarTraduccion(CodigoIdiomaActual, clave, valor);
        //   if (!ok) { MostrarError("No fue posible guardar la traducción."); return; }
        //   BLL.DigitoVerificadorBLL.RecalcularTabla("Traducciones");
        //   BLL.BitacoraBLL.Registrar(idAdmin, "Administración",
        //       "Traducción actualizada: " + CodigoIdiomaActual + " / " + clave, criticidad: 3);

        MostrarExito("Traducción \"" + clave + "\" actualizada correctamente para " +
                     ObtenerNombreIdiomaDemo(CodigoIdiomaActual) + ".");
    }

    // =========================================================
    // PAGINACIÓN DE TRADUCCIONES
    // =========================================================
    protected void gvTraducciones_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvTraducciones.PageIndex = e.NewPageIndex;
        CargarTraducciones(CodigoIdiomaActual);
    }

    // =========================================================
    // EVENTO: CERRAR PANEL DE TRADUCCIONES
    // =========================================================
    protected void btnCerrarTrad_Click(object sender, EventArgs e)
    {
        pnlTraducciones.Visible = false;
        CodigoIdiomaActual = string.Empty;
    }

    // =========================================================
    // DATOS DEMO
    // =========================================================
    private class IdiomaDemo
    {
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public string Flag { get; set; }
        public int Traducciones { get; set; }
        public bool Activo { get; set; }
    }

    private List<IdiomaDemo> ObtenerIdiomasDemo()
    {
        return new List<IdiomaDemo>
        {
            new IdiomaDemo { Codigo="ES", Nombre="Español",   Flag="🇦🇷", Traducciones=240, Activo=true  },
            new IdiomaDemo { Codigo="EN", Nombre="English",   Flag="🇺🇸", Traducciones=240, Activo=true  },
            new IdiomaDemo { Codigo="PT", Nombre="Português", Flag="🇧🇷", Traducciones=240, Activo=true  },
            new IdiomaDemo { Codigo="FR", Nombre="Français",  Flag="🇫🇷", Traducciones=240, Activo=false },
        };
    }

    private bool ValidarDuplicadoDemo(string nombre, string codigo)
    {
        var existentes = ObtenerIdiomasDemo();
        foreach (var i in existentes)
            if (i.Nombre.Equals(nombre, StringComparison.OrdinalIgnoreCase) ||
                i.Codigo.Equals(codigo, StringComparison.OrdinalIgnoreCase))
                return true;
        return false;
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
