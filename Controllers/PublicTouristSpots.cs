using KernalTravelGuide.Data;
using KernalTravelGuide.Models;
using KernalTravelGuide.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KernalTravelGuide.Controllers
{
    public class PublicTouristSpotsController : Controller
    {
        private readonly AppDbContext _context;

        public PublicTouristSpotsController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var spots = await _context.TouristSpots
                                      .Include(s => s.City)
                                      .ToListAsync();

            var viewModel = new HomeViewModel
            {
                TouristSpots = spots ?? new List<TouristSpot>()
            };

            return View(viewModel);
        }

        // GET: PublicTouristSpots/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var touristSpot = await _context.TouristSpots
                                            .Include(s => s.City)
                                            .FirstOrDefaultAsync(m => m.Id == id);

            if (touristSpot == null)
            {
                return NotFound();
            }

            return View(touristSpot);
        }

        [Authorize]
        public IActionResult BookNow(int touristSpotId)
        {
            return RedirectToAction("Create", "MyBookings", new { touristSpotId = touristSpotId });
        }
    }
}