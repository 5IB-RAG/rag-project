using server.Model;
using server.Services;

namespace server.Embedding;

public abstract class EmbeddingParser : IService
{
    private IService _serviceImplementation;
    public abstract Task<float[][]> GetChunkEmbeddingAsync(DocumentChunk[] chunks);

    public async Task<float[][]> GetChunksEmbeddingAsync(DocumentChunk[] chunks)
    {
        List<Task<float[]>> embeddingChunk = new();
        chunks.ToList()
            .ForEach(chunk => embeddingChunk.Add(GetChunkEmbeddingAsync(chunk)));
        
        return await Task.WhenAll(embeddingChunk);
    }

    public abstract Task<DocumentChunk> GetContextChunk(Message message);

    public abstract void PreLoad(WebApplicationBuilder builder);
    public abstract void Enable(WebApplication app);
    public abstract void Disable();
}