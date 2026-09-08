using BE;
using System;

namespace SERVICIOS
{
    public class PasarelaPagoSimulada : IPasarelaPago
    {
        private const string TARJETA_DE_PRUEBA_RECHAZO = "4000000000000002";

        public ResultadoPago CrearPago(DatosPago datosPago)
        {
            bool forzarRechazo = datosPago.TokenTarjeta == TARJETA_DE_PRUEBA_RECHAZO;

            if (forzarRechazo)
            {
                return new ResultadoPago
                {
                    Aprobado = false,
                    IdPagoExterno = null,
                    UltimosCuatroTarjeta = null,
                    MotivoRechazo = "pago rechazado (simulado, tarjeta de prueba de rechazo)"
                };
            }

            return new ResultadoPago
            {
                Aprobado = true,
                IdPagoExterno = "SIMULADO-" + Guid.NewGuid().ToString("N").Substring(0, 12).ToUpperInvariant(),
                UltimosCuatroTarjeta = "0000"
            };
        }
    }
}