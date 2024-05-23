using server.Parsing;

namespace server.Services;

public class ServiceHandler
{
    private static readonly List<Type> _services = new()
    {
        typeof(ParsingService)
    };

    private readonly IServiceProvider _serviceProvider;

    public ServiceHandler(WebApplicationBuilder builder)
    {
        // Register services
        _services.ForEach(service => builder.Services.AddSingleton(service));
        _serviceProvider = builder.Services.BuildServiceProvider();
    }

    public void PreLoad(WebApplicationBuilder builder)
    {
        foreach (var serviceType in _services)
        {
            var service = _serviceProvider.GetService(serviceType) as IService;
            service?.PreLoad(builder);
        }
    }

    public void Start(WebApplication app)
    {
        foreach (var serviceType in _services)
        {
            var service = _serviceProvider.GetService(serviceType) as IService;
            service?.Enable(app);
        }
    }

    public void Stop()
    {
        foreach (var serviceType in _services)
        {
            var service = _serviceProvider.GetService(serviceType) as IService;
            service?.Disable();
        }
    }
}
