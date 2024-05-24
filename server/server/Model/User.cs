
using System.ComponentModel.DataAnnotations;
using server.Enum;
using System.ComponentModel.DataAnnotations.Schema;

namespace server.Model;

public class User
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    [Required]
    public string Username { get; set; } = null!;
    
    [Required]
    public string Password { get; set; } = null!;
    
    [Required]
    public string EmailAddress { get; set; }
    
    public UserRole Role { get; set; } 
    public IEnumerable<UserChat> UserChats {  get; set; } =null!;
    public IEnumerable<Document>? Documents { get; set; }
}