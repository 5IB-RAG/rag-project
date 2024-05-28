namespace client.Services;

public abstract class Service
{
    private readonly IServiceProvider _provider;
    
    protected Service(IServiceProvider provider)
    {
        _provider = provider;
    }
    
    public abstract void PreLoad(WebApplicationBuilder builder);
    public abstract void Enable(WebApplication app);
    public abstract void Disable();
}