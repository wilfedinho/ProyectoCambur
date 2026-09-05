using BE;
using SERVICIOS;
using System;
using System.Collections.Generic;
using System.Linq;
using PdfSharp.Drawing;
using PdfSharp.Pdf;

namespace BLL
{

    public class GestorExportacion
    {
        public const string TIPO_RESUMEN = "RESUMEN";
        public const string TIPO_DERIVACION = "DERIVACION";
        public const string TIPO_PERFIL = "PERFIL";

        static GestorExportacion()
        {
            ResolvedorFuentesPdf.Registrar();
        }
        public byte[] Generar(int idPsicologo, int idPaciente, string tipoDocumento, int? idDocumento, out string nombreArchivoSugerido)
        {
            GestorPaciente gestorPaciente = new GestorPaciente();
            Paciente paciente = gestorPaciente.BuscarPorId(idPaciente);
            if (paciente == null || paciente.IdPsicologo != idPsicologo)
            {

                throw new ExcepcionTraducible("error_paciente_no_propio");
            }

            byte[] pdf;
            string tituloDocumento;

            switch (tipoDocumento)
            {
                case TIPO_RESUMEN:
                    pdf = GenerarResumenClinicoPdf(idPsicologo, paciente, idDocumento, out tituloDocumento);
                    break;
                case TIPO_DERIVACION:
                    pdf = GenerarInformeDerivacionPdf(idPsicologo, paciente, idDocumento, out tituloDocumento);
                    break;
                case TIPO_PERFIL:
                    pdf = GenerarPerfilPacientePdf(idPsicologo, paciente, idDocumento, out tituloDocumento);
                    break;
                default:
                    throw new ExcepcionTraducible("error_documento_no_disponible");
            }

            GestorBitacora gestorBitacora = new GestorBitacora();
            gestorBitacora.RegistrarEvento(EventosBitacora.MOD_REPORTES, EventosBitacora.DESC_EXPORTAR_PDF, EventosBitacora.CRIT_EXPORTAR_PDF);

            nombreArchivoSugerido = ArmarNombreArchivo(paciente, tituloDocumento);
            return pdf;
        }

