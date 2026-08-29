using BE;

namespace SERVICIOS
{
    public interface IPasarelaPago
    {
        ResultadoPago CrearPago(DatosPago datosPago);
    }
}