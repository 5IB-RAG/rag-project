using System.Net.Http.Headers;
using System.Text.Json;
using client.Models;

namespace client.Services;

public enum RequestType
{
    GET,
    POST,
    DELETE
}

public class RequestService
{
    public readonly HttpClient Client = new();

    public async Task<T?> SendRequest<T>(RequestType type, string url, string? token = null, HttpContent? content = null, JsonSerializerOptions? options = null)
    {
        if (token != null) 
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        if (options == null) options = new JsonSerializerOptions(JsonSerializerDefaults.Web);
        
        try
        {
            HttpResponseMessage response = type switch
            {
                RequestType.GET => await Client.GetAsync(url),
                RequestType.POST => await Client.PostAsync(url, content),
                RequestType.DELETE => await Client.DeleteAsync(url),
                _ => throw new Exception("non-existent request type")
            };

            response.EnsureSuccessStatusCode();
            
            return await response.Content.ReadFromJsonAsync<T>(options);
        }
        finally
        {
            if (token != null)
                Client.DefaultRequestHeaders.Authorization = null;
        }
    }

    public async Task SendRequest(RequestType type, string url, string? token = null, HttpContent? content = null, JsonSerializerOptions? options = null)
    {
        try
        {
            await SendRequest<object>(type, url, token, content, options);
        } catch (Exception ignore) {}
    }
}


public class RequestRoute
{
    private static readonly string BaseAddress = "http://localhost:5000";

    public static readonly string Documents = BaseAddress + "/document";
    
    public static readonly string Chats = BaseAddress + "/chat";
    
    public static readonly string Login = BaseAddress + "/auth/login";
    public static readonly string SignUp = BaseAddress + "/auth/signup";
}