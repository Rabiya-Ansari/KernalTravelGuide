using KernalTravelGuide.Data;
using KernalTravelGuide.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KernalTravelGuide.Controllers
{
    public class PublicRestaurantsController : Controller
    {
        private readonly AppDbContext _context;

        public PublicRestaurantsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: PublicRestaurants/Index
        public async Task<IActionResult> Index()
        {
            var restaurants = await _context.Restaurants
                .Include(r => r.City)
                .AsNoTracking()
                .ToListAsync();

            return View(restaurants ?? new List<Restaurant>());
        }

        // GET: PublicRestaurants/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var restaurant = await _context.Restaurants
                .Include(r => r.City)
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.Id == id);

            if (restaurant == null)
                return NotFound();

            return View(restaurant);
        }

        [Authorize]
        public IActionResult BookNow(int restaurantId)
        {
            return RedirectToAction("Create", "MyBookings", new { restaurantId = restaurantId });
        }
    }
}