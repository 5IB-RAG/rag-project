using client.Model;
using client.Services;

namespace client.Parsing;

public interface IParsingDocument : IService
{ 
    Task<Document> ParseDocument(FileStream documentStream, List<string> metadata);
    Task<Document[]> GetUserDocuments(User user); //Maybe insert in inside user
}