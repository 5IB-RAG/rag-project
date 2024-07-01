using Mantenimento_Contesto.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Mantenimento_Contesto
{
    public class Embedding
    {
        #region variabili
        public static readonly string azureApiKey = "5619cf938d0241c2a7bc08eb7a692a83";
        public static readonly string azureEndpoint = "https://Passoni-Embedding.openai.azure.com/openai/deployments/ada-embedding/embeddings?api-version=2023-03-15-preview";
        #endregion

        public Embedding() { }

        public static async Task<Response> RichiestaEmbedding(string testText)
        {
            if (testText is null || testText == "")
            {
                return null;
            }
            Console.WriteLine("richiesta partita");
            // Invia la richiesta e ottieni la risposta
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("api-key", azureApiKey);
                var inputContent = new { input = testText };
                var json = JsonSerializer.Serialize(inputContent);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                Console.WriteLine(content.Headers);
                var response = await client.PostAsync(azureEndpoint, content);

                Console.WriteLine("STATUS CODE");
                Console.WriteLine(response);
                //Console.WriteLine("------------------------------------------------------------------------");
                //Console.WriteLine("CONTENT");
                //Console.WriteLine("------------------------------------------------------------------------");

                if (response.IsSuccessStatusCode)
                {
                    Response? obj = await response.Content.ReadFromJsonAsync<Response>();
                    Console.WriteLine(await response.Content.ReadAsStringAsync());
                    return obj;
                }
                else
                {
                    Console.WriteLine("è andato male qualcosa nella richiesta alle Azure API");
                    return null;
                }
            }
        }
    }
}
