using client.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace client;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        
        // Add services to the container.
        builder.Services.AddCors();
        
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.Authority = string.Format("https://login.microsoftonline.com/{0}", 
                    builder.Configuration["AzureAD:TenantId"]);
                options.Audience = builder.Configuration["AzureAD:Audience"];
                options.TokenValidationParameters.ValidateIssuer = true;
            });
        builder.Services.AddAuthorization();


        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();   

        // Add authentication and configure Microsoft Account
        

        ServiceHandler serviceHandler = new ServiceHandler();
        serviceHandler.PreLoad(builder);

        var app = builder.Build();
        
        app.UseCors();
        app.UseAuthentication();
        app.UseAuthorization();
        
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
        
        app.MapGet("/weatherforecast", () =>
        {
            return "Ciao";
        }).RequireAuthorization();

        app.Run();
    }
}