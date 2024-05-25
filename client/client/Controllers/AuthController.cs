using client.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text;
using client.Models;
using System.Diagnostics;

namespace client.Controllers
{
    public class AuthController : Controller
    {
        HttpClient client = new HttpClient();
        ApiService ApiService { get; set; }

        static JsonSerializerOptions jsonDeserializationOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
        };

        public AuthController(TokenService tokenService, ApiService apiService)
        {
            client.DefaultRequestHeaders.Add("Bearer", tokenService.Token);

            ApiService = apiService;
            client.BaseAddress = new Uri(apiService.BaseAddress);
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
            using (client)
            {
                try
                {
                    HttpResponseMessage response = await client.PostAsync(ApiService.Login, content);

                    AuthResult? authResult = JsonSerializer.Deserialize<AuthResult>(await response.Content.ReadAsStringAsync(), jsonDeserializationOptions);

                    if (authResult != null)
                    {
                        if (authResult.Success)
                        {
                            TempData["authResult"] = authResult.Username;
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
            }
            return NotFound();
        }
        #endregion

        #region REGISTRAZIONE
        // ----------
        // | SIGNUP |
        // ----------

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
            using (client)
            {
                var json = JsonSerializer.Serialize(signup);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                try
                {
                    HttpResponseMessage response = await client.PostAsync(ApiService.SignUp, content);

                    AuthResult? authResult = JsonSerializer.Deserialize<AuthResult>(await response.Content.ReadAsStringAsync(), jsonDeserializationOptions);

                    if (authResult != null)
                    {
                        if (authResult.Success)
                        {
                            TempData["authResult"] = authResult.Username;
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
