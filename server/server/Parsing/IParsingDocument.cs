using client.Services;
using server.Model;
using server.Services;

namespace server.Parsing;

public interface IParsingDocument : IService
{
    
    public Task<Document> ParseDocument(IFormFile formFile, List<string> metadata, int userId);
    public Task<Document[]> GetUserDocuments(User user);
}