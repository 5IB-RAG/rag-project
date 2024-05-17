namespace client.Parsing;

public abstract class DocumentConvertor
{
    public abstract Task<string> GetTextAsync(FileStream stream);
}