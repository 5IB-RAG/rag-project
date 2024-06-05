using server.Enum;

namespace server.Model.Dto;

public class MessageDto
{
    public string Text { get; set; }
    public ChatRole Role { get; set; }
    public int ChatId { get; set; } 
    
    public List<DocumentDto> UsedDocument { get; set; }
}