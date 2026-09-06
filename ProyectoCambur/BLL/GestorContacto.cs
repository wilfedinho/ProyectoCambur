using BE;
using DAL;
using SERVICIOS;
using System;
using System.Text.RegularExpressions;

namespace BLL
{
    public class GestorContacto
    {
        private static readonly Regex PatronEmail = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled);

        private readonly ContactoDAL contactoDAL = new ContactoDAL();

        public MensajeContacto EnviarMensaje(string nombre, string email, string asunto, string mensaje)
        {
            nombre = (nombre ?? string.Empty).Trim();
            email = (email ?? string.Empty).Trim();
            asunto = (asunto ?? string.Empty).Trim();
            mensaje = (mensaje ?? string.Empty).Trim();

            if (nombre.Length == 0 || nombre.Length > 150
                || asunto.Length == 0 || asunto.Length > 200
                || mensaje.Length == 0 || mensaje.Length > 1000)
            {
                throw new ExcepcionTraducible("error_contacto_datos_invalidos");
            }

            if (email.Length == 0 || email.Length > 150 || !PatronEmail.IsMatch(email))
            {
                throw new ExcepcionTraducible("error_contacto_email_invalido");
            }

            MensajeContacto nuevo = new MensajeContacto
            {
                Nombre = nombre,
                Email = email,
                Asunto = asunto,
                Mensaje = mensaje,
                FechaEnvio = DateTime.Now
            };

            contactoDAL.Alta(nuevo);
            new GestorBitacora().RegistrarEvento(
                email,
                EventosBitacora.MOD_LANDING,
                EventosBitacora.DESC_MENSAJE_CONTACTO,
                EventosBitacora.CRIT_MENSAJE_CONTACTO);

            return nuevo;
        }
    }
}