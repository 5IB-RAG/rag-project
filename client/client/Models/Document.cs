namespace client.Models;

public class Document
{
    public string Name { get; set; }
    public string Extension { get; set; }
    private List<string> Metadata { get; set; }
    public int UserId { get; set; }
}
