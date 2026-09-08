using System.Configuration;

namespace SERVICIOS
{
    public static class FabricaPasarelaPago
    {
        public static IPasarelaPago ObtenerPasarelaActiva()
        {
            string modo = ConfigurationManager.AppSettings["PasarelaPagoActiva"];

            IPasarelaPago pasarelaBase;
            switch (modo)
            {
                case "MercadoPago":
                    pasarelaBase = new PasarelaMercadoPago();
                    break;
                case "Simulada":
                    pasarelaBase = new PasarelaPagoSimulada();
                    break;
                case "Stripe":
                default:
                    pasarelaBase = new PasarelaStripe();
                    break;
            }

            return new PasarelaPagoConBitacora(pasarelaBase);
        }
    }
}