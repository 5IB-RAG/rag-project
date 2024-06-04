using System.Text.Json;
using System.Text;
using Pgvector;
using server.Embedding;
using server.Model;
using Exception = System.Exception;
using iTextSharp.text;

namespace server.Embedding
{
    public class EmbeddingService : IEmbeddingParser
    {
        private HttpClient client = new();
        private OpenAiParameters? embeddingParameters = new();
        private string? urlEmbedding;

        public void Disable()
        {
        }

        public void Enable(WebApplication app)
        {
        }

        public async Task<List<Vector>> GetChunkEmbeddingAsync(DocumentChunk[] chunks)
        {
            client.DefaultRequestHeaders.Add("api-key", embeddingParameters.ApiKey);
            var requestBody = new
            {
                input = chunks.Select(chunk => chunk.Text.Trim())
            };
            var json = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(urlEmbedding, content);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadFromJsonAsync<EmbeddingResponse>();
                List<Vector> embeddings = new List<Vector>();
                responseContent.Data.ForEach(embedding => embeddings.Add(new Vector(embedding.Embedding)));
                return embeddings;
            }
            throw new Exception();
        }

        //public async Task<Vector> GetContextChunk(Message message)
        //{
        //    client.DefaultRequestHeaders.Add("api-key", embeddingParameters.ApiKey);
        //    var requestBody = new
        //    {
        //        input = message.Text
        //    };
        //    var json = JsonSerializer.Serialize(requestBody);
        //    var content = new StringContent(json, Encoding.UTF8, "application/json");

        //    var response = await client.PostAsync(urlEmbedding, content);

        //    if (response.IsSuccessStatusCode)
        //    {
        //        var responseContent = await response.Content.ReadFromJsonAsync<EmbeddingResponse>();
        //        return new Vector(responseContent.Data[0].Embedding); ;
        //    }
        //    throw new Exception();
        //    //save su db del messaggio???
        //}


        public void PreLoad(WebApplicationBuilder builder, IServiceProvider provider)
        {
            // Get the embedding parameters
            embeddingParameters = builder.Configuration.GetSection("EmbeddingParameters").Get<OpenAiParameters>();
            urlEmbedding = $"https://{embeddingParameters.ResourceName}.openai.azure.com/openai/deployments/{embeddingParameters.DeploymentId}/embeddings?api-version={embeddingParameters.ApiVersion}";
        }

    }
}
