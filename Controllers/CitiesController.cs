using KernalTravelGuide.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

[Authorize(Roles = "Admin")]
public class CitiesController : Controller
{
    private readonly AppDbContext _context;

    public CitiesController(AppDbContext context)
    {
        _context = context;
    }

    // GET: Cities
    public async Task<IActionResult> Index()
    {
        var cities = _context.Cities.Include(c => c.Country);
        return View(await cities.ToListAsync());
    }

    // GET: Cities/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var city = await _context.Cities
            .Include(c => c.Country)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (city == null)
        {
            return NotFound();
        }

        return View(city);
    }

    // GET: Cities/Create
    public IActionResult Create()
    {
        ViewBag.CountryId = new SelectList(_context.Countries, "Id", "Name");
        return View();
    }

    // POST: Cities/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Name,CountryId")] City city)
    {
        if (ModelState.IsValid)
        {
            _context.Add(city);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        ViewBag.CountryId = new SelectList(_context.Countries, "Id", "Name", city.CountryId);
        return View(city);
    }

    // GET: Cities/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var city = await _context.Cities.FindAsync(id);

        if (city == null)
        {
            return NotFound();
        }

        ViewBag.CountryId = new SelectList(_context.Countries, "Id", "Name", city.CountryId);
        return View(city);
    }

    // POST: Cities/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id, [Bind("Id,Name,CountryId")] City city)
    {
        if (id != city.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(city);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CityExists(city.Id))
                {
                    return NotFound();
                }

                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        ViewBag.CountryId = new SelectList(_context.Countries, "Id", "Name", city.CountryId);
        return View(city);
    }

    // GET: Cities/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var city = await _context.Cities
            .Include(c => c.Country)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (city == null)
        {
            return NotFound();
        }

        return View(city);
    }

    // POST: Cities/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var city = await _context.Cities.FindAsync(id);

        if (city != null)
        {
            _context.Cities.Remove(city);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool CityExists(int? id)
    {
        return _context.Cities.Any(e => e.Id == id);
    }
}