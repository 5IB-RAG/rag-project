namespace client.Services;

public interface IService
{
    void Enable(WebApplicationBuilder builder);
    void Disable();
}