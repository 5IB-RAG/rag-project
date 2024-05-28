using Microsoft.AspNetCore.Mvc;
using server.Db;
using server.Model;

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
    }
}