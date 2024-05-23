using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using client.Models;
using client.Model;
using client.Enum;
using client.Services;
using System.Text.Json;

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
        // Richiesta nomi storia chat

        // Riciesta nomi storia documenti caricati
        var documents = DocumentsGet();

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
                HttpResponseMessage response = await client.GetAsync(ApiService.DocumentsGet);
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
    public async Task<IActionResult> DocumentGetById(int id)
    {
        // richiesta API con id del documento che si vule riceve interamente
        return null;
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DocumentPost()
    {
        // API per caricare un documento
        return null;
    }

    [HttpDelete]
    public async Task<IActionResult> DocumentDelete(int id) { 
        // API per cancellare un documento dato id
        return null;
    }
    #endregion

    #region CHATS
    // ---------
    // | CHATS |
    // ---------

    [HttpGet]
    public async Task<IActionResult> ChatsGet()
    {
        // richiesta API pe ricevere le info generali di tutti i documenti caricati
        return null;
    }

    [HttpGet]
    public async Task<IActionResult> ChatGetById(int id)
    {
        // richiesta API con id della chat che si vule riceve completamente
        return null;
    }

    [HttpDelete]
    public async Task<IActionResult> ChatDelete(int id)
    {
        // API per cancellare una chat dato id
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
        //Mandre richista API con testo, token e id chat riferimento
        return null;
    }
    #endregion

    #region LOGIN
    // ---------
    // | LOGIN |
    // ---------

    //POST
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> LoginPost(string username, string password)
    {
        //Mandare API per essere autenticati
        return null;
    }
    #endregion

    #region REGISTRAZIONE
    // ----------
    // | SIGNUP |
    // ----------

    //POST
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SignUpPost(string username, string password)
    {
        //Mandare API per essere registrati
        return null;
    }
    #endregion
}