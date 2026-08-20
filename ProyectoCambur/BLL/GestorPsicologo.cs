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
        private const int MAX_INTENTOS = 3;
        private const int MINUTOS_DECAIMIENTO_INTENTOS = 10;
        private const string TABLA = "Profesional";

        #region Operaciones Psicologo

        public int Alta(Psicologo psicologoAlta)
        {
            ValidarDatosPsicologo(psicologoAlta);

            if (!VerificarFormatoContrasena(psicologoAlta.Contrasena))
            {
                throw new ArgumentException("La contrasena no cumple con el formato requerido (minimo 8 caracteres, una mayuscula, un numero).");
            }

            PsicologoDAL psicologoDAL = new PsicologoDAL();
            if (psicologoDAL.ExisteEmail(psicologoAlta.Email))
            {
                throw new InvalidOperationException("Ya existe un profesional registrado con ese email.");
            }

            psicologoAlta.Contrasena = Cifrador.GestorCifrador.EncriptarIrreversible(psicologoAlta.Contrasena);
            psicologoAlta.Activo = true;
            psicologoAlta.FechaRegistro = DateTime.Now;

            int idGenerado = psicologoDAL.Alta(psicologoAlta);

            new DigitoVerificador().ActualizarDVH(psicologoAlta, TABLA);

            return idGenerado;
        }

        public void Baja(int idPsicologo)
        {
            PsicologoDAL psicologoDAL = new PsicologoDAL();
            psicologoDAL.Baja(idPsicologo);
            RecalcularDVHDe(idPsicologo);
        }

        public void Activar(int idPsicologo)
        {
            PsicologoDAL psicologoDAL = new PsicologoDAL();
            psicologoDAL.Activar(idPsicologo);
            RecalcularDVHDe(idPsicologo);
        }

        public void Habilitar(int idPsicologo)
        {
            PsicologoDAL psicologoDAL = new PsicologoDAL();
            psicologoDAL.Habilitar(idPsicologo);
            RecalcularDVHDe(idPsicologo);
        }

        public void Deshabilitar(int idPsicologo)
        {
            PsicologoDAL psicologoDAL = new PsicologoDAL();
            psicologoDAL.Deshabilitar(idPsicologo);
            RecalcularDVHDe(idPsicologo);
        }

        public void Desbloquear(int idPsicologo)
        {
            PsicologoDAL psicologoDAL = new PsicologoDAL();
            Psicologo psicologo = psicologoDAL.BuscarPorId(idPsicologo);
            if (psicologo == null)
            {
                throw new InvalidOperationException("No se encontro el profesional.");
            }

            string contrasenaTemporal = psicologo.Dni + psicologo.Email;
            string hashTemporal = Cifrador.GestorCifrador.EncriptarIrreversible(contrasenaTemporal);

            psicologoDAL.Desbloquear(idPsicologo, hashTemporal);
            RecalcularDVHDe(idPsicologo);
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
            RecalcularDVHDe(psicologoModificado.IdPsicologo);
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
            RecalcularDVHDe(idPsicologo);
        }

        public ResultadoLogin ValidarCredenciales(string email, string contrasena, out Psicologo psicologoLogueado)
        {
            psicologoLogueado = null;
            PsicologoDAL psicologoDAL = new PsicologoDAL();
            Psicologo psicologo = psicologoDAL.BuscarPorEmail(email);

            if (psicologo == null)
            {
                return ResultadoLogin.CredencialesInvalidas;
            }

            if (!psicologo.Activo)
            {
                return ResultadoLogin.CuentaInactiva;
            }

            if (!psicologo.IsHabilitado)
            {
                return ResultadoLogin.CuentaDeshabilitada;
            }

            if (psicologo.Intentos > 0 && psicologo.Intentos < MAX_INTENTOS)
            {
                double minutosTranscurridos = (DateTime.Now - psicologo.HoraUltimaSesion).TotalMinutes;
                if (minutosTranscurridos > MINUTOS_DECAIMIENTO_INTENTOS)
                {
                    psicologo.Intentos = 0;
                    psicologoDAL.ActualizarIntentos(psicologo.IdPsicologo, 0, DateTime.Now);
                }
            }

            if (psicologo.IsBloqueado)
            {
                return ResultadoLogin.CuentaBloqueada;
            }

            if (psicologo.Contrasena != Cifrador.GestorCifrador.EncriptarIrreversible(contrasena))
            {
                int intentosNuevos = psicologo.Intentos + 1;

                if (intentosNuevos >= MAX_INTENTOS)
                {
                    psicologoDAL.ActualizarIntentos(psicologo.IdPsicologo, intentosNuevos, DateTime.Now);
                    psicologoDAL.Bloquear(psicologo.IdPsicologo);
                    return ResultadoLogin.CuentaBloqueada;
                }

                psicologoDAL.ActualizarIntentos(psicologo.IdPsicologo, intentosNuevos, DateTime.Now);
                return ResultadoLogin.CredencialesInvalidas;
            }

            psicologoDAL.ActualizarIntentos(psicologo.IdPsicologo, 0, DateTime.Now);
            psicologo.Intentos = 0;
            psicologo.HoraUltimaSesion = DateTime.Now;

            psicologoLogueado = psicologo;
            return ResultadoLogin.Ok;
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
            Regex rgx = new Regex(@"^(?=.*[A-Z])(?=.*[0-9]).{8,}$");
            return rgx.IsMatch(contrasena);
        }

        #endregion

        #region Digito Verificador

        private void RecalcularDVHDe(int idPsicologo)
        {
            PsicologoDAL psicologoDAL = new PsicologoDAL();
            Psicologo psicologoActualizado = psicologoDAL.BuscarPorId(idPsicologo);
            if (psicologoActualizado != null)
            {
                new DigitoVerificador().ActualizarDVH(psicologoActualizado, TABLA);
            }
        }

        #endregion
    }
}