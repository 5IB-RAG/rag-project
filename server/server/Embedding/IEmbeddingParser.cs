using client.Services;
using Pgvector;
using server.Model;
using server.Services;

namespace server.Embedding;

public interface IEmbeddingParser : IService
{
    public Task<List<Vector>> GetChunkEmbeddingAsync(DocumentChunk[] chunks);
    public Task<DocumentChunk> GetContextChunk(Message message);
}