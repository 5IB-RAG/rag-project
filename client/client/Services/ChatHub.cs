using Microsoft.AspNetCore.SignalR;

namespace client.Services;

public class ChatHub : Hub
{
    public async Task SendMessage(string user, string message)
    {
        await Clients.All.SendAsync("ReceiveMessage", user, message);
    }

    public async Task ShowTypingIndicator(string user)
    {
        await Clients.All.SendAsync("ShowTypingIndicator", user);
    }
}