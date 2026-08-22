using BE;
using BLL;
using SERVICIOS;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class FormCambiarIdioma : GUI.PaginaBase
{
 
    private static readonly Dictionary<string, string> Banderas = new Dictionary<string, string>
    {
        { "es", "🇦🇷" },
        { "en", "🇺🇸" },
        { "pt", "🇧🇷" },
        { "fr", "🇫🇷" },
        { "de", "🇩🇪" },
        { "it", "🇮🇹" },
    };

    private class FilaIdioma
    {
        public string NombreIdioma { get; set; }
        public string CodigoIso { get; set; }
        public string Flag { get; set; }
        public bool Disponible { get; set; }
        public bool EsActual { get; set; }
        public string TextoNoDisponible { get; set; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!GestorSesion.EstaAutenticado)
        {
            Response.Redirect("FormLogin.aspx");
            return;
        }

        Psicologo psicologoActual = GestorSesion.PsicologoActual;

        AplicarTraducciones(psicologoActual);

        if (!IsPostBack)
        {
            lblNombreProfesional.Text = psicologoActual.Nombre + " " + psicologoActual.Apellido;
            lblIniciales.Text = ObtenerIniciales(psicologoActual.Nombre, psicologoActual.Apellido);
            lnkVolverMenu.NavigateUrl = DestinoSegunRol(psicologoActual.RolPermiso);
            lnkCancelar.NavigateUrl = DestinoSegunRol(psicologoActual.RolPermiso);

            CargarIdiomaActivo(psicologoActual);
            CargarGrillaIdiomas(psicologoActual);

            if (Request.QueryString["idioma"] == "ok")
            {
                MostrarExito(Traducir("msg_idioma_actualizado"));
            }
        }
    }

    private void AplicarTraducciones(Psicologo psicologoActual)
    {
        lblTaglineSidebar.Text = Traducir("tagline_configuracion");
        lblMenuCerrarSesion.Text = Traducir("menu_cerrar_sesion");
        lblHeaderSeccion.Text = Traducir("header_configuracion");
        lblHeaderPagina.Text = Traducir("header_cambiar_idioma");
        lblRolActual.Text = psicologoActual.RolPermiso;

        lblTituloCard.Text = Traducir("titulo_idioma_interfaz");
        lblSubtituloCard.Text = Traducir("subtitulo_idioma_interfaz");
        lblSeccionActual.Text = Traducir("seccion_idioma_actual");
        lblBadgeActivo.Text = Traducir("badge_activo");
        lblSeccionDisponibles.Text = Traducir("seccion_idiomas_disponibles");
        lnkCancelar.Text = Traducir("btn_cancelar");
        btnGuardar.Text = Traducir("btn_confirmar_cambio_idioma");
        lblTituloSeleccion.Text = Traducir("titulo_idioma_seleccionado");
        lblSinSeleccion.Text = Traducir("msg_ningun_idioma_seleccionado");
        lblAvisoInmediatoTitulo.Text = Traducir("aviso_inmediato_titulo");
        lblAvisoInmediatoTexto.Text = Traducir("aviso_inmediato_texto");
        lblAvisoClinicoTitulo.Text = Traducir("aviso_clinico_titulo");
        lblAvisoClinicoTexto.Text = Traducir("aviso_clinico_texto");
    }

    private void CargarIdiomaActivo(Psicologo psicologoActual)
    {
        string codigoIso = ObtenerCodigoIso(psicologoActual.Idioma);
        lblIdiomaActivoFlag.Text = ObtenerBandera(codigoIso);
        lblIdiomaActivoNombre.Text = psicologoActual.Idioma;
        lblIdiomaActivoCodigo.Text = codigoIso.ToUpper();
    }

    private void CargarGrillaIdiomas(Psicologo psicologoActual)
    {
        GestorIdioma gestorIdioma = new GestorIdioma();
        List<Idioma> idiomas = gestorIdioma.ObtenerTodos();

        List<FilaIdioma> filas = idiomas.Select(i => new FilaIdioma
        {
            NombreIdioma = i.NombreIdioma,
            CodigoIso = i.CodigoIso,
            Flag = ObtenerBandera(i.CodigoIso),
            Disponible = i.IsDisponible,
            EsActual = i.NombreIdioma == psicologoActual.Idioma,
            TextoNoDisponible = Traducir("idioma_no_disponible")
        }).ToList();

        rptIdiomas.DataSource = filas;
        rptIdiomas.DataBind();
    }

    protected void btnGuardar_Click(object sender, EventArgs e)
    {
        lblMensaje.Visible = false;

        string nuevoIdioma = hfIdiomaSeleccionado.Value;

        if (string.IsNullOrEmpty(nuevoIdioma))
        {
            MostrarError(Traducir("error_seleccionar_idioma"));
            return;
        }

        GestorPsicologo gestorPsicologo = new GestorPsicologo();

        try
        {
            Psicologo psicologoActualizado = gestorPsicologo.CambiarIdioma(GestorSesion.PsicologoActual.IdPsicologo, nuevoIdioma);

            
            GestorSesion.Login(psicologoActualizado);

            Response.Redirect("FormCambiarIdioma.aspx?idioma=ok");
        }
        catch (ExcepcionTraducible ex)
        {
            MostrarError(TraducirExcepcion(ex));
            CargarIdiomaActivo(GestorSesion.PsicologoActual);
            CargarGrillaIdiomas(GestorSesion.PsicologoActual);
        }
    }

    private string ObtenerCodigoIso(string nombreIdioma)
    {
        GestorIdioma gestorIdioma = new GestorIdioma();
        Idioma idioma = gestorIdioma.BuscarPorNombre(nombreIdioma);
        return idioma != null ? idioma.CodigoIso : "";
    }

    private string ObtenerBandera(string codigoIso)
    {
        if (!string.IsNullOrEmpty(codigoIso) && Banderas.ContainsKey(codigoIso.ToLower()))
        {
            return Banderas[codigoIso.ToLower()];
        }
        return "🌐";
    }

    private string DestinoSegunRol(string rolPermiso)
    {
        switch (rolPermiso)
        {
            case "Administrador":
                return "FormMenuAdministrador.aspx";
            case "Web Master":
                return "FormMenuWebMaster.aspx";
            default:
                return "FormMenuProfesional.aspx";
        }
    }

    private string ObtenerIniciales(string nombre, string apellido)
    {
        string inicialNombre = string.IsNullOrEmpty(nombre) ? "" : nombre.Substring(0, 1);
        string inicialApellido = string.IsNullOrEmpty(apellido) ? "" : apellido.Substring(0, 1);
        return (inicialNombre + inicialApellido).ToUpper();
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