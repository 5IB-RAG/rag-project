using System.Net;
using Microsoft.AspNetCore.Mvc;
using client.Models;
using client.Services;
using System.Text.Json;
using System.Text;
using client.Model.Dto;
using client.Extentions;

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
    ApiService ApiService { get; set; } //Non ti serve piu'

    private readonly RequestService _requestService;
    
    public HomeController(RequestService requestService, ApiService apiService)
    {
        _requestService = requestService;
        ApiService = apiService;
    }

    public async Task<IActionResult> Index()
    {
        if (Request.Cookies["authentication"] == null)
        {
            return Unauthorized();
        }

        if (!TempData.ContainsKey("HomeModel"))
        {
            TempData.Put("HomeModel", new HomeModel());
            TempData.Keep("HomeModel");
        }

        HomeModel homeModel = TempData.Get<HomeModel>("HomeModel");
        // Richiesta nomi storia chat
        homeModel.Chats = await ChatsGet();
        if(homeModel.Chats == null)
        {
            homeModel.Chats = new();
        }

        // Riciesta nomi storia documenti caricatiS
        homeModel.Documents = await DocumentsGet();
        if (homeModel.Documents == null)
        {
            homeModel.Documents = new();
        }

        TempData.Put("HomeModel",homeModel);

        return View(homeModel);
    }

    #region DOCUMENTI
    // -------------
    // | DOCUMENTI |
    // -------------

    public async Task<List<Document>> DocumentsGet()
    {
        // richiesta API pe ricevere le info generali di tutti i documenti caricati

        try
        {
            var documents = await _requestService.SendRequest<List<Document>>(
                RequestType.GET,
                RequestRoute.Documents,
                Request.Cookies["authentication"]
                );
            
            return documents;
        }
        catch (HttpRequestException e)
        {
            //TODO: Fofo ricordati che quando la richiesta fallisce per unauthorized ritorna questo cosi lo reindirizza al login
            //if (e.StatusCode == HttpStatusCode.Unauthorized) return Unauthorized();
            
            Console.WriteLine("\nException Caught!");
            Console.WriteLine($"Message :{e.Message}");
        }

        //Ritorna quello che vuoi
        //return Forbid();
        return new List<Document>();
    }

    //public async Task<IActionResult> DocumentGetById(int id)
    //{
    //    // richiesta API con id del documento che si vule riceve interamente
    //    try
    //    {
            
    //        var document = await _requestService.SendRequest<Document>(
    //            RequestType.GET,
    //            RequestRoute.Documents + "/" + id,
    //            Request.Cookies["authentication"]
    //        );

    //        return Json(document);
    //    }
    //    catch (HttpRequestException e)
    //    {
    //        if (e.StatusCode == HttpStatusCode.Unauthorized) return Unauthorized();
            
    //        Console.WriteLine("\nException Caught!");
    //        Console.WriteLine($"Message :{e.Message}");
    //    }
    //    return null;
    //}

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DocumentPost(HomeModel model )
    {
        var formData = new MultipartFormDataContent();
        foreach (var item in model.UploadDocument.FormFiles)
        {
            var file = new StreamContent(item.OpenReadStream());
            formData.Add(file, "FormFiles", item.FileName);
        }
        formData.Add(new StringContent(model.UploadDocument.MetaData), "MetaData");


        // API per caricare un documento
        try
        {
            await _requestService.SendRequest(
                RequestType.POST,
                RequestRoute.Documents + "/upload",
                Request.Cookies["authentication"],
                formData
            );
            return RedirectToAction(nameof(Index));
        }
        catch (HttpRequestException e)
        {
            if (e.StatusCode == HttpStatusCode.Unauthorized) return Unauthorized();
            
            Console.WriteLine("\nException Caught!");
            Console.WriteLine($"Message :{e.Message}");
        }
        return Forbid();
    }

    //@ToDo in index.cshtml
    public async Task<IActionResult> DocumentDelete(int id) {
        // API per cancellare un documento dato id
        
        try
        {
            await _requestService.SendRequest(
                RequestType.DELETE,
                RequestRoute.Documents + "/" + id,
                Request.Cookies["authentication"]
            );

            return null;
        }
        catch (HttpRequestException e)
        {
            if (e.StatusCode == HttpStatusCode.Unauthorized) return Unauthorized();
            
            Console.WriteLine("\nException Caught!");
            Console.WriteLine($"Message :{e.Message}");
        }
        return null;
    }
    #endregion

    #region CHATS
    // ---------
    // | CHATS |
    // ---------

    public async Task<List<UserChat>> ChatsGet()
    {
        // richiesta API pe ricevere le info generali di tutti i documenti caricati
        
        try
        {
            
            var chats = await _requestService.SendRequest<List<UserChat>>(
                RequestType.GET,
                RequestRoute.Chats,
                Request.Cookies["authentication"]
            );

            return chats;
        }
        catch (HttpRequestException e)
        {
            //if (e.StatusCode == HttpStatusCode.Unauthorized) return Unauthorized();
            
            Console.WriteLine("\nException Caught!");
            Console.WriteLine($"Message :{e.Message}");
        }
        return null;
    }

    [HttpPost]
    public async Task<IActionResult> ChatPost()
    {
        // richiesta API per creare una nuova chat

        try
        {
            var chat = await _requestService.SendRequest<UserChat>(
                RequestType.POST,
                RequestRoute.Chats,
                Request.Cookies["authentication"]
            );
            HomeModel homeModel = TempData.Get<HomeModel>("HomeModel");
            homeModel.Chats.Add(chat);
            homeModel.SelectedChat = chat;
            TempData.Put("HomeModel", homeModel);

            return RedirectToAction(nameof(Index));
        }
        catch (HttpRequestException e)
        {
            if (e.StatusCode == HttpStatusCode.Unauthorized) return Unauthorized();

            Console.WriteLine("\nException Caught!");
            Console.WriteLine($"Message :{e.Message}");
        }
        return Forbid();
    }

    [HttpGet]
    public async Task<IActionResult> ChatGetById(int id)
    {
        // richiesta API con id della chat che si vule riceve completamente
        try
        {
            var chat = await _requestService.SendRequest<UserChat>(
                RequestType.GET,
                RequestRoute.Chats + "/" + id,
                Request.Cookies["authentication"]
            );
            HomeModel homeModel = TempData.Get<HomeModel>("HomeModel");
            homeModel.SelectedChat = chat;
            TempData.Put("HomeModel", homeModel);

            return RedirectToAction(nameof(Index));
        }
        catch (HttpRequestException e)
        {
            if (e.StatusCode == HttpStatusCode.Unauthorized) return Unauthorized();
            
            Console.WriteLine("\nException Caught!");
            Console.WriteLine($"Message :{e.Message}");
        }
        return Forbid();
    }

    //@ToDo in index.cshtml
    public async Task<IActionResult> ChatDelete(int id)
    {
        // API per cancellare una chat dato id
        try
        {
            await _requestService.SendRequest(
                RequestType.DELETE,
                RequestRoute.Documents + "/" + id,
                Request.Cookies["authentication"]
            );

            return null;
        }
        catch (HttpRequestException e)
        {
            if (e.StatusCode == HttpStatusCode.Unauthorized) return Unauthorized();
            
            Console.WriteLine("\nException Caught!");
            Console.WriteLine($"Message :{e.Message}");
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
    public async Task<IActionResult> MessagePost(string message)
    {
        //Mandare richiesta API con testo e id chat riferimento
        var content = new StringContent(message);
        try
        {
            HomeModel homeModel = TempData.Get<HomeModel>("HomeModel");
            //Ritornare la chat o il messaggio?
            await _requestService.SendRequest(
                RequestType.POST,
                RequestRoute.Chats + "/" + homeModel.SelectedChat.Id,
                Request.Cookies["authentication"],
                content
            );
            
            return RedirectToAction(nameof(Index));
        }
        catch (HttpRequestException e)
        {
            if (e.StatusCode == HttpStatusCode.Unauthorized) return Unauthorized();
            
            Console.WriteLine("\nException Caught!");
            Console.WriteLine($"Message :{e.Message}");
        }
        return null;
    }
    #endregion
}