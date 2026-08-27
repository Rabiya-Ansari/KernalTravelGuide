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


    // ============================================================
    // INDEX
    // Displays all restaurants with their city information.
    // ============================================================

    public async Task<IActionResult> Index()
    {
        var restaurants = await _context.Restaurants
            .Include(r => r.City)
            .OrderBy(r => r.Name)
            .ToListAsync();

        return View(restaurants);
    }


    // ============================================================
    // DETAILS - GET
    // Displays complete information about one restaurant.
    // ============================================================

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


    // ============================================================
    // CREATE - GET
    // Opens the restaurant creation form.
    // ============================================================

    public IActionResult Create()
    {
        LoadCities();

        return View();
    }


    // ============================================================
    // CREATE - POST
    // Saves restaurant information and uploaded image.
    // ============================================================

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        [Bind("Id,Name,CityId,Rating,AveragePrice,Capacity,Availability,Phone")]
        Restaurant restaurant,
        IFormFile? ImageFile)
    {
        // Validate model data first.
        if (!ModelState.IsValid)
        {
            LoadCities(restaurant.CityId);

            return View(restaurant);
        }


        // --------------------------------------------------------
        // IMAGE UPLOAD
        // --------------------------------------------------------

        if (ImageFile != null && ImageFile.Length > 0)
        {
            // Validate image.
            if (!ValidateImage(ImageFile))
            {
                LoadCities(restaurant.CityId);

                return View(restaurant);
            }


            // Create upload directory.
            string uploadsFolder = GetRestaurantImageFolder();


            // Generate a unique filename.
            string extension =
                Path.GetExtension(ImageFile.FileName)
                .ToLowerInvariant();

            string fileName =
                Guid.NewGuid().ToString("N") + extension;


            // Complete physical path.
            string filePath = Path.Combine(
                uploadsFolder,
                fileName
            );


            // Save image to wwwroot/uploads/restaurants.
            using (var stream = new FileStream(
                filePath,
                FileMode.Create))
            {
                await ImageFile.CopyToAsync(stream);
            }


            // Save relative path in database.
            restaurant.ImagePath =
                "/uploads/restaurants/" + fileName;
        }


        // Add restaurant to database.
        _context.Restaurants.Add(restaurant);

        await _context.SaveChangesAsync();


        // Redirect to restaurant list.
        return RedirectToAction(nameof(Index));
    }


    // ============================================================
    // EDIT - GET
    // Opens existing restaurant for editing.
    // ============================================================

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }


        var restaurant =
            await _context.Restaurants
                .FirstOrDefaultAsync(r => r.Id == id);


        if (restaurant == null)
        {
            return NotFound();
        }


        // Load cities for dropdown.
        LoadCities(restaurant.CityId);


        return View(restaurant);
    }


    // ============================================================
    // EDIT - POST
    // Updates restaurant and optionally replaces its image.
    // ============================================================

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        int id,
        [Bind("Id,Name,CityId,Rating,AveragePrice,Capacity,Availability,Phone")]
        Restaurant restaurant,
        IFormFile? ImageFile)
    {
        // Check route ID and model ID.
        if (id != restaurant.Id)
        {
            return NotFound();
        }


        // Get original restaurant from database.
        var existingRestaurant =
            await _context.Restaurants
                .FirstOrDefaultAsync(r => r.Id == id);


        if (existingRestaurant == null)
        {
            return NotFound();
        }


        // Validate restaurant information.
        if (!ModelState.IsValid)
        {
            LoadCities(restaurant.CityId);

            return View(restaurant);
        }


        // --------------------------------------------------------
        // UPDATE NORMAL RESTAURANT INFORMATION
        // --------------------------------------------------------

        existingRestaurant.Name =
            restaurant.Name;

        existingRestaurant.CityId =
            restaurant.CityId;

        existingRestaurant.Rating =
            restaurant.Rating;

        existingRestaurant.AveragePrice =
            restaurant.AveragePrice;

        existingRestaurant.Capacity =
            restaurant.Capacity;

        existingRestaurant.Availability =
            restaurant.Availability;

        existingRestaurant.Phone =
            restaurant.Phone;


        // --------------------------------------------------------
        // IMAGE REPLACEMENT
        // --------------------------------------------------------

        if (ImageFile != null && ImageFile.Length > 0)
        {
            // Validate new image.
            if (!ValidateImage(ImageFile))
            {
                LoadCities(restaurant.CityId);

                return View(restaurant);
            }


            // Create upload directory.
            string uploadsFolder =
                GetRestaurantImageFolder();


            // Get extension.
            string extension =
                Path.GetExtension(ImageFile.FileName)
                .ToLowerInvariant();


            // Generate unique filename.
            string fileName =
                Guid.NewGuid().ToString("N")
                + extension;


            // Physical path for new image.
            string newFilePath =
                Path.Combine(
                    uploadsFolder,
                    fileName
                );


            // Save new image.
            using (var stream = new FileStream(
                newFilePath,
                FileMode.Create))
            {
                await ImageFile.CopyToAsync(stream);
            }


            // ----------------------------------------------------
            // DELETE OLD IMAGE
            // ----------------------------------------------------

            DeleteRestaurantImage(
                existingRestaurant.ImagePath
            );


            // Save new image path.
            existingRestaurant.ImagePath =
                "/uploads/restaurants/" + fileName;
        }


        // Save changes.
        await _context.SaveChangesAsync();


        // Return to restaurant list.
        return RedirectToAction(nameof(Index));
    }


    // ============================================================
    // DELETE - GET
    // Shows confirmation page before deleting restaurant.
    // ============================================================

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }


        var restaurant =
            await _context.Restaurants
                .Include(r => r.City)
                .FirstOrDefaultAsync(r => r.Id == id);


        if (restaurant == null)
        {
            return NotFound();
        }


        return View(restaurant);
    }

    // DELETE - POST
    // Deletes restaurant and its physical image.

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        // Find restaurant.
        var restaurant =
            await _context.Restaurants
                .FirstOrDefaultAsync(r => r.Id == id);


        if (restaurant == null)
        {
            return NotFound();
        }


        // Delete restaurant image.
        DeleteRestaurantImage(
            restaurant.ImagePath
        );


        // Delete restaurant record.
        _context.Restaurants.Remove(restaurant);

        await _context.SaveChangesAsync();


        return RedirectToAction(nameof(Index));
    }

    // LOAD CITIES
    // Loads cities for Create/Edit dropdown.

    private void LoadCities(int? selectedCityId = null)
    {
        ViewBag.CityId = new SelectList(
            _context.Cities
                .OrderBy(c => c.Name)
                .ToList(),
            "Id",
            "Name",
            selectedCityId
        );
    }

    // GET IMAGE FOLDER
    // Returns the physical restaurant image folder.

    private string GetRestaurantImageFolder()
    {
        // wwwroot/uploads/restaurants
        string uploadsFolder = Path.Combine(
            _environment.WebRootPath,
            "uploads",
            "restaurants"
        );


        // Create folder if it does not exist.
        if (!Directory.Exists(uploadsFolder))
        {
            Directory.CreateDirectory(uploadsFolder);
        }


        return uploadsFolder;
    }

    // IMAGE VALIDATION
    // Checks image extension and file size.

    private bool ValidateImage(IFormFile imageFile)
    {
        // Allowed image extensions.
        string[] allowedExtensions =
        {
            ".jpg",
            ".jpeg",
            ".png",
            ".webp"
        };


        // Get file extension.
        string extension =
            Path.GetExtension(imageFile.FileName)
            .ToLowerInvariant();


        // Check extension.
        if (!allowedExtensions.Contains(extension))
        {
            ModelState.AddModelError(
                "ImagePath",
                "Only JPG, JPEG, PNG and WEBP images are allowed."
            );

            return false;
        }


        // Maximum file size = 5 MB.
        const long maxFileSize =
            5 * 1024 * 1024;


        if (imageFile.Length > maxFileSize)
        {
            ModelState.AddModelError(
                "ImagePath",
                "Image size must not exceed 5 MB."
            );

            return false;
        }


        return true;
    }

    // DELETE IMAGE
    // Deletes the physical image from wwwroot.

    private void DeleteRestaurantImage(string? imagePath)
    {
        // No image means nothing to delete.
        if (string.IsNullOrWhiteSpace(imagePath))
        {
            return;
        }


        // Remove starting slash.
        string relativePath =
            imagePath.TrimStart(
                '/',
                '\\'
            );


        string fullPath =
            Path.Combine(
                _environment.WebRootPath,
                relativePath
            );

        if (System.IO.File.Exists(fullPath))
        {
            System.IO.File.Delete(fullPath);
        }
    }


    
    // RESTAURANT EXISTS
    // Checks whether restaurant exists.


    private bool RestaurantExists(int id)
    {
        return _context.Restaurants
            .Any(r => r.Id == id);
    }
}