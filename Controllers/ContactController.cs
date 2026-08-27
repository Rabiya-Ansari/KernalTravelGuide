using KernalTravelGuide.Data;
using KernalTravelGuide.Models;
using Microsoft.AspNetCore.Mvc;

namespace KernalTravelGuide.Controllers
{
    public class ContactController : Controller
    {
        private readonly AppDbContext _context;

        public ContactController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Contact
        public IActionResult Index()
        {
            return View();
        }

        // POST: Contact
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(ContactMessage model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            model.SentOn = DateTime.Now;

            _context.ContactMessages.Add(model);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Your message has been sent successfully.";

            return RedirectToAction(nameof(Index));
        }
    }
}