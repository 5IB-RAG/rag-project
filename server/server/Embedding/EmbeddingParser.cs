using client.Model;
using client.Services;

namespace client.Embedding;

public abstract class EmbeddingParser : IService
{
    private IService _serviceImplementation;
    public abstract Task<float[][]> GetChunkEmbeddingAsync(DocumentChunk[] chunk);

    public abstract Task<DocumentChunk> GetContextChunk(Message message);

    public abstract void PreLoad(WebApplicationBuilder builder);
    public abstract void Enable(WebApplication app);
    public abstract void Disable();
}