        public bool DocumentoDisponible(int idPsicologo, int idPaciente, string tipoDocumento)
        {
            switch (tipoDocumento)
            {
                case TIPO_RESUMEN:
                    GestorResumenClinico gestorResumen = new GestorResumenClinico();
                    return gestorResumen.ObtenerPorPaciente(idPaciente).Count > 0;

                case TIPO_DERIVACION:
                    GestorInformeDerivacion gestorInforme = new GestorInformeDerivacion();
                    return gestorInforme.ObtenerPorPaciente(idPaciente).Any(i => i.Estado == EstadoInforme.Auditado);

                case TIPO_PERFIL:
                    GestorPerfilPaciente gestorPerfil = new GestorPerfilPaciente();
                    return gestorPerfil.ObtenerPorPaciente(idPaciente).Count > 0;

                default:
                    return false;
            }
        }
        public bool DocumentoPendienteAuditoria(int idPaciente)
        {
            GestorInformeDerivacion gestorInforme = new GestorInformeDerivacion();
            return gestorInforme.ObtenerPorPaciente(idPaciente).Any(i => i.Estado == EstadoInforme.Borrador);
        }
        public List<DocumentoExportable> ObtenerDocumentosDisponibles(int idPsicologo, int idPaciente, string tipoDocumento)
        {
            GestorPaciente gestorPaciente = new GestorPaciente();
            Paciente paciente = gestorPaciente.BuscarPorId(idPaciente);
            if (paciente == null || paciente.IdPsicologo != idPsicologo)
            {
                throw new ExcepcionTraducible("error_paciente_no_propio");
            }

            List<DocumentoExportable> documentos = new List<DocumentoExportable>();

            switch (tipoDocumento)
            {
                case TIPO_RESUMEN:
                    GestorResumenClinico gestorResumen = new GestorResumenClinico();
                    foreach (ResumenClinico r in gestorResumen.ObtenerPorPaciente(idPaciente).OrderByDescending(r => r.FechaGeneracion))
                    {
                        documentos.Add(new DocumentoExportable
                        {
                            IdDocumento = r.IdResumen,
                            Fecha = r.FechaGeneracion,
                            Detalle = "Rango " + r.RangoDesde.ToString("dd/MM/yyyy") + " al " + r.RangoHasta.ToString("dd/MM/yyyy")
                        });
                    }
                    break;

                case TIPO_DERIVACION:
                    GestorInformeDerivacion gestorInforme = new GestorInformeDerivacion();
                    foreach (InformeDerivacion i in gestorInforme.ObtenerPorPaciente(idPaciente)
                        .Where(i => i.Estado == EstadoInforme.Auditado)
                        .OrderByDescending(i => i.FechaAuditoria ?? i.FechaGeneracion))
                    {
                        SeccionesInformeDerivacion secciones = gestorInforme.ObtenerSecciones(i);
                        documentos.Add(new DocumentoExportable
                        {
                            IdDocumento = i.IdInforme,
                            Fecha = i.FechaAuditoria ?? i.FechaGeneracion,
                            Detalle = secciones != null && !string.IsNullOrWhiteSpace(secciones.EspecialidadDerivacion)
                                ? "Especialidad: " + secciones.EspecialidadDerivacion
                                : ""
                        });
                    }
                    break;

                case TIPO_PERFIL:
                    GestorPerfilPaciente gestorPerfil = new GestorPerfilPaciente();
                    foreach (PerfilPaciente p in gestorPerfil.ObtenerPorPaciente(idPaciente).OrderByDescending(p => p.FechaGeneracion))
                    {
                        SeccionesPerfilPaciente secciones = gestorPerfil.ObtenerSecciones(p);
                        documentos.Add(new DocumentoExportable
                        {
                            IdDocumento = p.IdPerfil,
                            Fecha = p.FechaGeneracion,
                            Detalle = secciones != null && !string.IsNullOrWhiteSpace(secciones.NombreModelo)
                                ? "Modelo: " + secciones.NombreModelo
                                : ""
                        });
                    }
                    break;
            }

            return documentos;
        }

        public List<Bitacora> ObtenerExportacionesRecientes(int idPsicologo, int cantidad = 6)
        {
            if (!GestorSesion.EstaAutenticado) return new List<Bitacora>();

            GestorBitacora gestorBitacora = new GestorBitacora();
            string email = GestorSesion.PsicologoActual.Email;
            List<Bitacora> eventos = gestorBitacora.ObtenerPorFiltros(null, null, EventosBitacora.MOD_REPORTES, email, null);

            return eventos
                .Where(e => e.Descripcion == EventosBitacora.DESC_EXPORTAR_PDF)
                .OrderByDescending(e => e.FechaEvento)
                .Take(cantidad)
                .ToList();
        }

        #region Generación PDF - Resumen Clínico IA

