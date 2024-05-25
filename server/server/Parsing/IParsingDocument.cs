using server.Model;
using server.Services;

namespace server.Parsing;

public interface IParsingDocument : IService
{ 
    Task<Document> ParseDocument(FileStream documentStream, List<string> metadata, int userId);
    Task<Document[]> GetUserDocuments(User user); //Maybe insert in inside user
}