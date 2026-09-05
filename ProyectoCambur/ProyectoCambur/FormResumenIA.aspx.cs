using BE;
using BLL;
using SERVICIOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using GUI;

public partial class FormResumenIA : PaginaBase
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
        if (!gestorPermiso.TienePermiso(psicologoActual.RolPermiso, "acceder_resumen_ia"))
        {
            DenegarAcceso();
            return;
        }

        AplicarTraducciones();

        if (!IsPostBack)
        {
            lblTaglineSidebar.Text = Traducir("tagline_panel_gestion");
            CargarComboPacientes();
            CargarFiltrosFechaPorDefecto();
            MostrarEstado(1);
        }
    }

    private void AplicarTraducciones()
    {
        lblMenuCerrarSesionSidebar.Text = Traducir("menu_cerrar_sesion");
        lblHeaderSeccion.Text = Traducir("seccion_modulo_ia");
        lblHeaderPagina.Text = Traducir("nav_resumen_ia");

        lblFormTitulo.Text = Traducir("titulo_resumen_ia");
        lblFormSubtitulo.Text = Traducir("subtitulo_resumen_ia");
        lblAvisoIA.Text = Traducir("aviso_ia_resumen");

        lblSeccionFiltros.Text = Traducir("seccion_filtros_busqueda");
        lblEtiquetaPaciente.Text = Traducir("lbl_paciente");
        rfvPaciente.ErrorMessage = Traducir("error_consulta_sin_paciente");
        lblEtiquetaFechaDesde.Text = Traducir("lbl_fecha_desde");
        rfvDesde.ErrorMessage = Traducir("error_fecha_desde_obligatoria");
        lblEtiquetaFechaHasta.Text = Traducir("lbl_fecha_hasta");
        rfvHasta.ErrorMessage = Traducir("error_fecha_hasta_obligatoria");
        btnBuscar.Text = Traducir("btn_buscar_consultas");

        lblTituloConsultasEncontradas.Text = Traducir("titulo_consultas_encontradas");
        lblHintSeleccion.Text = Traducir("hint_seleccionar_consultas");
        lblThFecha.Text = Traducir("th_fecha");
        lblThDuracion.Text = Traducir("th_duracion");
        lblThResumenObjetivos.Text = Traducir("th_resumen_objetivos");
        btnVolver.Text = Traducir("btn_cambiar_filtros");
        btnGenerar.Text = Traducir("btn_generar_resumen_ia");
        lblCargaTitulo.Text = Traducir("carga_titulo_resumen_ia");
        lblCargaSubtitulo.Text = Traducir("carga_subtitulo_resumen_ia");

        lblTituloResumenGenerado.Text = Traducir("titulo_resumen_generado");
        btnNuevoResumen.Text = Traducir("btn_nuevo_resumen");
        lblAvisoResultado.Text = Traducir("aviso_ia_resultado");

        lblTituloContexto.Text = Traducir("seccion_contexto_general");
        lblTituloEvolucion.Text = Traducir("seccion_evolucion_observada");
        lblTituloTemas.Text = Traducir("seccion_temas_recurrentes");
        lblTituloIntervenciones.Text = Traducir("seccion_intervenciones_destacadas");
        lblTituloObservaciones.Text = Traducir("seccion_observaciones_periodo");

        lblAccesosTitulo.Text = Traducir("titulo_detalles_resumen");
        lblMetaLabelPaciente.Text = Traducir("lbl_paciente");
        lblMetaLabelPeriodo.Text = Traducir("lbl_periodo");
        lblMetaLabelConsultas.Text = Traducir("lbl_consultas_analizadas");
        lblMetaLabelFecha.Text = Traducir("lbl_generado");

        lblAvisoEncriptadoTitulo.Text = Traducir("titulo_resumen_encriptado");
        lblAvisoEncriptadoTexto.Text = Traducir("aviso_resumen_encriptado");

        lblTituloResumenesAnteriores.Text = Traducir("titulo_resumenes_anteriores");
        lblSinResumenesAnteriores.Text = Traducir("sin_resumenes_anteriores");
    }
    protected void ddlPaciente_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblMensaje.Visible = false;
        MostrarEstado(1);

        if (!string.IsNullOrEmpty(ddlPaciente.SelectedValue))
        {
            CargarResumenesAnteriores(Convert.ToInt32(ddlPaciente.SelectedValue));
        }
        else
        {
            pnlResumenesAnteriores.Visible = false;
        }
    }

    private void CargarResumenesAnteriores(int idPaciente)
    {
        GestorResumenClinico gestorResumenClinico = new GestorResumenClinico();
        List<ResumenClinico> resumenes = gestorResumenClinico.ObtenerPorPaciente(idPaciente);

        pnlResumenesAnteriores.Visible = true;

        if (resumenes.Count > 0)
        {
            rptResumenesAnteriores.DataSource = resumenes.Select(r => new
            {
                IdResumen = r.IdResumen,
                Periodo = r.RangoDesde.ToString("dd/MM/yyyy") + " " + Traducir("lbl_al") + " " + r.RangoHasta.ToString("dd/MM/yyyy"),
                FechaGeneracion = r.FechaGeneracion
            }).ToList();
            rptResumenesAnteriores.DataBind();
            lblSinResumenesAnteriores.Visible = false;
        }
        else
        {
            rptResumenesAnteriores.DataSource = null;
            rptResumenesAnteriores.DataBind();
            lblSinResumenesAnteriores.Visible = true;
        }
    }
    protected void rptResumenesAnteriores_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        if (e.CommandName != "VerResumen") return;

        lblMensaje.Visible = false;
        int idPsicologo = GestorSesion.PsicologoActual.IdPsicologo;
        int idResumen = Convert.ToInt32(e.CommandArgument);

        GestorResumenClinico gestorResumenClinico = new GestorResumenClinico();
        ResumenClinico resumen = gestorResumenClinico.BuscarPorId(idResumen);
        if (resumen == null || resumen.IdProfesional != idPsicologo)
        {
            MostrarError(Traducir("error_paciente_no_propio"));
            return;
        }

        GestorConsulta gestorConsulta = new GestorConsulta();
        int cantidadConsultas = gestorConsulta.ObtenerPorPaciente(resumen.IdPaciente)
            .Count(c => c.FechaConsulta.Date >= resumen.RangoDesde.Date && c.FechaConsulta.Date <= resumen.RangoHasta.Date);

        MostrarResumenGenerado(idResumen, cantidadConsultas);
        MostrarEstado(3);
    }

    protected void btnBuscar_Click(object sender, EventArgs e)
    {
        lblMensaje.Visible = false;

        if (!Page.IsValid) return;

        int idPsicologo = GestorSesion.PsicologoActual.IdPsicologo;
        int idPaciente = Convert.ToInt32(ddlPaciente.SelectedValue);

        GestorPaciente gestorPaciente = new GestorPaciente();
        List<Paciente> propios = gestorPaciente.ObtenerPorPsicologo(idPsicologo);
        if (!propios.Any(p => p.IdPaciente == idPaciente))
        {
            MostrarError(Traducir("error_paciente_no_propio"));
            return;
        }

        DateTime desde, hasta;
        if (!DateTime.TryParse(txtFechaDesde.Text, out desde) || !DateTime.TryParse(txtFechaHasta.Text, out hasta))
        {
            MostrarError(Traducir("error_fecha_formato_invalido"));
            return;
        }

        if (desde > hasta)
        {
            MostrarError(Traducir("error_rango_fechas_invalido"));
            return;
        }

        GestorConsulta gestorConsulta = new GestorConsulta();
        List<Consulta> consultas = gestorConsulta.ObtenerPorPaciente(idPaciente).Where(c => c.FechaConsulta.Date >= desde.Date && c.FechaConsulta.Date <= hasta.Date).OrderBy(c => c.FechaConsulta).ToList();

        if (consultas.Count == 0)
        {
            MostrarError(Traducir("error_resumen_sin_consultas"));
            return;
        }

        rptConsultas.DataSource = consultas.Select(c => new
        {
            IdConsulta = c.IdConsulta,
            Fecha = c.FechaConsulta,
            Duracion = c.TiempoConsulta,
            ResumenObjetivos = TruncarResumen(c.Objetivos)
        }).ToList();
        rptConsultas.DataBind();

        lblCantConsultas.Text = consultas.Count + " " + Traducir("lbl_consultas_plural");
        lblCantConsultas.Visible = true;
        lblRangoBusqueda.Text = desde.ToString("dd/MM/yyyy") + " " + Traducir("lbl_al") + " " + hasta.ToString("dd/MM/yyyy");

        MostrarEstado(2);
    }

    protected void btnVolver_Click(object sender, EventArgs e)
    {
        lblMensaje.Visible = false;
        MostrarEstado(1);
    }

    protected void btnGenerar_Click(object sender, EventArgs e)
    {
        lblMensaje.Visible = false;

        int idPsicologo = GestorSesion.PsicologoActual.IdPsicologo;
        int idPaciente = Convert.ToInt32(ddlPaciente.SelectedValue);

        DateTime desde, hasta;
        DateTime.TryParse(txtFechaDesde.Text, out desde);
        DateTime.TryParse(txtFechaHasta.Text, out hasta);

        List<int> idsSeleccionados = new List<int>();
        foreach (RepeaterItem item in rptConsultas.Items)
        {
            CheckBox chkConsulta = item.FindControl("chkConsulta") as CheckBox;
            HiddenField hfIdConsulta = item.FindControl("hfIdConsulta") as HiddenField;
            if (chkConsulta != null && chkConsulta.Checked && hfIdConsulta != null)
            {
                idsSeleccionados.Add(Convert.ToInt32(hfIdConsulta.Value));
            }
        }

        if (idsSeleccionados.Count == 0)
        {
            MostrarError(Traducir("error_resumen_sin_consultas_seleccionadas"));
            return;
        }

        GestorResumenClinico gestorResumenClinico = new GestorResumenClinico();
        try
        {
            int idGenerado = gestorResumenClinico.Generar(idPsicologo, idPaciente, desde, hasta, idsSeleccionados);
            MostrarResumenGenerado(idGenerado, idsSeleccionados.Count);
            MostrarEstado(3);
        }
        catch (ExcepcionTraducible ex)
        {
            MostrarError(TraducirExcepcion(ex));
        }
    }

    protected void btnNuevoResumen_Click(object sender, EventArgs e)
    {
        lblMensaje.Visible = false;
        CargarFiltrosFechaPorDefecto();
        if (!string.IsNullOrEmpty(ddlPaciente.SelectedValue))
        {
            CargarResumenesAnteriores(Convert.ToInt32(ddlPaciente.SelectedValue));
        }
        MostrarEstado(1);
    }

    private void MostrarResumenGenerado(int idResumen, int cantidadConsultas)
    {
        GestorResumenClinico gestorResumenClinico = new GestorResumenClinico();
        ResumenClinico resumen = gestorResumenClinico.BuscarPorId(idResumen);
        SeccionesResumenClinico secciones = gestorResumenClinico.ObtenerSecciones(resumen);

        string nombrePaciente = ddlPaciente.SelectedItem.Text;
        string periodoTexto = resumen.RangoDesde.ToString("dd/MM/yyyy") + " " + Traducir("lbl_al") + " " + resumen.RangoHasta.ToString("dd/MM/yyyy");

        lblResumenMeta.Text = nombrePaciente + " · " + periodoTexto;
        lblMetaPaciente.Text = nombrePaciente;
        lblMetaPeriodo.Text = periodoTexto;
        lblMetaConsultas.Text = cantidadConsultas + " " + Traducir("lbl_consultas_analizadas_valor");
        lblMetaFecha.Text = resumen.FechaGeneracion.ToString("dd/MM/yyyy HH:mm");

        lblContextoGeneral.Text = secciones.ContextoGeneral;
        lblEvolucion.Text = secciones.Evolucion;
        lblTemasRecurrentes.Text = secciones.TemasRecurrentes;
        lblIntervenciones.Text = secciones.Intervenciones;
        lblObservaciones.Text = secciones.Observaciones;
    }

    private void MostrarEstado(int estado)
    {
        pnlFiltros.Visible = (estado == 1);
        pnlConsultas.Visible = (estado == 2);
        pnlResumen.Visible = (estado == 3);
        pnlResumenesAnteriores.Visible = (estado == 1) && !string.IsNullOrEmpty(ddlPaciente.SelectedValue);
    }

    private void CargarComboPacientes()
    {
        GestorPaciente gestorPaciente = new GestorPaciente();
        int idPropio = GestorSesion.PsicologoActual.IdPsicologo;
        List<Paciente> propios = gestorPaciente.ObtenerPorPsicologo(idPropio);
        ddlPaciente.Items.Clear();
        ddlPaciente.Items.Add(new ListItem(Traducir("opt_seleccionar"), ""));

        foreach (Paciente p in propios.OrderBy(x => x.Apellido))
        {
            ddlPaciente.Items.Add(new ListItem(p.Nombre + " " + p.Apellido, p.IdPaciente.ToString()));
        }
    }

    private void CargarFiltrosFechaPorDefecto()
    {
        txtFechaDesde.Text = DateTime.Today.AddMonths(-3).ToString("yyyy-MM-dd");
        txtFechaHasta.Text = DateTime.Today.ToString("yyyy-MM-dd");
    }

    private string TruncarResumen(string objetivos)
    {
        if (string.IsNullOrWhiteSpace(objetivos)) return "--";
        return objetivos.Length > 60 ? objetivos.Substring(0, 60) + "…" : objetivos;
    }

    private void MostrarError(string msg)
    {
        lblMensaje.Text = msg;
        lblMensaje.CssClass = "server-error";
        lblMensaje.Visible = true;
    }
}