using server.Services;
using server.Model;

namespace server.Chat;

public interface IChat : IService
{
    Task SendAsync(User user, Message message);
    Task<List<UserChat>> GetUserChats(User user); //Maybe insert in inside the user?
    
}