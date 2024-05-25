using System.ComponentModel.DataAnnotations;

namespace server.Model
{
    public class UserAuthSignup
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string EmailAddress { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
