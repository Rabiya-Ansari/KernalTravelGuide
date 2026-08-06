using Microsoft.AspNetCore.Mvc;

namespace KernalTravelGuide.Controllers
{
    public class BookingController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
