using BE;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace SERVICIOS
{
    public class PasarelaMercadoPago : IPasarelaPago
    {
        private const string URL_PAGOS = "https://api.mercadopago.com/v1/payments";
        static PasarelaMercadoPago()
        {
            ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;
        }
        private static readonly Dictionary<string, string> MOTIVOS_RECHAZO = new Dictionary<string, string>
        {
            { "cc_rejected_insufficient_amount", "fondos insuficientes" },
            { "cc_rejected_bad_filled_security_code", "código de seguridad (CVV) inválido" },
            { "cc_rejected_bad_filled_date", "fecha de vencimiento inválida" },
            { "cc_rejected_bad_filled_card_number", "número de tarjeta inválido" },
            { "cc_rejected_bad_filled_other", "datos de la tarjeta inválidos" },
            { "cc_rejected_call_for_authorize", "tu banco requiere autorizar el pago telefónicamente" },
            { "cc_rejected_card_disabled", "tarjeta deshabilitada — contactá a tu banco" },
            { "cc_rejected_duplicated_payment", "ya se registró un pago idéntico hace instantes" },
            { "cc_rejected_high_risk", "el pago fue rechazado por el sistema antifraude" },
            { "cc_rejected_max_attempts", "se superó la cantidad máxima de intentos permitidos" },
            { "cc_rejected_blacklist", "la tarjeta no puede procesar el pago" },
            { "cc_rejected_other_reason", "tu banco rechazó el pago" },
        };

        public ResultadoPago CrearPago(DatosPago datosPago)
        {
            string accessToken = ConfigurationManager.AppSettings["MercadoPagoAccessToken"];
            if (string.IsNullOrWhiteSpace(accessToken))
            {
                throw new InvalidOperationException(
                    "Falta configurar MercadoPagoAccessToken en informacion_traductor.config para poder procesar pagos.");
            }

            try
            {
                Dictionary<string, object> cuerpo = new Dictionary<string, object>
                {
                    { "transaction_amount", datosPago.Monto },
                    { "token", datosPago.TokenTarjeta },
                    { "payment_method_id", datosPago.PaymentMethodId },
                    { "description", datosPago.Descripcion },
                    { "installments", 1 },
                    { "payer", new Dictionary<string, object>
                        {
                            { "email", datosPago.EmailPagador },
                            { "identification", new Dictionary<string, object>
                                {
                                    { "type", "DNI" },
                                    { "number", datosPago.DniPagador }
                                }
                            }
                        }
                    }
                };

                JavaScriptSerializer serializador = new JavaScriptSerializer();
                string json = serializador.Serialize(cuerpo);

                using (HttpClient cliente = new HttpClient())
                {
                    cliente.Timeout = TimeSpan.FromSeconds(15);
                    cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                    cliente.DefaultRequestHeaders.Add("X-Idempotency-Key", Guid.NewGuid().ToString());

                    StringContent contenido = new StringContent(json, Encoding.UTF8, "application/json");

                    HttpResponseMessage respuesta = EjecutarSincrono(() => cliente.PostAsync(URL_PAGOS, contenido));
                    string cuerpoRespuesta = EjecutarSincrono(() => respuesta.Content.ReadAsStringAsync());

                    Dictionary<string, object> resultado =
                        serializador.Deserialize<Dictionary<string, object>>(cuerpoRespuesta);

                    if (resultado == null || !resultado.ContainsKey("status"))
                    {
                        throw new ExcepcionTraducible("error_pago_timeout");
                    }

                    string estado = resultado["status"].ToString();
                    string idPago = resultado.ContainsKey("id") ? resultado["id"].ToString() : null;
                    string ultimosCuatro = ExtraerUltimosCuatro(resultado);

                    if (estado == "approved")
                    {
                        return new ResultadoPago
                        {
                            Aprobado = true,
                            IdPagoExterno = idPago,
                            UltimosCuatroTarjeta = ultimosCuatro
                        };
                    }

                    string statusDetail = resultado.ContainsKey("status_detail") ? resultado["status_detail"].ToString() : null;
                    string motivo = (statusDetail != null && MOTIVOS_RECHAZO.ContainsKey(statusDetail))
                        ? MOTIVOS_RECHAZO[statusDetail]
                        : "tu banco rechazó el pago";

                    return new ResultadoPago
                    {
                        Aprobado = false,
                        IdPagoExterno = idPago,
                        UltimosCuatroTarjeta = ultimosCuatro,
                        MotivoRechazo = motivo
                    };
                }
            }
            catch (ExcepcionTraducible)
            {
                throw;
            }
            catch (Exception)
            {
                throw new ExcepcionTraducible("error_pago_timeout");
            }
        }

        private string ExtraerUltimosCuatro(Dictionary<string, object> resultado)
        {
            if (!resultado.ContainsKey("card") || resultado["card"] == null) return null;
            Dictionary<string, object> tarjeta = resultado["card"] as Dictionary<string, object>;
            if (tarjeta == null || !tarjeta.ContainsKey("last_four_digits")) return null;
            return tarjeta["last_four_digits"] as string;
        }
        private T EjecutarSincrono<T>(Func<Task<T>> tarea)
        {
            return Task.Run(tarea).GetAwaiter().GetResult();
        }
    }
}