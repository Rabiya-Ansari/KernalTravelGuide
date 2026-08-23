using KernalTravelGuide.Data;
using KernalTravelGuide.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KernalTravelGuide.Controllers
{
    // Controller responsible for the public website Home page.
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        // Inject the database context so the Home page can load real data.
        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        // Display the public Home page with dynamic database information.
        public async Task<IActionResult> Index()
        {
            // Build the Home page view model.
            var model = new HomeViewModel
            {
                // Load a limited number of active tourist spots.
                TouristSpots = await _context.TouristSpots
                    .Include(x => x.City)
                    .Where(x => x.IsActive)
                    .OrderByDescending(x => x.Id)
                    .Take(6)
                    .ToListAsync(),

                // Load available hotels.
                Hotels = await _context.Hotels
                    .Include(x => x.City)
                    .Where(x => x.Availability)
                    .OrderByDescending(x => x.Id)
                    .Take(4)
                    .ToListAsync(),

                // Load restaurants.
                Restaurants = await _context.Restaurants
                    .Include(x => x.City)
                    .OrderByDescending(x => x.Id)
                    .Take(4)
                    .ToListAsync(),

                // Load available resorts.
                Resorts = await _context.Resorts
                    .Include(x => x.City)
                    .Where(x => x.Availability)
                    .OrderByDescending(x => x.Id)
                    .Take(4)
                    .ToListAsync(),

                // Load available tour packages.
                TourPackages = await _context.TourPackages
                    .Where(x => x.IsAvailable)
                    .OrderByDescending(x => x.Id)
                    .Take(4)
                    .ToListAsync(),

                // Load latest reviews for the testimonial section.
                Reviews = await _context.Reviews
                    .Include(x => x.User)
                    .Include(x => x.TouristSpot)
                    .Include(x => x.Hotel)
                    .Include(x => x.Restaurant)
                    .Include(x => x.Resort)
                    .OrderByDescending(x => x.ReviewDate)
                    .Take(6)
                    .ToListAsync(),

                // Load counts for the statistics section.
                TouristSpotCount = await _context.TouristSpots
                    .CountAsync(x => x.IsActive),

                HotelCount = await _context.Hotels
                    .CountAsync(x => x.Availability),

                RestaurantCount = await _context.Restaurants
                    .CountAsync(),

                ResortCount = await _context.Resorts
                    .CountAsync(x => x.Availability)
            };

            // Send the populated model to the Home view.
            return View(model);
        }

        // Display the privacy page.
        public IActionResult Privacy()
        {
            return View();
        }
    }
}