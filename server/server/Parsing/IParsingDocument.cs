using client.Services;
using server.Model;
using server.Services;

namespace server.Parsing;

public abstract class IParsingDocument : Service
{
    protected IParsingDocument(IServiceProvider provider) : base(provider)
    {
    }
    
    public abstract Task<Document> ParseDocument(FileStream documentStream, List<string> metadata);
    public abstract Task<Document[]> GetUserDocuments(User user);
}