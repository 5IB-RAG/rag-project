using client.Model;

namespace client.Embedding
{
    public interface IEmbedding
    {
        Task<EmbeddingResponse> GetEmbedding();
    }
}
