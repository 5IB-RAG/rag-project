using client.Services;
using Microsoft.AspNetCore.Authentication.MicrosoftAccount;

namespace client;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddAuthorization();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();   

        // Add authentication and configure Microsoft Account
        builder.Services.AddAuthentication().AddMicrosoftAccount(microsoftOptions =>
        {
            microsoftOptions.ClientId = builder.Configuration["Authentication:Microsoft:ClientId"];
            microsoftOptions.ClientSecret = builder.Configuration["Authentication:Microsoft:ClientSecret"];
        });

        ServiceHandler serviceHandler = new ServiceHandler();
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

        app.UseAuthentication();
        app.UseAuthorization();

        app.Run();
    }
}