using BE;
using DAL;
using SERVICIOS;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace BLL
{
    public class GestorPsicologo
    {
        #region Operaciones Psicologo

        public int Alta(Psicologo psicologoAlta)
        {
            ValidarDatosPsicologo(psicologoAlta);

            PsicologoDAL psicologoDAL = new PsicologoDAL();
            if (psicologoDAL.ExisteEmail(psicologoAlta.Email))
            {
                throw new InvalidOperationException("Ya existe un profesional registrado con ese email.");
            }

            psicologoAlta.Contrasena = Cifrador.GestorCifrador.EncriptarIrreversible(psicologoAlta.Contrasena);
            psicologoAlta.Activo = true;
            psicologoAlta.FechaRegistro = DateTime.Now;

            return psicologoDAL.Alta(psicologoAlta);
        }

        public void Baja(int idPsicologo)
        {
            PsicologoDAL psicologoDAL = new PsicologoDAL();
            psicologoDAL.Baja(idPsicologo);
        }

        public void Activar(int idPsicologo)
        {
            PsicologoDAL psicologoDAL = new PsicologoDAL();
            psicologoDAL.Activar(idPsicologo);
        }

        public void Modificar(Psicologo psicologoModificado)
        {
            ValidarDatosPsicologo(psicologoModificado);

            PsicologoDAL psicologoDAL = new PsicologoDAL();
            Psicologo psicologoExistente = psicologoDAL.BuscarPorEmail(psicologoModificado.Email);
            if (psicologoExistente != null && psicologoExistente.IdPsicologo != psicologoModificado.IdPsicologo)
            {
                throw new InvalidOperationException("Ya existe otro profesional registrado con ese email.");
            }

            psicologoDAL.Modificar(psicologoModificado);
        }

        public void CambiarContrasena(int idPsicologo, string contrasenaActual, string contrasenaNueva)
        {
            PsicologoDAL psicologoDAL = new PsicologoDAL();
            Psicologo psicologo = psicologoDAL.BuscarPorId(idPsicologo);
            if (psicologo == null)
            {
                throw new InvalidOperationException("No se encontro el profesional.");
            }

            if (psicologo.Contrasena != Cifrador.GestorCifrador.EncriptarIrreversible(contrasenaActual))
            {
                throw new InvalidOperationException("La contrasena actual no es correcta.");
            }

            if (!VerificarFormatoContrasena(contrasenaNueva))
            {
                throw new ArgumentException("La contrasena nueva no cumple con el formato requerido (minimo 8 caracteres, una mayuscula, un numero).");
            }

            psicologoDAL.CambiarContrasena(idPsicologo, Cifrador.GestorCifrador.EncriptarIrreversible(contrasenaNueva));
        }

        
        public Psicologo ValidarCredenciales(string email, string contrasena)
        {
            PsicologoDAL psicologoDAL = new PsicologoDAL();
            Psicologo psicologo = psicologoDAL.BuscarPorEmail(email);

            if (psicologo == null || !psicologo.Activo)
            {
                return null;
            }

            if (psicologo.Contrasena != Cifrador.GestorCifrador.EncriptarIrreversible(contrasena))
            {
                return null;
            }

            return psicologo;
        }

        #endregion

        #region Busquedas Psicologo

        public Psicologo BuscarPorId(int idPsicologo)
        {
            PsicologoDAL psicologoDAL = new PsicologoDAL();
            return psicologoDAL.BuscarPorId(idPsicologo);
        }

        public Psicologo BuscarPorEmail(string email)
        {
            PsicologoDAL psicologoDAL = new PsicologoDAL();
            return psicologoDAL.BuscarPorEmail(email);
        }

        public List<Psicologo> ObtenerTodos()
        {
            PsicologoDAL psicologoDAL = new PsicologoDAL();
            return psicologoDAL.ObtenerTodos();
        }

        #endregion

        #region Validaciones

        private void ValidarDatosPsicologo(Psicologo psicologo)
        {
            if (string.IsNullOrWhiteSpace(psicologo.Nombre))
            {
                throw new ArgumentException("El nombre es obligatorio.");
            }

            if (string.IsNullOrWhiteSpace(psicologo.Apellido))
            {
                throw new ArgumentException("El apellido es obligatorio.");
            }

            if (!VerificarFormatoDni(psicologo.Dni))
            {
                throw new ArgumentException("El DNI no cumple con el formato esperado (ej: 12.345.678).");
            }

            if (!VerificarFormatoEmail(psicologo.Email))
            {
                throw new ArgumentException("El email no cumple con el formato esperado.");
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

        public bool VerificarFormatoContrasena(string contrasena)
        {
            
            Regex rgx = new Regex(@"^(?=.*[A-Z])(?=.*[0-9]).{7,}$");
            return rgx.IsMatch(contrasena);
        }

        #endregion

    }
}