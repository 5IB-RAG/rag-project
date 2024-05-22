using client.Db;
using client.Parsing;
using Microsoft.EntityFrameworkCore;

namespace client.Services;

public class ServiceHandler
{
    private static readonly List<Type> _services = new()
    {
        typeof(ParsingService)
    };

    private IServiceProvider _serviceProvider;

    public ServiceHandler(WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<PgVectorContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("PgVectorContext"), o => o.UseVector()));
        
        // Register services
        _services.ForEach(service => builder.Services.AddSingleton(service));

        // Register DbContext
    }

    public void PreLoad(WebApplicationBuilder builder)
    {
        _serviceProvider = builder.Services.BuildServiceProvider();
        
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

        // Esegui eventuali operazioni di inizializzazione del DbContext qui
        using (var scope = app.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<PgVectorContext>();
            // Esegui eventuali operazioni di inizializzazione del DbContext, come migrazioni
            dbContext.Database.Migrate();
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

