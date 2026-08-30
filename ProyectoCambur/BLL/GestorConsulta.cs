using BE;
using DAL;
using SERVICIOS;
using System;
using System.Collections.Generic;

namespace BLL
{
    public class GestorConsulta
    {
        private const string TABLA = "Consulta";

        #region Operaciones Consulta
        public int Alta(Consulta consultaAlta)
        {
            ValidarDatosConsulta(consultaAlta);

            consultaAlta.FechaRegistro = DateTime.Now;
            EncriptarCamposSensibles(consultaAlta);

            ConsultaDAL consultaDAL = new ConsultaDAL();
            int idGenerado = consultaDAL.Alta(consultaAlta);

            DigitoVerificador digitoVerificador = new DigitoVerificador();
            digitoVerificador.ActualizarDVH(consultaAlta, TABLA);

            GestorBitacora gestorBitacora = new GestorBitacora();
            gestorBitacora.RegistrarEvento(EventosBitacora.MOD_CONSULTAS, EventosBitacora.DESC_REGISTRO_CONSULTA, EventosBitacora.CRIT_REGISTRO_CONSULTA);

            return idGenerado;
        }
        public const int DIAS_LIMITE_MODIFICACION = 3;

        public void Modificar(Consulta consultaModificada)
        {
            ValidarDatosConsulta(consultaModificada);

            if ((DateTime.Now - consultaModificada.FechaRegistro).TotalDays > DIAS_LIMITE_MODIFICACION)
            {
                throw new ExcepcionTraducible("error_consulta_fuera_de_plazo_modificacion", DIAS_LIMITE_MODIFICACION);
            }

            consultaModificada.FechaModificacion = DateTime.Now;
            EncriptarCamposSensibles(consultaModificada);

            ConsultaDAL consultaDAL = new ConsultaDAL();
            consultaDAL.Modificar(consultaModificada);

            DigitoVerificador digitoVerificador = new DigitoVerificador();
            digitoVerificador.ActualizarDVH(consultaModificada, TABLA);

            GestorBitacora gestorBitacora = new GestorBitacora();
            gestorBitacora.RegistrarEvento(EventosBitacora.MOD_CONSULTAS, EventosBitacora.DESC_MODIF_CONSULTA, EventosBitacora.CRIT_MODIF_CONSULTA);
        }

        #endregion

        #region Busquedas Consulta
        public Consulta BuscarPorId(int idConsulta)
        {
            ConsultaDAL consultaDAL = new ConsultaDAL();
            Consulta consulta = consultaDAL.BuscarPorId(idConsulta);
            if (consulta == null) return null;

            DesencriptarCamposSensibles(consulta);
            return consulta;
        }
        public List<Consulta> ObtenerPorPaciente(int idPaciente)
        {
            ConsultaDAL consultaDAL = new ConsultaDAL();
            List<Consulta> consultas = consultaDAL.ObtenerPorPaciente(idPaciente);

            foreach (Consulta consulta in consultas)
            {
                DesencriptarCamposSensibles(consulta);
            }

            return consultas;
        }

        public List<Consulta> ObtenerTodas()
        {
            ConsultaDAL consultaDAL = new ConsultaDAL();
            return consultaDAL.ObtenerTodas();
        }
        public List<Consulta> ObtenerPorPsicologo(int idPsicologo)
        {
            GestorPaciente gestorPaciente = new GestorPaciente();
            List<Consulta> todas = new List<Consulta>();

            foreach (Paciente paciente in gestorPaciente.ObtenerPorPsicologo(idPsicologo, soloActivos: false))
            {
                todas.AddRange(ObtenerPorPaciente(paciente.IdPaciente));
            }

            return todas;
        }

        #endregion

        #region Encriptacion

        private void EncriptarCamposSensibles(Consulta consulta)
        {
            Cifrador cifrador = Cifrador.GestorCifrador;

            consulta.Objetivos = cifrador.EncriptarReversible(consulta.Objetivos);
            consulta.Observaciones = cifrador.EncriptarReversible(consulta.Observaciones);
            consulta.Hipotesis = cifrador.EncriptarReversible(consulta.Hipotesis);
            consulta.Intervenciones = cifrador.EncriptarReversible(consulta.Intervenciones);
            consulta.EvolucionObservada = cifrador.EncriptarReversible(consulta.EvolucionObservada);
            consulta.Diagnostico = cifrador.EncriptarReversible(consulta.Diagnostico);
            consulta.Tratamiento = cifrador.EncriptarReversible(consulta.Tratamiento);
        }

        private void DesencriptarCamposSensibles(Consulta consulta)
        {
            Cifrador cifrador = Cifrador.GestorCifrador;

            consulta.Objetivos = cifrador.DesencriptarReversible(consulta.Objetivos);
            consulta.Observaciones = cifrador.DesencriptarReversible(consulta.Observaciones);
            consulta.Hipotesis = cifrador.DesencriptarReversible(consulta.Hipotesis);
            consulta.Intervenciones = cifrador.DesencriptarReversible(consulta.Intervenciones);
            consulta.EvolucionObservada = cifrador.DesencriptarReversible(consulta.EvolucionObservada);
            consulta.Diagnostico = cifrador.DesencriptarReversible(consulta.Diagnostico);
            consulta.Tratamiento = cifrador.DesencriptarReversible(consulta.Tratamiento);
        }

        #endregion

        #region Validaciones

        private void ValidarDatosConsulta(Consulta consulta)
        {
            if (consulta.IdPaciente <= 0)
            {
                throw new ExcepcionTraducible("error_consulta_sin_paciente");
            }

            if (consulta.IdPsicologo <= 0)
            {
                throw new ExcepcionTraducible("error_paciente_sin_profesional");
            }

            if (consulta.FechaConsulta == default(DateTime))
            {
                throw new ExcepcionTraducible("error_fecha_consulta_obligatoria");
            }

            if (consulta.FechaConsulta > DateTime.Now)
            {
                throw new ExcepcionTraducible("error_fecha_consulta_futura");
            }

            if (consulta.TiempoConsulta <= 0)
            {
                throw new ExcepcionTraducible("error_tiempo_consulta_invalido");
            }
        }

        #endregion
    }
}