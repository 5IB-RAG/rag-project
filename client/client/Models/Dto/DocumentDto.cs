namespace client.Model.Dto;

public class DocumentDto
{
    public string Name { get; set; }
    public string Extension { get; set; }
    private List<string> Metadata { get; set; }
    public int UserId { get; set; }
}