using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class SeccionesInformeDerivacion
    {
        public bool InformacionSuficiente { get; set; }
        public string SintesisDiagnostica { get; set; }
        public string Andamiajes { get; set; }
        public string Objetivos { get; set; }
        public string ModalidadTrabajo { get; set; }
        public string EspecialidadDerivacion { get; set; }
        public string ProfesionalDestinatario { get; set; }
        public string Institucion { get; set; }
        public string MotivoDerivacion { get; set; }
        public string Firma { get; set; }

        public SeccionesInformeDerivacion()
        {
            InformacionSuficiente = true;
        }

        public bool SeccionesIACompletas()
        {
            return !string.IsNullOrWhiteSpace(SintesisDiagnostica) && !string.IsNullOrWhiteSpace(Andamiajes) &&
                   !string.IsNullOrWhiteSpace(Objetivos) && !string.IsNullOrWhiteSpace(ModalidadTrabajo);
        }
    }
}