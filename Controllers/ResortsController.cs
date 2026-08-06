
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KernalTravelGuide.Models;

public class ResortsController : Controller
{
    private readonly AppDbContext _context;

    public ResortsController(AppDbContext context)
    {
        _context = context;
    }

    // GET: RESORTS
    public async Task<IActionResult> Index()    
    {
        return View(await _context.Resorts.ToListAsync());
    }

    // GET: RESORTS/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var resort = await _context.Resorts
            .FirstOrDefaultAsync(m => m.Id == id);
        if (resort == null)
        {
            return NotFound();
        }

        return View(resort);
    }

    // GET: RESORTS/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: RESORTS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Name,CityId,City,Price,Rating,Availability,ImagePath")] Resort resort)
    {
        if (ModelState.IsValid)
        {
            _context.Add(resort);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(resort);
    }

    // GET: RESORTS/Edit/5
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
        return View(resort);
    }

    // POST: RESORTS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id, [Bind("Id,Name,CityId,City,Price,Rating,Availability,ImagePath")] Resort resort)
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
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }
        return View(resort);
    }

    // GET: RESORTS/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var resort = await _context.Resorts
            .FirstOrDefaultAsync(m => m.Id == id);
        if (resort == null)
        {
            return NotFound();
        }

        return View(resort);
    }

    // POST: RESORTS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var resort = await _context.Resorts.FindAsync(id);
        if (resort != null)
        {
            _context.Resorts.Remove(resort);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool ResortExists(int? id)
    {
        return _context.Resorts.Any(e => e.Id == id);
    }
}
