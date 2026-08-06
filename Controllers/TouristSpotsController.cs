
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KernalTravelGuide.Models;

public class TouristSpotsController : Controller
{
    private readonly AppDbContext _context;

    public TouristSpotsController(AppDbContext context)
    {
        _context = context;
    }

    // GET: TOURISTSPOTS
    public async Task<IActionResult> Index()    
    {
        return View(await _context.TouristSpots.ToListAsync());
    }

    // GET: TOURISTSPOTS/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var touristspot = await _context.TouristSpots
            .FirstOrDefaultAsync(m => m.Id == id);
        if (touristspot == null)
        {
            return NotFound();
        }

        return View(touristspot);
    }

    // GET: TOURISTSPOTS/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: TOURISTSPOTS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Name,Description,CityId,City,EntryFee,MapUrl,ImagePath,IsActive")] TouristSpot touristspot)
    {
        if (ModelState.IsValid)
        {
            _context.Add(touristspot);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(touristspot);
    }

    // GET: TOURISTSPOTS/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var touristspot = await _context.TouristSpots.FindAsync(id);
        if (touristspot == null)
        {
            return NotFound();
        }
        return View(touristspot);
    }

    // POST: TOURISTSPOTS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id, [Bind("Id,Name,Description,CityId,City,EntryFee,MapUrl,ImagePath,IsActive")] TouristSpot touristspot)
    {
        if (id != touristspot.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(touristspot);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TouristSpotExists(touristspot.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }
        return View(touristspot);
    }

    // GET: TOURISTSPOTS/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var touristspot = await _context.TouristSpots
            .FirstOrDefaultAsync(m => m.Id == id);
        if (touristspot == null)
        {
            return NotFound();
        }

        return View(touristspot);
    }

    // POST: TOURISTSPOTS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var touristspot = await _context.TouristSpots.FindAsync(id);
        if (touristspot != null)
        {
            _context.TouristSpots.Remove(touristspot);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool TouristSpotExists(int? id)
    {
        return _context.TouristSpots.Any(e => e.Id == id);
    }
}
