using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class SeccionesPerfilPaciente
    {
        public bool InformacionSuficiente { get; set; }

        public string Descripcion { get; set; }
        public string Dimensiones { get; set; }
        public string Patrones { get; set; }
        public string Consideraciones { get; set; }

        public string NombreModelo { get; set; }

        public SeccionesPerfilPaciente()
        {
            InformacionSuficiente = true;
        }

        public bool SeccionesIACompletas()
        {
            return !string.IsNullOrWhiteSpace(Descripcion) && !string.IsNullOrWhiteSpace(Dimensiones) &&
                   !string.IsNullOrWhiteSpace(Patrones) && !string.IsNullOrWhiteSpace(Consideraciones);
        }
    }
}