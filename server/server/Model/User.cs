
using server.Enum;
using System.ComponentModel.DataAnnotations.Schema;

namespace server.Model;

public class User
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;
    public UserRole Role { get; set; } 
    public IEnumerable<UserChat> UserChats {  get; set; } =null!;
    public IEnumerable<Document>? Documents { get; set; }
}