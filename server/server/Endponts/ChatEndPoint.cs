using Microsoft.AspNetCore.Mvc;
using server.Chat;
using server.Db;
using server.Embedding;
using server.Model;
using server.Parsing;

namespace server.Endponts
{
    public class ChatEndPoint
    {
        public static void MapParsingEndPoints(IEndpointRouteBuilder endpoint)
        {

            //endpoint.MapPost("/sendMessage", async(PgVectorContext context, ChatService chatService,[FromBody] Message message ) =>
            //{
            //    chatService.Ciao();
            //});
            endpoint.MapGet("/sendMessage", async (PgVectorContext context, ChatService chatService) =>
            {
                return Results.Text(await chatService.SendRequest());
            });
            //endpoint.MapGet("/getChats", async (PgVectorContext context, ChatService chatService) =>
            //{
            //    User user = new User();
            //    return Results.Text(await chatService.GetUserChats(user));
            //});
            //endpoint.MapGet("/getMessages", async (PgVectorContext context, ChatService chatService) =>
            //{
            //    int chatId = 0;
            //    int userId = 0;
            //    return Results.Text(await chatService.GetConversation(chatId,userId));
            //});
        }
    }
}
