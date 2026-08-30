using BE;
using DAL;
using SERVICIOS;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace BLL
{
    public class GestorPsicologo
    {
        private const int MAX_INTENTOS = 3;
        private const int MINUTOS_DECAIMIENTO_INTENTOS = 10;
        private const int MINUTOS_EXPIRACION_TOKEN_RECUPERACION = 30;
        private const string TABLA = "Profesional";
        private readonly IPasarelaPago pasarelaPago;

        public GestorPsicologo() : this(new PasarelaMercadoPago())
        {
        }

        public GestorPsicologo(IPasarelaPago pasarelaPagoAUsar)
        {
            pasarelaPago = pasarelaPagoAUsar;
        }

        #region Operaciones Psicologo
        public int Alta(Psicologo psicologoAlta)
        {
            ValidarDatosPsicologo(psicologoAlta);
            if (!VerificarFormatoContrasena(psicologoAlta.Contrasena))
            {
                throw new ExcepcionTraducible("error_formato_contrasena");
            }
            psicologoAlta.Contrasena = Cifrador.GestorCifrador.EncriptarIrreversible(psicologoAlta.Contrasena);
            return RegistrarPsicologoValidado(psicologoAlta);
        }
        public int AltaPorAdministrador(Psicologo psicologoAlta)
        {
            ValidarDatosPsicologo(psicologoAlta);

            string contrasenaInicial = psicologoAlta.Dni + psicologoAlta.Email;
            psicologoAlta.Contrasena = Cifrador.GestorCifrador.EncriptarIrreversible(contrasenaInicial);
            return RegistrarPsicologoValidado(psicologoAlta);
        }
        public Psicologo RegistrarProfesionalConSuscripcion(Psicologo psicologoAlta, string contrasenaPlana, int idPlan, string tokenTarjeta, string paymentMethodId)
        {
            ValidarDatosPsicologo(psicologoAlta);

            if (!VerificarFormatoContrasena(contrasenaPlana))
            {
                throw new ExcepcionTraducible("error_formato_contrasena");
            }

            InfoPlan plan = CatalogoPlanes.ObtenerPorId(idPlan);
            if (plan == null)
            {
                throw new InvalidOperationException("Plan de suscripción inválido: " + idPlan);
            }

            if (string.IsNullOrWhiteSpace(tokenTarjeta) || string.IsNullOrWhiteSpace(paymentMethodId))
            {
                throw new ExcepcionTraducible("error_pago_timeout");
            }
            PsicologoDAL psicologoDAL = new PsicologoDAL();
            if (psicologoDAL.ExisteEmail(psicologoAlta.Email))
            {
                throw new ExcepcionTraducible("error_email_duplicado");
            }

            DatosPago datosPago = new DatosPago
            {
                TokenTarjeta = tokenTarjeta,
                PaymentMethodId = paymentMethodId,
                Monto = plan.Precio,
                Descripcion = "Cambur — Suscripción plan " + plan.NombreComercial,
                EmailPagador = psicologoAlta.Email,
                DniPagador = psicologoAlta.Dni.Replace(".", "")
            };

            ResultadoPago resultadoPago = pasarelaPago.CrearPago(datosPago);

            if (!resultadoPago.Aprobado)
            {
                throw new ExcepcionTraducible("error_pago_rechazado", resultadoPago.MotivoRechazo);
            }
            psicologoAlta.RolPermiso = plan.RolPermiso;
            psicologoAlta.Contrasena = Cifrador.GestorCifrador.EncriptarIrreversible(contrasenaPlana);
            int idPsicologo = RegistrarPsicologoValidado(psicologoAlta);
            DateTime fechaInicioSuscripcion = DateTime.Now;
            Suscripcion nuevaSuscripcion = new Suscripcion
            {
                IdProfesional = idPsicologo,
                Plan = plan.Plan,
                Estado = EstadoSuscripcion.Activa,
                FechaInicio = fechaInicioSuscripcion,
                FechaFin = fechaInicioSuscripcion.AddMonths(1),
                Precio = plan.Precio,
                IdPagoExterno = resultadoPago.IdPagoExterno,
                UltimosCuatroTarjeta = resultadoPago.UltimosCuatroTarjeta
            };
            new SuscripcionDAL().Alta(nuevaSuscripcion);
            new DigitoVerificador().ActualizarDVH(nuevaSuscripcion, "Suscripcion");
            GestorBitacora gestorBitacora = new GestorBitacora();
            gestorBitacora.RegistrarEvento(psicologoAlta.Email, EventosBitacora.MOD_PROFESIONALES, EventosBitacora.DESC_REGISTRO_PROFESIONAL, EventosBitacora.CRIT_REGISTRO_PROFESIONAL);
            gestorBitacora.RegistrarEvento(psicologoAlta.Email, EventosBitacora.MOD_SUSCRIPCION, EventosBitacora.DESC_ALTA_SUSCRIPCION, EventosBitacora.CRIT_ALTA_SUSCRIPCION);

            return psicologoAlta;
        }

        private int RegistrarPsicologoValidado(Psicologo psicologoAlta)
        {
            PsicologoDAL psicologoDAL = new PsicologoDAL();
            if (psicologoDAL.ExisteEmail(psicologoAlta.Email))
            {
                throw new ExcepcionTraducible("error_email_duplicado");
            }

            psicologoAlta.Activo = true;
            psicologoAlta.FechaRegistro = DateTime.Now;

            int idGenerado = psicologoDAL.Alta(psicologoAlta);

            new DigitoVerificador().ActualizarDVH(psicologoAlta, TABLA);
            new GestorBitacora().RegistrarEvento(EventosBitacora.MOD_PROFESIONALES, EventosBitacora.DESC_REGISTRO_PROFESIONAL, EventosBitacora.CRIT_REGISTRO_PROFESIONAL);

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


        public Psicologo CambiarIdioma(int idPsicologo, string nuevoIdioma)
        {
            if (string.IsNullOrWhiteSpace(nuevoIdioma))
            {
                throw new ExcepcionTraducible("error_idioma_obligatorio");
            }

            IdiomaDAL idiomaDAL = new IdiomaDAL();
            Idioma idioma = idiomaDAL.BuscarPorNombre(nuevoIdioma);

            if (idioma == null || !idioma.IsDisponible)
            {
                throw new ExcepcionTraducible("error_idioma_no_disponible");
            }

            PsicologoDAL psicologoDAL = new PsicologoDAL();
            Psicologo psicologo = psicologoDAL.BuscarPorId(idPsicologo);
            if (psicologo == null)
            {
                throw new ExcepcionTraducible("error_profesional_no_encontrado");
            }

            if (psicologo.Idioma == nuevoIdioma)
            {
                throw new ExcepcionTraducible("error_idioma_ya_configurado");
            }

            psicologoDAL.CambiarIdioma(idPsicologo, nuevoIdioma);
            psicologo.Idioma = nuevoIdioma;

            new DigitoVerificador().ActualizarDVH(psicologo, "Profesional");
            new GestorBitacora().RegistrarEvento(EventosBitacora.MOD_CONFIGURACION, EventosBitacora.DESC_CAMBIO_IDIOMA, EventosBitacora.CRIT_CAMBIO_IDIOMA);

            return psicologo;
        }

        public void Desbloquear(int idPsicologo)
        {
            PsicologoDAL psicologoDAL = new PsicologoDAL();
            Psicologo psicologo = psicologoDAL.BuscarPorId(idPsicologo);
            if (psicologo == null)
            {
                throw new ExcepcionTraducible("error_profesional_no_encontrado");
            }

            string contrasenaTemporal = psicologo.Dni + psicologo.Email;
            string hashTemporal = Cifrador.GestorCifrador.EncriptarIrreversible(contrasenaTemporal);

            psicologoDAL.Desbloquear(idPsicologo, hashTemporal);
            RecalcularDVHDe(idPsicologo);
            new GestorBitacora().RegistrarEvento(EventosBitacora.MOD_AUTENTICACION, EventosBitacora.DESC_DESBLOQUEO_MANUAL, EventosBitacora.CRIT_DESBLOQUEO_MANUAL);
        }

        public void Modificar(Psicologo psicologoModificado)
        {
            ValidarDatosPsicologo(psicologoModificado);

            PsicologoDAL psicologoDAL = new PsicologoDAL();
            Psicologo psicologoExistente = psicologoDAL.BuscarPorEmail(psicologoModificado.Email);
            if (psicologoExistente != null && psicologoExistente.IdPsicologo != psicologoModificado.IdPsicologo)
            {
                throw new ExcepcionTraducible("error_email_duplicado_otro");
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
                throw new ExcepcionTraducible("error_profesional_no_encontrado");
            }

            if (psicologo.Contrasena != Cifrador.GestorCifrador.EncriptarIrreversible(contrasenaActual))
            {
                throw new ExcepcionTraducible("error_contrasena_actual_incorrecta");
            }

            if (!VerificarFormatoContrasena(contrasenaNueva))
            {
                throw new ExcepcionTraducible("error_formato_contrasena");
            }

            if (Cifrador.GestorCifrador.EncriptarIrreversible(contrasenaNueva) == psicologo.Contrasena)
            {
                throw new ExcepcionTraducible("error_contrasena_igual_actual");
            }

            psicologoDAL.CambiarContrasena(idPsicologo, Cifrador.GestorCifrador.EncriptarIrreversible(contrasenaNueva));
            RecalcularDVHDe(idPsicologo);
            new GestorBitacora().RegistrarEvento(EventosBitacora.MOD_CONFIGURACION, EventosBitacora.DESC_CAMBIO_CLAVE, EventosBitacora.CRIT_CAMBIO_CLAVE);
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

        #region Recuperación de clave (olvido de contraseña, vía email)
        public void SolicitarRecuperacionClave(string email, string urlBaseSitio)
        {
            if (!VerificarFormatoEmail(email))
            {
                throw new ExcepcionTraducible("error_formato_email");
            }

            PsicologoDAL psicologoDAL = new PsicologoDAL();
            Psicologo psicologo = psicologoDAL.BuscarPorEmail(email.Trim().ToLower());

            if (psicologo == null || !psicologo.Activo || !psicologo.IsHabilitado)
            {
                return;
            }

            TokenRecuperacionDAL tokenDAL = new TokenRecuperacionDAL();
            DigitoVerificador digitoVerificadorTokens = new DigitoVerificador();
            foreach (TokenRecuperacion vigente in tokenDAL.BuscarVigentesDe(psicologo.IdPsicologo))
            {
                tokenDAL.MarcarUsado(vigente.IdToken);
                vigente.Usado = true;
                digitoVerificadorTokens.ActualizarDVH(vigente, "TokenRecuperacion");
            }

            string tokenPlano = GenerarTokenAleatorio();
            string tokenHash = Cifrador.GestorCifrador.EncriptarIrreversible(tokenPlano);

            TokenRecuperacion nuevoToken = new TokenRecuperacion
            {
                IdProfesional = psicologo.IdPsicologo,
                TokenHash = tokenHash,
                FechaGeneracion = DateTime.Now,
                FechaExpiracion = DateTime.Now.AddMinutes(MINUTOS_EXPIRACION_TOKEN_RECUPERACION),
                Usado = false
            };
            tokenDAL.Alta(nuevoToken);
            digitoVerificadorTokens.ActualizarDVH(nuevoToken, "TokenRecuperacion");

            string link = urlBaseSitio.TrimEnd('/') + "/FormRestablecerClave.aspx?token=" + tokenPlano;

            GestorEmail gestorEmail = new GestorEmail();
            gestorEmail.EnviarCorreoRecuperacionClave(psicologo.Email, psicologo.Nombre, link);

            new GestorBitacora().RegistrarEvento(psicologo.Email, EventosBitacora.MOD_AUTENTICACION, EventosBitacora.DESC_SOLICITUD_RECUPERACION_CLAVE, EventosBitacora.CRIT_SOLICITUD_RECUPERACION_CLAVE);
        }
        public bool ValidarTokenRecuperacion(string tokenPlano, out Psicologo psicologoDelToken)
        {
            psicologoDelToken = null;

            if (string.IsNullOrWhiteSpace(tokenPlano))
            {
                return false;
            }

            string tokenHash = Cifrador.GestorCifrador.EncriptarIrreversible(tokenPlano);
            TokenRecuperacionDAL tokenDAL = new TokenRecuperacionDAL();
            TokenRecuperacion token = tokenDAL.BuscarPorHash(tokenHash);

            if (token == null || token.Usado || token.FechaExpiracion < DateTime.Now)
            {
                return false;
            }

            PsicologoDAL psicologoDAL = new PsicologoDAL();
            psicologoDelToken = psicologoDAL.BuscarPorId(token.IdProfesional);

            return psicologoDelToken != null;
        }
        public void RestablecerClave(string tokenPlano, string contrasenaNueva)
        {
            Psicologo psicologo;
            if (!ValidarTokenRecuperacion(tokenPlano, out psicologo))
            {
                throw new ExcepcionTraducible("error_token_invalido_o_expirado");
            }

            if (!VerificarFormatoContrasena(contrasenaNueva))
            {
                throw new ExcepcionTraducible("error_formato_contrasena");
            }

            string hashNuevo = Cifrador.GestorCifrador.EncriptarIrreversible(contrasenaNueva);
            if (hashNuevo == psicologo.Contrasena)
            {
                throw new ExcepcionTraducible("error_contrasena_igual_actual");
            }

            PsicologoDAL psicologoDAL = new PsicologoDAL();
            psicologoDAL.Desbloquear(psicologo.IdPsicologo, hashNuevo);

            string tokenHash = Cifrador.GestorCifrador.EncriptarIrreversible(tokenPlano);
            TokenRecuperacionDAL tokenDAL = new TokenRecuperacionDAL();
            TokenRecuperacion token = tokenDAL.BuscarPorHash(tokenHash);
            if (token != null)
            {
                tokenDAL.MarcarUsado(token.IdToken);
                token.Usado = true;
                new DigitoVerificador().ActualizarDVH(token, "TokenRecuperacion");
            }

            RecalcularDVHDe(psicologo.IdPsicologo);
            new GestorBitacora().RegistrarEvento(psicologo.Email, EventosBitacora.MOD_AUTENTICACION, EventosBitacora.DESC_RESTABLECIMIENTO_CLAVE, EventosBitacora.CRIT_RESTABLECIMIENTO_CLAVE);
        }

        private string GenerarTokenAleatorio()
        {
            byte[] bytesAleatorios = new byte[32];
            using (RandomNumberGenerator generador = RandomNumberGenerator.Create())
            {
                generador.GetBytes(bytesAleatorios);
            }

            return Convert.ToBase64String(bytesAleatorios)
                .Replace("+", "-")
                .Replace("/", "_")
                .Replace("=", "");
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
                throw new ExcepcionTraducible("error_nombre_obligatorio");
            }

            if (string.IsNullOrWhiteSpace(psicologo.Apellido))
            {
                throw new ExcepcionTraducible("error_apellido_obligatorio");
            }

            if (!VerificarFormatoDni(psicologo.Dni))
            {
                throw new ExcepcionTraducible("error_formato_dni");
            }

            if (!VerificarFormatoEmail(psicologo.Email))
            {
                throw new ExcepcionTraducible("error_formato_email");
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