using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class ResultadoPago
    {
        public bool Aprobado { get; set; }
        public string IdPagoExterno { get; set; }
        public string UltimosCuatroTarjeta { get; set; }
        public string MotivoRechazo { get; set; }
    }
}