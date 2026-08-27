using KernalTravelGuide.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace KernalTravelGuide.Controllers
{
    [Authorize(Roles = "Customer")]
    public class DashboardController : Controller
    {
        private readonly AppDbContext _context;

        public DashboardController(AppDbContext context)
        {
            _context = context;
        }

        // Customer Dashboard
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(
                ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                return Challenge();

            // Only current user's bookings.
            var myBookings = await _context.Bookings
                .Include(b => b.TourPackage)
                .Where(b => b.UserId == userId)
                .OrderByDescending(b => b.BookingDate)
                .ToListAsync();

            ViewBag.TotalBookings = myBookings.Count;

            ViewBag.PendingBookings =
                myBookings.Count(b =>
                    b.Status.ToString() == "Pending");

            ViewBag.ApprovedBookings =
                myBookings.Count(b =>
                    b.Status.ToString() == "Confirmed" ||
                    b.Status.ToString() == "Approved");

            ViewBag.RecentBookings =
                myBookings.Take(5).ToList();

            return View();
        }
    }
}