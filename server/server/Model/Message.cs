using server.Enum;

namespace server.Model;

public class Message
{
    public string? DataText { get; set; } //this can contain text that rag uses to give information to the ai
    public string Text { get; set; }
    public ChatRole Role { get; set; }
}