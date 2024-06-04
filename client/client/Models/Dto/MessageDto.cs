using client.Enum;

namespace client.Model.Dto;

public class MessageDto
{
    public string Text { get; set; }
    public ChatRole Role { get; set; }
    public int ChatId { get; set; }
    public List<DocumentChunkDto>? DocumentChunks { get; set; }
    public List<string>? DocumentChunksUniqueNames { get; set; }
}