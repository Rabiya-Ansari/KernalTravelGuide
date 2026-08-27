using KernalTravelGuide.Data;
using KernalTravelGuide.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KernalTravelGuide.Controllers
{
    
    public class InformationController : Controller
    {
        private readonly AppDbContext _context;

       
        public InformationController(AppDbContext context)
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
                    .ToListAsync()
            };

            return View(model);
        }
    }
}
