namespace client.Services;

public interface IService
{
    void PreLoad(WebApplicationBuilder builder);
    void Enable(WebApplication app);
    void Disable();
}