using client.Model;
using client.Parsing.Convertors;

namespace client.Parsing;

public class ParsingService : IParsingDocument
{
    private static Dictionary<string, DocumentConvertor> _convertors = new()
    {
        { ".pdf", new PdfConvertor() }
    };
    
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
        //throw new NotImplementedException();
    }

    public void Enable(WebApplication app)
    {
        //throw new NotImplementedException();
    }

    public void Disable()
    {
        //throw new NotImplementedException();
    }
}