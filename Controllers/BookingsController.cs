using KernalTravelGuide.Data;
using KernalTravelGuide.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KernalTravelGuide.Controllers
{
    // Only administrators can manage customer bookings.
    [Authorize(Roles = "Admin")]
    public class BookingsController : Controller
    {
        private readonly AppDbContext _context;

        // Inject the application database context.
        public BookingsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Bookings
        // Display all bookings for the administrator.
        public async Task<IActionResult> Index()
        {
            var bookings = await _context.Bookings
                .Include(b => b.User)
                .Include(b => b.TourPackage)
                .OrderByDescending(b => b.BookingDate)
                .ToListAsync();

            return View(bookings);
        }

        // GET: Bookings/Details/5
        // Display complete information about one booking.
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var booking = await _context.Bookings
                .Include(b => b.User)
                .Include(b => b.TourPackage)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (booking == null)
                return NotFound();

            return View(booking);
        }

        // GET: Bookings/Edit/5
        // Admin can open a booking and change only its status.
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var booking = await _context.Bookings
                .Include(b => b.User)
                .Include(b => b.TourPackage)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (booking == null)
                return NotFound();

            return View(booking);
        }

        // POST: Bookings/Edit/5
        // Update only the booking status.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            Booking booking)
        {
            if (id != booking.Id)
                return NotFound();

            var existingBooking = await _context.Bookings
                .FirstOrDefaultAsync(b => b.Id == id);

            if (existingBooking == null)
                return NotFound();

            // Update only the status selected by the administrator.
            existingBooking.Status = booking.Status;

            await _context.SaveChangesAsync();

            TempData["Success"] =
                "Booking status updated successfully.";

            return RedirectToAction(nameof(Index));
        }

        // GET: Bookings/Delete/5
        // Display confirmation before deleting a booking.
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var booking = await _context.Bookings
                .Include(b => b.User)
                .Include(b => b.TourPackage)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (booking == null)
                return NotFound();

            return View(booking);
        }

        // POST: Bookings/Delete/5
        // Permanently delete the selected booking.
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var booking = await _context.Bookings
                .FirstOrDefaultAsync(b => b.Id == id);

            if (booking == null)
                return NotFound();

            _context.Bookings.Remove(booking);

            await _context.SaveChangesAsync();

            TempData["Success"] =
                "Booking deleted successfully.";

            return RedirectToAction(nameof(Index));
        }
    }
}