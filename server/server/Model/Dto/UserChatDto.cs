namespace server.Model.Dto;

public class UserChatDto
{
    public int Id { get; set; }
    public int TokenUsed { get; set; } 
    public int UserId { get; set; }

    public List<MessageDto> Messages { get; set; }
}