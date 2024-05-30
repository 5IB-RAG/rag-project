
using server.Model;
using server.Services;

namespace server.Parsing;

public interface IParsingDocument : IService
{
    
    public Task<Document> ParseDocument(FileStream documentStream, List<string> metadata);
    public Task<Document[]> GetUserDocuments(User user);
}