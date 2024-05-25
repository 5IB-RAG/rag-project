using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using server.Db;
using server.Helpers;
using server.Model;

namespace server.Endponts;

public class ChatEndPoint
{
    public static void MapChatEndpoint(IEndpointRouteBuilder endpoint)
    {
        endpoint.MapGet("/chat", async (PgVectorContext database, ClaimsPrincipal user) =>
        {
            User reqUser = UserHelper.GetCurrentUser(user.Identity);
            User dbUser = await database.Users.FindAsync(reqUser.Id);

            if (dbUser == null) return Results.NotFound();

            List<UserChat> chats = database.UserChats.Include(chat => chat.Messages)
                .Where(chat => chat.UserId == dbUser.Id).ToList();
            
            return Results.Json(chats.Select(chat => chat.ToDto()));
        }).RequireAuthorization();
        
        endpoint.MapGet("/chat/{id}", async ([FromServices] PgVectorContext database, ClaimsPrincipal user, int id) =>
        {
            User reqUser = UserHelper.GetCurrentUser(user.Identity);
            User dbUser = await database.Users.FindAsync(reqUser.Id);

            if (dbUser == null) return Results.NotFound();

            UserChat userChat = database.UserChats.Include(chat => chat.Messages)
                .Where(chat => chat.UserId == dbUser.Id && chat.Id == id).FirstOrDefault();

            if (userChat == null) return Results.NotFound();
            
            return Results.Json(userChat.ToDto());
        }).RequireAuthorization();
    }
}