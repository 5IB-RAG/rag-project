using client.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text;
using client.Models;

namespace client.Controllers
{
    public class AuthController : Controller
    {
        private readonly RequestService _requestService;
        

        static JsonSerializerOptions jsonDeserializationOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
        };

        public AuthController(RequestService requestService)
        {
            _requestService = requestService;
        }
        public IActionResult Index()
        {
            return View();
        }


        #region LOGIN
        // ---------
        // | LOGIN |
        // ---------

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(UserAuthLogin login)
        {
            if (!ModelState.IsValid)
            {
                return NotFound();
            }

            var json = JsonSerializer.Serialize(login);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            
            //Mandare API per essere autenticati
            try
            {
                var authResult = await _requestService.SendRequest<AuthResult>(
                    RequestType.POST,
                    RequestRoute.Login,
                    content: content,
                    options: jsonDeserializationOptions
                );
                
                if (authResult != null)
                {
                    if (authResult.Success)
                    {
                        TempData["authResult"] = authResult.Username;
                        
                        Response.Cookies.Append(
                            "authentication",
                            authResult.Token, new CookieOptions() { Path = "/", Expires = DateTimeOffset.Now.AddHours(12)}
                            );
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        TempData["authResult"] = authResult.Error;
                        
                    }
                }
                else
                {
                    TempData["authResult"] = HttpRequestError.InvalidResponse;
                }
                return RedirectToAction(nameof(Result));
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine($"Message :{e.Message}");
            }
            return NotFound();
        }
        #endregion

        #region REGISTRAZIONE
        // ----------
        // | SIGNUP |
        // ----------

        //GET
        public IActionResult SignUp()
        {
            return View();
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignUp(UserAuthSignup signup)
        {
            if (!ModelState.IsValid)
            {
                return NotFound();
            }

            //Mandare API per essere registrati
            var json = JsonSerializer.Serialize(signup);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                var authResult = await _requestService.SendRequest<AuthResult>(
                    RequestType.POST,
                    RequestRoute.SignUp,
                    content: content,
                    options: jsonDeserializationOptions
                );

                if (authResult != null)
                {
                    if (authResult.Success)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        TempData["authResult"] = authResult.Error;
                    }
                }
                else
                {
                    TempData["authResult"] = HttpRequestError.InvalidResponse;
                }
                return RedirectToAction(nameof(Result));
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine($"Message :{e.Message}");
            }
            return NotFound();
        }
        #endregion

        public IActionResult Result()
        {
            return View(TempData["authResult"]);
        }
    }
}
