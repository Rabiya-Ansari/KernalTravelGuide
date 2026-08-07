using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using KernalTravelGuide.Models;

public class TouristSpotsController : Controller
{
    private readonly AppDbContext _context;

    public TouristSpotsController(AppDbContext context)
    {
        _context = context;
    }

    // GET: TouristSpots
    public async Task<IActionResult> Index()
    {
        var touristSpots = await _context.TouristSpots
            .Include(t => t.City)
            .ToListAsync();

        return View(touristSpots);
    }

    // GET: TouristSpots/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var touristSpot = await _context.TouristSpots
            .Include(t => t.City)
            .FirstOrDefaultAsync(t => t.Id == id);

        if (touristSpot == null)
        {
            return NotFound();
        }

        return View(touristSpot);
    }

    // GET: TouristSpots/Create
    public IActionResult Create()
    {
        ViewBag.CityId = new SelectList(
            _context.Cities,
            "Id",
            "Name"
        );

        return View();
    }

    // POST: TouristSpots/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        [Bind("Id,Name,Description,CityId,EntryFee,MapUrl,ImagePath,IsActive")]
        TouristSpot touristSpot)
    {
        if (ModelState.IsValid)
        {
            _context.TouristSpots.Add(touristSpot);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        ViewBag.CityId = new SelectList(
            _context.Cities,
            "Id",
            "Name",
            touristSpot.CityId
        );

        return View(touristSpot);
    }

    // GET: TouristSpots/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var touristSpot = await _context.TouristSpots.FindAsync(id);

        if (touristSpot == null)
        {
            return NotFound();
        }

        ViewBag.CityId = new SelectList(
            _context.Cities,
            "Id",
            "Name",
            touristSpot.CityId
        );

        return View(touristSpot);
    }

    // POST: TouristSpots/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        int? id,
        [Bind("Id,Name,Description,CityId,EntryFee,MapUrl,ImagePath,IsActive")]
        TouristSpot touristSpot)
    {
        if (id != touristSpot.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(touristSpot);

                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TouristSpotExists(touristSpot.Id))
                {
                    return NotFound();
                }

                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        ViewBag.CityId = new SelectList(
            _context.Cities,
            "Id",
            "Name",
            touristSpot.CityId
        );

        return View(touristSpot);
    }

    // GET: TouristSpots/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var touristSpot = await _context.TouristSpots
            .Include(t => t.City)
            .FirstOrDefaultAsync(t => t.Id == id);

        if (touristSpot == null)
        {
            return NotFound();
        }

        return View(touristSpot);
    }

    // POST: TouristSpots/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var touristSpot = await _context.TouristSpots.FindAsync(id);

        if (touristSpot != null)
        {
            _context.TouristSpots.Remove(touristSpot);

            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }

    private bool TouristSpotExists(int id)
    {
        return _context.TouristSpots.Any(e => e.Id == id);
    }
}