namespace server.Model;

public class UploadDto
{
    public IFormFile File { get; set; }
    public List<string> Metadata { get; set; }
}