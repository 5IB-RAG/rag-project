﻿using server.Embedding;
using Microsoft.EntityFrameworkCore;
using server.Db;
using server.Parsing;

namespace server.Services;

public class ServiceHandler
{
    private static readonly List<Type> _services = new()
    {
        typeof(ParsingService),
        typeof(EmbeddingService)
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

