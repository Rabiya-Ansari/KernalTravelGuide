
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

    // GET: TRAVELINFORMATIONS
    public async Task<IActionResult> Index()    
    {
        return View(await _context.TravelInformations.ToListAsync());
    }

    // GET: TRAVELINFORMATIONS/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var travelinformation = await _context.TravelInformations
            .FirstOrDefaultAsync(m => m.Id == id);
        if (travelinformation == null)
        {
            return NotFound();
        }

        return View(travelinformation);
    }

    // GET: TRAVELINFORMATIONS/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: TRAVELINFORMATIONS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,TransportName,FromCityId,ToCityId,FromCity,ToCity,Fare,Description")] TravelInformation travelinformation)
    {
        if (ModelState.IsValid)
        {
            _context.Add(travelinformation);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(travelinformation);
    }

    // GET: TRAVELINFORMATIONS/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var travelinformation = await _context.TravelInformations.FindAsync(id);
        if (travelinformation == null)
        {
            return NotFound();
        }
        return View(travelinformation);
    }

    // POST: TRAVELINFORMATIONS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id, [Bind("Id,TransportName,FromCityId,ToCityId,FromCity,ToCity,Fare,Description")] TravelInformation travelinformation)
    {
        if (id != travelinformation.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(travelinformation);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TravelInformationExists(travelinformation.Id))
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
        return View(travelinformation);
    }

    // GET: TRAVELINFORMATIONS/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var travelinformation = await _context.TravelInformations
            .FirstOrDefaultAsync(m => m.Id == id);
        if (travelinformation == null)
        {
            return NotFound();
        }

        return View(travelinformation);
    }

    // POST: TRAVELINFORMATIONS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var travelinformation = await _context.TravelInformations.FindAsync(id);
        if (travelinformation != null)
        {
            _context.TravelInformations.Remove(travelinformation);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool TravelInformationExists(int? id)
    {
        return _context.TravelInformations.Any(e => e.Id == id);
    }
}
