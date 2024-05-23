using System.ComponentModel.DataAnnotations;

namespace client.Model
{
    public class UserAuthSignup
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string EmailAddress { get; set; }
        public string Surname { get; set; }
        public string Name { get; set; }
    }
}
