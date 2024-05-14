using client.Model;
using client.Parsing.Convertors;

namespace client.Parsing;

public class ParsingService : IParsingDocument
{
    private static Dictionary<string, DocumentConvertor> _convertors = new()
    {
        { ".pdf", new PdfConvertor() }
    };
    
    public async Task<Document> ParseDocument(FileStream documentStream, string[] metadata)
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
}