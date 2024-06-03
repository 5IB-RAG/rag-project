namespace server.Model.Dto;

public class DocumentChunkDto
{
    public int Id { get; set; }
    public List<string> Metadata { get; set; } = null!;
    public string Text { get; set; } = null!;
    
    public int DocumentId { get; set; }  
}