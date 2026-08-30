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
    public class ClienteOpenRouter : IClienteIA
    {
        private const string URL_CHAT_COMPLETIONS = "https://openrouter.ai/api/v1/chat/completions";
        private const string MODELO_POR_DEFECTO = "openai/gpt-4o-mini";
        private const int TIMEOUT_SEGUNDOS_POR_DEFECTO = 30;

        private const string PROMPT_SISTEMA =
            "Sos un asistente que organiza y sintetiza información clínica ya registrada por un " +
            "psicólogo sobre su paciente. Tu tarea es exclusivamente organizativa: reformular y " +
            "agrupar la información que se te da, identificar patrones y repeticiones. NUNCA " +
            "emitas diagnósticos, juicios clínicos, opiniones ni recomendaciones de tratamiento " +
            "que no estén explícitamente presentes en el material recibido, y no inventes ni " +
            "asumas datos que no se te hayan dado. Respondé siempre en español rioplatense, en " +
            "tono profesional y neutro. Respondé ÚNICAMENTE con un objeto JSON válido, sin texto " +
            "adicional antes ni después, con exactamente estas 5 claves de tipo string: " +
            "\"ContextoGeneral\", \"Evolucion\", \"TemasRecurrentes\", \"Intervenciones\", " +
            "\"Observaciones\". Cada valor debe ser un párrafo (o una lista de puntos separados " +
            "por saltos de línea, usando \"• \" al inicio de cada ítem) redactado en base " +
            "únicamente a la información provista.";

        static ClienteOpenRouter()
        {
            ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;
        }

        public SeccionesResumenClinico GenerarResumenClinico(string informacionClinica)
        {
            string apiKey = ConfigurationManager.AppSettings["OpenRouterApiKey"];
            if (string.IsNullOrWhiteSpace(apiKey))
            {
                throw new InvalidOperationException(
                    "Falta configurar OpenRouterApiKey en informacion_traductor.config para poder generar resúmenes con IA.");
            }

            string modelo = ConfigurationManager.AppSettings["OpenRouterModel"];
            if (string.IsNullOrWhiteSpace(modelo)) modelo = MODELO_POR_DEFECTO;

            int timeoutSegundos = TIMEOUT_SEGUNDOS_POR_DEFECTO;
            int.TryParse(ConfigurationManager.AppSettings["OpenRouterTimeoutSegundos"], out int timeoutConfigurado);
            if (timeoutConfigurado > 0) timeoutSegundos = timeoutConfigurado;

            JavaScriptSerializer serializador = new JavaScriptSerializer();

            try
            {
                HttpStatusCode statusCode;
                string cuerpoRespuestaTexto = IntentarGenerar(apiKey, modelo, timeoutSegundos, informacionClinica, serializador, incluirResponseFormat: true, out bool exito, out statusCode);

                if (!exito && statusCode == (HttpStatusCode)429)
                {
                    System.Threading.Thread.Sleep(3000);
                    cuerpoRespuestaTexto = IntentarGenerar(apiKey, modelo, timeoutSegundos, informacionClinica, serializador, incluirResponseFormat: true, out exito, out statusCode);
                }
                else if (!exito)
                {
                    cuerpoRespuestaTexto = IntentarGenerar(apiKey, modelo, timeoutSegundos, informacionClinica, serializador, incluirResponseFormat: false, out exito, out statusCode);

                    if (!exito && statusCode == (HttpStatusCode)429)
                    {
                        System.Threading.Thread.Sleep(3000);
                        cuerpoRespuestaTexto = IntentarGenerar(apiKey, modelo, timeoutSegundos, informacionClinica, serializador, incluirResponseFormat: false, out exito, out statusCode);
                    }
                }

                if (!exito)
                {
                    if (statusCode == (HttpStatusCode)429)
                    {
                        throw new ExcepcionTraducible("error_ia_saturado");
                    }

                    throw new ExcepcionTraducible("error_ia_comunicacion");
                }

                Dictionary<string, object> respuestaCompleta;
                try
                {
                    respuestaCompleta = serializador.Deserialize<Dictionary<string, object>>(cuerpoRespuestaTexto);
                }
                catch (Exception)
                {
                    throw new ExcepcionTraducible("error_ia_respuesta_invalida");
                }

                string contenidoJson = LimpiarBloqueMarkdown(ExtraerContenidoDeChoice(respuestaCompleta));
                if (string.IsNullOrWhiteSpace(contenidoJson))
                {
                    throw new ExcepcionTraducible("error_ia_respuesta_invalida");
                }

                SeccionesResumenClinico secciones;
                try
                {
                    secciones = serializador.Deserialize<SeccionesResumenClinico>(contenidoJson);
                }
                catch (Exception)
                {
                    throw new ExcepcionTraducible("error_ia_respuesta_invalida");
                }

                if (secciones == null || !secciones.EstaCompleta())
                {
                    throw new ExcepcionTraducible("error_ia_respuesta_invalida");
                }

                return secciones;
            }
            catch (ExcepcionTraducible)
            {
                throw;
            }
            catch (Exception)
            {
                throw new ExcepcionTraducible("error_ia_comunicacion");
            }
        }
        private string IntentarGenerar(string apiKey, string modelo, int timeoutSegundos, string informacionClinica, JavaScriptSerializer serializador, bool incluirResponseFormat, out bool exito, out HttpStatusCode statusCode)
        {
            Dictionary<string, object> cuerpo = new Dictionary<string, object>
            {
                { "model", modelo },
                { "temperature", 0.3 },
                { "messages", new List<object>
                    {
                        new Dictionary<string, object> { { "role", "system" }, { "content", PROMPT_SISTEMA } },
                        new Dictionary<string, object> { { "role", "user" }, { "content", informacionClinica } }
                    }
                }
            };

            if (incluirResponseFormat)
            {
                cuerpo["response_format"] = new Dictionary<string, object> { { "type", "json_object" } };
            }

            string json = serializador.Serialize(cuerpo);

            using (HttpClient cliente = new HttpClient())
            {
                cliente.Timeout = TimeSpan.FromSeconds(timeoutSegundos);
                cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
                cliente.DefaultRequestHeaders.Add("HTTP-Referer", "https://cambur.local");
                cliente.DefaultRequestHeaders.Add("X-Title", "Cambur");

                StringContent contenido = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage respuesta;
                string cuerpoRespuesta;
                try
                {
                    respuesta = EjecutarSincrono(() => cliente.PostAsync(URL_CHAT_COMPLETIONS, contenido));
                    cuerpoRespuesta = EjecutarSincrono(() => respuesta.Content.ReadAsStringAsync());
                }
                catch (Exception ex) when (EsTimeout(ex))
                {
                    throw new ExcepcionTraducible("error_ia_timeout");
                }

                exito = respuesta.IsSuccessStatusCode;
                statusCode = respuesta.StatusCode;
                return cuerpoRespuesta;
            }
        }

        private string ExtraerContenidoDeChoice(Dictionary<string, object> respuestaCompleta)
        {
            if (respuestaCompleta == null || !respuestaCompleta.ContainsKey("choices")) return null;

            var choices = respuestaCompleta["choices"] as System.Collections.ArrayList;
            if (choices == null || choices.Count == 0) return null;

            var primerChoice = choices[0] as Dictionary<string, object>;
            if (primerChoice == null || !primerChoice.ContainsKey("message")) return null;

            var mensaje = primerChoice["message"] as Dictionary<string, object>;
            if (mensaje == null || !mensaje.ContainsKey("content")) return null;

            return mensaje["content"] as string;
        }
        private string LimpiarBloqueMarkdown(string texto)
        {
            if (string.IsNullOrWhiteSpace(texto)) return texto;
            string limpio = texto.Trim();
            if (limpio.StartsWith("```"))
            {
                int primerSalto = limpio.IndexOf('\n');
                if (primerSalto >= 0) limpio = limpio.Substring(primerSalto + 1);
                int cierre = limpio.LastIndexOf("```", StringComparison.Ordinal);
                if (cierre >= 0) limpio = limpio.Substring(0, cierre);
            }
            return limpio.Trim();
        }

        private bool EsTimeout(Exception ex)
        {
            return ex is TaskCanceledException || ex is OperationCanceledException;
        }

        private T EjecutarSincrono<T>(Func<Task<T>> tarea)
        {
            return Task.Run(tarea).GetAwaiter().GetResult();
        }
    }
}