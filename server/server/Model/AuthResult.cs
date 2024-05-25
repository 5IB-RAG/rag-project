namespace server.Model;

public class AuthResult
{
    public string Username { get; set; }
    public string Token { get; set; }

    public bool Success { get; set; }
    public string? Error { get; set; }
}