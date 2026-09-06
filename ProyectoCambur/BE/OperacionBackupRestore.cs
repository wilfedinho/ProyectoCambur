using System;

namespace BE
{
    public class OperacionBackupRestore
    {
        public int IdHistorial { get; set; }
        public string TipoOperacion { get; set; }
        public string NombreArchivo { get; set; }
        public DateTime FechaOperacion { get; set; }
        public string Resultado { get; set; }
        public string DetalleError { get; set; }
        public string Usuario { get; set; }

        public OperacionBackupRestore()
        {
        }
    }
}