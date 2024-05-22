using System.ComponentModel.DataAnnotations;

namespace client.Model
{
    public class UserAuth
    {

        [Key]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string EmailAddress { get; set; }

        [Required]
        public string Role { get; set; }

        [Required]
        public string Surname { get; set; }

        [Required]
        public string GivenName { get; set; }
    }
}
