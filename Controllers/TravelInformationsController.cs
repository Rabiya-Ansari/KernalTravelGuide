using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KernalTravelGuide.Models;

public class TravelInformationsController : Controller
{
    private readonly AppDbContext _context;

    public TravelInformationsController(AppDbContext context)
    {
        _context = context;
    }

    // GET: TravelInformations
    public async Task<IActionResult> Index()
    {
        var travelInformations = await _context.TravelInformations
            .Include(t => t.FromCity)
            .Include(t => t.ToCity)
            .ToListAsync();

        return View(travelInformations);
    }

    // GET: TravelInformations/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
            return NotFound();

        var travelInformation = await _context.TravelInformations
            .Include(t => t.FromCity)
            .Include(t => t.ToCity)
            .FirstOrDefaultAsync(t => t.Id == id);

        if (travelInformation == null)
            return NotFound();

        return View(travelInformation);
    }

    // GET: TravelInformations/Create
    public IActionResult Create()
    {
        ViewBag.Cities = _context.Cities
            .OrderBy(c => c.Name)
            .ToList();

        return View();
    }

    // POST: TravelInformations/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        [Bind("TransportName,FromCityId,ToCityId,Fare,Description")]
        TravelInformation travelInformation)
    {
        if (travelInformation.FromCityId == travelInformation.ToCityId)
        {
            ModelState.AddModelError(
                "ToCityId",
                "From City and To City cannot be the same.");
        }

        if (ModelState.IsValid)
        {
            _context.TravelInformations.Add(travelInformation);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        ViewBag.Cities = _context.Cities
            .OrderBy(c => c.Name)
            .ToList();

        return View(travelInformation);
    }

    // GET: TravelInformations/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
            return NotFound();

        var travelInformation = await _context.TravelInformations
            .FindAsync(id);

        if (travelInformation == null)
            return NotFound();

        ViewBag.Cities = _context.Cities
            .OrderBy(c => c.Name)
            .ToList();

        return View(travelInformation);
    }

    // POST: TravelInformations/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        int id,
        [Bind("Id,TransportName,FromCityId,ToCityId,Fare,Description")]
        TravelInformation travelInformation)
    {
        if (id != travelInformation.Id)
            return NotFound();

        if (travelInformation.FromCityId == travelInformation.ToCityId)
        {
            ModelState.AddModelError(
                "ToCityId",
                "From City and To City cannot be the same.");
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(travelInformation);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TravelInformationExists(travelInformation.Id))
                    return NotFound();

                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        ViewBag.Cities = _context.Cities
            .OrderBy(c => c.Name)
            .ToList();

        return View(travelInformation);
    }

    // GET: TravelInformations/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
            return NotFound();

        var travelInformation = await _context.TravelInformations
            .Include(t => t.FromCity)
            .Include(t => t.ToCity)
            .FirstOrDefaultAsync(t => t.Id == id);

        if (travelInformation == null)
            return NotFound();

        return View(travelInformation);
    }

    // POST: TravelInformations/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var travelInformation =
            await _context.TravelInformations.FindAsync(id);

        if (travelInformation != null)
        {
            _context.TravelInformations.Remove(travelInformation);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }

    private bool TravelInformationExists(int id)
    {
        return _context.TravelInformations
            .Any(e => e.Id == id);
    }
}