using System.Collections.Generic;

namespace SERVICIOS
{
    public interface ITraductorAutomatico
    {
        List<string> Traducir(List<string> textos, string idiomaOrigenIso, string idiomaDestinoIso);
    }
}