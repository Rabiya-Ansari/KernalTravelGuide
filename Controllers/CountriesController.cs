using KernalTravelGuide.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


[Authorize(Roles = "Admin")]
public class CountriesController : Controller
{
    private readonly AppDbContext _context;

    public CountriesController(AppDbContext context)
    {
        _context = context;
    }

    // GET: Countries
    public async Task<IActionResult> Index()
    {
        return View(await _context.Countries.ToListAsync());
    }

    // GET: Countries/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
            return NotFound();

        var country = await _context.Countries
            .FirstOrDefaultAsync(c => c.Id == id);

        if (country == null)
            return NotFound();

        return View(country);
    }

    // GET: Countries/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Countries/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Name")] Country country)
    {
        if (ModelState.IsValid)
        {
            _context.Add(country);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        return View(country);
    }

    // GET: Countries/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
            return NotFound();

        var country = await _context.Countries.FindAsync(id);

        if (country == null)
            return NotFound();

        return View(country);
    }

    // POST: Countries/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id, [Bind("Id,Name")] Country country)
    {
        if (id != country.Id)
            return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(country);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CountryExists(country.Id))
                    return NotFound();

                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        return View(country);
    }

    // GET: Countries/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
            return NotFound();

        var country = await _context.Countries
            .FirstOrDefaultAsync(c => c.Id == id);

        if (country == null)
            return NotFound();

        return View(country);
    }

    // POST: Countries/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var country = await _context.Countries.FindAsync(id);

        if (country != null)
            _context.Countries.Remove(country);

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool CountryExists(int? id)
    {
        return _context.Countries.Any(e => e.Id == id);
    }
}