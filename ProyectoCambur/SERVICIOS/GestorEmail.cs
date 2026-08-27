using System;
using System.Configuration;
using System.Net;
using System.Net.Mail;

namespace SERVICIOS
{
    public class GestorEmail
    {
        public void EnviarCorreoRecuperacionClave(string emailDestino, string nombreDestino, string linkRecuperacion)
        {
            string asunto = "Cambur - Recuperación de contraseña";
            string cuerpo = ArmarCuerpoRecuperacion(nombreDestino, linkRecuperacion);
            EnviarCorreo(emailDestino, asunto, cuerpo);
        }

        private void EnviarCorreo(string emailDestino, string asunto, string cuerpoHtml)
        {
            string host = ConfigurationManager.AppSettings["SmtpHost"];
            string puertoConfig = ConfigurationManager.AppSettings["SmtpPuerto"];
            string usuario = ConfigurationManager.AppSettings["SmtpUsuario"];
            string contrasena = ConfigurationManager.AppSettings["SmtpContrasena"];
            string usaSslConfig = ConfigurationManager.AppSettings["SmtpUsaSsl"];
            string emailOrigen = ConfigurationManager.AppSettings["SmtpEmailOrigen"];
            string nombreOrigen = ConfigurationManager.AppSettings["SmtpNombreOrigen"];

            if (string.IsNullOrWhiteSpace(host) || string.IsNullOrWhiteSpace(emailOrigen))
            {
                throw new InvalidOperationException(
                    "Falta configurar los datos de SMTP (SmtpHost / SmtpEmailOrigen) en informacion_traductor.config para poder enviar correos.");
            }

            int puerto;
            if (!int.TryParse(puertoConfig, out puerto))
            {
                puerto = 587;
            }

            bool usaSsl;
            if (!bool.TryParse(usaSslConfig, out usaSsl))
            {
                usaSsl = true;
            }

            using (MailMessage mensaje = new MailMessage())
            {
                mensaje.From = new MailAddress(emailOrigen, string.IsNullOrWhiteSpace(nombreOrigen) ? "Cambur" : nombreOrigen);
                mensaje.To.Add(emailDestino);
                mensaje.Subject = asunto;
                mensaje.Body = cuerpoHtml;
                mensaje.IsBodyHtml = true;

                using (SmtpClient smtp = new SmtpClient(host, puerto))
                {
                    smtp.EnableSsl = usaSsl;
                    if (!string.IsNullOrWhiteSpace(usuario))
                    {
                        smtp.Credentials = new NetworkCredential(usuario, contrasena);
                    }
                    smtp.Send(mensaje);
                }
            }
        }

        private string ArmarCuerpoRecuperacion(string nombreDestino, string linkRecuperacion)
        {
            return "<div style=\"font-family:Arial,sans-serif;max-width:480px;margin:0 auto;padding:24px;\">" +
                   "<h2 style=\"color:#1B2A3B;margin:0 0 16px;\">CAM<span style=\"color:#E8455A;\">BUR</span></h2>" +
                   "<p style=\"color:#1B2A3B;\">Hola " + WebUtility.HtmlEncode(nombreDestino) + ",</p>" +
                   "<p style=\"color:#3D566E;line-height:1.6;\">Recibimos una solicitud para restablecer la contraseña de tu cuenta. " +
                   "Si fuiste vos, hacé clic en el siguiente botón (el enlace es válido por 30 minutos):</p>" +
                   "<p style=\"margin:24px 0;\"><a href=\"" + linkRecuperacion + "\" " +
                   "style=\"background:#1B2A3B;color:#F2EEE8;padding:12px 28px;border-radius:6px;text-decoration:none;display:inline-block;font-weight:bold;\">" +
                   "Restablecer contraseña</a></p>" +
                   "<p style=\"font-size:12px;color:#9AABBF;line-height:1.6;\">Si el botón no funciona, copiá y pegá este enlace en tu navegador:<br/>" +
                   "<a href=\"" + linkRecuperacion + "\" style=\"color:#3D566E;\">" + linkRecuperacion + "</a></p>" +
                   "<p style=\"font-size:12px;color:#9AABBF;line-height:1.6;\">Si no solicitaste este cambio, podés ignorar este correo: tu contraseña actual sigue siendo válida.</p>" +
                   "</div>";
        }
    }
}