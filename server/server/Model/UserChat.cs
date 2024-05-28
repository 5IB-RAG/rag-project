using System.ComponentModel.DataAnnotations.Schema;
using server.Model.Dto;

namespace server.Model;

public class UserChat
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }   
    public string Title { get; set; }
    public int TokenUsed { get; set; } 
    public int UserId { get; set; } 
    public User User { get; set; } = null!;
    public IEnumerable<Message>? Messages { get; set; }

    public UserChatDto ToDto(bool lighweight)
    {
        return new UserChatDto() { Id = Id, Title = Title, TokenUsed = TokenUsed, UserId = UserId, Messages = lighweight ? null : Messages.Select(message => message.ToDto()).ToList()};
    }
}