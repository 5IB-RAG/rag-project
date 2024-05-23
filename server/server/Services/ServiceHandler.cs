using server.Parsing;

namespace server.Services;

public class ServiceHandler
{
    private static List<Type> _services = new()
    {
        typeof(ParsingService)
    };

    private ServiceProvider _serviceProvider;
    
    public ServiceHandler() { }

    public void Start(WebApplicationBuilder builder)
    {
        _services.ForEach(service =>
        {
            if (service.IsAssignableFrom(typeof(IService))) //Check if service extend IService
                builder.Services.AddSingleton(service);
        });
        _serviceProvider = builder.Services.BuildServiceProvider();
        
        _services.ForEach(service => ((IService) _serviceProvider.GetService(service)).Enable(builder));
    }

    public void Stop()
    {
        _services.ForEach(service => ((IService) _serviceProvider.GetService(service)).Disable());
    }
}