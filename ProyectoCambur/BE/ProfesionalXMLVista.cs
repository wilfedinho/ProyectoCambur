using System;

namespace BE
{
    [Serializable]
    public class ProfesionalXmlVista
    {
        public int IdOrigen { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Dni { get; set; }
        public string Email { get; set; }
        public string Idioma { get; set; }
        public string RolPermiso { get; set; }
        public bool Activo { get; set; }
        public DateTime FechaRegistro { get; set; }

        public ProfesionalXmlVista()
        {
        }
    }
}