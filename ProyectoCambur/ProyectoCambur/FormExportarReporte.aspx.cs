using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;


public partial class FormExportarReporte : System.Web.UI.Page
{
    // =========================================================
    // PAGE LOAD
    // =========================================================
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            CargarProfesionalDemo();
            CargarDropdownPacientes();
            CargarDocumentosDemo(1);     // primer paciente por defecto
            CargarExportacionesRecientes();
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
    // DROPDOWN DE PACIENTES
    // TODO: reemplazar por BLL.PacienteBLL.ObtenerActivosPorProfesional()
    // =========================================================
    private void CargarDropdownPacientes()
    {
        ddlPaciente.Items.Clear();
        ddlPaciente.Items.Add(new ListItem("Martín González", "1"));
        ddlPaciente.Items.Add(new ListItem("Sofía Ramírez", "2"));
        ddlPaciente.Items.Add(new ListItem("Carlos Ibáñez", "3"));
        ddlPaciente.Items.Add(new ListItem("Valentina Moreno", "4"));
        ddlPaciente.Items.Add(new ListItem("Facundo Pérez", "5"));
    }

    // =========================================================
    // CARGAR ESTADO DE DOCUMENTOS DEL PACIENTE
    // TODO: reemplazar por BLL.DocumentoBLL.ObtenerEstadoPorPaciente(id)
    // =========================================================
    private void CargarDocumentosDemo(int idPaciente)
    {
        // Datos demo según paciente seleccionado
        switch (idPaciente)
        {
            case 1: // Martín González — todos disponibles
                SetDocumento(lblFechaResumen, lblEstadoResumen,
                    "Generado el 08/05/2026", "✓ Disponible", "doc-badge doc-badge-ok");
                SetDocumento(lblFechaDerivacion, lblEstadoDerivacion,
                    "Generado el 15/04/2026", "✓ Validado", "doc-badge doc-badge-ok");
                SetDocumento(lblFechaPerfil, lblEstadoPerfil,
                    "Modelo: Big Five · 10/03/2026", "✓ Disponible", "doc-badge doc-badge-ok");
                break;

            case 2: // Sofía Ramírez — derivación pendiente
                SetDocumento(lblFechaResumen, lblEstadoResumen,
                    "Generado el 02/05/2026", "✓ Disponible", "doc-badge doc-badge-ok");
                SetDocumento(lblFechaDerivacion, lblEstadoDerivacion,
                    "Sin informe generado", "⚠ No disponible", "doc-badge doc-badge-no-disponible");
                SetDocumento(lblFechaPerfil, lblEstadoPerfil,
                    "Sin perfil generado", "⚠ No disponible", "doc-badge doc-badge-no-disponible");
                break;

            case 3: // Carlos Ibáñez — derivación pendiente de validación
                SetDocumento(lblFechaResumen, lblEstadoResumen,
                    "Generado el 06/05/2026", "✓ Disponible", "doc-badge doc-badge-ok");
                SetDocumento(lblFechaDerivacion, lblEstadoDerivacion,
                    "Generado el 22/04/2026", "⏳ Pendiente de validación", "doc-badge doc-badge-pendiente");
                SetDocumento(lblFechaPerfil, lblEstadoPerfil,
                    "Modelo: COPE · 01/04/2026", "✓ Disponible", "doc-badge doc-badge-ok");
                break;

            default:
                SetDocumento(lblFechaResumen, lblEstadoResumen,
                    "Sin documento", "⚠ No disponible", "doc-badge doc-badge-no-disponible");
                SetDocumento(lblFechaDerivacion, lblEstadoDerivacion,
                    "Sin documento", "⚠ No disponible", "doc-badge doc-badge-no-disponible");
                SetDocumento(lblFechaPerfil, lblEstadoPerfil,
                    "Sin documento", "⚠ No disponible", "doc-badge doc-badge-no-disponible");
                break;
        }
    }

    private void SetDocumento(Label lblFecha, Label lblEstado,
                               string fecha, string estado, string cssClass)
    {
        lblFecha.Text = fecha;
        lblEstado.Text = estado;
        lblEstado.CssClass = cssClass;
    }

    // =========================================================
    // EXPORTACIONES RECIENTES
    // TODO: reemplazar por BLL.ExportacionBLL.ObtenerUltimas(idProfesional, 5)
    // =========================================================
    private void CargarExportacionesRecientes()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("Icono", typeof(string));
        dt.Columns.Add("Tipo", typeof(string));
        dt.Columns.Add("Paciente", typeof(string));
        dt.Columns.Add("Fecha", typeof(DateTime));

        dt.Rows.Add("🤖", "Resumen Clínico IA", "Martín González", new DateTime(2026, 5, 8));
        dt.Rows.Add("📤", "Informe de Derivación", "Martín González", new DateTime(2026, 4, 20));
        dt.Rows.Add("🧠", "Perfil Evolutivo", "Carlos Ibáñez", new DateTime(2026, 4, 1));
        dt.Rows.Add("🤖", "Resumen Clínico IA", "Sofía Ramírez", new DateTime(2026, 3, 15));

