using client.Model;

namespace client.Parsing;

public interface IParsingDocument
{ 
    Task<Document> ParseDocument(FileStream documentStream);
    Task<Document[]> GetUserDocument(User user); //Maybe insert in inside user
}