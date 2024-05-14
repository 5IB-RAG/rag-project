using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;
using client.Model;

namespace client;

public class Program
{     
    static HttpClient client = new HttpClient();
    public static void Main(string[] args)
    {
        string resourceName = "your-resource-name"; //inserire il proprio resource name
        string apiKey = "your-api-key"; //inserire la propria api-key
        string deploymentId = "ada-embedding"; // inserire il modello del deploy
        string apiVersion = "2024-03-01-preview"; // inserire la versione delle api
        string urlEmbedding = $"https://{resourceName}.openai.azure.com/openai/deployments/{deploymentId}/embeddings?api-version={apiVersion}";

        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddAuthorization();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapGet("/embedding", async (HttpContext httpContext) =>
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("api-key", apiKey);

            var requestBody = new
            {
                input = new[] { "Let me cook" } //poi da inserire i chunk
            };

            var json = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(urlEmbedding, content);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStreamAsync();
                var resultJson = JsonSerializer.Serialize(responseContent, new JsonSerializerOptions { WriteIndented = true });
                return Results.Content(resultJson, "application/json");
            }
            else
            {
                return Results.StatusCode((int)response.StatusCode);
            }


        }).WithName("Embedding");

        app.Run();
    }
}