        if (dt.Rows.Count > 0)
        {
            rptExportaciones.DataSource = dt;
            rptExportaciones.DataBind();
            lblSinExportaciones.Visible = false;
        }
        else
        {
            rptExportaciones.DataSource = null;
            rptExportaciones.DataBind();
            lblSinExportaciones.Visible = true;
        }
    }

    // =========================================================
    // EVENTO: CAMBIO DE PACIENTE (AutoPostBack)
    // =========================================================
    protected void ddlPaciente_SelectedIndexChanged(object sender, EventArgs e)
    {
        int idPaciente = 0;
        int.TryParse(ddlPaciente.SelectedValue, out idPaciente);
        CargarDocumentosDemo(idPaciente);
        pnlPreview.Visible = false;
        hfTipoSeleccionado.Value = string.Empty;
        lblMensaje.Visible = false;
    }

    // =========================================================
    // EVENTO: EXPORTAR EN PDF
    // =========================================================
    protected void btnExportar_Click(object sender, EventArgs e)
    {
        lblMensaje.Visible = false;

        string tipo = hfTipoSeleccionado.Value;

        if (string.IsNullOrEmpty(tipo))
        {
            MostrarError("Seleccioná el tipo de documento a exportar.");
            return;
        }

        int idPaciente = 0;
        int.TryParse(ddlPaciente.SelectedValue, out idPaciente);

        // Validar disponibilidad del documento
        if (!DocumentoDisponible(idPaciente, tipo))
        {
            MostrarError("El documento seleccionado no está disponible para este paciente. " +
                         "Generalo primero desde la sección correspondiente.");
            return;
        }

        // Validar informe de derivación validado
        if (tipo == "DERIVACION" && !DerivacionValidada(idPaciente))
        {
            MostrarError("El informe de derivación debe ser revisado y validado antes de exportarse. " +
                         "Revisalo en la sección Derivaciones.");
            return;
        }

        // TODO: reemplazar por:
        //   int idProfesional = (int)Session["IdProfesional"];
        //   byte[] pdfBytes = BLL.ExportacionBLL.GenerarPDF(idPaciente, idProfesional, tipo);
        //   // Registrar evento en bitácora:
        //   BLL.BitacoraBLL.Registrar(idProfesional, "Exportar", "Exportación de " + tipo, criticidad: 3);
        //   // Enviar el PDF al navegador:
        //   Response.ContentType = "application/pdf";
        //   Response.AddHeader("Content-Disposition", "attachment; filename=Reporte_" + tipo + ".pdf");
        //   Response.BinaryWrite(pdfBytes);
        //   Response.End();

        // DEMO: mostrar preview en pantalla
        MostrarPreviewDemo(idPaciente, tipo);
        MostrarExito("Reporte generado correctamente. En producción se descargará como PDF.");
    }

    // =========================================================
    // VALIDACIONES DE DISPONIBILIDAD (demo)
    // TODO: reemplazar por BLL.DocumentoBLL.EstaDisponible(idPaciente, tipo)
    // =========================================================
    private bool DocumentoDisponible(int idPaciente, string tipo)
    {
        if (idPaciente <= 1) return true;
        if (idPaciente == 2 && (tipo == "DERIVACION" || tipo == "PERFIL")) return false;
        return true;
    }

    private bool DerivacionValidada(int idPaciente)
    {
        if (idPaciente == 3) return false; // Carlos tiene derivación pendiente de validación
        return true;
    }

    // =========================================================
    // PREVIEW DEL DOCUMENTO (demo)
    // =========================================================
    private void MostrarPreviewDemo(int idPaciente, string tipo)
    {
        string nombrePaciente = ddlPaciente.SelectedItem.Text;
        string tipoLabel = tipo == "RESUMEN" ? "Resumen Clínico IA"
                              : tipo == "DERIVACION" ? "Informe de Derivación"
                              : "Perfil Evolutivo del Paciente";

        pnlPreview.Visible = true;
        lblPreviewMeta.Text = tipoLabel + " · " + nombrePaciente;
        lblPreviewBadge.Text = "✓ Listo para descargar";
        lblPreviewBadge.CssClass = "doc-badge doc-badge-ok";

        lblPrevFechDoc.Text = "Fecha: " + DateTime.Today.ToString("dd/MM/yyyy");
        lblPrevProfesional.Text = "Prof. Lucía Martínez · Mat. 12345";
        lblPrevPaciente.Text = nombrePaciente;
        lblPrevDatosPaciente.Text = "Paciente activo · " + DateTime.Today.ToString("yyyy");
        lblPrevTipoDoc.Text = tipoLabel;
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
