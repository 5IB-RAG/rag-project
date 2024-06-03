namespace server.Services;

public interface IService
{
    public void PreLoad(WebApplicationBuilder builder, IServiceProvider provider);
    public void Enable(WebApplication app);
    public void Disable();
}