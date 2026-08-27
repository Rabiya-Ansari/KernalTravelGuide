using KernalTravelGuide.Data;
using KernalTravelGuide.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KernalTravelGuide.Controllers
{
    // Combines the five required information categories into one directory page.
    public class InformationController : Controller
    {
        private readonly AppDbContext _context;

        // Inject the database context for live information and package data.
        public InformationController(AppDbContext context)
        {
            _context = context;
        }

        // Display links and current highlights required by the project specification.
        public async Task<IActionResult> Index()
        {
            // Reuse the home view model because it already contains the required categories.
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
                    .ToListAsync()
            };

            return View(model);
        }
    }
}
