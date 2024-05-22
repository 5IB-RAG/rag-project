using System.ComponentModel.DataAnnotations.Schema;
using Pgvector;

namespace server.Model;

public class DocumentChunk
{
    public int Id { get; set; }
    public List<string> Metadata { get; set; } = null!; //could use another model (Metadata)
    public string Text { get; set; }
    
    [Column(TypeName = "vector(1536)")]
    public Vector Embedding { get; set; }

    public DocumentChunk(List<string> metadata, string text)
    {
        this.Metadata = metadata;
        this.Text = text;
        //this.Embedding = embedding;
    }

    public DocumentChunk()
    {
        
    }

    public static DocumentChunkBuilder Builder()
    {
        return new DocumentChunkBuilder();
    }
}

public class DocumentChunkBuilder
{
    private List<string> metadata;
    private string text;
    private Vector embedding;

    public DocumentChunkBuilder Metadata(List<string> metadata)
    {
        this.metadata = metadata;
        return this;
    }

    public DocumentChunkBuilder Text(string text)
    {
        this.text = text;
        return this;
    }

    public DocumentChunkBuilder Embedding(Vector embedding)
    {
        this.embedding = embedding;
        return this;
    }

    public DocumentChunk Build()
    {
        return new DocumentChunk(metadata, text);
    }
}