using System.Collections.Generic;

namespace SERVICIOS
{
    public interface ITraductorAutomatico
    {
        Dictionary<string, string> Traducir(List<string> claves, string idiomaDestinoIso);
    }
}