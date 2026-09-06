using System;

namespace BE
{
    public class BackupDisponible
    {
        public string NombreArchivo { get; set; }
        public DateTime Fecha { get; set; }
        public long TamanioBytes { get; set; }
        public string NombreBaseDatosOrigen { get; set; }
        public bool CoincideBaseDatos { get; set; }
    }
}