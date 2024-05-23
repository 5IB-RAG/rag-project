namespace server.Model;

public class UserChat
{
    public int Id { get; set; }   
    public int TokenUsed { get; set; } 
    public int UserId { get; set; } 
    public User User { get; set; } = null!;
    public IEnumerable<Message>? Messages { get; set; }  
}