namespace server.Model;

public class UserChat
{
    public int Id { get; set; } 
    public Message[] Messages { get; set; }
    public int TokenUsed { get; set; }
}