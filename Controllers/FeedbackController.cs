using KernalTravelGuide.Data;
using KernalTravelGuide.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

[Authorize(Roles = "Customer")]
public class FeedbackController : Controller
{
    private readonly AppDbContext _context;

    public FeedbackController(AppDbContext context)
    {
        _context = context;
    }

    // GET: Feedback/Create
    [HttpGet]
    public async Task<IActionResult> Create(
        int? hotelId,
        int? resortId,
        int? restaurantId,
        int? touristSpotId,
        int? tourPackageId)
    {
        string targetName = string.Empty;

        if (hotelId.HasValue)
        {
            var hotel = await _context.Hotels.FindAsync(hotelId.Value);
            targetName = hotel?.Name ?? "Hotel";
        }
        else if (resortId.HasValue)
        {
            var resort = await _context.Resorts.FindAsync(resortId.Value);
            targetName = resort?.Name ?? "Resort";
        }
        else if (restaurantId.HasValue)
        {
            var restaurant = await _context.Restaurants.FindAsync(restaurantId.Value);
            targetName = restaurant?.Name ?? "Restaurant";
        }
        else if (touristSpotId.HasValue)
        {
            var spot = await _context.TouristSpots.FindAsync(touristSpotId.Value);
            targetName = spot?.Name ?? "Tourist Spot";
        }
        else if (tourPackageId.HasValue)
        {
            var package = await _context.TourPackages.FindAsync(tourPackageId.Value);
            targetName = package?.PackageName ?? "Tour Package";
        }

        ViewBag.TargetName = targetName;

        var model = new Feedback
        {
            HotelId = hotelId,
            ResortId = resortId,
            RestaurantId = restaurantId,
            TouristSpotId = touristSpotId,
            TourPackageId = tourPackageId
        };

        return View(model);
    }

    // POST: Feedback/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Feedback model)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var user = userId == null ? null : await _context.Users.FindAsync(userId);

        if (user == null)
            return Challenge();

        model.CustomerName = $"{user.FirstName} {user.LastName}".Trim();
        model.Email = user.Email ?? string.Empty;
        model.FeedbackDate = DateTime.Now;

        ModelState.Remove(nameof(Feedback.CustomerName));
        ModelState.Remove(nameof(Feedback.Email));

        int selectedItems =
            (model.HotelId.HasValue ? 1 : 0) +
            (model.ResortId.HasValue ? 1 : 0) +
            (model.RestaurantId.HasValue ? 1 : 0) +
            (model.TouristSpotId.HasValue ? 1 : 0) +
            (model.TourPackageId.HasValue ? 1 : 0);

        if (selectedItems != 1)
        {
            ModelState.AddModelError("", "Please select exactly one item to review.");
        }

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        _context.Feedbacks.Add(model);
        await _context.SaveChangesAsync();

        TempData["Success"] = "Thank you for your feedback.";
        return RedirectToAction(nameof(MyFeedbacks));
    }

    // GET: Feedback/MyFeedbacks
    public async Task<IActionResult> MyFeedbacks()
    {
        // Logged-in user ka email get karein
        var userEmail = User.FindFirstValue(ClaimTypes.Email) ?? User.Identity?.Name;

        var userFeedbacks = await _context.Feedbacks
            .Include(f => f.Hotel)
            .Include(f => f.Resort)
            .Include(f => f.Restaurant)
            .Include(f => f.TouristSpot)
            .Include(f => f.TourPackage)
            .Where(f => f.Email == userEmail) 
            .OrderByDescending(f => f.FeedbackDate)
            .ToListAsync();

        return View(userFeedbacks);
    }
}