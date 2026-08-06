using Microsoft.AspNetCore.Mvc;

namespace KernalTravelGuide.Controllers
{
    public class UsersController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
