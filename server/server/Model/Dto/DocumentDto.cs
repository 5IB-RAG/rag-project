namespace server.Model.Dto;

public class DocumentDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Extension { get; set; }
    private List<string> Metadata { get; set; }
    public int UserId { get; set; }
}