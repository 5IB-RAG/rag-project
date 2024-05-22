using client.Data;
using client.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace client.Auth
{
    [Route("api/[controller]")]
    [ApiController]
    public class SigninController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public SigninController(ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] UserAuthSignin userLogin)
        {
            if (_db.Users.Any(u => u.Username.ToLower() == userLogin.Username.ToLower()))
            {
                return BadRequest("Username already exists");
            }

            // add validation and other parameters

            UserAuth user = new() //da sistemare i parametri
            {
                Username = userLogin.Username,
                Password = userLogin.Password,
                EmailAddress = "bo@gmail.com",
                Role = "User",
                Surname = "bo",
                GivenName = "bo"
            };
            _db.Users.Add(user);
            await _db.SaveChangesAsync();

            return Ok("User registered successfully");
        }
    }
}
