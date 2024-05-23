using server.Model;
using server.Services;

namespace server.Embedding;

public abstract class EmbeddingParser : IService
{
    private IService _serviceImplementation;
    public abstract Task<float[]> GetChunkEmbeddingAsync(DocumentChunk chunk);


    public async Task<float[][]> GetChunksEmbeddingAsync(List<DocumentChunk> chunks)
    {
        List<Task<float[]>> embeddingChunk = new();
        chunks.ToList()
            .ForEach(chunk => embeddingChunk.Add(GetChunkEmbeddingAsync(chunk)));
        
        return await Task.WhenAll(embeddingChunk);
    }

    public abstract void Enable(WebApplicationBuilder builder);
    public abstract void Disable();
}