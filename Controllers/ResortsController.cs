using KernalTravelGuide.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;


[Authorize(Roles = "Admin")]
public class ResortsController : Controller
{
    private readonly AppDbContext _context;

    public ResortsController(AppDbContext context)
    {
        _context = context;
    }

    // GET: Resorts
    public async Task<IActionResult> Index()
    {
        var resorts = _context.Resorts
            .Include(r => r.City);

        return View(await resorts.ToListAsync());
    }

    // GET: Resorts/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var resort = await _context.Resorts
            .Include(r => r.City)
            .FirstOrDefaultAsync(r => r.Id == id);

        if (resort == null)
        {
            return NotFound();
        }

        return View(resort);
    }

    // GET: Resorts/Create
    public IActionResult Create()
    {
        ViewBag.CityId = new SelectList(
            _context.Cities,
            "Id",
            "Name"
        );

        return View();
    }

    // POST: Resorts/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        [Bind("Id,Name,CityId,Price,Rating,Availability,ImagePath")] Resort resort)
    {
        if (ModelState.IsValid)
        {
            _context.Resorts.Add(resort);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        ViewBag.CityId = new SelectList(
            _context.Cities,
            "Id",
            "Name",
            resort.CityId
        );

        return View(resort);
    }

    // GET: Resorts/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var resort = await _context.Resorts.FindAsync(id);

        if (resort == null)
        {
            return NotFound();
        }

        ViewBag.CityId = new SelectList(
            _context.Cities,
            "Id",
            "Name",
            resort.CityId
        );

        return View(resort);
    }

    // POST: Resorts/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        int? id,
        [Bind("Id,Name,CityId,Price,Rating,Availability,ImagePath")] Resort resort)
    {
        if (id != resort.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(resort);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ResortExists(resort.Id))
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
            resort.CityId
        );

        return View(resort);
    }

    // GET: Resorts/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var resort = await _context.Resorts
            .Include(r => r.City)
            .FirstOrDefaultAsync(r => r.Id == id);

        if (resort == null)
        {
            return NotFound();
        }

        return View(resort);
    }

    // POST: Resorts/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var resort = await _context.Resorts.FindAsync(id);

        if (resort != null)
        {
            _context.Resorts.Remove(resort);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
        //return RedirectToAction(nameof)
    }

    private bool ResortExists(int id)
    {
        return _context.Resorts.Any(e => e.Id == id);
    }
}