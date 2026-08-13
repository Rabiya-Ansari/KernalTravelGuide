using KernalTravelGuide.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Authorize(Roles = "Admin")]
public class TourPackagesController : Controller
{
    private readonly AppDbContext _context;

    public TourPackagesController(AppDbContext context)
    {
        _context = context;
    }

    // GET: TourPackages
    public async Task<IActionResult> Index()
    {
        return View(await _context.TourPackages.ToListAsync());
    }

    // GET: TourPackages/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
            return NotFound();

        var tourPackage = await _context.TourPackages
            .FirstOrDefaultAsync(x => x.Id == id);

        if (tourPackage == null)
            return NotFound();

        return View(tourPackage);
    }

    // GET: TourPackages/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: TourPackages/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        [Bind("Id,PackageName,DurationDays,Price,Description,ImagePath,IsAvailable")]
        TourPackage tourPackage)
    {
        if (ModelState.IsValid)
        {
            _context.TourPackages.Add(tourPackage);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        return View(tourPackage);
    }

    // GET: TourPackages/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
            return NotFound();

        var tourPackage = await _context.TourPackages.FindAsync(id);

        if (tourPackage == null)
            return NotFound();

        return View(tourPackage);
    }

    // POST: TourPackages/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        int? id,
        [Bind("Id,PackageName,DurationDays,Price,Description,ImagePath,IsAvailable")]
        TourPackage tourPackage)
    {
        if (id != tourPackage.Id)
            return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(tourPackage);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TourPackageExists(tourPackage.Id))
                    return NotFound();

                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        return View(tourPackage);
    }

    // GET: TourPackages/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
            return NotFound();

        var tourPackage = await _context.TourPackages
            .FirstOrDefaultAsync(x => x.Id == id);

        if (tourPackage == null)
            return NotFound();

        return View(tourPackage);
    }

    // POST: TourPackages/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var tourPackage = await _context.TourPackages.FindAsync(id);

        if (tourPackage != null)
        {
            _context.TourPackages.Remove(tourPackage);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }

    private bool TourPackageExists(int id)
    {
        return _context.TourPackages.Any(x => x.Id == id);
    }
}