        private byte[] GenerarResumenClinicoPdf(int idPsicologo, Paciente paciente, int? idDocumento, out string tituloDocumento)
        {
            tituloDocumento = "Resumen Clínico IA";

            GestorResumenClinico gestorResumen = new GestorResumenClinico();
            List<ResumenClinico> resumenes = gestorResumen.ObtenerPorPaciente(paciente.IdPaciente)
                .OrderByDescending(r => r.FechaGeneracion)
                .ToList();

            if (resumenes.Count == 0)
            {
                throw new ExcepcionTraducible("error_documento_no_disponible");
            }

            ResumenClinico resumen = idDocumento.HasValue
                ? resumenes.FirstOrDefault(r => r.IdResumen == idDocumento.Value)
                : resumenes[0];

            if (resumen == null)
            {
                throw new ExcepcionTraducible("error_documento_no_disponible");
            }
            SeccionesResumenClinico secciones = gestorResumen.ObtenerSecciones(resumen);

            string nombrePsicologo = "";
            if (GestorSesion.EstaAutenticado)
            {
                nombrePsicologo = "Prof. " + GestorSesion.PsicologoActual.Nombre + " " + GestorSesion.PsicologoActual.Apellido;
            }

            using (PdfDocument documento = new PdfDocument())
            {
                documento.Info.Title = tituloDocumento + " - " + paciente.Nombre + " " + paciente.Apellido;
                documento.Info.Author = nombrePsicologo;

                PdfPage pagina = documento.AddPage();
                pagina.Size = PdfSharp.PageSize.A4;
                XGraphics gfx = XGraphics.FromPdfPage(pagina);

                XFont fuenteTitulo = new XFont("Verdana", 18, XFontStyleEx.Bold);
                XFont fuenteSubtitulo = new XFont("Verdana", 10, XFontStyleEx.Regular);
                XFont fuenteSeccion = new XFont("Verdana", 12, XFontStyleEx.Bold);
                XFont fuenteTexto = new XFont("Verdana", 10, XFontStyleEx.Regular);
                XFont fuenteAviso = new XFont("Verdana", 8, XFontStyleEx.Italic);

                double margen = 40;
                double y = margen;
                double anchoPagina = pagina.Width;
                double altoPagina = pagina.Height;
                double anchoUtil = anchoPagina - (margen * 2);

                gfx.DrawString("CAMBUR", fuenteTitulo, XBrushes.DarkSlateBlue, Rect(margen, y, anchoUtil, 30), XStringFormats.TopLeft);
                gfx.DrawString(DateTime.Today.ToString("dd/MM/yyyy"), fuenteSubtitulo, XBrushes.Gray, Rect(margen, y, anchoUtil, 30), XStringFormats.TopRight);
                y += 32;
                gfx.DrawString(nombrePsicologo, fuenteSubtitulo, XBrushes.Gray, Rect(margen, y, anchoUtil, 16), XStringFormats.TopLeft);
                y += 22;
                gfx.DrawLine(XPens.LightGray, margen, y, anchoPagina - margen, y);
                y += 16;

                gfx.DrawString(tituloDocumento, fuenteSeccion, XBrushes.Black, Rect(margen, y, anchoUtil, 20), XStringFormats.TopLeft);
                y += 24;
                gfx.DrawString(paciente.Nombre + " " + paciente.Apellido, fuenteTexto, XBrushes.Black, Rect(margen, y, anchoUtil, 16), XStringFormats.TopLeft);
                y += 16;
                gfx.DrawString("DNI: " + paciente.DNI + "   ·   Rango: " + resumen.RangoDesde.ToString("dd/MM/yyyy") + " al " + resumen.RangoHasta.ToString("dd/MM/yyyy"), fuenteTexto, XBrushes.Gray, Rect(margen, y, anchoUtil, 16), XStringFormats.TopLeft);
                y += 24;
                gfx.DrawLine(XPens.LightGray, margen, y, anchoPagina - margen, y);
                y += 16;

                if (secciones != null)
                {
                    y = EscribirSeccion(ref gfx, documento, ref pagina, "Contexto General", secciones.ContextoGeneral, margen, y, anchoUtil, fuenteSeccion, fuenteTexto);
                    y = EscribirSeccion(ref gfx, documento, ref pagina, "Evolución", secciones.Evolucion, margen, y, anchoUtil, fuenteSeccion, fuenteTexto);
                    y = EscribirSeccion(ref gfx, documento, ref pagina, "Temas Recurrentes", secciones.TemasRecurrentes, margen, y, anchoUtil, fuenteSeccion, fuenteTexto);
                    y = EscribirSeccion(ref gfx, documento, ref pagina, "Intervenciones", secciones.Intervenciones, margen, y, anchoUtil, fuenteSeccion, fuenteTexto);
                    y = EscribirSeccion(ref gfx, documento, ref pagina, "Observaciones", secciones.Observaciones, margen, y, anchoUtil, fuenteSeccion, fuenteTexto);
                }
                else
                {
                    gfx.DrawString("No se pudo recuperar el contenido del resumen.", fuenteTexto, XBrushes.Black, Rect(margen, y, anchoUtil, 16), XStringFormats.TopLeft);
                    y += 20;
                }

                altoPagina = pagina.Height;
                anchoPagina = pagina.Width;
                if (y > altoPagina - 60)
                {
                    pagina = documento.AddPage();
                    gfx = XGraphics.FromPdfPage(pagina);
                    y = margen;
                    altoPagina = pagina.Height;
                    anchoPagina = pagina.Width;
                }

                gfx.DrawLine(XPens.LightGray, margen, altoPagina - 50, anchoPagina - margen, altoPagina - 50);
                gfx.DrawString("Documento generado por Cambur · Contenido clínico confidencial, uso exclusivo del profesional autenticado.", fuenteAviso, XBrushes.Gray, Rect(margen, altoPagina - 42, anchoUtil, 20), XStringFormats.TopLeft);

                using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
                {
                    documento.Save(ms, false);
                    return ms.ToArray();
                }
            }
        }

