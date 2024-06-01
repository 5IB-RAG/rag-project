
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.OpenApi.Extensions;
using server.Enum;
using server.Helpers;
using server.Model;

namespace server.Auth
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        [HttpGet("Admins")]
        [Authorize(Roles = "Admin")]
        public IActionResult AdminsEndpoint()
        {
            var currentUser = UserHelper.GetCurrentUser(HttpContext.User.Identity);

            return Ok($"Hi {currentUser.Username}, you are an {currentUser.Role}");
        }

        [HttpGet("Sellers")]
        [Authorize(Roles = "Seller")]
        public IActionResult SellersEndpoint()
        {
            var currentUser = UserHelper.GetCurrentUser(HttpContext.User.Identity);

            return Ok($"Hi {currentUser.Username}, you are a {currentUser.Role}");
        }

        [HttpGet("AdminsAndSellers")]
        [Authorize(Roles = "Admin,Seller")]
        public IActionResult AdminsAndSellersEndpoint()
        {
            var currentUser = UserHelper.GetCurrentUser(HttpContext.User.Identity);

            return Ok($"Hi {currentUser.Username}, you are an {currentUser.Role}");
        }

        [HttpGet("Public")]
        public IActionResult Public()
        {
            var currentUser = UserHelper.GetCurrentUser(HttpContext.User.Identity);
            
            return Ok($"Hi {currentUser.Username}, you're on public property and you are an {currentUser.Role}");
        }
    }
}
