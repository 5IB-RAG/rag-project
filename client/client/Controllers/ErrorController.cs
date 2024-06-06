using Microsoft.AspNetCore.Mvc;

namespace client.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
