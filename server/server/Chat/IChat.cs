using client.Model;

namespace client.Chat;

public interface IChat
{
    Task SendAsync(User user, Message message);
    Task<UserChat[]> GetUserChats(User user); //Maybe insert in inside the user?
}