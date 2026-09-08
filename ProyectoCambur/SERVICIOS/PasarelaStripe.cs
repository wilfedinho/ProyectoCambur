using BE;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace SERVICIOS
{
    public class PasarelaStripe : IPasarelaPago
    {
        private const string URL_PAYMENT_INTENTS = "https://api.stripe.com/v1/payment_intents";
        private const string URL_PAYMENT_METHODS = "https://api.stripe.com/v1/payment_methods/";

        static PasarelaStripe()
        {
            ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;
        }

        public ResultadoPago CrearPago(DatosPago datosPago)
        {
            string secretKey = ConfigurationManager.AppSettings["StripeSecretKey"];
            if (string.IsNullOrWhiteSpace(secretKey))
            {
                throw new InvalidOperationException(
                    "Falta configurar StripeSecretKey en informacion_traductor.config para poder procesar pagos.");
            }

            try
            {
                int montoEnCentavos = (int)Math.Round(datosPago.Monto * 100m, MidpointRounding.AwayFromZero);

                List<KeyValuePair<string, string>> parametros = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("amount", montoEnCentavos.ToString(CultureInfo.InvariantCulture)),
                    new KeyValuePair<string, string>("currency", "usd"),
                    new KeyValuePair<string, string>("payment_method", datosPago.TokenTarjeta),
                    new KeyValuePair<string, string>("confirm", "true"),
                    new KeyValuePair<string, string>("description", datosPago.Descripcion),
                    new KeyValuePair<string, string>("receipt_email", datosPago.EmailPagador),
                    new KeyValuePair<string, string>("payment_method_types[]", "card"),
                };

                using (HttpClient cliente = new HttpClient())
                {
                    cliente.Timeout = TimeSpan.FromSeconds(15);
                    cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", secretKey);
                    cliente.DefaultRequestHeaders.Add("Idempotency-Key", Guid.NewGuid().ToString());

                    HttpContent contenido = new FormUrlEncodedContent(parametros);

                    HttpResponseMessage respuesta = EjecutarSincrono(() => cliente.PostAsync(URL_PAYMENT_INTENTS, contenido));
                    string cuerpoRespuesta = EjecutarSincrono(() => respuesta.Content.ReadAsStringAsync());

                    JavaScriptSerializer serializador = new JavaScriptSerializer();
                    Dictionary<string, object> resultado =
                        serializador.Deserialize<Dictionary<string, object>>(cuerpoRespuesta);

                    if (resultado == null)
                    {
                        throw new ExcepcionTraducible("error_pago_timeout");
                    }

                    if (!respuesta.IsSuccessStatusCode)
                    {
                        return ResultadoDesdeError(resultado);
                    }

                    string estado = resultado.ContainsKey("status") ? resultado["status"].ToString() : null;
                    string idPago = resultado.ContainsKey("id") ? resultado["id"].ToString() : null;

                    if (estado == "succeeded")
                    {
                        string ultimosCuatro = ObtenerUltimosCuatro(datosPago.TokenTarjeta, secretKey);
                        return new ResultadoPago
                        {
                            Aprobado = true,
                            IdPagoExterno = idPago,
                            UltimosCuatroTarjeta = ultimosCuatro
                        };
                    }
                    return new ResultadoPago
                    {
                        Aprobado = false,
                        IdPagoExterno = idPago,
                        MotivoRechazo = "el pago requiere un paso de verificación adicional de tu banco que esta integración todavía no soporta — probá con otra tarjeta"
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

        private ResultadoPago ResultadoDesdeError(Dictionary<string, object> resultado)
        {
            string motivo = "tu banco rechazó el pago";

            if (resultado.ContainsKey("error") && resultado["error"] is Dictionary<string, object>)
            {
                Dictionary<string, object> error = (Dictionary<string, object>)resultado["error"];
                if (error.ContainsKey("message") && error["message"] != null)
                {
                    motivo = error["message"].ToString();
                }
            }

            return new ResultadoPago
            {
                Aprobado = false,
                MotivoRechazo = motivo
            };
        }

        private string ObtenerUltimosCuatro(string paymentMethodId, string secretKey)
        {
            try
            {
                using (HttpClient cliente = new HttpClient())
                {
                    cliente.Timeout = TimeSpan.FromSeconds(10);
                    cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", secretKey);

                    HttpResponseMessage respuesta = EjecutarSincrono(() => cliente.GetAsync(URL_PAYMENT_METHODS + paymentMethodId));
                    if (!respuesta.IsSuccessStatusCode) return null;

                    string cuerpoRespuesta = EjecutarSincrono(() => respuesta.Content.ReadAsStringAsync());
                    JavaScriptSerializer serializador = new JavaScriptSerializer();
                    Dictionary<string, object> resultado =
                        serializador.Deserialize<Dictionary<string, object>>(cuerpoRespuesta);

                    if (resultado == null || !resultado.ContainsKey("card") || resultado["card"] == null) return null;
                    Dictionary<string, object> tarjeta = resultado["card"] as Dictionary<string, object>;
                    if (tarjeta == null || !tarjeta.ContainsKey("last4")) return null;
                    return tarjeta["last4"] as string;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        private T EjecutarSincrono<T>(Func<Task<T>> tarea)
        {
            return Task.Run(tarea).GetAwaiter().GetResult();
        }
    }
}