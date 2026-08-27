using KernalTravelGuide.Data;
using KernalTravelGuide.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KernalTravelGuide.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var model = new HomeViewModel
            {
                TouristSpots = await _context.TouristSpots
                    .Include(x => x.City)
                    .Where(x => x.IsActive)
                    .OrderByDescending(x => x.Id)
                    .Take(6)
                    .ToListAsync(),

                Hotels = await _context.Hotels
                    .Include(x => x.City)
                    .Where(x => x.Availability)
                    .OrderByDescending(x => x.Id)
                    .Take(4)
                    .ToListAsync(),

                Restaurants = await _context.Restaurants
                    .Include(x => x.City)
                    .OrderByDescending(x => x.Id)
                    .Take(4)
                    .ToListAsync(),

                Resorts = await _context.Resorts
                    .Include(x => x.City)
                    .Where(x => x.Availability)
                    .OrderByDescending(x => x.Id)
                    .Take(4)
                    .ToListAsync(),

                TourPackages = await _context.TourPackages
                    .Where(x => x.IsAvailable)
                    .OrderByDescending(x => x.Id)
                    .Take(4)
                    .ToListAsync(),

                // Map Feedbacks to Reviews property in HomeViewModel
                Reviews = await _context.Feedbacks
                    .Include(x => x.TouristSpot)
                    .Include(x => x.Hotel)
                    .Include(x => x.Restaurant)
                    .Include(x => x.Resort)
                    .Include(x => x.TourPackage)
                    .OrderByDescending(x => x.FeedbackDate)
                    .Take(6)
                    .ToListAsync(),

                TouristSpotCount = await _context.TouristSpots
                    .CountAsync(x => x.IsActive),

                HotelCount = await _context.Hotels
                    .CountAsync(x => x.Availability),

                RestaurantCount = await _context.Restaurants
                    .CountAsync(),

                ResortCount = await _context.Resorts
                    .CountAsync(x => x.Availability)
            };

            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}