        #endregion

        #region Generación PDF - Informe de Derivación IA

        private byte[] GenerarInformeDerivacionPdf(int idPsicologo, Paciente paciente, int? idDocumento, out string tituloDocumento)
        {
            tituloDocumento = "Informe de Derivación";

            GestorInformeDerivacion gestorInforme = new GestorInformeDerivacion();
            List<InformeDerivacion> informes = gestorInforme.ObtenerPorPaciente(paciente.IdPaciente)
                .Where(i => i.Estado == EstadoInforme.Auditado)
                .OrderByDescending(i => i.FechaAuditoria ?? i.FechaGeneracion)
                .ToList();

            InformeDerivacion informe = idDocumento.HasValue
                ? informes.FirstOrDefault(i => i.IdInforme == idDocumento.Value)
                : informes.FirstOrDefault();

            if (informe == null)
            {
                throw new ExcepcionTraducible("error_documento_no_disponible");
            }

            SeccionesInformeDerivacion secciones = gestorInforme.ObtenerSecciones(informe);

            string nombrePsicologo = "";
            if (GestorSesion.EstaAutenticado)
            {
                nombrePsicologo = "Prof. " + GestorSesion.PsicologoActual.Nombre + " " + GestorSesion.PsicologoActual.Apellido;
            }

            using (PdfDocument documento = new PdfDocument())
            {
                documento.Info.Title = tituloDocumento + " - " + paciente.Nombre + " " + paciente.Apellido;
                documento.Info.Author = nombrePsicologo;

                PdfPage pagina = documento.AddPage();
                pagina.Size = PdfSharp.PageSize.A4;
                XGraphics gfx = XGraphics.FromPdfPage(pagina);

                XFont fuenteTitulo = new XFont("Verdana", 18, XFontStyleEx.Bold);
                XFont fuenteSubtitulo = new XFont("Verdana", 10, XFontStyleEx.Regular);
                XFont fuenteSeccion = new XFont("Verdana", 12, XFontStyleEx.Bold);
                XFont fuenteTexto = new XFont("Verdana", 10, XFontStyleEx.Regular);
                XFont fuenteAviso = new XFont("Verdana", 8, XFontStyleEx.Italic);

                double margen = 40;
                double y = margen;
                double anchoPagina = pagina.Width;
                double altoPagina = pagina.Height;
                double anchoUtil = anchoPagina - (margen * 2);

                gfx.DrawString("CAMBUR", fuenteTitulo, XBrushes.DarkSlateBlue, Rect(margen, y, anchoUtil, 30), XStringFormats.TopLeft);
                gfx.DrawString((informe.FechaAuditoria ?? informe.FechaGeneracion).ToString("dd/MM/yyyy"), fuenteSubtitulo, XBrushes.Gray, Rect(margen, y, anchoUtil, 30), XStringFormats.TopRight);
                y += 32;
                gfx.DrawString(nombrePsicologo, fuenteSubtitulo, XBrushes.Gray, Rect(margen, y, anchoUtil, 16), XStringFormats.TopLeft);
                y += 22;
                gfx.DrawLine(XPens.LightGray, margen, y, anchoPagina - margen, y);
                y += 16;

                gfx.DrawString(tituloDocumento, fuenteSeccion, XBrushes.Black, Rect(margen, y, anchoUtil, 20), XStringFormats.TopLeft);
                y += 24;
                gfx.DrawString(paciente.Nombre + " " + paciente.Apellido, fuenteTexto, XBrushes.Black, Rect(margen, y, anchoUtil, 16), XStringFormats.TopLeft);
                y += 16;

                if (secciones != null)
                {
                    string metaLinea = "DNI: " + paciente.DNI;
                    if (!string.IsNullOrWhiteSpace(secciones.EspecialidadDerivacion))
                        metaLinea += "   ·   Especialidad destino: " + secciones.EspecialidadDerivacion;
                    if (!string.IsNullOrWhiteSpace(secciones.ProfesionalDestinatario))
                        metaLinea += "   ·   Destinatario: " + secciones.ProfesionalDestinatario;
                    gfx.DrawString(metaLinea, fuenteTexto, XBrushes.Gray, Rect(margen, y, anchoUtil, 16), XStringFormats.TopLeft);
                    y += 24;
                    gfx.DrawLine(XPens.LightGray, margen, y, anchoPagina - margen, y);
                    y += 16;

                    if (!string.IsNullOrWhiteSpace(secciones.Institucion))
                    {
                        y = EscribirSeccion(ref gfx, documento, ref pagina, "Institución", secciones.Institucion, margen, y, anchoUtil, fuenteSeccion, fuenteTexto);
                    }
                    y = EscribirSeccion(ref gfx, documento, ref pagina, "Motivo de Derivación", secciones.MotivoDerivacion, margen, y, anchoUtil, fuenteSeccion, fuenteTexto);
                    y = EscribirSeccion(ref gfx, documento, ref pagina, "Síntesis Diagnóstica", secciones.SintesisDiagnostica, margen, y, anchoUtil, fuenteSeccion, fuenteTexto);
                    y = EscribirSeccion(ref gfx, documento, ref pagina, "Andamiajes Implementados", secciones.Andamiajes, margen, y, anchoUtil, fuenteSeccion, fuenteTexto);
                    y = EscribirSeccion(ref gfx, documento, ref pagina, "Objetivos Terapéuticos", secciones.Objetivos, margen, y, anchoUtil, fuenteSeccion, fuenteTexto);
                    y = EscribirSeccion(ref gfx, documento, ref pagina, "Modalidad de Trabajo", secciones.ModalidadTrabajo, margen, y, anchoUtil, fuenteSeccion, fuenteTexto);

                    if (!string.IsNullOrWhiteSpace(secciones.Firma))
                    {
                        y += 8;
                        y = EscribirSeccion(ref gfx, documento, ref pagina, "Firma Profesional", secciones.Firma, margen, y, anchoUtil, fuenteSeccion, fuenteTexto);
                    }
                }
                else
                {
                    gfx.DrawString("No se pudo recuperar el contenido del informe.", fuenteTexto, XBrushes.Black, Rect(margen, y, anchoUtil, 16), XStringFormats.TopLeft);
                    y += 20;
                }

                altoPagina = pagina.Height;
                anchoPagina = pagina.Width;
                if (y > altoPagina - 60)
                {
                    pagina = documento.AddPage();
                    gfx = XGraphics.FromPdfPage(pagina);
                    altoPagina = pagina.Height;
                    anchoPagina = pagina.Width;
                }

                gfx.DrawLine(XPens.LightGray, margen, altoPagina - 50, anchoPagina - margen, altoPagina - 50);
                gfx.DrawString("Documento generado por Cambur · Contenido clínico confidencial, uso exclusivo del profesional autenticado.", fuenteAviso, XBrushes.Gray, Rect(margen, altoPagina - 42, anchoUtil, 20), XStringFormats.TopLeft);

                using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
                {
                    documento.Save(ms, false);
                    return ms.ToArray();
                }
            }
        }

