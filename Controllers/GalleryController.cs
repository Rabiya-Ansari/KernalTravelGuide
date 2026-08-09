using KernalTravelGuide.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class GalleryController : Controller
{
    private readonly AppDbContext _context;

    public GalleryController(AppDbContext context)
    {
        _context = context;
    }

    // GET: Gallery
    public async Task<IActionResult> Index()
    {
        var galleries = await _context.Galleries
            .Include(g => g.TouristSpot)
            .Include(g => g.Hotel)
            .Include(g => g.Restaurant)
            .Include(g => g.Resort)
            .Include(g => g.TourPackage)
            .ToListAsync();

        return View(galleries);
    }

    // GET: Gallery/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
            return NotFound();

        var gallery = await _context.Galleries
            .Include(g => g.TouristSpot)
            .Include(g => g.Hotel)
            .Include(g => g.Restaurant)
            .Include(g => g.Resort)
            .Include(g => g.TourPackage)
            .FirstOrDefaultAsync(g => g.Id == id);

        if (gallery == null)
            return NotFound();

        return View(gallery);
    }

    // GET: Gallery/Create
    public async Task<IActionResult> Create()
    {
        await LoadDropdowns();
        return View();
    }

    // POST: Gallery/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Gallery gallery)
    {
        if (!ModelState.IsValid)
        {
            await LoadDropdowns();
            return View(gallery);
        }

        _context.Galleries.Add(gallery);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    // GET: Gallery/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
            return NotFound();

        var gallery = await _context.Galleries.FindAsync(id);

        if (gallery == null)
            return NotFound();

        await LoadDropdowns();
        return View(gallery);
    }

    // POST: Gallery/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Gallery gallery)
    {
        if (id != gallery.Id)
            return NotFound();

        if (!ModelState.IsValid)
        {
            await LoadDropdowns();
            return View(gallery);
        }

        try
        {
            _context.Update(gallery);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!GalleryExists(gallery.Id))
                return NotFound();

            throw;
        }

        return RedirectToAction(nameof(Index));
    }

    // GET: Gallery/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
            return NotFound();

        var gallery = await _context.Galleries
            .Include(g => g.TouristSpot)
            .Include(g => g.Hotel)
            .Include(g => g.Restaurant)
            .Include(g => g.Resort)
            .Include(g => g.TourPackage)
            .FirstOrDefaultAsync(g => g.Id == id);

        if (gallery == null)
            return NotFound();

        return View(gallery);
    }

    // POST: Gallery/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var gallery = await _context.Galleries.FindAsync(id);

        if (gallery == null)
            return NotFound();

        _context.Galleries.Remove(gallery);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    // Dropdown data
    private async Task LoadDropdowns()
    {
        ViewBag.TouristSpots = await _context.TouristSpots
            .OrderBy(x => x.Name)
            .ToListAsync();

        ViewBag.Hotels = await _context.Hotels
            .OrderBy(x => x.Name)
            .ToListAsync();

        ViewBag.Restaurants = await _context.Restaurants
            .OrderBy(x => x.Name)
            .ToListAsync();

        ViewBag.Resorts = await _context.Resorts
            .OrderBy(x => x.Name)
            .ToListAsync();

        ViewBag.TourPackages = await _context.TourPackages
            .OrderBy(x => x.PackageName)
            .ToListAsync();
    }

    private bool GalleryExists(int id)
    {
        return _context.Galleries.Any(e => e.Id == id);
    }
}