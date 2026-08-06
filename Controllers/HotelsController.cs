
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KernalTravelGuide.Models;

public class HotelsController : Controller
{
    private readonly AppDbContext _context;

    public HotelsController(AppDbContext context)
    {
        _context = context;
    }

    // GET: HOTELS
    public async Task<IActionResult> Index()    
    {
        return View(await _context.Hotels.ToListAsync());
    }

    // GET: HOTELS/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var hotel = await _context.Hotels
            .FirstOrDefaultAsync(m => m.Id == id);
        if (hotel == null)
        {
            return NotFound();
        }

        return View(hotel);
    }

    // GET: HOTELS/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: HOTELS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Name,CityId,City,PricePerNight,StarRating,ContactNo,Email,Website,ImagePath,Availability")] Hotel hotel)
    {
        if (ModelState.IsValid)
        {
            _context.Add(hotel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(hotel);
    }

    // GET: HOTELS/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var hotel = await _context.Hotels.FindAsync(id);
        if (hotel == null)
        {
            return NotFound();
        }
        return View(hotel);
    }

    // POST: HOTELS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id, [Bind("Id,Name,CityId,City,PricePerNight,StarRating,ContactNo,Email,Website,ImagePath,Availability")] Hotel hotel)
    {
        if (id != hotel.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(hotel);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HotelExists(hotel.Id))
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
        return View(hotel);
    }

    // GET: HOTELS/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var hotel = await _context.Hotels
            .FirstOrDefaultAsync(m => m.Id == id);
        if (hotel == null)
        {
            return NotFound();
        }

        return View(hotel);
    }

    // POST: HOTELS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var hotel = await _context.Hotels.FindAsync(id);
        if (hotel != null)
        {
            _context.Hotels.Remove(hotel);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool HotelExists(int? id)
    {
        return _context.Hotels.Any(e => e.Id == id);
    }
}
