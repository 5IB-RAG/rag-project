using Pgvector;
using System.ComponentModel.DataAnnotations.Schema;
using server.Model.Dto;

namespace server.Model;

public class Document
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Extension { get; set; } = null!;
    private List<string> Metadata { get; set; } = new List<string>();
    public int UserId { get; set; }
    public User User { get; set; } = null!;
    //Probably need an user
    public IEnumerable<DocumentChunk> Chunks { get; set; } = null!;

    public Document()
    {
    }
    
    public Document(string name, string extension, List<string> metadata, List<DocumentChunk> chunks, int userId)
    {
        this.Name = name;
        this.Extension = extension;
        this.Metadata = metadata;
        this.Chunks = chunks;
        this.UserId = userId;
    }

    public DocumentDto ToDto()
    {
        return new DocumentDto() { Id = Id, Name = Name, Extension = Extension, UserId = UserId };
    }
    
    public static DocumentBuilder Builder()
    {
        return new DocumentBuilder();
    }
}

public class DocumentBuilder
{
    private string name;
    private string extension;
    private List<string> metadata;
    private int userId;
    
    private List<DocumentChunk> chunks;

    public DocumentBuilder Name(string name)
    {
        this.name = name;
        return this;
    }

    public DocumentBuilder UserId(int userId)
    {
        this.userId = userId;
        return this;
    }

    public DocumentBuilder Extension(string extension)
    {
        this.extension = extension;
        return this;
    }
        
    public DocumentBuilder Chunk(List<DocumentChunk> chunks)
    {
        this.chunks = chunks;
        return this;
    }

    public DocumentBuilder Metadata(List<string> metadata)
    {
        this.metadata = metadata;
        return this;
    }
         
    public Document Build()
    {
        return new Document(name, extension, metadata, chunks, userId);
    }
}