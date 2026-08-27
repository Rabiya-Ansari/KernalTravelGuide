using KernalTravelGuide.Data;
using KernalTravelGuide.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KernalTravelGuide.Controllers
{
    public class PublicResortsController : Controller
    {
        private readonly AppDbContext _context;

        public PublicResortsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: PublicResorts/Index
        public async Task<IActionResult> Index()
        {
            var resorts = await _context.Resorts
                .Include(r => r.City)
                .AsNoTracking()
                .ToListAsync();

            return View(resorts ?? new List<Resort>());
        }

        // GET: PublicResorts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var resort = await _context.Resorts
                .Include(r => r.City)
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.Id == id);

            if (resort == null)
                return NotFound();

            return View(resort);
        }

        [Authorize]
        public IActionResult BookNow(int resortId)
        {
            return RedirectToAction("Create", "MyBookings", new { resortId = resortId });
        }
    }
}