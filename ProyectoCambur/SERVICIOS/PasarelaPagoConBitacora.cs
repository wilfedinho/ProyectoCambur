using BE;

namespace SERVICIOS
{
    public class PasarelaPagoConBitacora : IPasarelaPago
    {
        private readonly IPasarelaPago pasarelaDecorada;

        public PasarelaPagoConBitacora(IPasarelaPago pasarelaAEnvolver)
        {
            pasarelaDecorada = pasarelaAEnvolver;
        }

        public ResultadoPago CrearPago(DatosPago datosPago)
        {
            ResultadoPago resultado = pasarelaDecorada.CrearPago(datosPago);

            GestorBitacora gestorBitacora = new GestorBitacora();
            if (resultado.Aprobado)
            {
                gestorBitacora.RegistrarEvento(
                    datosPago.EmailPagador,
                    EventosBitacora.MOD_PAGOS,
                    EventosBitacora.DESC_PAGO_APROBADO,
                    EventosBitacora.CRIT_PAGO_APROBADO);
            }
            else
            {
                gestorBitacora.RegistrarEvento(
                    datosPago.EmailPagador,
                    EventosBitacora.MOD_PAGOS,
                    EventosBitacora.DESC_PAGO_RECHAZADO,
                    EventosBitacora.CRIT_PAGO_RECHAZADO);
            }

            return resultado;
        }
    }
}