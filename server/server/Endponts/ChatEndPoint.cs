using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using server.Chat;
using Microsoft.EntityFrameworkCore;
using server.Db;
using server.Embedding;
using server.Helpers;
using server.Model;
using server.Parsing;

namespace server.Endponts;

    public class ChatEndPoint
    {
    public static void MapChatEndpoint(IEndpointRouteBuilder endpoint)
        {
        endpoint.MapGet("/chat", async (PgVectorContext database, ClaimsPrincipal claim) =>
        {
            User user = UserHelper.GetCurrentUser(claim.Identity);
            if (user == null) return Results.NotFound();

            List<UserChat> chats = database.UserChats.Include(chat => chat.Messages)
                .Where(chat => chat.UserId == user.Id).ToList();
            
            return Results.Json(chats.Select(chat => chat.ToDto(true)));
        }).RequireAuthorization();
        
        endpoint.MapPost("/chat", async (PgVectorContext database, ClaimsPrincipal claim) =>
        {
            User user = UserHelper.GetCurrentUser(claim.Identity);
            if (user == null) return Results.NotFound();

            var result = database.UserChats.Add(new UserChat() { Title = "New Chat", UserId = user.Id });
            await database.SaveChangesAsync();

            return Results.Json(result.Entity);
        }).RequireAuthorization();
        
        endpoint.MapGet("/chat/{id}", async ([FromServices] PgVectorContext database, ClaimsPrincipal claim, int id) =>
            {
                User user = UserHelper.GetCurrentUser(claim.Identity);
                if (user == null) return Results.NotFound();
                
            UserChat? userChat = database.UserChats
                .Include(chat => chat.Messages)
                .FirstOrDefault(chat => chat.UserId == user.Id && chat.Id == id);

            if (userChat == null) return Results.NotFound();
            
            return Results.Json(userChat.ToDto(false));
        }).RequireAuthorization();
        
        endpoint.MapPost("/chat/{id}/rename", async ([FromServices] PgVectorContext database, ClaimsPrincipal claim, int id) =>
        {
            //Da fare dopos
            }).RequireAuthorization();
        }
    endpoint.MapPost("chat/{id}", async([FromServices] ChatService chatService, ClaimsPrincipal claim, int id, [FromBody] string message) =>
            {
                User user = UserHelper.GetCurrentUser(claim.Identity);
                if (user == null) return Results.NotFound();
        return Results.Json(await chatService.SendRequest(id, user.Id, message));
            }).RequireAuthorization();
    }
}
