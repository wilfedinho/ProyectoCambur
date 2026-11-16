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

        public List<EventoTimeline> ObtenerLineaTemporal(int idPsicologo, int idPaciente, string tipoFiltro, DateTime? desde, DateTime? hasta, Func<string, string> traducir)
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
                    eventos.Add(EventoTimeline.DesdeConsulta(c, traducir));
                }
            }

            if (todos || tipoFiltro == TIPO_HISTORIAL)
            {
                GestorHistorialClinico gestorHistorial = new GestorHistorialClinico();
                HistorialClinico h = gestorHistorial.BuscarPorPaciente(idPaciente);
                if (h != null)
                {
                    eventos.Add(EventoTimeline.DesdeHistorial(h, traducir));
                }
            }

            if (todos || tipoFiltro == TIPO_RESUMEN_IA)
            {
                GestorResumenClinico gestorResumen = new GestorResumenClinico();
                foreach (ResumenClinico r in gestorResumen.ObtenerPorPaciente(idPaciente))
                {
                    SeccionesResumenClinico secciones = gestorResumen.ObtenerSecciones(r);
                    eventos.Add(EventoTimeline.DesdeResumenClinico(r, secciones, traducir));
                }
            }

            if (todos || tipoFiltro == TIPO_PERFILACION)
            {
                GestorPerfilPaciente gestorPerfil = new GestorPerfilPaciente();
                foreach (PerfilPaciente p in gestorPerfil.ObtenerPorPaciente(idPaciente))
                {
                    SeccionesPerfilPaciente secciones = gestorPerfil.ObtenerSecciones(p);
                    eventos.Add(EventoTimeline.DesdePerfilPaciente(p, secciones, traducir));
                }
            }

            if (todos || tipoFiltro == TIPO_INFORME_DERIVACION)
            {
                GestorInformeDerivacion gestorInforme = new GestorInformeDerivacion();
                foreach (InformeDerivacion inf in gestorInforme.ObtenerPorPaciente(idPaciente))
                {
                    SeccionesInformeDerivacion secciones = gestorInforme.ObtenerSecciones(inf);
                    eventos.Add(EventoTimeline.DesdeInformeDerivacion(inf, secciones, traducir));
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

        public static EventoTimeline DesdeConsulta(Consulta c, Func<string, string> t)
        {
            return new EventoTimeline
            {
                IdEvento = c.IdConsulta,
                Tipo = GestorLineaTemporal.TIPO_CONSULTA,
                TipoLabel = t("lt_tipo_consulta"),
                TipoCss = "consulta",
                Icono = "🗒️",
                Fecha = c.FechaConsulta,
                Resumen = TruncarTexto(!string.IsNullOrWhiteSpace(c.Diagnostico) ? c.Diagnostico : c.Objetivos, t),
                Detalle = ArmarDetalleConsulta(c, t),
                Duracion = c.TiempoConsulta
            };
        }

        public static EventoTimeline DesdeHistorial(HistorialClinico h, Func<string, string> t)
        {
            return new EventoTimeline
            {
                IdEvento = h.IdHistorial,
                Tipo = GestorLineaTemporal.TIPO_HISTORIAL,
                TipoLabel = t("lt_tipo_historial"),
                TipoCss = "historial",
                Icono = "📋",
                Fecha = h.FechaRegistro,
                Resumen = t("lt_resumen_historial"),
                Detalle = ArmarDetalleHistorial(h, t)
            };
        }

        public static EventoTimeline DesdeResumenClinico(ResumenClinico r, SeccionesResumenClinico secciones, Func<string, string> t)
        {
            return new EventoTimeline
            {
                IdEvento = r.IdResumen,
                Tipo = GestorLineaTemporal.TIPO_RESUMEN_IA,
                TipoLabel = t("lt_tipo_resumen_ia"),
                TipoCss = "evento",
                Icono = "🤖",
                Fecha = r.FechaGeneracion,
                Resumen = TruncarTexto(secciones != null ? secciones.ContextoGeneral : null, t),
                Detalle = ArmarDetalleResumen(secciones, t)
            };
        }

        public static EventoTimeline DesdePerfilPaciente(PerfilPaciente p, SeccionesPerfilPaciente secciones, Func<string, string> t)
        {
            string nombreModelo = secciones != null && !string.IsNullOrWhiteSpace(secciones.NombreModelo)
                ? secciones.NombreModelo
                : t("lt_perfilacion_default");
            return new EventoTimeline
            {
                IdEvento = p.IdPerfil,
                Tipo = GestorLineaTemporal.TIPO_PERFILACION,
                TipoLabel = t("lt_tipo_perfilacion"),
                TipoCss = "evento",
                Icono = "🧭",
                Fecha = p.FechaGeneracion,
                Resumen = TruncarTexto(nombreModelo + (secciones != null && !string.IsNullOrWhiteSpace(secciones.Descripcion) ? ": " + secciones.Descripcion : ""), t),
                Detalle = ArmarDetallePerfil(secciones, t)
            };
        }

        public static EventoTimeline DesdeInformeDerivacion(InformeDerivacion inf, SeccionesInformeDerivacion secciones, Func<string, string> t)
        {
            bool auditado = inf.Estado == EstadoInforme.Auditado;
            return new EventoTimeline
            {
                IdEvento = inf.IdInforme,
                Tipo = GestorLineaTemporal.TIPO_INFORME_DERIVACION,
                TipoLabel = t("lt_tipo_informe_derivacion"),
                TipoCss = "evento",
                Icono = "📄",
                Fecha = inf.FechaGeneracion,
                Resumen = TruncarTexto(
                    (auditado ? t("lt_informe_firmado_prefijo") : t("lt_informe_borrador_prefijo")) +
                    (secciones != null ? secciones.MotivoDerivacion : null), t),
                Detalle = ArmarDetalleInforme(secciones, auditado, t)
            };
        }

        private static string TruncarTexto(string texto, Func<string, string> t)
        {
            if (string.IsNullOrWhiteSpace(texto)) return t("lt_sin_detalle");
            texto = texto.Trim();
            return texto.Length > 160 ? texto.Substring(0, 160) + "…" : texto;
        }

        private static string ArmarDetalleConsulta(Consulta c, Func<string, string> t)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            AgregarSiHayValor(sb, t("lbl_objetivos"), c.Objetivos);
            AgregarSiHayValor(sb, t("lbl_observaciones"), c.Observaciones);
            AgregarSiHayValor(sb, t("lbl_hipotesis"), c.Hipotesis);
            AgregarSiHayValor(sb, t("lbl_intervenciones"), c.Intervenciones);
            AgregarSiHayValor(sb, t("lbl_evolucion_observada"), c.EvolucionObservada);
            AgregarSiHayValor(sb, t("lbl_diagnostico"), c.Diagnostico);
            AgregarSiHayValor(sb, t("lbl_tratamiento"), c.Tratamiento);
            return sb.Length > 0 ? sb.ToString() : t("lt_sin_detalle_consulta");
        }

        private static string ArmarDetalleHistorial(HistorialClinico h, Func<string, string> t)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            AgregarSiHayValor(sb, t("lt_lbl_habitos_nocivos"), h.HabitosNocivos);
            AgregarSiHayValor(sb, t("lt_lbl_contexto_familiar"), h.ContextoFamiliar);
            AgregarSiHayValor(sb, t("lt_lbl_antecedentes_familiares"), h.AntecedentesFamiliares);
            AgregarSiHayValor(sb, t("lt_lbl_antecedentes_medicos"), h.AntecedentesMedicos);
            AgregarSiHayValor(sb, t("lt_lbl_situacion_laboral"), h.SituacionLaboral);
            AgregarSiHayValor(sb, t("lt_lbl_eventos_traumaticos"), h.EventosTraumaticos);
            return sb.Length > 0 ? sb.ToString() : t("lt_sin_detalle");
        }

        private static string ArmarDetalleResumen(SeccionesResumenClinico secciones, Func<string, string> t)
        {
            if (secciones == null) return t("lt_sin_detalle");
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            AgregarSiHayValor(sb, t("lt_lbl_contexto_general"), secciones.ContextoGeneral);
            AgregarSiHayValor(sb, t("lt_lbl_evolucion"), secciones.Evolucion);
            AgregarSiHayValor(sb, t("lt_lbl_temas_recurrentes"), secciones.TemasRecurrentes);
            AgregarSiHayValor(sb, t("lbl_intervenciones"), secciones.Intervenciones);
            AgregarSiHayValor(sb, t("lbl_observaciones"), secciones.Observaciones);
            return sb.Length > 0 ? sb.ToString() : t("lt_sin_detalle");
        }

        private static string ArmarDetallePerfil(SeccionesPerfilPaciente secciones, Func<string, string> t)
        {
            if (secciones == null) return t("lt_sin_detalle");
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            AgregarSiHayValor(sb, t("lt_lbl_modelo"), secciones.NombreModelo);
            AgregarSiHayValor(sb, t("lt_lbl_descripcion"), secciones.Descripcion);
            AgregarSiHayValor(sb, t("lt_lbl_dimensiones"), secciones.Dimensiones);
            AgregarSiHayValor(sb, t("lt_lbl_patrones"), secciones.Patrones);
            AgregarSiHayValor(sb, t("lt_lbl_consideraciones"), secciones.Consideraciones);
            return sb.Length > 0 ? sb.ToString() : t("lt_sin_detalle");
        }

        private static string ArmarDetalleInforme(SeccionesInformeDerivacion secciones, bool auditado, Func<string, string> t)
        {
            if (secciones == null) return t("lt_sin_detalle");
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            AgregarSiHayValor(sb, t("lt_lbl_estado"), auditado ? t("lt_estado_firmado") : t("lt_estado_borrador_sin_firmar"));
            AgregarSiHayValor(sb, t("lbl_especialidad_derivacion"), secciones.EspecialidadDerivacion);
            AgregarSiHayValor(sb, t("lbl_profesional_destinatario"), secciones.ProfesionalDestinatario);
            AgregarSiHayValor(sb, t("lt_lbl_institucion"), secciones.Institucion);
            AgregarSiHayValor(sb, t("lbl_motivo_derivacion"), secciones.MotivoDerivacion);
            AgregarSiHayValor(sb, t("lbl_sintesis_diagnostica"), secciones.SintesisDiagnostica);
            AgregarSiHayValor(sb, t("lt_lbl_andamiajes"), secciones.Andamiajes);
            AgregarSiHayValor(sb, t("lbl_objetivos"), secciones.Objetivos);
            AgregarSiHayValor(sb, t("lbl_modalidad_trabajo"), secciones.ModalidadTrabajo);
            if (auditado) AgregarSiHayValor(sb, t("lt_lbl_firma"), secciones.Firma);
            return sb.Length > 0 ? sb.ToString() : t("lt_sin_detalle");
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