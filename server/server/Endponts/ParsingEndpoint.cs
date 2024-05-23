﻿using server.Embedding;
using server.Model;
using server.Parsing;

namespace server.Endponts
{
    public static class ParsingEndpoint
    {
        public static void MapParsingEndPoints(IEndpointRouteBuilder endpoint)
        {
            endpoint.MapPost("/upload", async (ParsingService parsingService, EmbeddingParser embeddingParser, FileStream stream, List<string> metadata, int chunkLenght) =>
            {
                try
                {
                    Document doc = await parsingService.ParseDocument(stream, metadata, chunkLenght);

                    float[][] docEmbeddings = await embeddingParser.GetChunksEmbeddingAsync(doc.Chunks);

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