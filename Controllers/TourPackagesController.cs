
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KernalTravelGuide.Models;

public class TourPackagesController : Controller
{
    private readonly AppDbContext _context;

    public TourPackagesController(AppDbContext context)
    {
        _context = context;
    }

    // GET: TOURPACKAGES
    public async Task<IActionResult> Index()    
    {
        return View(await _context.TourPackages.ToListAsync());
    }

    // GET: TOURPACKAGES/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var tourpackage = await _context.TourPackages
            .FirstOrDefaultAsync(m => m.Id == id);
        if (tourpackage == null)
        {
            return NotFound();
        }

        return View(tourpackage);
    }

    // GET: TOURPACKAGES/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: TOURPACKAGES/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,PackageName,DurationDays,Price,Description,ImagePath,IsAvailable")] TourPackage tourpackage)
    {
        if (ModelState.IsValid)
        {
            _context.Add(tourpackage);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(tourpackage);
    }

    // GET: TOURPACKAGES/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var tourpackage = await _context.TourPackages.FindAsync(id);
        if (tourpackage == null)
        {
            return NotFound();
        }
        return View(tourpackage);
    }

    // POST: TOURPACKAGES/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id, [Bind("Id,PackageName,DurationDays,Price,Description,ImagePath,IsAvailable")] TourPackage tourpackage)
    {
        if (id != tourpackage.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(tourpackage);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TourPackageExists(tourpackage.Id))
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
        return View(tourpackage);
    }

    // GET: TOURPACKAGES/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var tourpackage = await _context.TourPackages
            .FirstOrDefaultAsync(m => m.Id == id);
        if (tourpackage == null)
        {
            return NotFound();
        }

        return View(tourpackage);
    }

    // POST: TOURPACKAGES/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var tourpackage = await _context.TourPackages.FindAsync(id);
        if (tourpackage != null)
        {
            _context.TourPackages.Remove(tourpackage);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool TourPackageExists(int? id)
    {
        return _context.TourPackages.Any(e => e.Id == id);
    }
}
