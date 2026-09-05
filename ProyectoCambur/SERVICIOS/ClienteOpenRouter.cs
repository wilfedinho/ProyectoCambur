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
            "tono profesional y neutro. " +
            "Antes de redactar nada, evaluá si el contenido recibido tiene sustancia clínica real " +
            "y suficiente para elaborar una síntesis genuina: registros vacíos o casi vacíos, " +
            "texto sin relación con una consulta psicológica (por ejemplo bromas, relleno al " +
            "azar, cadenas de caracteres sin sentido, o contenido genérico que no aporta ningún " +
            "dato concreto del paciente) cuentan como información insuficiente, " +
            "independientemente de cuántas consultas se hayan incluido. " +
            "Respondé ÚNICAMENTE con un objeto JSON válido, sin texto adicional antes ni después, " +
            "con exactamente estas 6 claves: \"InformacionSuficiente\" (booleano) y, de tipo " +
            "string, \"ContextoGeneral\", \"Evolucion\", \"TemasRecurrentes\", \"Intervenciones\", " +
            "\"Observaciones\". Si determinás que la información es insuficiente según el " +
            "criterio anterior, poné \"InformacionSuficiente\" en false y dejá las 5 claves de " +
            "texto como string vacío (\"\") — no completes ninguna sección igual. Si la " +
            "información sí es suficiente, poné \"InformacionSuficiente\" en true y completá las " +
            "5 claves normalmente, donde cada valor debe ser un párrafo (o una lista de puntos " +
            "separados por saltos de línea, usando \"• \" al inicio de cada ítem) redactado en " +
            "base únicamente a la información provista.";

        private const string PROMPT_INFORME =
            "Sos un asistente que redacta la sección clínica de un informe de derivación, a partir " +
            "de información ya registrada por un psicólogo sobre su paciente (consultas, historial " +
            "clínico y evolución observada). Tu tarea es exclusivamente organizativa: reformular y " +
            "sintetizar en un formato apto para ser leído por otro profesional de la salud al que " +
            "se deriva al paciente. NUNCA emitas diagnósticos definitivos, juicios clínicos, ni " +
            "recomendaciones que no estén explícitamente respaldadas por el material recibido, y no " +
            "inventes ni asumas datos que no se te hayan dado. Respondé siempre en español " +
            "rioplatense, en tono profesional y neutro, apropiado para comunicación entre " +
            "profesionales de la salud. " +
            "Antes de redactar nada, evaluá si el contenido recibido tiene sustancia clínica real y " +
            "suficiente para elaborar un informe de derivación genuino: registros vacíos o casi " +
            "vacíos, texto sin relación con una consulta psicológica (bromas, relleno al azar, " +
            "cadenas de caracteres sin sentido, o contenido genérico que no aporta ningún dato " +
            "concreto del paciente) cuentan como información insuficiente, independientemente de " +
            "cuántas consultas se hayan incluido. " +
            "Respondé ÚNICAMENTE con un objeto JSON válido, sin texto adicional antes ni después, " +
            "con exactamente estas 5 claves: \"InformacionSuficiente\" (booleano) y, de tipo " +
            "string, \"SintesisDiagnostica\", \"Andamiajes\", \"Objetivos\", \"ModalidadTrabajo\". " +
            "\"SintesisDiagnostica\" resume el cuadro clínico observado; \"Andamiajes\" describe las " +
            "técnicas e intervenciones terapéuticas utilizadas (como lista de puntos separados por " +
            "saltos de línea, usando \"• \" al inicio de cada ítem); \"Objetivos\" enumera los " +
            "objetivos terapéuticos trabajados (mismo formato de lista); \"ModalidadTrabajo\" " +
            "describe la modalidad y frecuencia del tratamiento. Si determinás que la información " +
            "es insuficiente según el criterio anterior, poné \"InformacionSuficiente\" en false y " +
            "dejá las 4 claves de texto como string vacío (\"\") — no completes ninguna sección " +
            "igual. Si la información sí es suficiente, poné \"InformacionSuficiente\" en true y " +
            "completá las 4 claves normalmente en base únicamente a la información provista.";

        private const string PROMPT_PERFIL =
            "Sos un asistente que redacta perfiles descriptivos y contextuales de pacientes en base a " +
            "información clínica ya registrada por su psicólogo (consultas, historial clínico y " +
            "evolución observada) y al modelo de evaluación psicológica indicado en el mensaje del " +
            "usuario. Tu tarea es exclusivamente descriptiva: interpretar la información disponible a " +
            "la luz del modelo indicado, NUNCA emitir un diagnóstico clínico formal. Dejá siempre en " +
            "claro que se trata de una representación orientativa, no de un diagnóstico. No inventes " +
            "ni asumas datos que no se te hayan dado. Respondé siempre en español rioplatense, en tono " +
            "profesional y neutro. " +
            "Antes de redactar nada, evaluá si el contenido recibido tiene sustancia clínica real y " +
            "suficiente para elaborar un perfil genuino según el modelo indicado: registros vacíos o " +
            "casi vacíos, texto sin relación con una consulta psicológica (bromas, relleno al azar, " +
            "cadenas de caracteres sin sentido, o contenido genérico que no aporta ningún dato " +
            "concreto del paciente) cuentan como información insuficiente, independientemente de " +
            "cuántas consultas se hayan incluido. " +
            "Respondé ÚNICAMENTE con un objeto JSON válido, sin texto adicional antes ni después, " +
            "con exactamente estas 5 claves: \"InformacionSuficiente\" (booleano) y, de tipo string, " +
            "\"Descripcion\", \"Dimensiones\", \"Patrones\", \"Consideraciones\". \"Descripcion\" es un " +
            "párrafo con la descripción general del perfil según el modelo indicado; \"Dimensiones\" " +
            "detalla cada dimensión/eje propio de ese modelo evaluado en el paciente (como lista de " +
            "puntos separados por saltos de línea, usando \"• \" al inicio de cada ítem); \"Patrones\" " +
            "enumera patrones recurrentes observados en el contexto clínico relacionados con esas " +
            "dimensiones (mismo formato de lista); \"Consideraciones\" sugiere consideraciones para el " +
            "tratamiento derivadas del perfil. Si determinás que la información es insuficiente según " +
            "el criterio anterior, poné \"InformacionSuficiente\" en false y dejá las 4 claves de " +
            "texto como string vacío (\"\") — no completes ninguna sección igual. Si la información sí " +
            "es suficiente, poné \"InformacionSuficiente\" en true y completá las 4 claves normalmente " +
            "en base únicamente a la información provista y al modelo de evaluación indicado.";

        static ClienteOpenRouter()
        {
            ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;
        }

        public SeccionesResumenClinico GenerarResumenClinico(string informacionClinica)
        {
            JavaScriptSerializer serializador = new JavaScriptSerializer();

            string contenidoJson = EjecutarLlamadaIA(PROMPT_SISTEMA, informacionClinica, serializador);

            SeccionesResumenClinico secciones;
            try
            {
                secciones = serializador.Deserialize<SeccionesResumenClinico>(contenidoJson);
            }
            catch (Exception)
            {
                throw new ExcepcionTraducible("error_ia_respuesta_invalida");
            }

            if (secciones == null)
            {
                throw new ExcepcionTraducible("error_ia_respuesta_invalida");
            }
            if (!secciones.InformacionSuficiente)
            {
                throw new ExcepcionTraducible("error_resumen_informacion_insuficiente");
            }

            if (!secciones.EstaCompleta())
            {
                throw new ExcepcionTraducible("error_ia_respuesta_invalida");
            }

            return secciones;
        }
        public SeccionesInformeDerivacion GenerarInformeDerivacion(string informacionClinica)
        {
            JavaScriptSerializer serializador = new JavaScriptSerializer();

            string contenidoJson = EjecutarLlamadaIA(PROMPT_INFORME, informacionClinica, serializador);

            SeccionesInformeDerivacion secciones;
            try
            {
                secciones = serializador.Deserialize<SeccionesInformeDerivacion>(contenidoJson);
            }
            catch (Exception)
            {
                throw new ExcepcionTraducible("error_ia_respuesta_invalida");
            }

            if (secciones == null)
            {
                throw new ExcepcionTraducible("error_ia_respuesta_invalida");
            }
            if (!secciones.InformacionSuficiente)
            {
                throw new ExcepcionTraducible("error_informe_informacion_insuficiente");
            }

            if (!secciones.SeccionesIACompletas())
            {
                throw new ExcepcionTraducible("error_ia_respuesta_invalida");
            }

            return secciones;
        }
        public SeccionesPerfilPaciente GenerarPerfilPaciente(string informacionClinica)
        {
            JavaScriptSerializer serializador = new JavaScriptSerializer();

            string contenidoJson = EjecutarLlamadaIA(PROMPT_PERFIL, informacionClinica, serializador);

            SeccionesPerfilPaciente secciones;
            try
            {
                secciones = serializador.Deserialize<SeccionesPerfilPaciente>(contenidoJson);
            }
            catch (Exception)
            {
                throw new ExcepcionTraducible("error_ia_respuesta_invalida");
            }

            if (secciones == null)
            {
                throw new ExcepcionTraducible("error_ia_respuesta_invalida");
            }
            if (!secciones.InformacionSuficiente)
            {
                throw new ExcepcionTraducible("error_perfil_informacion_insuficiente");
            }

            if (!secciones.SeccionesIACompletas())
            {
                throw new ExcepcionTraducible("error_ia_respuesta_invalida");
            }

            return secciones;
        }
        private string EjecutarLlamadaIA(string promptSistema, string informacionUsuario, JavaScriptSerializer serializador)
        {
            string apiKey = ConfigurationManager.AppSettings["OpenRouterApiKey"];
            if (string.IsNullOrWhiteSpace(apiKey))
            {
                throw new InvalidOperationException(
                    "Falta configurar OpenRouterApiKey en informacion_traductor.config para poder generar contenido con IA.");
            }

            string modelo = ConfigurationManager.AppSettings["OpenRouterModel"];
            if (string.IsNullOrWhiteSpace(modelo)) modelo = MODELO_POR_DEFECTO;

            int timeoutSegundos = TIMEOUT_SEGUNDOS_POR_DEFECTO;
            int.TryParse(ConfigurationManager.AppSettings["OpenRouterTimeoutSegundos"], out int timeoutConfigurado);
            if (timeoutConfigurado > 0) timeoutSegundos = timeoutConfigurado;

            try
            {
                HttpStatusCode statusCode;
                string cuerpoRespuestaTexto = IntentarGenerar(apiKey, modelo, timeoutSegundos, promptSistema, informacionUsuario, serializador, incluirResponseFormat: true, out bool exito, out statusCode);

                if (!exito && statusCode == (HttpStatusCode)429)
                {
                    System.Threading.Thread.Sleep(3000);
                    cuerpoRespuestaTexto = IntentarGenerar(apiKey, modelo, timeoutSegundos, promptSistema, informacionUsuario, serializador, incluirResponseFormat: true, out exito, out statusCode);
                }
                else if (!exito)
                {
                    cuerpoRespuestaTexto = IntentarGenerar(apiKey, modelo, timeoutSegundos, promptSistema, informacionUsuario, serializador, incluirResponseFormat: false, out exito, out statusCode);

                    if (!exito && statusCode == (HttpStatusCode)429)
                    {
                        System.Threading.Thread.Sleep(3000);
                        cuerpoRespuestaTexto = IntentarGenerar(apiKey, modelo, timeoutSegundos, promptSistema, informacionUsuario, serializador, incluirResponseFormat: false, out exito, out statusCode);
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

                return contenidoJson;
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

        private string IntentarGenerar(string apiKey, string modelo, int timeoutSegundos, string promptSistema, string informacionUsuario, JavaScriptSerializer serializador, bool incluirResponseFormat, out bool exito, out HttpStatusCode statusCode)
        {
            Dictionary<string, object> cuerpo = new Dictionary<string, object>
            {
                { "model", modelo },
                { "temperature", 0.3 },
                { "messages", new List<object>
                    {
                        new Dictionary<string, object> { { "role", "system" }, { "content", promptSistema } },
                        new Dictionary<string, object> { { "role", "user" }, { "content", informacionUsuario } }
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