using KernalTravelGuide.Data;
using KernalTravelGuide.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KernalTravelGuide.Controllers
{
    public class PublicHotelsController : Controller
    {
        private readonly AppDbContext _context;

        public PublicHotelsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: PublicHotels/Index
        public async Task<IActionResult> Index()
        {
            var hotels = await _context.Hotels
                .Include(h => h.City)
                .AsNoTracking()
                .ToListAsync();

            return View(hotels ?? new List<Hotel>());
        }

        // GET: PublicHotels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var hotel = await _context.Hotels
                .Include(h => h.City)
                .AsNoTracking()
                .FirstOrDefaultAsync(h => h.Id == id);

            if (hotel == null)
                return NotFound();

            return View(hotel);
        }

        [Authorize]
        public IActionResult BookNow(int hotelId)
        {
            return RedirectToAction("Create", "MyBookings", new { hotelId = hotelId });
        }
    }
}