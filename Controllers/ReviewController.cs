using KernalTravelGuide.Data;
using KernalTravelGuide.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class ReviewsController : Controller
{
    private readonly AppDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public ReviewsController(
        AppDbContext context,
        UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    // GET: Reviews
    public async Task<IActionResult> Index()
    {
        var reviews = await _context.Reviews
            .Include(r => r.User)
            .Include(r => r.TouristSpot)
            .Include(r => r.Hotel)
            .Include(r => r.Restaurant)
            .Include(r => r.Resort)
            .ToListAsync();

        return View(reviews);
    }

    // GET: Reviews/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
            return NotFound();

        var review = await _context.Reviews
            .Include(r => r.User)
            .Include(r => r.TouristSpot)
            .Include(r => r.Hotel)
            .Include(r => r.Restaurant)
            .Include(r => r.Resort)
            .FirstOrDefaultAsync(r => r.Id == id);

        if (review == null)
            return NotFound();

        return View(review);
    }

    // GET: Reviews/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Reviews/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Review review)
    {
        if (!ModelState.IsValid)
            return View(review);

        var user = await _userManager.GetUserAsync(User);

        if (user == null)
            return Challenge();

        review.UserId = user.Id;
        review.ReviewDate = DateTime.Now;

        _context.Reviews.Add(review);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    // GET: Reviews/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
            return NotFound();

        var review = await _context.Reviews
            .Include(r => r.User)
            .FirstOrDefaultAsync(r => r.Id == id);

        if (review == null)
            return NotFound();

        return View(review);
    }

    // POST: Reviews/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var review = await _context.Reviews.FindAsync(id);

        if (review == null)
            return NotFound();

        _context.Reviews.Remove(review);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }
}