namespace server.Parsing;

public abstract class DocumentConvertor
{
    public abstract Task<string> GetTextAsync(Stream stream);
}