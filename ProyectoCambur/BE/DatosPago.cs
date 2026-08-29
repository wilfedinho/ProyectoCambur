using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class DatosPago
    {
        public string TokenTarjeta { get; set; }
        public string PaymentMethodId { get; set; }
        public decimal Monto { get; set; }
        public string Descripcion { get; set; }
        public string EmailPagador { get; set; }
        public string DniPagador { get; set; }
    }
}