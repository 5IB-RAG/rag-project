using server.Model;
using server.Embedding;
using server.Services;
using server.Endponts;

namespace server;

public class Program
{
    static HttpClient client = new HttpClient();
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddAuthorization();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        
        ServiceHandler serviceHandler = new ServiceHandler(builder);
        serviceHandler.PreLoad(builder);

        var app = builder.Build();
        
        serviceHandler.Start(app);

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.UseEndpoints(ParsingEndpoint.MapParsingEndPoints);

        app.Run();
        
        serviceHandler.Stop();
    }
}