namespace client.Models;

public class UserChat
{
    public int Id { get; set; }
    public string Title { get; set; }
    public int TokenUsed { get; set; } 
    public int UserId { get; set; }

    public List<Message> Messages { get; set; }
}