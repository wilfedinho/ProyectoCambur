using BE;
using System.Collections.Generic;

namespace BLL
{
    public interface IExportadorDocumento
    {
        bool EstaDisponible(int idPaciente);
        List<DocumentoExportable> ObtenerDisponibles(int idPaciente);
        byte[] GenerarPdf(int idPsicologo, Paciente paciente, int? idDocumento, out string tituloDocumento);
    }
}