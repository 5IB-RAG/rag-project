using System.Linq.Expressions;
using System.Text;
using System.Text.Json;
using server.Db;
using server.Model;
using server.Parsing;
using server.Parsing.Convertors;

namespace server.Parsing;

public class ParsingService : IParsingDocument
{
    private static Dictionary<string, DocumentConvertor> _convertors = new()
    {
        { ".pdf", new PdfConvertor() }
    };

    private PgVectorContext _context;

    public ParsingService(IServiceProvider provider)
    {
        _context = provider.GetService<PgVectorContext>() ?? throw new ApplicationException();
    }
    
    public async Task<Document> ParseDocument(FileStream documentStream, List<string> metadata)
    {
        string text =  await _convertors[documentStream.Name.Split(".").Last()].GetTextAsync(documentStream);

        return Document.Builder()
            .Name("c")
            .Extension("c")
            .Build();
    }

    public Task<Document[]> GetUserDocuments(User user)
    {
        throw new NotImplementedException();
    }

    public void PreLoad(WebApplicationBuilder builder)
    {
    }

    public async void Enable(WebApplication app)
    {
    }

    public void Disable()
    {
    }
}