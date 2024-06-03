
using Pgvector;
using server.Model;
using server.Services;

namespace server.Embedding;

public interface IEmbeddingParser : IService
{
    public Task<List<Vector>> GetChunkEmbeddingAsync(DocumentChunk[] chunks);
    //public Task<Vector> GetContextChunk(Message message);
}