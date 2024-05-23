using client.Data;
using client.Enum;
using client.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace client.Auth
{
    [Route("auth/[controller]")]
    [ApiController]
    public class SignupController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public SignupController(ApplicationDbContext db)
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
         
            UserAuth user = new()
            {
                Username = userSignup.Username,
                Password = userSignup.Password,
                EmailAddress = userSignup.EmailAddress,
                Role = "User",
                Surname = userSignup.Surname,
                Name = userSignup.Name
            };
            _db.Users.Add(user);
            await _db.SaveChangesAsync();

            return Ok("User registered successfully");
        }
    }
}
