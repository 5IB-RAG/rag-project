namespace client.Model;

public class Document
{
    public string Name { get; set; }
    public string Extension { get; set; }
    
    //Probably need an user

    public DocumentChunk[] Chunks { get; set; }

    public Document(string name, string extension, DocumentChunk[] chunks)
    {
        this.Name = name;
        this.Extension = extension;
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
    private DocumentChunk[] chunks;

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
        
    public DocumentBuilder Chunk(DocumentChunk[] chunks)
    {
        this.chunks = chunks;
        return this;
    }
        
    public Document Build()
    {
        return new Document(name, extension, chunks);
    }
}