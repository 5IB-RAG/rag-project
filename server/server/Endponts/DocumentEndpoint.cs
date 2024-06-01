using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using server.Db;
using server.Embedding;
using server.Helpers;
using server.Model;
using server.Model.Dto;
using server.Parsing;

namespace server.Endponts;

public static class DocumentEndpoint
{
    public static void MapDocumentEndPoint(IEndpointRouteBuilder endpoint)
    {
        endpoint.MapGet("/document", async ([FromServices] PgVectorContext database) =>
        {
            return Results.Json(database.Documents.Select(document => document.ToDto()));
        }).RequireAuthorization();
        
        endpoint.MapGet("/document/{id}", async ([FromServices] PgVectorContext database, int id) =>
        {
            Document? document = await database.Documents.FindAsync(id);
            if (document == null) return Results.NotFound();
            
            return Results.Json(document.ToDto());
        }).RequireAuthorization();
        
        endpoint.MapDelete("/document/{id}", async ([FromServices] PgVectorContext database, int id) =>
        {
            Document? document = await database.Documents.FindAsync(id);
            if (document == null) return Results.NotFound();

            database.Documents.Remove(document);

            return Results.Ok();
        }).RequireAuthorization();
        
        endpoint.MapPost("/document/upload", 
            async ([FromServices] PgVectorContext context ,[FromServices] ParsingService parsingService, [FromServices] EmbeddingService embeddingService, ClaimsPrincipal claim, [FromForm] UploadDTO upload) =>
            {

                User user = UserHelper.GetCurrentUser(claim.Identity);

                try
                {
                    List<Document> documents = new List<Document>();

                    foreach (var item in upload.FormFiles)
                    {
                        documents.Add(await parsingService.ParseDocument(item, upload.MetaData.Split(';').ToList(), user.Id));
                    }
                    foreach (Document document in documents)
                    {
                        await parsingService.SaveDocumentAsync(document, context, embeddingService);                     
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }).RequireAuthorization().DisableAntiforgery();
    }
}