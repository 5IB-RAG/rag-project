using server.Db;
using server.Model;
using server.Parsing.Convertors;

namespace server.Parsing;

public class ParsingService : IParsingDocument
{
    private static Dictionary<string, DocumentConvertor> _convertors = new()
    {
        { "pdf", new PdfConvertor() },
        { "txt", new TxtConvertor() },
        { "docx", new DocxConvertor() },
        { "md", new MdConvertor() }
    };

    private PgVectorContext _context;

    public ParsingService(IServiceProvider provider) : base(provider)
    {
        _context = provider.GetService<PgVectorContext>() ?? throw new ApplicationException();
    }
    
    public override async Task<Document> ParseDocument(IFormFile formFile, List<string> metadata, int userId)
    {
        string extention = formFile.FileName.Split(".").Last();
        string name = formFile.FileName.Remove(formFile.FileName.IndexOf(extention));
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

    public override Task<Document[]> GetUserDocuments(User user)
    {
        throw new NotImplementedException();
    }
    public override void PreLoad(WebApplicationBuilder builder)
    {
    }

    public override void Enable(WebApplication app) { }
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

    public override void Disable()
    {
    }

}