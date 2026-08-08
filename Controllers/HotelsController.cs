using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using KernalTravelGuide.Models;

public class HotelsController : Controller
{
    private readonly AppDbContext _context;

    public HotelsController(AppDbContext context)
    {
        _context = context;
    }

    // GET: Hotels
    public async Task<IActionResult> Index()
    {
        var hotels = _context.Hotels
            .Include(h => h.City);

        return View(await hotels.ToListAsync());
    }

    // GET: Hotels/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var hotel = await _context.Hotels
            .Include(h => h.City)
            .FirstOrDefaultAsync(h => h.Id == id);

        if (hotel == null)
        {
            return NotFound();
        }

        return View(hotel);
    }

    // GET: Hotels/Create
    public IActionResult Create()
    {
        ViewBag.CityId = new SelectList(
            _context.Cities,
            "Id",
            "Name"
        );

        return View();
    }

    // POST: Hotels/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        [Bind("Id,Name,CityId,PricePerNight,StarRating,ContactNo,Email,Website,ImagePath,Availability")]
        Hotel hotel)
    {
        if (ModelState.IsValid)
        {
            _context.Add(hotel);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        ViewBag.CityId = new SelectList(
            _context.Cities,
            "Id",
            "Name",
            hotel.CityId
        );

        return View(hotel);
    }

    // GET: Hotels/Edit/5
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

        ViewBag.CityId = new SelectList(
            _context.Cities,
            "Id",
            "Name",
            hotel.CityId
        );

        return View(hotel);
    }

    // POST: Hotels/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        int? id,
        [Bind("Id,Name,CityId,PricePerNight,StarRating,ContactNo,Email,Website,ImagePath,Availability")]
        Hotel hotel)
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

                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        ViewBag.CityId = new SelectList(
            _context.Cities,
            "Id",
            "Name",
            hotel.CityId
        );

        return View(hotel);
    }

    // GET: Hotels/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var hotel = await _context.Hotels
            .Include(h => h.City)
            .FirstOrDefaultAsync(h => h.Id == id);

        if (hotel == null)
        {
            return NotFound();
        }

        return View(hotel);
    }

    // POST: Hotels/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var hotel = await _context.Hotels.FindAsync(id);

        if (hotel != null)
        {
            _context.Hotels.Remove(hotel);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }

    private bool HotelExists(int id)
    {
        return _context.Hotels.Any(e => e.Id == id);
    }
}