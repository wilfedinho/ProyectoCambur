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

        public List<EventoTimeline> ObtenerLineaTemporal(int idPsicologo, int idPaciente, string tipoFiltro, DateTime? desde, DateTime? hasta)
        {
            GestorPaciente gestorPaciente = new GestorPaciente();
            Paciente paciente = gestorPaciente.BuscarPorId(idPaciente);
            if (paciente == null || paciente.IdPsicologo != idPsicologo)
            {
                throw new ExcepcionTraducible("error_paciente_no_propio");
            }

            List<EventoTimeline> eventos = new List<EventoTimeline>();

            if (string.IsNullOrEmpty(tipoFiltro) || tipoFiltro == "TODOS" || tipoFiltro == TIPO_CONSULTA)
            {
                GestorConsulta gestorConsulta = new GestorConsulta();
                foreach (Consulta c in gestorConsulta.ObtenerPorPaciente(idPaciente))
                {
                    eventos.Add(EventoTimeline.DesdeConsulta(c));
                }
            }

            if (string.IsNullOrEmpty(tipoFiltro) || tipoFiltro == "TODOS" || tipoFiltro == TIPO_HISTORIAL)
            {
                GestorHistorialClinico gestorHistorial = new GestorHistorialClinico();
                HistorialClinico h = gestorHistorial.BuscarPorPaciente(idPaciente);
                if (h != null)
                {
                    eventos.Add(EventoTimeline.DesdeHistorial(h));
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