using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web.Script.Serialization;

namespace SERVICIOS
{
    public class TraductorAzure : ITraductorAutomatico
    {
        private const string ENDPOINT_BASE = "https://api.cognitive.microsofttranslator.com/translate?api-version=3.0";
        private const int MAX_TEXTOS_POR_LLAMADA = 100; 

        public List<string> Traducir(List<string> textos, string idiomaOrigenIso, string idiomaDestinoIso)
        {
            if (textos == null || textos.Count == 0)
            {
                return new List<string>();
            }

            string apiKey = ConfigurationManager.AppSettings["AzureTranslatorKey"];
            string region = ConfigurationManager.AppSettings["AzureTranslatorRegion"];

            if (string.IsNullOrWhiteSpace(apiKey))
            {
                throw new InvalidOperationException(
                    "Falta configurar AzureTranslatorKey en el Web.config para poder traducir automaticamente.");
            }

            List<string> resultado = new List<string>();

           
            for (int i = 0; i < textos.Count; i += MAX_TEXTOS_POR_LLAMADA)
            {
                List<string> lote = textos.Skip(i).Take(MAX_TEXTOS_POR_LLAMADA).ToList();
                resultado.AddRange(TraducirLote(lote, idiomaOrigenIso, idiomaDestinoIso, apiKey, region));
            }

            return resultado;
        }

        private List<string> TraducirLote(List<string> lote, string idiomaOrigenIso, string idiomaDestinoIso, string apiKey, string region)
        {
            string url = ENDPOINT_BASE + "&from=" + idiomaOrigenIso + "&to=" + idiomaDestinoIso;

            JavaScriptSerializer serializer = new JavaScriptSerializer();

            var cuerpoPedido = lote.Select(texto => new Dictionary<string, string> { { "Text", texto } }).ToList();
            string jsonPedido = serializer.Serialize(cuerpoPedido);

            using (HttpClient cliente = new HttpClient())
            {
                cliente.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", apiKey);
                if (!string.IsNullOrWhiteSpace(region))
                {
                    cliente.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Region", region);
                }

                HttpContent contenido = new StringContent(jsonPedido, Encoding.UTF8, "application/json");
                HttpResponseMessage respuesta = cliente.PostAsync(url, contenido).Result;
                respuesta.EnsureSuccessStatusCode();

                string jsonRespuesta = respuesta.Content.ReadAsStringAsync().Result;

                
                var itemsRespuesta = serializer.Deserialize<List<Dictionary<string, object>>>(jsonRespuesta);
                List<string> traducciones = new List<string>();

                foreach (var item in itemsRespuesta)
                {
                    var listaTraducciones = (System.Collections.ArrayList)item["translations"];
                    var primeraTraduccion = (Dictionary<string, object>)listaTraducciones[0];
                    traducciones.Add(primeraTraduccion["text"].ToString());
                }

                return traducciones;
            }
        }
    }
}