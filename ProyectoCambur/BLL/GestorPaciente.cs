using BE;
using DAL;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace BLL
{
    public class GestorPaciente
    {
        #region Operaciones Paciente

        public int Alta(Paciente pacienteAlta)
        {
            ValidarDatosPaciente(pacienteAlta);

            PacienteDAL pacienteDAL = new PacienteDAL();
            pacienteAlta.Activo = true;
            pacienteAlta.FechaRegistro = DateTime.Now;

            return pacienteDAL.Alta(pacienteAlta);
        }

        public void Baja(int idPaciente)
        {
            PacienteDAL pacienteDAL = new PacienteDAL();
            pacienteDAL.Baja(idPaciente);
        }

        public void Activar(int idPaciente)
        {
            PacienteDAL pacienteDAL = new PacienteDAL();
            pacienteDAL.Activar(idPaciente);
        }

        public void Modificar(Paciente pacienteModificado)
        {
            ValidarDatosPaciente(pacienteModificado);

            PacienteDAL pacienteDAL = new PacienteDAL();
            pacienteDAL.Modificar(pacienteModificado);
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

        #region Validaciones

        private void ValidarDatosPaciente(Paciente paciente)
        {
            if (string.IsNullOrWhiteSpace(paciente.Nombre))
            {
                throw new ArgumentException("El nombre es obligatorio.");
            }

            if (string.IsNullOrWhiteSpace(paciente.Apellido))
            {
                throw new ArgumentException("El apellido es obligatorio.");
            }

            if (paciente.IdPsicologo <= 0)
            {
                throw new ArgumentException("El paciente debe estar asociado a un profesional.");
            }

            if (!string.IsNullOrWhiteSpace(paciente.DNI) && !VerificarFormatoDni(paciente.DNI))
            {
                throw new ArgumentException("El DNI no cumple con el formato esperado (ej: 12.345.678).");
            }

            if (!string.IsNullOrWhiteSpace(paciente.Email) && !VerificarFormatoEmail(paciente.Email))
            {
                throw new ArgumentException("El email no cumple con el formato esperado.");
            }

            if (paciente.FechaNacimiento > DateTime.Now)
            {
                throw new ArgumentException("La fecha de nacimiento no puede ser futura.");
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