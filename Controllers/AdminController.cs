using Microsoft.AspNetCore.Mvc;

namespace KernalTravelGuide.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