        #endregion

        #region Generación PDF - Perfil Evolutivo del Paciente (IA)

        private byte[] GenerarPerfilPacientePdf(int idPsicologo, Paciente paciente, int? idDocumento, out string tituloDocumento)
        {
            tituloDocumento = "Perfil Evolutivo del Paciente";

            GestorPerfilPaciente gestorPerfil = new GestorPerfilPaciente();
            List<PerfilPaciente> perfiles = gestorPerfil.ObtenerPorPaciente(paciente.IdPaciente)
                .OrderByDescending(p => p.FechaGeneracion)
                .ToList();

            PerfilPaciente perfil = idDocumento.HasValue
                ? perfiles.FirstOrDefault(p => p.IdPerfil == idDocumento.Value)
                : perfiles.FirstOrDefault();

            if (perfil == null)
            {
                throw new ExcepcionTraducible("error_documento_no_disponible");
            }

            SeccionesPerfilPaciente secciones = gestorPerfil.ObtenerSecciones(perfil);

            string nombrePsicologo = "";
            if (GestorSesion.EstaAutenticado)
            {
                nombrePsicologo = "Prof. " + GestorSesion.PsicologoActual.Nombre + " " + GestorSesion.PsicologoActual.Apellido;
            }

            using (PdfDocument documento = new PdfDocument())
            {
                documento.Info.Title = tituloDocumento + " - " + paciente.Nombre + " " + paciente.Apellido;
                documento.Info.Author = nombrePsicologo;

                PdfPage pagina = documento.AddPage();
                pagina.Size = PdfSharp.PageSize.A4;
                XGraphics gfx = XGraphics.FromPdfPage(pagina);

                XFont fuenteTitulo = new XFont("Verdana", 18, XFontStyleEx.Bold);
                XFont fuenteSubtitulo = new XFont("Verdana", 10, XFontStyleEx.Regular);
                XFont fuenteSeccion = new XFont("Verdana", 12, XFontStyleEx.Bold);
                XFont fuenteTexto = new XFont("Verdana", 10, XFontStyleEx.Regular);
                XFont fuenteAviso = new XFont("Verdana", 8, XFontStyleEx.Italic);

                double margen = 40;
                double y = margen;
                double anchoPagina = pagina.Width;
                double altoPagina = pagina.Height;
                double anchoUtil = anchoPagina - (margen * 2);

                gfx.DrawString("CAMBUR", fuenteTitulo, XBrushes.DarkSlateBlue, Rect(margen, y, anchoUtil, 30), XStringFormats.TopLeft);
                gfx.DrawString(perfil.FechaGeneracion.ToString("dd/MM/yyyy"), fuenteSubtitulo, XBrushes.Gray, Rect(margen, y, anchoUtil, 30), XStringFormats.TopRight);
                y += 32;
                gfx.DrawString(nombrePsicologo, fuenteSubtitulo, XBrushes.Gray, Rect(margen, y, anchoUtil, 16), XStringFormats.TopLeft);
                y += 22;
                gfx.DrawLine(XPens.LightGray, margen, y, anchoPagina - margen, y);
                y += 16;

                gfx.DrawString(tituloDocumento, fuenteSeccion, XBrushes.Black, Rect(margen, y, anchoUtil, 20), XStringFormats.TopLeft);
                y += 24;
                gfx.DrawString(paciente.Nombre + " " + paciente.Apellido, fuenteTexto, XBrushes.Black, Rect(margen, y, anchoUtil, 16), XStringFormats.TopLeft);
                y += 16;
                string modeloTexto = "DNI: " + paciente.DNI;
                if (secciones != null && !string.IsNullOrWhiteSpace(secciones.NombreModelo))
                {
                    modeloTexto += "   ·   Modelo de evaluación: " + secciones.NombreModelo;
                }
                gfx.DrawString(modeloTexto, fuenteTexto, XBrushes.Gray, Rect(margen, y, anchoUtil, 16), XStringFormats.TopLeft);
                y += 24;
                gfx.DrawLine(XPens.LightGray, margen, y, anchoPagina - margen, y);
                y += 16;

                if (secciones != null)
                {
                    y = EscribirSeccion(ref gfx, documento, ref pagina, "Descripción", secciones.Descripcion, margen, y, anchoUtil, fuenteSeccion, fuenteTexto);
                    y = EscribirSeccion(ref gfx, documento, ref pagina, "Dimensiones Evaluadas", secciones.Dimensiones, margen, y, anchoUtil, fuenteSeccion, fuenteTexto);
                    y = EscribirSeccion(ref gfx, documento, ref pagina, "Patrones Observados", secciones.Patrones, margen, y, anchoUtil, fuenteSeccion, fuenteTexto);
                    y = EscribirSeccion(ref gfx, documento, ref pagina, "Consideraciones", secciones.Consideraciones, margen, y, anchoUtil, fuenteSeccion, fuenteTexto);
                }
                else
                {
                    gfx.DrawString("No se pudo recuperar el contenido del perfil.", fuenteTexto, XBrushes.Black, Rect(margen, y, anchoUtil, 16), XStringFormats.TopLeft);
                    y += 20;
                }

                altoPagina = pagina.Height;
                anchoPagina = pagina.Width;
                if (y > altoPagina - 60)
                {
                    pagina = documento.AddPage();
                    gfx = XGraphics.FromPdfPage(pagina);
                    altoPagina = pagina.Height;
                    anchoPagina = pagina.Width;
                }

                gfx.DrawLine(XPens.LightGray, margen, altoPagina - 50, anchoPagina - margen, altoPagina - 50);
                gfx.DrawString("Documento generado por Cambur · Contenido clínico confidencial, uso exclusivo del profesional autenticado.", fuenteAviso, XBrushes.Gray, Rect(margen, altoPagina - 42, anchoUtil, 20), XStringFormats.TopLeft);

                using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
                {
                    documento.Save(ms, false);
                    return ms.ToArray();
                }
            }
        }

