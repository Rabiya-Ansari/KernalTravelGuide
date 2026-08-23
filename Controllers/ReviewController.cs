using KernalTravelGuide.Data;
using KernalTravelGuide.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// Only administrators are allowed to manage customer reviews.
[Authorize(Roles = "Admin")]
public class ReviewsController : Controller
{
    private readonly AppDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    // Inject database and Identity services.
    public ReviewsController(
        AppDbContext context,
        UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    // Display all customer reviews for the administrator.
    public async Task<IActionResult> Index()
    {
        // Load the review together with its user and reviewed place.
        var reviews = await _context.Reviews
            .Include(r => r.User)
            .Include(r => r.TouristSpot)
            .Include(r => r.Hotel)
            .Include(r => r.Restaurant)
            .Include(r => r.Resort)
            .OrderByDescending(r => r.ReviewDate)
            .ToListAsync();

        return View(reviews);
    }

    // Display complete information for one review.
    public async Task<IActionResult> Details(int? id)
    {
        // Validate the review ID.
        if (id == null)
            return NotFound();

        // Load the review and all related information.
        var review = await _context.Reviews
            .Include(r => r.User)
            .Include(r => r.TouristSpot)
            .Include(r => r.Hotel)
            .Include(r => r.Restaurant)
            .Include(r => r.Resort)
            .FirstOrDefaultAsync(r => r.Id == id);

        // Return 404 when the review does not exist.
        if (review == null)
            return NotFound();

        return View(review);
    }

    // Display the delete confirmation page.
    public async Task<IActionResult> Delete(int? id)
    {
        // Validate the review ID.
        if (id == null)
            return NotFound();

        // Load the review with related customer and place information.
        var review = await _context.Reviews
            .Include(r => r.User)
            .Include(r => r.TouristSpot)
            .Include(r => r.Hotel)
            .Include(r => r.Restaurant)
            .Include(r => r.Resort)
            .FirstOrDefaultAsync(r => r.Id == id);

        // Return 404 when the review does not exist.
        if (review == null)
            return NotFound();

        return View(review);
    }

    // Permanently delete a customer review.
    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        // Find the review that should be deleted.
        var review = await _context.Reviews.FindAsync(id);

        // Return 404 if the review no longer exists.
        if (review == null)
            return NotFound();

        // Remove the review from the database.
        _context.Reviews.Remove(review);

        // Save the deletion.
        await _context.SaveChangesAsync();

        // Show a confirmation message on the Reviews page.
        TempData["Success"] = "Review deleted successfully.";

        return RedirectToAction(nameof(Index));
    }
}