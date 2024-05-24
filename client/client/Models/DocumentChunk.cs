namespace client.Models;

public class DocumentChunk
{
    public List<string> Metadata { get; set; } //could use another model (Metadata)
    public string Text { get; set; }

    public DocumentChunk(List<string> metadata, string text)
    {
        this.Metadata = metadata;
        this.Text = text;
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

    public DocumentChunk Build()
    {
        return new DocumentChunk(metadata, text);
    }
}