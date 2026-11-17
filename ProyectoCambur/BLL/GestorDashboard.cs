using BE;
using SERVICIOS;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace BLL
{
    public class GestorDashboard
    {
        public const string PERIODO_SEMANA = "SEMANA";
        public const string PERIODO_MES = "MES";
        public const string PERIODO_TRIMESTRE = "TRIMESTRE";
        public const string PERIODO_ANIO = "ANIO";

        public DatosDashboard ObtenerIndicadores(int idPsicologo, string periodo, Func<string, string> traducir)
        {
            DateTime desdeActual, hastaActual, desdeAnterior, hastaAnterior;
            CalcularRango(periodo, out desdeActual, out hastaActual, out desdeAnterior, out hastaAnterior);
            GestorPaciente gestorPaciente = new GestorPaciente();
            List<Paciente> pacientes = gestorPaciente.ObtenerPorPsicologo(idPsicologo, soloActivos: false);
            GestorConsulta gestorConsulta = new GestorConsulta();
            List<Consulta> consultas = gestorConsulta.ObtenerPorPsicologo(idPsicologo);
            GestorResumenClinico gestorResumen = new GestorResumenClinico();
            List<ResumenClinico> resumenes = gestorResumen.ObtenerPorPsicologo(idPsicologo);
            int exportacionesActual = ContarExportaciones(idPsicologo, desdeActual, hastaActual);
            int exportacionesAnterior = ContarExportaciones(idPsicologo, desdeAnterior, hastaAnterior);
            DatosDashboard datos = new DatosDashboard();
            datos.TotalPacientes = pacientes.Count;
            datos.NuevosPacientes = pacientes.Count(p => EnRango(p.FechaRegistro, desdeActual, hastaActual));
            int nuevosAnterior = pacientes.Count(p => EnRango(p.FechaRegistro, desdeAnterior, hastaAnterior));
            datos.Consultas = consultas.Count(c => EnRango(c.FechaConsulta, desdeActual, hastaActual));
            int consultasAnterior = consultas.Count(c => EnRango(c.FechaConsulta, desdeAnterior, hastaAnterior));
            datos.ResumenesIA = resumenes.Count(r => EnRango(r.FechaGeneracion, desdeActual, hastaActual));
            datos.Derivaciones = 0;
            datos.Perfilaciones = 0;
            datos.Exportaciones = exportacionesActual;
            datos.DeltaPacientes = ArmarDeltaAbsoluto(pacientes.Count, pacientes.Count - datos.NuevosPacientes + nuevosAnterior, "dashboard_palabra_paciente_singular", "dashboard_palabra_paciente_plural", traducir);
            datos.DeltaNuevos = ArmarDeltaComparativo(datos.NuevosPacientes, nuevosAnterior, periodo, traducir);
            datos.DeltaConsultas = ArmarDeltaComparativo(datos.Consultas, consultasAnterior, periodo, traducir);
          
            datos.DeltaDerivaciones = null;
            datos.DeltaExportaciones = ArmarDeltaComparativo(datos.Exportaciones, exportacionesAnterior, periodo, traducir);
            datos.NotaPerfilaciones = null;

            return datos;
        }

        public List<PuntoGrafico> ObtenerGraficoConsultasPorMes(int idPsicologo, int cantidadMeses = 6)
        {
            GestorConsulta gestorConsulta = new GestorConsulta();
            List<Consulta> consultas = gestorConsulta.ObtenerPorPsicologo(idPsicologo);

            CultureInfo cultura = new CultureInfo("es-AR");
            List<PuntoGrafico> puntos = new List<PuntoGrafico>();
            DateTime mesCursor = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).AddMonths(-(cantidadMeses - 1));

            for (int i = 0; i < cantidadMeses; i++)
            {
                DateTime mesInicio = mesCursor.AddMonths(i);
                DateTime mesFin = mesInicio.AddMonths(1).AddDays(-1);
                int valor = consultas.Count(c => c.FechaConsulta.Date >= mesInicio.Date && c.FechaConsulta.Date <= mesFin.Date);
                string nombreMes = cultura.DateTimeFormat.GetMonthName(mesInicio.Month);
                nombreMes = char.ToUpper(nombreMes[0]) + nombreMes.Substring(1);

                puntos.Add(new PuntoGrafico
                {
                    Mes = nombreMes,
                    MesCorto = nombreMes.Length >= 3 ? nombreMes.Substring(0, 3) : nombreMes,
                    Valor = valor
                });
            }

            int maxValor = puntos.Count > 0 ? puntos.Max(p => p.Valor) : 0;
            foreach (PuntoGrafico p in puntos)
            {
                p.PctAltura = maxValor > 0 ? (int)Math.Round((double)p.Valor / maxValor * 100) : 0;
            }

            return puntos;
        }

        public List<Consulta> ObtenerUltimasConsultas(int idPsicologo, int cantidad = 5)
        {
            GestorConsulta gestorConsulta = new GestorConsulta();
            return gestorConsulta.ObtenerPorPsicologo(idPsicologo).OrderByDescending(c => c.FechaConsulta).Take(cantidad).ToList();
        }

        public List<Paciente> ObtenerPacientesActivos(int idPsicologo, int cantidad = 5)
        {
            GestorPaciente gestorPaciente = new GestorPaciente();
            GestorConsulta gestorConsulta = new GestorConsulta();

            List<Paciente> pacientes = gestorPaciente.ObtenerPorPsicologo(idPsicologo, soloActivos: true);
            List<Consulta> consultas = gestorConsulta.ObtenerPorPsicologo(idPsicologo);

            var ultimaPorPaciente = consultas.GroupBy(c => c.IdPaciente).ToDictionary(g => g.Key, g => g.Max(c => c.FechaConsulta));

            return pacientes.Where(p => ultimaPorPaciente.ContainsKey(p.IdPaciente)).OrderByDescending(p => ultimaPorPaciente[p.IdPaciente]).Take(cantidad).ToList();
        }

        public DateTime? ObtenerUltimaConsultaDe(int idPaciente, List<Consulta> consultasDelPsicologo)
        {
            var deEstePaciente = consultasDelPsicologo.Where(c => c.IdPaciente == idPaciente).ToList();
            return deEstePaciente.Count > 0 ? deEstePaciente.Max(c => c.FechaConsulta) : (DateTime?)null;
        }

        #region Exportaciones (bitácora)
        private int ContarExportaciones(int idPsicologo, DateTime desde, DateTime hasta)
        {
            string email = ObtenerEmailPsicologo(idPsicologo);
            if (string.IsNullOrEmpty(email)) return 0;

            GestorBitacora gestorBitacora = new GestorBitacora();
            List<Bitacora> eventos = gestorBitacora.ObtenerPorFiltros(desde, hasta, EventosBitacora.MOD_REPORTES, email, null);
            return eventos.Count(e => e.Descripcion == EventosBitacora.DESC_EXPORTAR_PDF);
        }

        private string ObtenerEmailPsicologo(int idPsicologo)
        {
            if (GestorSesion.EstaAutenticado && GestorSesion.PsicologoActual.IdPsicologo == idPsicologo)
            {
                return GestorSesion.PsicologoActual.Email;
            }
            return null;
        }

        #endregion

        #region Cálculo de rangos y deltas

        private void CalcularRango(string periodo, out DateTime desdeActual, out DateTime hastaActual, out DateTime desdeAnterior, out DateTime hastaAnterior)
        {
            DateTime hoy = DateTime.Today;
            hastaActual = hoy;

            switch (periodo)
            {
                case PERIODO_SEMANA:
                    desdeActual = hoy.AddDays(-6);
                    hastaAnterior = desdeActual.AddDays(-1);
                    desdeAnterior = hastaAnterior.AddDays(-6);
                    break;
                case PERIODO_TRIMESTRE:
                    desdeActual = hoy.AddMonths(-3).AddDays(1);
                    hastaAnterior = desdeActual.AddDays(-1);
                    desdeAnterior = hastaAnterior.AddMonths(-3).AddDays(1);
                    break;
                case PERIODO_ANIO:
                    desdeActual = hoy.AddYears(-1).AddDays(1);
                    hastaAnterior = desdeActual.AddDays(-1);
                    desdeAnterior = hastaAnterior.AddYears(-1).AddDays(1);
                    break;
                case PERIODO_MES:
                default:
                    desdeActual = hoy.AddMonths(-1).AddDays(1);
                    hastaAnterior = desdeActual.AddDays(-1);
                    desdeAnterior = hastaAnterior.AddMonths(-1).AddDays(1);
                    break;
            }
        }

        private bool EnRango(DateTime fecha, DateTime desde, DateTime hasta)
        {
            return fecha.Date >= desde.Date && fecha.Date <= hasta.Date;
        }

        private string ArmarDeltaAbsoluto(int totalActual, int totalAnterior, string claveSingular, string clavePlural, Func<string, string> t)
        {
            int diferencia = totalActual - totalAnterior;
            if (diferencia == 0) return "= " + t("dashboard_delta_sin_cambios");
            string palabra = Math.Abs(diferencia) == 1 ? t(claveSingular) : t(clavePlural);
            return (diferencia > 0 ? "↑ " : "↓ ") + Math.Abs(diferencia) + " " + palabra;
        }

        private string ArmarDeltaComparativo(int actual, int anterior, string periodo, Func<string, string> t)
        {
            string etiquetaPeriodo = EtiquetaPeriodoAnterior(periodo, t);
            if (anterior == 0 && actual == 0) return string.Format(t("dashboard_sin_actividad_en"), etiquetaPeriodo);
            if (anterior == 0) return "↑ " + string.Format(t("dashboard_vs_cero_en"), etiquetaPeriodo);

            double variacionPct = ((double)(actual - anterior) / anterior) * 100;
            if (Math.Abs(variacionPct) < 0.5) return "= " + string.Format(t("dashboard_igual_que"), etiquetaPeriodo);
            string flecha = variacionPct > 0 ? "↑" : "↓";
            return flecha + " " + string.Format(t("dashboard_pct_vs"), Math.Abs((int)Math.Round(variacionPct)), etiquetaPeriodo);
        }

        private string EtiquetaPeriodoAnterior(string periodo, Func<string, string> t)
        {
            switch (periodo)
            {
                case PERIODO_SEMANA: return t("dashboard_periodo_semana_anterior");
                case PERIODO_TRIMESTRE: return t("dashboard_periodo_trimestre_anterior");
                case PERIODO_ANIO: return t("dashboard_periodo_anio_anterior");
                case PERIODO_MES:
                default: return t("dashboard_periodo_mes_anterior");
            }
        }

        #endregion
    }

    public class DatosDashboard
    {
        public int TotalPacientes { get; set; }
        public int NuevosPacientes { get; set; }
        public int Consultas { get; set; }
        public int Derivaciones { get; set; }
        public int ResumenesIA { get; set; }
        public int Perfilaciones { get; set; }
        public int Exportaciones { get; set; }
        public string DeltaPacientes { get; set; }
        public string DeltaNuevos { get; set; }
        public string DeltaConsultas { get; set; }
        public string DeltaDerivaciones { get; set; }
        public string DeltaExportaciones { get; set; }
        public string NotaPerfilaciones { get; set; }
    }

    public class PuntoGrafico
    {
        public string Mes { get; set; }
        public string MesCorto { get; set; }
        public int Valor { get; set; }
        public int PctAltura { get; set; }
    }
}