
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KernalTravelGuide.Models;

public class GalleryController : Controller
{
    private readonly AppDbContext _context;

    public GalleryController(AppDbContext context)
    {
        _context = context;
    }

    // GET: GALLERYS
    public async Task<IActionResult> Index()    
    {
        return View(await _context.Galleries.ToListAsync());
    }

    // GET: GALLERYS/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var gallery = await _context.Galleries
            .FirstOrDefaultAsync(m => m.Id == id);
        if (gallery == null)
        {
            return NotFound();
        }

        return View(gallery);
    }

    // GET: GALLERYS/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: GALLERYS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,ImagePath,Caption,TouristSpotId,TouristSpot,HotelId,Hotel,RestaurantId,Restaurant,ResortId,Resort,TourPackageId,TourPackage")] Gallery gallery)
    {
        if (ModelState.IsValid)
        {
            _context.Add(gallery);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(gallery);
    }

    // GET: GALLERYS/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var gallery = await _context.Galleries.FindAsync(id);
        if (gallery == null)
        {
            return NotFound();
        }
        return View(gallery);
    }

    // POST: GALLERYS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id, [Bind("Id,ImagePath,Caption,TouristSpotId,TouristSpot,HotelId,Hotel,RestaurantId,Restaurant,ResortId,Resort,TourPackageId,TourPackage")] Gallery gallery)
    {
        if (id != gallery.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(gallery);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GalleryExists(gallery.Id))
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
        return View(gallery);
    }

    // GET: GALLERYS/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var gallery = await _context.Galleries
            .FirstOrDefaultAsync(m => m.Id == id);
        if (gallery == null)
        {
            return NotFound();
        }

        return View(gallery);
    }

    // POST: GALLERYS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var gallery = await _context.Galleries.FindAsync(id);
        if (gallery != null)
        {
            _context.Galleries.Remove(gallery);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool GalleryExists(int? id)
    {
        return _context.Galleries.Any(e => e.Id == id);
    }
}
