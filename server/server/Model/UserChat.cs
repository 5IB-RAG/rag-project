using System.ComponentModel.DataAnnotations.Schema;

namespace server.Model;

public class UserChat
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }   
    public int TokenUsed { get; set; } 
    public int UserId { get; set; } 
    public User User { get; set; } = null!;
    public IEnumerable<Message>? Messages { get; set; }  
}