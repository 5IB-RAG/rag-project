using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using client.Models;
using client.Enum;
using client.Services;
using System.Text.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Text;

namespace client.Controllers;

//public class HomeController : Controller
//{
//    private readonly ILogger<HomeController> _logger;

//    public HomeController(ILogger<HomeController> logger)
//    {
//        _logger = logger;
//    }

//    public IActionResult Index()
//    {
//        return View();
//    }

//    public IActionResult Privacy()
//    {
//        return View();
//    }

//    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
//    public IActionResult Error()
//    {
//        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
//    }
//}
public class HomeController : Controller
{
    HttpClient client = new HttpClient();
    ApiService ApiService { get; set; }

    public HomeController(TokenService tokenService, ApiService apiService)
    {
        client.DefaultRequestHeaders.Add("Bearer", tokenService.Token); 
        
        ApiService = apiService;
        client.BaseAddress = new Uri(apiService.BaseAddress);
    }

    public async Task<IActionResult> Index()
    {
        ////Oggetto viewModel
        //HomeModel homeModel = new HomeModel();

        //// Richiesta nomi storia chat
        //homeModel.Chats = ChatsGet().Result;

        //// Riciesta nomi storia documenti caricatiS
        //homeModel.Documents = DocumentsGet().Result;

        //return View(homeModel);
        return View();
    }

    #region DOCUMENTI
    // -------------
    // | DOCUMENTI |
    // -------------

    [HttpGet]
    public async Task<List<Document>> DocumentsGet()
    {
        // richiesta API pe ricevere le info generali di tutti i documenti caricati
        using (client)
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync(ApiService.Documents);
                response.EnsureSuccessStatusCode();

                List<Document> documents = await response.Content.ReadFromJsonAsync<List<Document>>();
                return documents;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine($"Message :{e.Message}");
            }
        }
        return null;
    }

    [HttpGet]
    public async Task<Document> DocumentGetById(int id)
    {
        // richiesta API con id del documento che si vule riceve interamente
        using (client)
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync($"{ApiService.Documents}/{id}");
                response.EnsureSuccessStatusCode();

                Document document = await response.Content.ReadFromJsonAsync<Document>();
                return document;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine($"Message :{e.Message}");
            }
        }
        return null;
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DocumentPost()
    {
        // API per caricare un documento
        using (client)
        {
            //@ToDO
            var json = JsonSerializer.Serialize(1);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            //HttpContent content = new();

            try
            {
                HttpResponseMessage response = await client.PostAsync(ApiService.Documents, content);
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

    [HttpDelete]
    public async Task<IActionResult> DocumentDelete(int id) {
        // API per cancellare un documento dato id
        using (client)
        {
            try
            {
                HttpResponseMessage response = await client.DeleteAsync($"{ApiService.Documents}/{id}");
                response.EnsureSuccessStatusCode();

                return null;
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

    #region CHATS
    // ---------
    // | CHATS |
    // ---------

    [HttpGet]
    public async Task<List<UserChat>> ChatsGet()
    {
        // richiesta API pe ricevere le info generali di tutti i documenti caricati
        using (client)
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync(ApiService.Chats);
                response.EnsureSuccessStatusCode();

                List<UserChat> chats = await response.Content.ReadFromJsonAsync<List<UserChat>>();
                return chats;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine($"Message :{e.Message}");
            }
        }
        return null;
    }

    [HttpGet]
    public async Task<UserChat> ChatGetById(int id)
    {
        // richiesta API con id della chat che si vule riceve completamente
        using (client)
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync($"{ApiService.Chats}/{id}");
                response.EnsureSuccessStatusCode();

                UserChat chat = await response.Content.ReadFromJsonAsync<UserChat>();
                return chat;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine($"Message :{e.Message}");
            }
        }
        return null;
    }

    [HttpDelete]
    public async Task<IActionResult> ChatDelete(int id)
    {
        // API per cancellare una chat dato id
        using (client)
        {
            try
            {
                HttpResponseMessage response = await client.DeleteAsync($"{ApiService.Chats}/{id}");
                response.EnsureSuccessStatusCode();

                return null;
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

    #region MESSAGGI
    // ------------
    // | MESSAGGI |
    // ------------

    //POST
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> MessagePost(string text)
    {
        //Mandre richista API con testo e id chat riferimento
        using (client)
        {
            //@ToDO
            var json = JsonSerializer.Serialize(1);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            //HttpContent content = new();

            try
            {
                HttpResponseMessage response = await client.PostAsync(ApiService.Message, content);
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