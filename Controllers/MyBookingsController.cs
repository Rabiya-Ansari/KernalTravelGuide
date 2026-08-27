using System.Security.Claims;
using KernalTravelGuide.Data;
using KernalTravelGuide.Models;
using KernalTravelGuide.Models.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KernalTravelGuide.Controllers
{
    [Authorize]
    public class MyBookingsController : Controller
    {
        private readonly AppDbContext _context;

        public MyBookingsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: MyBookings
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var userBookings = await _context.Bookings
                .Include(b => b.TourPackage)
                .Include(b => b.TouristSpot)
                .Include(b => b.Hotel)
                .Include(b => b.Resort)
                .Include(b => b.Restaurant)
                .Where(b => b.UserId == userId)
                .OrderByDescending(b => b.BookingDate)
                .ToListAsync();

            return View(userBookings);
        }

        // GET: MyBookings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var booking = await _context.Bookings
                .Include(b => b.TourPackage)
                .Include(b => b.TouristSpot)
                .Include(b => b.Hotel)
                .Include(b => b.Resort)
                .Include(b => b.Restaurant)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (booking == null) return NotFound();

            return View(booking);
        }

        // GET: MyBookings/Create
        [HttpGet]
        public async Task<IActionResult> Create(
            int? tourPackageId,
            int? touristSpotId,
            int? hotelId,
            int? resortId,
            int? restaurantId)
        {
            var booking = new Booking();

            if (tourPackageId.HasValue)
            {
                booking.TourPackageId = tourPackageId;
                booking.TourPackage = await _context.TourPackages.FindAsync(tourPackageId);
            }
            else if (touristSpotId.HasValue)
            {
                booking.TouristSpotId = touristSpotId;
                booking.TouristSpot = await _context.TouristSpots.FindAsync(touristSpotId);
            }
            else if (hotelId.HasValue)
            {
                booking.HotelId = hotelId;
                booking.Hotel = await _context.Hotels.FindAsync(hotelId);
            }
            else if (resortId.HasValue)
            {
                booking.ResortId = resortId;
                booking.Resort = await _context.Resorts.FindAsync(resortId);
            }
            else if (restaurantId.HasValue)
            {
                booking.RestaurantId = restaurantId;
                booking.Restaurant = await _context.Restaurants.FindAsync(restaurantId);
            }

            return View(booking);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Booking booking)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            booking.UserId = userId;
            booking.BookingDate = DateTime.Now;
            booking.Status = BookingStatus.Pending;

            double unitPrice = 0;

            if (booking.TourPackageId.HasValue)
            {
                var package = await _context.TourPackages.FindAsync(booking.TourPackageId);
                if (package != null) unitPrice = Convert.ToDouble(package.Price);
            }
            else if (booking.HotelId.HasValue)
            {
                var hotel = await _context.Hotels.FindAsync(booking.HotelId);
                if (hotel != null) unitPrice = Convert.ToDouble(hotel.PricePerNight);
            }
            else if (booking.ResortId.HasValue)
            {
                var resort = await _context.Resorts.FindAsync(booking.ResortId);
                if (resort != null) unitPrice = Convert.ToDouble(resort.Price);
            }
            else if (booking.TouristSpotId.HasValue)
            {
                unitPrice = 0;
            }

            // Total amount set karein
            booking.TotalAmount = unitPrice * booking.NumberOfPersons;

            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Booking created successfully!";
            return RedirectToAction(nameof(Index));
        }
    }
}