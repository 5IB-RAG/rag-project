using System.ComponentModel.DataAnnotations.Schema;
using Pgvector;
using server.Model.Dto;

namespace server.Model;

public class DocumentChunk
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public List<string> Metadata { get; set; } = null!; //could use another model (Metadata)
    public string Text { get; set; } = null!;
    
    [Column(TypeName = "vector(1536)")]
    public Vector Embedding { get; set; } 
    public int DocumentId { get; set; }  
    public Document Document { get; set; } = null!;

    public DocumentChunk(List<string> metadata, string text)
    {
        this.Metadata = metadata;
        this.Text = text;
        //this.Embedding = embedding;
    }

    public DocumentChunk()
    {
        
    }

    public DocumentChunkDto ToDto()
    {
        DocumentDto? documentDto = null;
        if (Document != null) documentDto = Document.ToDto();
        
        return new DocumentChunkDto() { Document = documentDto , Id = Id, Metadata = Metadata, Text = Text };
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