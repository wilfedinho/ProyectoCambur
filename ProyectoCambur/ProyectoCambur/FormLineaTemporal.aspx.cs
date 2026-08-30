using BE;
using BLL;
using SERVICIOS;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.UI.WebControls;
using GUI;

public partial class FormLineaTemporal : PaginaBase
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
        if (!gestorPermiso.TienePermiso(psicologoActual.RolPermiso, "acceder_linea_temporal"))
        {
            DenegarAcceso();
            return;
        }

        AplicarTraducciones();

        if (!IsPostBack)
        {
            CargarComboPacientes();
            MostrarEstado(1);
        }
    }

    private void AplicarTraducciones()
    {
        lblMenuCerrarSesionSidebar.Text = Traducir("menu_cerrar_sesion");
        lblHeaderPaciente.Text = Traducir("nav_linea_temporal");

        lblTituloSeleccion.Text = Traducir("titulo_linea_temporal_seleccion");
        lblSubtituloSeleccion.Text = Traducir("subtitulo_linea_temporal_seleccion");
        lblEtiquetaPacienteSel.Text = Traducir("lbl_paciente");
        rfvPacienteSel.ErrorMessage = Traducir("error_consulta_sin_paciente");
        btnContinuar.Text = Traducir("btn_continuar");

        btnFiltroTodos.Text = Traducir("filtro_todos");
        btnFiltroConsulta.Text = "🗒️ " + Traducir("filtro_consultas");
        btnFiltroHistorial.Text = "📋 " + Traducir("filtro_historial");
        lblFiltroTipoEtiqueta.Text = Traducir("lbl_tipo") + ":";
        btnAplicarFecha.Text = Traducir("btn_filtrar");

        lblLeyendaConsulta.Text = "● " + Traducir("filtro_consultas");
        lblLeyendaHistorial.Text = "● " + Traducir("filtro_historial");

        lblSinRegistros.Text = Traducir("msg_sin_registros_temporal");

        lblResumenTratamientoTitulo.Text = Traducir("titulo_resumen_tratamiento");
        lblDescConsultasTotales.Text = Traducir("lbl_consultas_totales");
        lblDescMesesTratamiento.Text = Traducir("lbl_meses_tratamiento");
        lblDescPrimeraSesion.Text = Traducir("lbl_primera_sesion");
        lblDescUltimaSesion.Text = Traducir("lbl_ultima_sesion");

        lblAccesosTitulo.Text = Traducir("titulo_acciones_relacionadas");
    }

    #region Estado / navegación entre pasos

    private void MostrarEstado(int estado)
    {
        pnlSeleccionPaciente.Visible = estado == 1;
        pnlTimeline.Visible = estado == 2;
    }

    private int IdPacienteActual
    {
        get { return ViewState["IdPacienteLT"] != null ? (int)ViewState["IdPacienteLT"] : 0; }
        set { ViewState["IdPacienteLT"] = value; }
    }

    private string TipoFiltroActual
    {
        get { return ViewState["TipoFiltroLT"] != null ? ViewState["TipoFiltroLT"].ToString() : "TODOS"; }
        set { ViewState["TipoFiltroLT"] = value; }
    }

    #endregion

    private void CargarComboPacientes()
    {
        Psicologo psicologoActual = GestorSesion.PsicologoActual;
        GestorPaciente gestorPaciente = new GestorPaciente();
        List<Paciente> pacientes = gestorPaciente.ObtenerPorPsicologo(psicologoActual.IdPsicologo);

        ddlPacienteSeleccion.Items.Clear();
        ddlPacienteSeleccion.Items.Add(new ListItem(Traducir("lbl_seleccionar_paciente"), ""));
        foreach (Paciente p in pacientes.OrderBy(p => p.Apellido).ThenBy(p => p.Nombre))
        {
            ddlPacienteSeleccion.Items.Add(new ListItem(p.Apellido + ", " + p.Nombre, p.IdPaciente.ToString()));
        }
    }

    protected void btnContinuar_Click(object sender, EventArgs e)
    {
        if (!Page.IsValid) return;

        int idPaciente;
        if (!int.TryParse(ddlPacienteSeleccion.SelectedValue, out idPaciente) || idPaciente <= 0)
        {
            MostrarError(Traducir("error_consulta_sin_paciente"));
            return;
        }

        Psicologo psicologoActual = GestorSesion.PsicologoActual;
        GestorPaciente gestorPaciente = new GestorPaciente();
        Paciente paciente = gestorPaciente.BuscarPorId(idPaciente);
        if (paciente == null || paciente.IdPsicologo != psicologoActual.IdPsicologo)
        {
            MostrarError(Traducir("error_paciente_no_propio"));
            return;
        }

        IdPacienteActual = idPaciente;
        TipoFiltroActual = "TODOS";
        txtDesde.Text = "";
        txtHasta.Text = "";

        CargarEncabezadoPaciente(paciente);
        ActualizarBotonesFiltro("TODOS");
        CargarTimeline("TODOS", null, null);
        CargarResumenTratamiento(idPaciente);

        MostrarEstado(2);
    }

    private void CargarEncabezadoPaciente(Paciente paciente)
    {
        lblPacienteNombre.Text = paciente.Nombre + " " + paciente.Apellido;
        lblPacienteIniciales.Text = ObtenerIniciales(paciente.Nombre, paciente.Apellido);
        lblPacienteEdad.Text = CalcularEdad(paciente.FechaNacimiento) + " " + Traducir("lbl_anios");
        lblPacienteEstado.Text = paciente.EstadoCivil;
        lblPacienteOcup.Text = paciente.Ocupacion;
    }

    #region Timeline

    private void CargarTimeline(string tipoFiltro, DateTime? desde, DateTime? hasta)
    {
        Psicologo psicologoActual = GestorSesion.PsicologoActual;
        GestorLineaTemporal gestorLinea = new GestorLineaTemporal();

        List<EventoTimeline> eventos;
        try
        {
            eventos = gestorLinea.ObtenerLineaTemporal(psicologoActual.IdPsicologo, IdPacienteActual, tipoFiltro, desde, hasta);
        }
        catch (ExcepcionTraducible ex)
        {
            MostrarError(TraducirExcepcion(ex));
            eventos = new List<EventoTimeline>();
        }

        List<TimelineItemVM> filas = new List<TimelineItemVM>();
        for (int i = 0; i < eventos.Count; i++)
        {
            EventoTimeline ev = eventos[i];
            filas.Add(new TimelineItemVM
            {
                IdEvento = ev.IdEvento,
                Tipo = ev.Tipo,
                TipoLabel = ev.Tipo == GestorLineaTemporal.TIPO_CONSULTA ? Traducir("filtro_consultas") : Traducir("filtro_historial"),
                TipoCss = ev.TipoCss,
                Icono = ev.Icono,
                Fecha = ev.Fecha,
                Resumen = ev.Resumen,
                Detalle = ev.Detalle,
                Duracion = ev.Duracion,
                LadoCss = i % 2 == 0 ? "izquierda" : "derecha"
            });
        }

        rptTimeline.DataSource = filas;
        rptTimeline.DataBind();

        lblSinRegistros.Visible = filas.Count == 0;
        lblTotalEventos.Text = filas.Count + " " + (filas.Count == 1 ? Traducir("lbl_registro") : Traducir("lbl_registros"));
    }

    protected void btnFiltro_Click(object sender, EventArgs e)
    {
        Button btn = (Button)sender;
        string tipo = btn.CommandArgument;
        TipoFiltroActual = tipo;
        ActualizarBotonesFiltro(tipo);

        DateTime? desde = ParsearFecha(txtDesde.Text);
        DateTime? hasta = ParsearFecha(txtHasta.Text);
        CargarTimeline(tipo, desde, hasta);
    }

    protected void btnAplicarFecha_Click(object sender, EventArgs e)
    {
        DateTime? desde = ParsearFecha(txtDesde.Text);
        DateTime? hasta = ParsearFecha(txtHasta.Text);
        if (desde.HasValue && hasta.HasValue && desde.Value > hasta.Value)
        {
            MostrarError(Traducir("error_rango_fecha_invalido"));
            return;
        }

        CargarTimeline(TipoFiltroActual, desde, hasta);
    }

    private void ActualizarBotonesFiltro(string tipoActivo)
    {
        btnFiltroTodos.CssClass = "filtro-btn" + (tipoActivo == "TODOS" ? " active" : "");
        btnFiltroConsulta.CssClass = "filtro-btn" + (tipoActivo == GestorLineaTemporal.TIPO_CONSULTA ? " active" : "");
        btnFiltroHistorial.CssClass = "filtro-btn" + (tipoActivo == GestorLineaTemporal.TIPO_HISTORIAL ? " active" : "");
    }

    private DateTime? ParsearFecha(string texto)
    {
        DateTime fecha;
        if (!string.IsNullOrWhiteSpace(texto) && DateTime.TryParse(texto, CultureInfo.InvariantCulture, DateTimeStyles.None, out fecha))
        {
            return fecha;
        }
        return null;
    }

    #endregion

    #region Resumen del tratamiento (aside)

    private void CargarResumenTratamiento(int idPaciente)
    {
        GestorConsulta gestorConsulta = new GestorConsulta();
        List<Consulta> consultas = gestorConsulta.ObtenerPorPaciente(idPaciente).OrderBy(c => c.FechaConsulta).ToList();

        lblStatConsultas.Text = consultas.Count.ToString();

        if (consultas.Count > 0)
        {
            DateTime primera = consultas.First().FechaConsulta;
            DateTime ultima = consultas.Last().FechaConsulta;
            int meses = ((DateTime.Today.Year - primera.Year) * 12) + DateTime.Today.Month - primera.Month;
            lblStatMeses.Text = Math.Max(meses, 0).ToString();
            lblStatInicio.Text = primera.ToString("dd/MM/yyyy");
            lblStatUltima.Text = ultima.ToString("dd/MM/yyyy");
        }
        else
        {
            lblStatMeses.Text = "0";
            lblStatInicio.Text = "—";
            lblStatUltima.Text = "—";
        }
    }

    #endregion

    #region Helpers

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

    private void MostrarError(string mensaje)
    {
        lblMensaje.Text = mensaje;
        lblMensaje.CssClass = "server-error";
        lblMensaje.Visible = true;
    }

    #endregion
    private class TimelineItemVM
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
        public string LadoCss { get; set; }
    }
}