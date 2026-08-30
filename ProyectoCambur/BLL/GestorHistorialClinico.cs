using BE;
using DAL;
using SERVICIOS;
using System;

namespace BLL
{
    public class GestorHistorialClinico
    {
        private const string TABLA = "HistorialClinico";

        #region CUN04 - Generar Historial Clínico
        public int Alta(HistorialClinico historialAlta)
        {
            ValidarPropiedadPaciente(historialAlta.IdPaciente);
            ValidarCamposObligatorios(historialAlta);

            HistorialClinicoDAL historialDAL = new HistorialClinicoDAL();
            if (historialDAL.BuscarPorPaciente(historialAlta.IdPaciente) != null)
            {
                throw new ExcepcionTraducible("error_historial_ya_existe");
            }
            historialAlta.FechaRegistro = DateTime.Now;
            EncriptarCamposSensibles(historialAlta);
            int idGenerado = historialDAL.Alta(historialAlta);
            DigitoVerificador digitoVerificador = new DigitoVerificador();
            digitoVerificador.ActualizarDVH(historialAlta, TABLA);
            GestorBitacora gestorBitacora = new GestorBitacora();

            gestorBitacora.RegistrarEvento(EventosBitacora.MOD_HISTORIAL_CLINICO, EventosBitacora.DESC_INCORP_HISTORIAL, EventosBitacora.CRIT_INCORP_HISTORIAL);

            return idGenerado;
        }
        public void Modificar(HistorialClinico historialModificado)
        {
            ValidarPropiedadPaciente(historialModificado.IdPaciente);
            ValidarCamposObligatorios(historialModificado);

            EncriptarCamposSensibles(historialModificado);

            HistorialClinicoDAL historialDAL = new HistorialClinicoDAL();
            historialDAL.Modificar(historialModificado);

            DigitoVerificador digitoVerificador = new DigitoVerificador();
            digitoVerificador.ActualizarDVH(historialModificado, TABLA);

            GestorBitacora gestorBitacora = new GestorBitacora();
            gestorBitacora.RegistrarEvento(EventosBitacora.MOD_HISTORIAL_CLINICO, EventosBitacora.DESC_MODIF_HISTORIAL, EventosBitacora.CRIT_MODIF_HISTORIAL);
        }

        #endregion

        #region Busquedas HistorialClinico

        public HistorialClinico BuscarPorId(int idHistorial)
        {
            HistorialClinicoDAL historialDAL = new HistorialClinicoDAL();
            HistorialClinico historial = historialDAL.BuscarPorId(idHistorial);
            if (historial == null) return null;

            DesencriptarCamposSensibles(historial);
            return historial;
        }
        public HistorialClinico BuscarPorPaciente(int idPaciente)
        {
            HistorialClinicoDAL historialDAL = new HistorialClinicoDAL();
            HistorialClinico historial = historialDAL.BuscarPorPaciente(idPaciente);
            if (historial == null) return null;

            DesencriptarCamposSensibles(historial);
            return historial;
        }

        #endregion

        #region Encriptacion

        private void EncriptarCamposSensibles(HistorialClinico historial)
        {
            try
            {
                Cifrador cifrador = Cifrador.GestorCifrador;

                historial.HabitosNocivos = cifrador.EncriptarReversible(historial.HabitosNocivos);
                historial.ContextoFamiliar = cifrador.EncriptarReversible(historial.ContextoFamiliar);
                historial.AntecedentesFamiliares = cifrador.EncriptarReversible(historial.AntecedentesFamiliares);
                historial.AntecedentesMedicos = cifrador.EncriptarReversible(historial.AntecedentesMedicos);
                historial.SituacionLaboral = cifrador.EncriptarReversible(historial.SituacionLaboral);
                historial.EventosTraumaticos = cifrador.EncriptarReversible(historial.EventosTraumaticos);
            }
            catch (Exception)
            {
                throw new ExcepcionTraducible("error_historial_encriptacion");
            }
        }

        private void DesencriptarCamposSensibles(HistorialClinico historial)
        {
            Cifrador cifrador = Cifrador.GestorCifrador;

            historial.HabitosNocivos = cifrador.DesencriptarReversible(historial.HabitosNocivos);
            historial.ContextoFamiliar = cifrador.DesencriptarReversible(historial.ContextoFamiliar);
            historial.AntecedentesFamiliares = cifrador.DesencriptarReversible(historial.AntecedentesFamiliares);
            historial.AntecedentesMedicos = cifrador.DesencriptarReversible(historial.AntecedentesMedicos);
            historial.SituacionLaboral = cifrador.DesencriptarReversible(historial.SituacionLaboral);
            historial.EventosTraumaticos = cifrador.DesencriptarReversible(historial.EventosTraumaticos);
        }

        #endregion

        #region Validaciones

        private void ValidarPropiedadPaciente(int idPaciente)
        {
            GestorPaciente gestorPaciente = new GestorPaciente();
            Paciente paciente = gestorPaciente.BuscarPorId(idPaciente);
            if (paciente == null)
            {
                throw new ExcepcionTraducible("error_consulta_sin_paciente");
            }
        }
        private void ValidarCamposObligatorios(HistorialClinico historial)
        {
            if (string.IsNullOrWhiteSpace(historial.HabitosNocivos) ||
                string.IsNullOrWhiteSpace(historial.ContextoFamiliar) ||
                string.IsNullOrWhiteSpace(historial.AntecedentesFamiliares) ||
                string.IsNullOrWhiteSpace(historial.AntecedentesMedicos) ||
                string.IsNullOrWhiteSpace(historial.SituacionLaboral) ||
                string.IsNullOrWhiteSpace(historial.EventosTraumaticos))
            {
                throw new ExcepcionTraducible("error_historial_campos_incompletos");
            }
        }

        #endregion
    }
}