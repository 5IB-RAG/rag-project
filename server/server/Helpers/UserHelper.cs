using System.Security.Claims;
using System.Security.Principal;
using server.Enum;
using server.Model;

namespace server.Helpers;

public class UserHelper
{
    public static User? GetCurrentUser(IIdentity httpIdentity)
    {
        var identity = httpIdentity as ClaimsIdentity;

        if (identity != null)
        {
            var userClaims = identity.Claims;

            bool result = UserRole.TryParse(userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Role)?.Value, out UserRole _role);

            if (!result) return null;

            return new User
            {
                Id = int.Parse(userClaims.FirstOrDefault(o => o.Type == ClaimTypes.PrimarySid)?.Value),
                Username = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.NameIdentifier)?.Value,
                EmailAddress = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Email)?.Value,
                Role = _role
            };
        }
        return null;
    }
}