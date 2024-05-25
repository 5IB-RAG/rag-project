using System.ComponentModel.DataAnnotations.Schema;
using server.Model.Dto;

namespace server.Model;

public class UserChat
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }   
    public int TokenUsed { get; set; } 
    public int UserId { get; set; } 
    public User User { get; set; } = null!;
    public IEnumerable<Message>? Messages { get; set; }

    public UserChatDto ToDto()
    {
        return new UserChatDto() { Id = Id, TokenUsed = TokenUsed, UserId = UserId, Messages = Messages.Select(message => message.ToDto()).ToList()};
    }
}