        #endregion

        #region Utilidades de armado de PDF

        private double EscribirSeccion(ref XGraphics gfx, PdfDocument documento, ref PdfPage pagina, string titulo, string contenido, double margen, double y, double anchoUtil, XFont fuenteSeccion, XFont fuenteTexto)
        {
            if (string.IsNullOrWhiteSpace(contenido)) return y;

            double altoPagina = pagina.Height;

            if (y > altoPagina - 100)
            {
                pagina = documento.AddPage();
                gfx = XGraphics.FromPdfPage(pagina);
                y = margen;
                altoPagina = pagina.Height;
            }

            gfx.DrawString(titulo, fuenteSeccion, XBrushes.DarkSlateBlue, Rect(margen, y, anchoUtil, 18), XStringFormats.TopLeft);
            y += 20;

            foreach (string linea in DividirEnLineas(contenido, fuenteTexto, gfx, anchoUtil))
            {
                if (y > altoPagina - 60)
                {
                    pagina = documento.AddPage();
                    gfx = XGraphics.FromPdfPage(pagina);
                    y = margen;
                    altoPagina = pagina.Height;
                }
                gfx.DrawString(linea, fuenteTexto, XBrushes.Black, Rect(margen, y, anchoUtil, 14), XStringFormats.TopLeft);
                y += 14;
            }

            y += 12;
            return y;
        }

