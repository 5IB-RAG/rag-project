using client.Model;

namespace client.Parsing;

public interface IParsingDocument
{ 
    Task<Document> ParseDocument(FileStream documentStream, List<string> metadata);
    Task<Document[]> GetUserDocuments(User user); //Maybe insert in inside user
}