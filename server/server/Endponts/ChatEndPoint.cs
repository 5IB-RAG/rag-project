using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using server.Chat;
using server.Db;
using server.Embedding;
using server.Helpers;
using server.Model;
using server.Parsing;

namespace server.Endponts
{
    public class ChatEndPoint
    {
        public static void MapParsingEndPoints(IEndpointRouteBuilder endpoint)
        {
            endpoint.MapPost("chat/{id}", async ([FromServices] ChatService chatService, ClaimsPrincipal claim, int id, [FromBody] string message) =>
            {
                User user = UserHelper.GetCurrentUser(claim.Identity);
                if (user == null) return Results.NotFound();
                
                return Results.Json(await chatService.SendRequest(id, user.Id, message));
            }).RequireAuthorization();
        }
    }
}
