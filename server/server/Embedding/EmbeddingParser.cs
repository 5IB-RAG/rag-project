using Pgvector;
using server.Model;
using server.Services;

namespace server.Embedding;

public abstract class EmbeddingParser : IService
{
    private IService _serviceImplementation;
    public abstract Task<List<Vector>> GetChunkEmbeddingAsync(DocumentChunk[] chunks);

    public abstract Task<DocumentChunk> GetContextChunk(Message message);

    public abstract void PreLoad(WebApplicationBuilder builder);
    public abstract void Enable(WebApplication app);
    public abstract void Disable();
}