using Microsoft.AspNetCore.Mvc;
using Pgvector;
using server.Db;
using server.Embedding;
using server.Helpers;
using server.Model;
using server.Model.Dto;
using server.Parsing;
using System.Security.Claims;

namespace server.Endponts
{
    public static class ParsingEndpoint
    {
        public static void MapParsingEndPoints(IEndpointRouteBuilder endpoint)
        {
            endpoint.MapPost("/upload", 
                async ([FromServices] PgVectorContext context ,[FromServices] ParsingService parsingService, [FromServices] EmbeddingService embeddingService, ClaimsPrincipal user, [FromForm] UploadDTO upload) =>
                {

                    User reqUser = UserHelper.GetCurrentUser(user.Identity);
                    User dbUser = await context.Users.FindAsync(reqUser.Id);

                    try
                    {
                        List<Document> documents = new List<Document>();

                        foreach (var item in upload.FormFiles)
                        {
                            documents.Add(await parsingService.ParseDocument(item, upload.MetaData.Split(';').ToList(), reqUser.Id));
                        }
                        foreach (var document in documents)
                        {
                            var doc = context.Documents.Add(document);                            
                            List<Vector> chunksEmbedding = await embeddingService.GetChunkEmbeddingAsync(document.Chunks.ToArray());
                            for (int i = 0; i < chunksEmbedding.Count() - 1; i++)
                            {
                                document.Chunks.ToList()[i].Embedding = chunksEmbedding[i];
                                document.Chunks.ToList()[i].DocumentId = doc.Entity.Id;
                                context.DocumentChunks.Add(document.Chunks.ToList()[i]);
                            }  
                            
                        }
                        await context.SaveChangesAsync();
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }
                }).RequireAuthorization().DisableAntiforgery();
        }
    }
}
