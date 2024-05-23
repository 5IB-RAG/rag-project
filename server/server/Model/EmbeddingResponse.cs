using Pgvector;

namespace server.Model;

public class EmbeddingResponse
{
    public string Object { get; set; }
    public List<EmbeddingData> Data { get; set; }
    public string Model { get; set; }
    public EmbeddingUsage Usage { get; set; }
}

public class EmbeddingData
{
    public string Object { get; set; }
    public int Index { get; set; }
    public float[] Embedding { get; set; }
}

public class EmbeddingUsage
{
    public int PromptTokens { get; set; }
    public int TotalTokens { get; set; }
}
