using DocumentFormat.OpenXml.InkML;
using iTextSharp.testutils;
using Pgvector;
using server.Db;
using server.Embedding;
using server.Model;
using server.Parsing.Convertors;

namespace server.Parsing;

public class ParsingService : IParsingDocument
{
    private static Dictionary<string, DocumentConvertor> _convertors = new()
    {
        { "pdf", new PdfConvertor() },
        { "txt", new GeneralConvertor() },
        { "docx", new DocxConvertor() },
        { "md", new GeneralConvertor() },
        { "json", new GeneralConvertor() },
        { "css", new GeneralConvertor() },
        { "html", new GeneralConvertor() },
        { "js", new GeneralConvertor() },
        { "", new GeneralConvertor() }
    };

    private PgVectorContext _context;

    public async Task<Document> ParseDocument(IFormFile formFile, List<string> metadata, int userId)
    {
        string[] spitFileName = formFile.FileName.Split(".");
        string extention = "";
        string name = "";
        if (spitFileName.Length > 1)
        {
            extention = spitFileName.Last();
            name = formFile.FileName.Remove(formFile.FileName.IndexOf(extention) - 1);
        }
        else
        {
            extention = "";
            name = spitFileName.First();
        }
        string text =  await _convertors[extention].GetTextAsync(formFile.OpenReadStream());

        List<DocumentChunk> chunks = SplitText(text, 400);

        return Document.Builder()
            .Name(name)
            .Extension(extention)
            .Chunk(chunks)
            .Metadata(metadata)
            .UserId(userId)
        .Build();
    }

    public async Task SaveDocumentAsync(Document document, PgVectorContext context, EmbeddingService embeddingService)
    {
        var doc = context.Documents.Add(document);
        List<Vector> chunksEmbedding = await embeddingService.GetChunkEmbeddingAsync(document.Chunks.ToArray());
        for (int i = 0; i < chunksEmbedding.Count(); i++)
        {
            document.Chunks.ToList()[i].Embedding = chunksEmbedding[i];
            document.Chunks.ToList()[i].DocumentId = doc.Entity.Id;
            context.DocumentChunks.Add(document.Chunks.ToList()[i]);
        }
        await context.SaveChangesAsync();
    }

    public Task<Document[]> GetUserDocuments(User user)
    {
        throw new NotImplementedException();
    }
    public void PreLoad(WebApplicationBuilder builder, IServiceProvider provider)
    {
        _context = provider.GetService<PgVectorContext>() ?? throw new ApplicationException();
    }

    public void Enable(WebApplication app) { }
    private List<DocumentChunk> SplitText(string text, int length)
    {
        List<DocumentChunk> splitText = new List<DocumentChunk>();

        int index = 0;
        int nextIndex = 0;
        if (text.Length <= length)
        {
            splitText.Add(new DocumentChunk([], text.Substring(index, text.Length - index)));
            return splitText;
        }
        else
            nextIndex = length;
        while (true)
        {
            while (nextIndex < text.Length && text[nextIndex] != '.')
                nextIndex++;
            splitText.Add(new DocumentChunk([], text.Substring(index, nextIndex - index)));
            index = nextIndex + 1;
            nextIndex = index + length;
            if (nextIndex >= text.Length || index >= text.Length)
            {
                splitText.Add(new DocumentChunk([], text.Substring(index, text.Length - index)));
                break;
            }
        }
        if (splitText != null)
        {
            return splitText;
        }
        return new List<DocumentChunk>();
    }

    public void Disable()
    {
    }
}