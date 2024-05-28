using client.Services;
using Pgvector;
using server.Model;
using server.Services;

namespace server.Embedding;

public abstract class EmbeddingParser : Service
{
    protected EmbeddingParser(IServiceProvider provider) : base(provider)
    {
    }
    
    public abstract Task<List<Vector>> GetChunkEmbeddingAsync(DocumentChunk[] chunks);
    public abstract Task<DocumentChunk> GetContextChunk(Message message);

    public abstract override void PreLoad(WebApplicationBuilder builder);
    public abstract override void Enable(WebApplication app);
    public abstract override void Disable();
}