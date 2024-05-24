using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using server.Db;
using server.Enum;
using server.Model;

namespace server.Auth
{
    [Route("auth/[controller]")]
    [ApiController]
    public class SignupController : ControllerBase
    {
        private readonly PgVectorContext _db;

        public SignupController(PgVectorContext db)
        {
            _db = db;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] UserAuthSignup userSignup)
        {
            if (_db.Users.Any(u => u.Username.ToLower() == userSignup.Username.ToLower() || 
                                   u.EmailAddress.ToLower() == userSignup.EmailAddress.ToLower()))
            {
                return BadRequest("Username or email already exists");
            }

            // add validation and other parameters
         
            User user = new()
            {
                Username = userSignup.Username,
                Password = userSignup.Password, //Crittarla ASSOLUTAMENTE
                EmailAddress = userSignup.EmailAddress,
                Role = UserRole.User
            };
            _db.Users.Add(user);
            await _db.SaveChangesAsync();

            return Ok("User registered successfully");
        }
    }
}
