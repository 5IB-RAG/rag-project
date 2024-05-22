using System.Numerics;
using System.Text;
using System.Text.Json;
using client.Db;
using client.Model;
using client.Parsing.Convertors;
using Vector = Pgvector.Vector;

namespace client.Parsing;

public class ParsingService : IParsingDocument
{
    private static Dictionary<string, DocumentConvertor> _convertors = new()
    {
        { ".pdf", new PdfConvertor() }
    };

    private PgVectorContext _context;

    public ParsingService(IServiceProvider provider)
    {
        _context = provider.GetService<PgVectorContext>() ?? throw new ApplicationException();
    }
    
    public async Task<Document> ParseDocument(FileStream documentStream, List<string> metadata)
    {
        string text =  await _convertors[documentStream.Name.Split(".").Last()].GetTextAsync(documentStream);

        return Document.Builder()
            .Name("c")
            .Extension("c")
            .Build();
    }

    public Task<Document[]> GetUserDocuments(User user)
    {
        throw new NotImplementedException();
    }

    public void PreLoad(WebApplicationBuilder builder)
    {
    }
    
    public async Task<List<Vector>> GetChunkEmbeddingAsync(DocumentChunk[] chunks)
    {
        HttpClient client = new HttpClient();
        client.DefaultRequestHeaders.Add("api-key", "c555bfb233b84c3987f8de7855fc1952");
        var requestBody = new
        {
            input = chunks.Select(chunk => chunk.Text)
        };
        var json = JsonSerializer.Serialize(requestBody);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await client.PostAsync("https://GojoAI-embedding.openai.azure.com/openai/deployments/ada-embedding/embeddings?api-version=2023-03-15-preview", content);

        if (response.IsSuccessStatusCode)
        {
            var responseContent = await response.Content.ReadFromJsonAsync<EmbeddingResponse>();
            List<Vector> embeddings = new List<Vector>();
            responseContent.Data.ForEach(embedding => embeddings.Add(new Vector(embedding.Embedding)));
            return embeddings;
        }
        throw new Exception();
    }

    public async void Enable(WebApplication app)
    {
        _context.DocumentChunks.Add(
            new DocumentChunk
            {
                Id = 1,
                Metadata = new List<string> { "metadata1", "metadata2" },
                Text = "Ciao",
                Embedding = (await GetChunkEmbeddingAsync([DocumentChunk.Builder().Text("Ciao").Build()]))[0]
            }
        );

        _context.SaveChanges();
    }

    public void Disable()
    {
    }
}