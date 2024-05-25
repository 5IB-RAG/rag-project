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
        public async Task<IActionResult> Login(Login login)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var json = JsonSerializer.Serialize(login);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            Debug.WriteLine("CIAO");
            //Mandare API per essere autenticati
            using (client)
            {

                try
                {
                    HttpResponseMessage response = await client.PostAsync(ApiService.Login, content);
                    //response.EnsureSuccessStatusCode();

                    //if (response.IsSuccessStatusCode)
                    //{
                    //    return RedirectToAction("Success");
                    //}
                    //else
                    //{
                    //    ModelState.AddModelError(string.Empty, "Login failed.");
                    //    return View(login);
                    //}
                    return Result(response.StatusCode.ToString());
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine("\nException Caught!");
                    Console.WriteLine($"Message :{e.Message}");
                }
            }
            return null;
        }

        public IActionResult Result(string result)
        {
            return View(result);
        }
        #endregion

        #region REGISTRAZIONE
        // ----------
        // | SIGNUP |
        // ----------

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignUp(string username, string password)
        {
            //Mandare API per essere registrati
            using (client)
            {
                var json = JsonSerializer.Serialize(new { username, password });
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                try
                {
                    HttpResponseMessage response = await client.PostAsync(ApiService.SignUp, content);
                    response.EnsureSuccessStatusCode();
                    //@ToDO
                    return View();
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine("\nException Caught!");
                    Console.WriteLine($"Message :{e.Message}");
                }
            }
            return null;
        }
        #endregion
    }
}
