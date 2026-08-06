using Microsoft.AspNetCore.Mvc;

namespace KernalTravelGuide.Controllers
{
    public class SearchController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
