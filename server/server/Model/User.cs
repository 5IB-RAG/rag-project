
using server.Enum;

namespace server.Model;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; } = null!;
    public UserRole Role { get; set; } 
    public IEnumerable<UserChat> UserChats {  get; set; } =null!;
    public IEnumerable<Document>? Documents { get; set; }
}