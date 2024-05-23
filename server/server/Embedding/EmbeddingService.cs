using client.Model;
using System.Text.Json;
using System.Text;

namespace client.Embedding
{
    public class EmbeddingService : EmbeddingParser
    {
        private HttpClient client = new();
        private EmbeddingParameters? embeddingParameters = new();
        private string? urlEmbedding;

        public override void Disable()
        {
            throw new NotImplementedException();
        }

        public override void Enable(WebApplication app)
        {
            app.MapGet("/embedding", async (HttpContext httpContext) =>
            {
                var result = await GetChunkEmbeddingAsync([DocumentChunk.Builder().Text("ciao a tutti").Build()]);

                return Results.Content(result[0][0].ToString()); //solo per test
            }).WithName("Embedding");
        }

        public override async Task<float[][]> GetChunkEmbeddingAsync(DocumentChunk[] chunks)
        {
            client.DefaultRequestHeaders.Add("api-key", embeddingParameters.ApiKey);
            var requestBody = new
            {
                input = chunks.Select(chunk => chunk.Text)
            };
            var json = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(urlEmbedding, content);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadFromJsonAsync<EmbeddingResponse>();
                List<float[]> embeddings = new List<float[]>();
                responseContent.Data.ForEach(embedding => embeddings.Add(embedding.Embedding.ToArray()));
                return embeddings.ToArray();
            }
            throw new Exception();
        }

        public override Task<DocumentChunk> GetContextChunk(Message message)
        {
            throw new NotImplementedException();
        }

        public override void PreLoad(WebApplicationBuilder builder)
        {
            // Get the embedding parameters
            embeddingParameters = builder.Configuration.GetSection("EmbeddingParameters").Get<EmbeddingParameters>();
            urlEmbedding = $"https://{embeddingParameters.ResourceName}.openai.azure.com/openai/deployments/{embeddingParameters.DeploymentId}/embeddings?api-version={embeddingParameters.ApiVersion}";
        }
    }
}
