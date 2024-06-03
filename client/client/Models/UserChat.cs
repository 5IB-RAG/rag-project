namespace client.Models;

public class UserChat
{
    public Message[] Messages { get; set; }
    public int TokenUsed { get; set; }
    public int Id { get; set; }
}