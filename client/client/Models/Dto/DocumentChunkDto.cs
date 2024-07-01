namespace client.Model.Dto;

public class DocumentChunkDto
{
    public int Id { get; set; }
    public List<string> Metadata { get; set; } = null!;
    public string Text { get; set; } = null!;
    
    public DocumentDto Document { get; set; }
}