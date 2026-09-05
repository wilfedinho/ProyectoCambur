using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;

namespace SERVICIOS
{
    public interface IClienteIA
    {
        SeccionesResumenClinico GenerarResumenClinico(string informacionClinica);
        SeccionesInformeDerivacion GenerarInformeDerivacion(string informacionClinica);
        SeccionesPerfilPaciente GenerarPerfilPaciente(string informacionClinica);
    }
}