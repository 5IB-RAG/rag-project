using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BCrypt.Net;
using server.Db;
using server.Model;

namespace server.Auth
{
    [Route("auth/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private IConfiguration _config;
        private readonly PgVectorContext _db;

        public LoginController(IConfiguration config, PgVectorContext db)
        {
            _config = config;
            _db = db;
        }

        [AllowAnonymous]
        [HttpPost]
        public JsonResult Login([FromBody] UserAuthLogin userLogin)
        {
            var user = Authenticate(userLogin);

            if (user != null)
            {
                var token = Generate(user);
                return new JsonResult(new AuthResult { Username = user.Username, Token = token, Success = true });
            }
            
            return new JsonResult(new AuthResult
                {Success = false, Error = "User not found" });
        }

        private string Generate(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.PrimarySid, user.Id.ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Username),
                new Claim(ClaimTypes.Email, user.EmailAddress),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
              _config["Jwt:Audience"],
              claims,
              expires: DateTime.Now.AddHours(12),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private User Authenticate(UserAuthLogin userLogin)
        {
            var currentUser = _db.Users.FirstOrDefault(o => o.Username.ToLower() == userLogin.Username.ToLower());

            if (currentUser != null && BCrypt.Net.BCrypt.Verify(userLogin.Password, currentUser.Password))
            {
                return currentUser;
            }

            return null;
        }
    }
}
