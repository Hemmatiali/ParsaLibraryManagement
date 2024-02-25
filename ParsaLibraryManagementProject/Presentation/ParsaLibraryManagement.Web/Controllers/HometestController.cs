using Microsoft.AspNetCore.Mvc;

namespace ParsaLibraryManagement.Web.Controllers
{
    public class HometestController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
