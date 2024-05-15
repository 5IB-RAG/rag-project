using client.Model;

namespace client.Embedding;

public abstract class EmbeddingParser
{
    public abstract Task<float[]> GetChunkEmbeddingAsync(DocumentChunk chunk);


    public async Task<float[][]> GetChunksEmbeddingAsync(DocumentChunk[] chunks)
    {
        List<Task<float[]>> embeddingChunk = new();
        chunks.ToList()
            .ForEach(chunk => embeddingChunk.Add(GetChunkEmbeddingAsync(chunk)));
        
        return await Task.WhenAll(embeddingChunk);
    }
}