using KernalTravelGuide.Data;
using KernalTravelGuide.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace KernalTravelGuide.Controllers
{
    // Only authenticated users can create and view their own bookings.
    [Authorize]
    public class MyBookingsController : Controller
    {
        private readonly AppDbContext _context;

        public MyBookingsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: MyBookings
        // Display only the bookings belonging to the logged-in user.
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(
                ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                return Challenge();

            var bookings = await _context.Bookings
                .Include(b => b.TourPackage)
                .Where(b => b.UserId == userId)
                .OrderByDescending(b => b.BookingDate)
                .ToListAsync();

            return View(bookings);
        }


        // GET: MyBookings/Create?tourPackageId=5
        // Display the booking form for the selected package.
        [HttpGet]
        public async Task<IActionResult> Create(int? tourPackageId)
        {
            if (tourPackageId == null)
                return NotFound();

            // Load the selected package from the database.
            var package = await _context.TourPackages
                .FirstOrDefaultAsync(p =>
                    p.Id == tourPackageId &&
                    p.IsAvailable);

            if (package == null)
                return NotFound();

            // Pass package information to the view.
            return View(package);
        }


        // POST: MyBookings/Create
        // Create a new booking for the logged-in user.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            int tourPackageId,
            DateTime travelDate,
            int numberOfPersons)
        {
            // Get the current logged-in user's ID.
            var userId = User.FindFirstValue(
                ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                return Challenge();

            // Load the package again from the database.
            // We NEVER trust the price sent by the browser.
            var package = await _context.TourPackages
                .FirstOrDefaultAsync(p =>
                    p.Id == tourPackageId &&
                    p.IsAvailable);

            if (package == null)
                return NotFound();


            // Validate number of persons.
            if (numberOfPersons < 1 || numberOfPersons > 20)
            {
                ModelState.AddModelError(
                    "numberOfPersons",
                    "Number of persons must be between 1 and 20.");

                return View(package);
            }


            // Travel date cannot be in the past.
            if (travelDate.Date < DateTime.Today)
            {
                ModelState.AddModelError(
                    "travelDate",
                    "Travel date cannot be in the past.");

                return View(package);
            }


            // Calculate total amount on the server.
            var totalAmount =
                package.Price * numberOfPersons;


            // Create the booking.
            var booking = new Booking
            {
                UserId = userId,
                TourPackageId = package.Id,
                TravelDate = travelDate.Date,
                NumberOfPersons = numberOfPersons,
                TotalAmount = totalAmount,
                Status = Models.Enums.BookingStatus.Pending,
                BookingDate = DateTime.Now
            };


            _context.Bookings.Add(booking);

            await _context.SaveChangesAsync();


            TempData["Success"] =
                "Your booking has been submitted successfully.";


            // Send the customer to their bookings.
            return RedirectToAction(nameof(Index));
        }


        // GET: MyBookings/Details/5
        // Display one booking belonging to the current user.
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var userId = User.FindFirstValue(
                ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                return Challenge();

            // Important security check:
            // The booking must belong to the logged-in user.
            var booking = await _context.Bookings
                .Include(b => b.TourPackage)
                .FirstOrDefaultAsync(
                    b => b.Id == id &&
                         b.UserId == userId);

            if (booking == null)
                return NotFound();

            return View(booking);
        }
    }
}