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

        }
    }
}
