using Microsoft.AspNetCore.Mvc;

namespace KernalTravelGuide.Controllers
{
    public class FeedbackController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
