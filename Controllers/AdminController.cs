using KernalTravelGuide.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KernalTravelGuide.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;

        public AdminController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
      
            ViewBag.TotalUsers = await _context.Users.CountAsync();
            ViewBag.TotalPackages = await _context.TourPackages.CountAsync();
            ViewBag.TotalBookings = await _context.Bookings.CountAsync();
            ViewBag.TotalTouristSpots = await _context.TouristSpots.CountAsync();
            ViewBag.TotalHotels = await _context.Hotels.CountAsync();
            ViewBag.TotalRestaurants = await _context.Restaurants.CountAsync();
            ViewBag.TotalResorts = await _context.Resorts.CountAsync();
            ViewBag.TotalFeedbacks = await _context.Feedbacks.CountAsync();

          
            ViewBag.RecentBookings = await _context.Bookings
    .Include(b => b.TourPackage)
    .Include(b => b.Hotel)      
    .Include(b => b.Resort)     
    .Include(b => b.Restaurant) 
    .Include(b => b.TouristSpot) 
    .Include(b => b.TravelInformation) 
    .OrderByDescending(b => b.BookingDate)
    .Take(5)
    .ToListAsync();

           
            ViewBag.RecentFeedbacks = await _context.Feedbacks
                .OrderByDescending(f => f.FeedbackDate)
                .Take(5)
                .ToListAsync();

            return View();
        }
    }
}