        private List<string> DividirEnLineas(string texto, XFont fuente, XGraphics gfx, double anchoMaximo)
        {
            List<string> lineas = new List<string>();
            foreach (string parrafo in texto.Replace("\r\n", "\n").Split('\n'))
            {
                string lineaActual = "";
                foreach (string palabra in parrafo.Split(' '))
                {
                    string prueba = lineaActual.Length == 0 ? palabra : lineaActual + " " + palabra;
                    if (gfx.MeasureString(prueba, fuente).Width > anchoMaximo && lineaActual.Length > 0)
                    {
                        lineas.Add(lineaActual);
                        lineaActual = palabra;
                    }
                    else
                    {
                        lineaActual = prueba;
                    }
                }
                lineas.Add(lineaActual);
            }
            return lineas;
        }
        private XRect Rect(double x, double y, double ancho, double alto)
        {
            return new XRect(XUnit.FromPoint(x), XUnit.FromPoint(y), XUnit.FromPoint(ancho), XUnit.FromPoint(alto));
        }

        private string ArmarNombreArchivo(Paciente paciente, string tituloDocumento)
        {
            string baseNombre = tituloDocumento.Replace(" ", "_") + "_" + paciente.Apellido + "_" + paciente.Nombre;
            baseNombre = string.Join("", baseNombre.Split(System.IO.Path.GetInvalidFileNameChars()));
            return baseNombre + "_" + DateTime.Today.ToString("yyyyMMdd") + ".pdf";
        }

        #endregion
    }
}