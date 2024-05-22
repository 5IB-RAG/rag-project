using Pgvector;

namespace client.Model;

public class Document
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Extension { get; set; }
    private List<string> Metadata { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }
    //Probably need an user

    public IEnumerable<DocumentChunk> Chunks { get; set; } = null!;

    public Document()
    {
    }
    
    public Document(string name, string extension, List<string> metadata, List<DocumentChunk> chunks)
    {
        this.Name = name;
        this.Extension = extension;
        this.Metadata = metadata;
        this.Chunks = chunks;
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
    
    private List<DocumentChunk> chunks;

    public DocumentBuilder Name(string name)
    {
        this.name = name;
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
        return new Document();
        //return new Document(name, extension, metadata, chunks);
    }
}