using KernalTravelGuide.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

[Authorize(Roles = "Admin")]
public class RestaurantsController : Controller
{
    private readonly AppDbContext _context;
    private readonly IWebHostEnvironment _environment;

    public RestaurantsController(
        AppDbContext context,
        IWebHostEnvironment environment)
    {
        _context = context;
        _environment = environment;
    }

    // GET: Restaurants
    public async Task<IActionResult> Index()
    {
        var restaurants = await _context.Restaurants
            .Include(r => r.City)
            .ToListAsync();

        return View(restaurants);
    }


    // GET: Restaurants/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
            return NotFound();

        var restaurant = await _context.Restaurants
            .Include(r => r.City)
            .FirstOrDefaultAsync(r => r.Id == id);

        if (restaurant == null)
            return NotFound();

        return View(restaurant);
    }


    // GET: Restaurants/Create
    public IActionResult Create()
    {
        ViewBag.CityId = new SelectList(
            _context.Cities.OrderBy(c => c.Name),
            "Id",
            "Name"
        );

        return View();
    }


    // POST: Restaurants/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        [Bind("Id,Name,CityId,Rating,Phone")]
        Restaurant restaurant,
        IFormFile? ImageFile)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.CityId = new SelectList(
                _context.Cities.OrderBy(c => c.Name),
                "Id",
                "Name",
                restaurant.CityId
            );

            return View(restaurant);
        }


        // IMAGE UPLOAD
        if (ImageFile != null && ImageFile.Length > 0)
        {
            string uploadsFolder = Path.Combine(
                _environment.WebRootPath,
                "uploads",
                "restaurants"
            );

            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }


            string extension =
                Path.GetExtension(ImageFile.FileName)
                .ToLowerInvariant();


            string[] allowedExtensions =
            {
                ".jpg",
                ".jpeg",
                ".png",
                ".webp"
            };


            if (!allowedExtensions.Contains(extension))
            {
                ModelState.AddModelError(
                    "ImageFile",
                    "Only JPG, JPEG, PNG and WEBP images are allowed."
                );

                ViewBag.CityId = new SelectList(
                    _context.Cities.OrderBy(c => c.Name),
                    "Id",
                    "Name",
                    restaurant.CityId
                );

                return View(restaurant);
            }


            // Unique filename
            string fileName =
                Guid.NewGuid().ToString()
                + extension;


            string filePath = Path.Combine(
                uploadsFolder,
                fileName
            );


            using (var stream = new FileStream(
                filePath,
                FileMode.Create))
            {
                await ImageFile.CopyToAsync(stream);
            }


            // Save path in database
            restaurant.ImagePath =
                "/uploads/restaurants/" + fileName;
        }


        _context.Restaurants.Add(restaurant);

        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }


    // GET: Restaurants/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
            return NotFound();

        var restaurant =
            await _context.Restaurants.FindAsync(id);

        if (restaurant == null)
            return NotFound();


        ViewBag.CityId = new SelectList(
            _context.Cities.OrderBy(c => c.Name),
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
        int id,
        [Bind("Id,Name,CityId,Rating,Phone,ImagePath")]
        Restaurant restaurant,
        IFormFile? ImageFile)
    {
        if (id != restaurant.Id)
            return NotFound();


        var existingRestaurant =
            await _context.Restaurants
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.Id == id);

        if (existingRestaurant == null)
            return NotFound();


        if (!ModelState.IsValid)
        {
            ViewBag.CityId = new SelectList(
                _context.Cities.OrderBy(c => c.Name),
                "Id",
                "Name",
                restaurant.CityId
            );

            return View(restaurant);
        }


        // Keep old image by default
        restaurant.ImagePath =
            existingRestaurant.ImagePath;


        // NEW IMAGE UPLOADED
        if (ImageFile != null && ImageFile.Length > 0)
        {
            string extension =
                Path.GetExtension(ImageFile.FileName)
                .ToLowerInvariant();


            string[] allowedExtensions =
            {
                ".jpg",
                ".jpeg",
                ".png",
                ".webp"
            };


            if (!allowedExtensions.Contains(extension))
            {
                ModelState.AddModelError(
                    "ImageFile",
                    "Only JPG, JPEG, PNG and WEBP images are allowed."
                );

                ViewBag.CityId = new SelectList(
                    _context.Cities.OrderBy(c => c.Name),
                    "Id",
                    "Name",
                    restaurant.CityId
                );

                return View(restaurant);
            }


            string uploadsFolder = Path.Combine(
                _environment.WebRootPath,
                "uploads",
                "restaurants"
            );


            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }


            string fileName =
                Guid.NewGuid().ToString()
                + extension;


            string newFilePath =
                Path.Combine(
                    uploadsFolder,
                    fileName
                );


            using (var stream = new FileStream(
                newFilePath,
                FileMode.Create))
            {
                await ImageFile.CopyToAsync(stream);
            }


            // Delete old image
            if (!string.IsNullOrEmpty(
                existingRestaurant.ImagePath))
            {
                string oldImagePath =
                    Path.Combine(
                        _environment.WebRootPath,
                        existingRestaurant.ImagePath.TrimStart('/')
                    );

                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }
            }


            restaurant.ImagePath =
                "/uploads/restaurants/" + fileName;
        }


        try
        {
            _context.Update(restaurant);

            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!RestaurantExists(restaurant.Id))
                return NotFound();

            throw;
        }


        return RedirectToAction(nameof(Index));
    }


    // GET: Restaurants/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
            return NotFound();

        var restaurant =
            await _context.Restaurants
                .Include(r => r.City)
                .FirstOrDefaultAsync(r => r.Id == id);

        if (restaurant == null)
            return NotFound();

        return View(restaurant);
    }


    // POST: Restaurants/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var restaurant =
            await _context.Restaurants.FindAsync(id);

        if (restaurant == null)
            return NotFound();


        // Delete physical image
        if (!string.IsNullOrEmpty(
            restaurant.ImagePath))
        {
            string imagePath =
                Path.Combine(
                    _environment.WebRootPath,
                    restaurant.ImagePath.TrimStart('/')
                );

            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }
        }


        _context.Restaurants.Remove(restaurant);

        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }


    private bool RestaurantExists(int id)
    {
        return _context.Restaurants
            .Any(e => e.Id == id);
    }
}