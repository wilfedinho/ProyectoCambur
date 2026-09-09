using System;

namespace BE
{
    [Serializable]
    public class ProfesionalXml
    {
        public int IdOrigen { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string DniEncriptado { get; set; }
        public string EmailEncriptado { get; set; }
        public string Idioma { get; set; }
        public string RolPermiso { get; set; }
        public bool Activo { get; set; }
        public DateTime FechaRegistro { get; set; }

        public ProfesionalXml()
        {
        }
    }
}