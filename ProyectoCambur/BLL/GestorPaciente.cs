using BE;
using DAL;
using SERVICIOS;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace BLL
{
    public class GestorPaciente
    {
        private const string TABLA = "Paciente";

        #region Operaciones Paciente

        public int Alta(Paciente pacienteAlta)
        {
            ValidarDatosPaciente(pacienteAlta);
            PacienteDAL pacienteDAL = new PacienteDAL();
            pacienteAlta.Activo = true;
            pacienteAlta.FechaRegistro = DateTime.Now;
            int idGenerado = pacienteDAL.Alta(pacienteAlta);
            DigitoVerificador digitoVerificador = new DigitoVerificador();
            digitoVerificador.ActualizarDVH(pacienteAlta, TABLA);
            GestorBitacora gestorBitacora = new GestorBitacora();
            gestorBitacora.RegistrarEvento(EventosBitacora.MOD_PACIENTES, EventosBitacora.DESC_REGISTRO_PACIENTE, EventosBitacora.CRIT_REGISTRO_PACIENTE);
            return idGenerado;
        }

        public void Baja(int idPaciente)
        {
            PacienteDAL pacienteDAL = new PacienteDAL();
            pacienteDAL.Baja(idPaciente);
            RecalcularDVHDe(idPaciente);
        }

        public void Activar(int idPaciente)
        {
            PacienteDAL pacienteDAL = new PacienteDAL();
            pacienteDAL.Activar(idPaciente);
            RecalcularDVHDe(idPaciente);
        }

        public void Modificar(Paciente pacienteModificado)
        {
            ValidarDatosPaciente(pacienteModificado);
            PacienteDAL pacienteDAL = new PacienteDAL();
            pacienteDAL.Modificar(pacienteModificado);
            DigitoVerificador digitoVerificador = new DigitoVerificador();
            digitoVerificador.ActualizarDVH(pacienteModificado, TABLA);
            GestorBitacora gestorBitacora = new GestorBitacora();
            gestorBitacora.RegistrarEvento(EventosBitacora.MOD_PACIENTES, EventosBitacora.DESC_MODIF_PACIENTE, EventosBitacora.CRIT_MODIF_PACIENTE);
        }

        #endregion

        #region Busquedas Paciente

        public Paciente BuscarPorId(int idPaciente)
        {
            PacienteDAL pacienteDAL = new PacienteDAL();
            return pacienteDAL.BuscarPorId(idPaciente);
        }

        public List<Paciente> ObtenerPorPsicologo(int idPsicologo, bool soloActivos = true)
        {
            PacienteDAL pacienteDAL = new PacienteDAL();
            return pacienteDAL.ObtenerPorProfesional(idPsicologo, soloActivos);
        }

        public List<Paciente> ObtenerTodos()
        {
            PacienteDAL pacienteDAL = new PacienteDAL();
            return pacienteDAL.ObtenerTodos();
        }

        #endregion

        #region Psicologos elegibles para tener pacientes asignados
        public List<Psicologo> ObtenerPsicologosClinicos()
        {
            PsicologoDAL psicologoDAL = new PsicologoDAL();
            GestorPermiso gestorPermiso = new GestorPermiso();

            List<Psicologo> todos = psicologoDAL.ObtenerTodos();
            List<Psicologo> clinicos = new List<Psicologo>();

            foreach (Psicologo p in todos)
            {
                if (p.Activo && gestorPermiso.TienePermiso(p.RolPermiso, "acceder_registrar_paciente"))
                {
                    clinicos.Add(p);
                }
            }

            return clinicos;
        }

        #endregion

        #region Digito Verificador

        private void RecalcularDVHDe(int idPaciente)
        {
            PacienteDAL pacienteDAL = new PacienteDAL();
            Paciente pacienteActualizado = pacienteDAL.BuscarPorId(idPaciente);
            if (pacienteActualizado != null)
            {
                DigitoVerificador digitoVerificador = new DigitoVerificador();
                digitoVerificador.ActualizarDVH(pacienteActualizado, TABLA);
            }
        }

        #endregion

        #region Validaciones

        private void ValidarDatosPaciente(Paciente paciente)
        {
            if (string.IsNullOrWhiteSpace(paciente.Nombre))
            {
                throw new ExcepcionTraducible("error_nombre_obligatorio");
            }

            if (string.IsNullOrWhiteSpace(paciente.Apellido))
            {
                throw new ExcepcionTraducible("error_apellido_obligatorio");
            }

            if (paciente.IdPsicologo <= 0)
            {
                throw new ExcepcionTraducible("error_paciente_sin_profesional");
            }

            if (!string.IsNullOrWhiteSpace(paciente.DNI) && !VerificarFormatoDni(paciente.DNI))
            {
                throw new ExcepcionTraducible("error_formato_dni");
            }

            if (!string.IsNullOrWhiteSpace(paciente.Email) && !VerificarFormatoEmail(paciente.Email))
            {
                throw new ExcepcionTraducible("error_formato_email");
            }

            if (paciente.FechaNacimiento > DateTime.Now)
            {
                throw new ExcepcionTraducible("error_fecha_nacimiento_futura");
            }
        }

        public bool VerificarFormatoDni(string dni)
        {
            Regex rgx = new Regex("^[0-9]{2}[.]{1}[0-9]{3}[.]{1}[0-9]{3}$");
            return rgx.IsMatch(dni);
        }

        public bool VerificarFormatoEmail(string email)
        {
            Regex rgx = new Regex(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$");
            return rgx.IsMatch(email);
        }

        #endregion
    }
}