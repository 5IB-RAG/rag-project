using client.Enum;

namespace client.Models;

public class Message
{
    public string Text { get; set; }
    public ChatRole Role { get; set; }
    public int ChatId { get; set; } 
}