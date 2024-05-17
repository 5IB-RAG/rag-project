using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;
using client.Model;
using client.Services;

namespace client;

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

        // Get the embedding parameters
        var embeddingParameters = builder.Configuration.GetSection("EmbeddingParameters").Get<EmbeddingParameters>();
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

        app.UseAuthorization();

        app.MapGet("/embedding", async (HttpContext httpContext) =>
        {
            string urlEmbedding = $"https://{embeddingParameters.ResourceName}.openai.azure.com/openai/deployments/{embeddingParameters.DeploymentId}/embeddings?api-version={embeddingParameters.ApiVersion}";
            
            client.DefaultRequestHeaders.Add("api-key", embeddingParameters.ApiKey);
            List<string> inputList = new List<string> { "iphone", "apple" };
            var requestBody = new
            {
                input = inputList  //TODO: qui andranno messi i chunk del parsing
            };
            var json = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(urlEmbedding, content);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var resultJson = JsonSerializer.Serialize(JsonDocument.Parse(responseContent).RootElement, new JsonSerializerOptions { WriteIndented = true });
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