using System;

namespace BE
{
    public class MensajeContacto
    {
        public int IdMensaje { get; set; }
        public string Nombre { get; set; }
        public string Email { get; set; }
        public string Asunto { get; set; }
        public string Mensaje { get; set; }
        public DateTime FechaEnvio { get; set; }
    }
}