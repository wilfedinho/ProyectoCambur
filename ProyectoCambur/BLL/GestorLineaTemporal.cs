using BE;
using SERVICIOS;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BLL
{
    public class GestorLineaTemporal
    {
        public const string TIPO_CONSULTA = "CONSULTA";
        public const string TIPO_HISTORIAL = "HISTORIAL";
        public const string TIPO_RESUMEN_IA = "RESUMEN_IA";
        public const string TIPO_PERFILACION = "PERFILACION";
        public const string TIPO_INFORME_DERIVACION = "INFORME_DERIVACION";

        public List<EventoTimeline> ObtenerLineaTemporal(int idPsicologo, int idPaciente, string tipoFiltro, DateTime? desde, DateTime? hasta)
        {
            GestorPaciente gestorPaciente = new GestorPaciente();
            Paciente paciente = gestorPaciente.BuscarPorId(idPaciente);
            if (paciente == null || paciente.IdPsicologo != idPsicologo)
            {
                throw new ExcepcionTraducible("error_paciente_no_propio");
            }

            bool todos = string.IsNullOrEmpty(tipoFiltro) || tipoFiltro == "TODOS";
            List<EventoTimeline> eventos = new List<EventoTimeline>();

            if (todos || tipoFiltro == TIPO_CONSULTA)
            {
                GestorConsulta gestorConsulta = new GestorConsulta();
                foreach (Consulta c in gestorConsulta.ObtenerPorPaciente(idPaciente))
                {
                    eventos.Add(EventoTimeline.DesdeConsulta(c));
                }
            }

            if (todos || tipoFiltro == TIPO_HISTORIAL)
            {
                GestorHistorialClinico gestorHistorial = new GestorHistorialClinico();
                HistorialClinico h = gestorHistorial.BuscarPorPaciente(idPaciente);
                if (h != null)
                {
                    eventos.Add(EventoTimeline.DesdeHistorial(h));
                }
            }

            if (todos || tipoFiltro == TIPO_RESUMEN_IA)
            {
                GestorResumenClinico gestorResumen = new GestorResumenClinico();
                foreach (ResumenClinico r in gestorResumen.ObtenerPorPaciente(idPaciente))
                {
                    SeccionesResumenClinico secciones = gestorResumen.ObtenerSecciones(r);
                    eventos.Add(EventoTimeline.DesdeResumenClinico(r, secciones));
                }
            }

            if (todos || tipoFiltro == TIPO_PERFILACION)
            {
                GestorPerfilPaciente gestorPerfil = new GestorPerfilPaciente();
                foreach (PerfilPaciente p in gestorPerfil.ObtenerPorPaciente(idPaciente))
                {
                    SeccionesPerfilPaciente secciones = gestorPerfil.ObtenerSecciones(p);
                    eventos.Add(EventoTimeline.DesdePerfilPaciente(p, secciones));
                }
            }

            if (todos || tipoFiltro == TIPO_INFORME_DERIVACION)
            {
                GestorInformeDerivacion gestorInforme = new GestorInformeDerivacion();
                foreach (InformeDerivacion inf in gestorInforme.ObtenerPorPaciente(idPaciente))
                {
                    SeccionesInformeDerivacion secciones = gestorInforme.ObtenerSecciones(inf);
                    eventos.Add(EventoTimeline.DesdeInformeDerivacion(inf, secciones));
                }
            }

            if (desde.HasValue)
            {
                eventos = eventos.Where(e => e.Fecha.Date >= desde.Value.Date).ToList();
            }
            if (hasta.HasValue)
            {
                eventos = eventos.Where(e => e.Fecha.Date <= hasta.Value.Date).ToList();
            }

            return eventos.OrderByDescending(e => e.Fecha).ToList();
        }
    }
    public class EventoTimeline
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

        public static EventoTimeline DesdeConsulta(Consulta c)
        {
            return new EventoTimeline
            {
                IdEvento = c.IdConsulta,
                Tipo = GestorLineaTemporal.TIPO_CONSULTA,
                TipoLabel = "Consulta",
                TipoCss = "consulta",
                Icono = "🗒️",
                Fecha = c.FechaConsulta,
                Resumen = TruncarTexto(!string.IsNullOrWhiteSpace(c.Diagnostico) ? c.Diagnostico : c.Objetivos),
                Detalle = ArmarDetalleConsulta(c),
                Duracion = c.TiempoConsulta
            };
        }

        public static EventoTimeline DesdeHistorial(HistorialClinico h)
        {
            return new EventoTimeline
            {
                IdEvento = h.IdHistorial,
                Tipo = GestorLineaTemporal.TIPO_HISTORIAL,
                TipoLabel = "Historial Clínico",
                TipoCss = "historial",
                Icono = "📋",
                Fecha = h.FechaRegistro,
                Resumen = "Historial clínico incorporado (hábitos, contexto familiar y antecedentes).",
                Detalle = ArmarDetalleHistorial(h)
            };
        }

        public static EventoTimeline DesdeResumenClinico(ResumenClinico r, SeccionesResumenClinico secciones)
        {
            return new EventoTimeline
            {
                IdEvento = r.IdResumen,
                Tipo = GestorLineaTemporal.TIPO_RESUMEN_IA,
                TipoLabel = "Resumen IA",
                TipoCss = "evento",
                Icono = "🤖",
                Fecha = r.FechaGeneracion,
                Resumen = TruncarTexto(secciones != null ? secciones.ContextoGeneral : null),
                Detalle = ArmarDetalleResumen(secciones)
            };
        }

        public static EventoTimeline DesdePerfilPaciente(PerfilPaciente p, SeccionesPerfilPaciente secciones)
        {
            string nombreModelo = secciones != null && !string.IsNullOrWhiteSpace(secciones.NombreModelo)
                ? secciones.NombreModelo
                : "Perfilación del paciente";
            return new EventoTimeline
            {
                IdEvento = p.IdPerfil,
                Tipo = GestorLineaTemporal.TIPO_PERFILACION,
                TipoLabel = "Perfilación",
                TipoCss = "evento",
                Icono = "🧭",
                Fecha = p.FechaGeneracion,
                Resumen = TruncarTexto(nombreModelo + (secciones != null && !string.IsNullOrWhiteSpace(secciones.Descripcion) ? ": " + secciones.Descripcion : "")),
                Detalle = ArmarDetallePerfil(secciones)
            };
        }

        public static EventoTimeline DesdeInformeDerivacion(InformeDerivacion inf, SeccionesInformeDerivacion secciones)
        {
            bool auditado = inf.Estado == EstadoInforme.Auditado;
            return new EventoTimeline
            {
                IdEvento = inf.IdInforme,
                Tipo = GestorLineaTemporal.TIPO_INFORME_DERIVACION,
                TipoLabel = "Informe de Derivación",
                TipoCss = "evento",
                Icono = "📄",
                Fecha = inf.FechaGeneracion,
                Resumen = TruncarTexto(
                    (auditado ? "Firmado. " : "Borrador sin firmar. ") +
                    (secciones != null ? secciones.MotivoDerivacion : null)),
                Detalle = ArmarDetalleInforme(secciones, auditado)
            };
        }

        private static string TruncarTexto(string texto)
        {
            if (string.IsNullOrWhiteSpace(texto)) return "Sin detalle registrado.";
            texto = texto.Trim();
            return texto.Length > 160 ? texto.Substring(0, 160) + "…" : texto;
        }

        private static string ArmarDetalleConsulta(Consulta c)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            AgregarSiHayValor(sb, "Objetivos", c.Objetivos);
            AgregarSiHayValor(sb, "Observaciones", c.Observaciones);
            AgregarSiHayValor(sb, "Hipótesis", c.Hipotesis);
            AgregarSiHayValor(sb, "Intervenciones", c.Intervenciones);
            AgregarSiHayValor(sb, "Evolución observada", c.EvolucionObservada);
            AgregarSiHayValor(sb, "Diagnóstico", c.Diagnostico);
            AgregarSiHayValor(sb, "Tratamiento", c.Tratamiento);
            return sb.Length > 0 ? sb.ToString() : "Sin detalle registrado para esta consulta.";
        }

        private static string ArmarDetalleHistorial(HistorialClinico h)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            AgregarSiHayValor(sb, "Hábitos nocivos", h.HabitosNocivos);
            AgregarSiHayValor(sb, "Contexto familiar", h.ContextoFamiliar);
            AgregarSiHayValor(sb, "Antecedentes familiares", h.AntecedentesFamiliares);
            AgregarSiHayValor(sb, "Antecedentes médicos", h.AntecedentesMedicos);
            AgregarSiHayValor(sb, "Situación laboral", h.SituacionLaboral);
            AgregarSiHayValor(sb, "Eventos traumáticos", h.EventosTraumaticos);
            return sb.Length > 0 ? sb.ToString() : "Sin detalle registrado.";
        }

        private static string ArmarDetalleResumen(SeccionesResumenClinico secciones)
        {
            if (secciones == null) return "Sin detalle registrado.";
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            AgregarSiHayValor(sb, "Contexto general", secciones.ContextoGeneral);
            AgregarSiHayValor(sb, "Evolución", secciones.Evolucion);
            AgregarSiHayValor(sb, "Temas recurrentes", secciones.TemasRecurrentes);
            AgregarSiHayValor(sb, "Intervenciones", secciones.Intervenciones);
            AgregarSiHayValor(sb, "Observaciones", secciones.Observaciones);
            return sb.Length > 0 ? sb.ToString() : "Sin detalle registrado.";
        }

        private static string ArmarDetallePerfil(SeccionesPerfilPaciente secciones)
        {
            if (secciones == null) return "Sin detalle registrado.";
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            AgregarSiHayValor(sb, "Modelo", secciones.NombreModelo);
            AgregarSiHayValor(sb, "Descripción", secciones.Descripcion);
            AgregarSiHayValor(sb, "Dimensiones", secciones.Dimensiones);
            AgregarSiHayValor(sb, "Patrones", secciones.Patrones);
            AgregarSiHayValor(sb, "Consideraciones", secciones.Consideraciones);
            return sb.Length > 0 ? sb.ToString() : "Sin detalle registrado.";
        }

        private static string ArmarDetalleInforme(SeccionesInformeDerivacion secciones, bool auditado)
        {
            if (secciones == null) return "Sin detalle registrado.";
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            AgregarSiHayValor(sb, "Estado", auditado ? "Firmado" : "Borrador sin firmar");
            AgregarSiHayValor(sb, "Especialidad", secciones.EspecialidadDerivacion);
            AgregarSiHayValor(sb, "Profesional destinatario", secciones.ProfesionalDestinatario);
            AgregarSiHayValor(sb, "Institución", secciones.Institucion);
            AgregarSiHayValor(sb, "Motivo de derivación", secciones.MotivoDerivacion);
            AgregarSiHayValor(sb, "Síntesis diagnóstica", secciones.SintesisDiagnostica);
            AgregarSiHayValor(sb, "Andamiajes", secciones.Andamiajes);
            AgregarSiHayValor(sb, "Objetivos", secciones.Objetivos);
            AgregarSiHayValor(sb, "Modalidad de trabajo", secciones.ModalidadTrabajo);
            if (auditado) AgregarSiHayValor(sb, "Firma", secciones.Firma);
            return sb.Length > 0 ? sb.ToString() : "Sin detalle registrado.";
        }

        private static void AgregarSiHayValor(System.Text.StringBuilder sb, string etiqueta, string valor)
        {
            if (!string.IsNullOrWhiteSpace(valor))
            {
                if (sb.Length > 0) sb.AppendLine();
                sb.Append(etiqueta + ": " + valor);
            }
        }
    }
}