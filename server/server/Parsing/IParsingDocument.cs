using client.Services;
using server.Model;
using server.Services;

namespace server.Parsing;

public abstract class IParsingDocument : Service
{
    protected IParsingDocument(IServiceProvider provider) : base(provider)
    {
    }

    public abstract Task<Document> ParseDocument(IFormFile formFile, List<string> metadata, int userId);
    public abstract Task<Document[]> GetUserDocuments(User user); //Maybe insert in inside user

}