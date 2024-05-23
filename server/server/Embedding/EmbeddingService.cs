using System.Text.Json;
using System.Text;
using Pgvector;
using server.Embedding;
using server.Model;
using Exception = System.Exception;

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
        }

        public override async Task<List<Vector>> GetChunkEmbeddingAsync(DocumentChunk[] chunks)
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
                List<Vector> embeddings = new List<Vector>();
                responseContent.Data.ForEach(embedding => embeddings.Add(new Vector(embedding.Embedding)));
                return embeddings;
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
