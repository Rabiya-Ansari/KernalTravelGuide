using Microsoft.AspNetCore.Mvc;

namespace KernalTravelGuide.Controllers
{
    public class AboutController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
