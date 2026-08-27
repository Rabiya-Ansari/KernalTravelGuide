using KernalTravelGuide.Data;
using KernalTravelGuide.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KernalTravelGuide.Controllers
{
    public class PublicTravelInformationController : Controller
    {
        private readonly AppDbContext _context;

        public PublicTravelInformationController(AppDbContext context)
        {
            _context = context;
        }

       
        public async Task<IActionResult> Index()
        {
            var travelInformations = await _context.TravelInformations
                .Include(t => t.FromCity)
                .Include(t => t.ToCity)
                .AsNoTracking()
                .ToListAsync();

            return View(travelInformations ?? new List<TravelInformation>());
        }

        // GET: PublicTravelInformation/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var travelInformation = await _context.TravelInformations
                .Include(t => t.FromCity)
                .Include(t => t.ToCity)
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.Id == id);

            if (travelInformation == null)
                return NotFound();

            return View(travelInformation);
        }

        [Authorize]
        public IActionResult BookNow(int travelInfoId)
        {
            return RedirectToAction("Create", "MyBookings", new { travelInfoId = travelInfoId });
        }
    }
}