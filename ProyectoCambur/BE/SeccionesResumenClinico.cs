using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class SeccionesResumenClinico
    {
        public string ContextoGeneral { get; set; }
        public string Evolucion { get; set; }
        public string TemasRecurrentes { get; set; }
        public string Intervenciones { get; set; }
        public string Observaciones { get; set; }

        public SeccionesResumenClinico()
        {
        }

        public bool EstaCompleta()
        {
            return !string.IsNullOrWhiteSpace(ContextoGeneral) && !string.IsNullOrWhiteSpace(Evolucion) && !string.IsNullOrWhiteSpace(TemasRecurrentes) && !string.IsNullOrWhiteSpace(Intervenciones) && !string.IsNullOrWhiteSpace(Observaciones);
        }
    }
}
