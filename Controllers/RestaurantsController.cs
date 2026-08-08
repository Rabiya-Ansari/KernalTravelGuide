using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using KernalTravelGuide.Models;

public class RestaurantsController : Controller
{
    private readonly AppDbContext _context;

    public RestaurantsController(AppDbContext context)
    {
        _context = context;
    }

    // GET: Restaurants
    public async Task<IActionResult> Index()
    {
        var restaurants = _context.Restaurants
            .Include(r => r.City);

        return View(await restaurants.ToListAsync());
    }

    // GET: Restaurants/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var restaurant = await _context.Restaurants
            .Include(r => r.City)
            .FirstOrDefaultAsync(r => r.Id == id);

        if (restaurant == null)
        {
            return NotFound();
        }

        return View(restaurant);
    }

    // GET: Restaurants/Create
    public IActionResult Create()
    {
        ViewBag.CityId = new SelectList(
            _context.Cities,
            "Id",
            "Name"
        );

        return View();
    }

    // POST: Restaurants/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        [Bind("Id,Name,CityId,Rating,Phone,ImagePath")]
        Restaurant restaurant)
    {
        if (ModelState.IsValid)
        {
            _context.Add(restaurant);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        ViewBag.CityId = new SelectList(
            _context.Cities,
            "Id",
            "Name",
            restaurant.CityId
        );

        return View(restaurant);
    }

    // GET: Restaurants/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var restaurant = await _context.Restaurants.FindAsync(id);

        if (restaurant == null)
        {
            return NotFound();
        }

        ViewBag.CityId = new SelectList(
            _context.Cities,
            "Id",
            "Name",
            restaurant.CityId
        );

        return View(restaurant);
    }

    // POST: Restaurants/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        int? id,
        [Bind("Id,Name,CityId,Rating,Phone,ImagePath")]
        Restaurant restaurant)
    {
        if (id != restaurant.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(restaurant);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RestaurantExists(restaurant.Id))
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
            restaurant.CityId
        );

        return View(restaurant);
    }

    // GET: Restaurants/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var restaurant = await _context.Restaurants
            .Include(r => r.City)
            .FirstOrDefaultAsync(r => r.Id == id);

        if (restaurant == null)
        {
            return NotFound();
        }

        return View(restaurant);
    }

    // POST: Restaurants/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var restaurant = await _context.Restaurants.FindAsync(id);

        if (restaurant != null)
        {
            _context.Restaurants.Remove(restaurant);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }

    private bool RestaurantExists(int id)
    {
        return _context.Restaurants.Any(e => e.Id == id);
    }
}