using Microsoft.AspNetCore.Mvc;
using Pgvector;
using server.Embedding;
using server.Model;
using server.Parsing;

namespace server.Endponts
{
    public static class ParsingEndpoint
    {
        public static void MapParsingEndPoints(IEndpointRouteBuilder endpoint)
        {
            endpoint.MapPost("/upload", async ([FromServices] ParsingService parsingService, [FromServices] IEmbeddingParser embeddingParser, [FromBody] UploadDto upload) =>
            {
                try
                {
                   // Document doc = await parsingService.ParseDocument(upload, upload.Metadata);

                    //List<Vector> docEmbeddings = await embeddingParser.GetChunkEmbeddingAsync(doc.Chunks.ToArray());

                    //Caricare in db

                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            });
        }
    }
}
