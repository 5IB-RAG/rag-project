using server.Db;
using server.Model;
using server.Parsing.Convertors;

namespace server.Parsing;

public class ParsingService : IParsingDocument
{
    private static Dictionary<string, DocumentConvertor> _convertors = new()
    {
        { ".pdf", new PdfConvertor() },
        { ".txt", new TxtConvertor() },
        { ".docx", new DocxConvertor() },
        { ".md", new MdConvertor() }
    };

    private PgVectorContext _context;

    public async Task<Document> ParseDocument(FileStream documentStream, List<string> metadata)
    {
        string extention = documentStream.Name.Split(".").Last();
        string name = documentStream.Name.Remove(documentStream.Name.IndexOf(extention));
        string text =  await _convertors[extention].GetTextAsync(documentStream);

        List<DocumentChunk> chunks = SplitText(text, 400);

        return Document.Builder()
            .Name(name)
            .Extension(extention)
            .Chunk(chunks)
            .Metadata(metadata)
            .Build();
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
        int nextIndex = length;
        while (true)
        {
            while (true)
            {
                if (nextIndex >= text.Length || text[nextIndex] == '.')
                    break;
                nextIndex++;
            }
            splitText.Add(new DocumentChunk([], text.Substring(index, nextIndex - index)));
            index = nextIndex + 1;
            nextIndex = index + length;
            if (nextIndex >= text.Length || index >= text.Length)
                break;
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