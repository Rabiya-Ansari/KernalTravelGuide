using KernalTravelGuide.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Authorize(Roles = "Admin")]
public class GalleryController : Controller
{
    private readonly AppDbContext _context;
    private readonly IWebHostEnvironment _environment;

    public GalleryController(
        AppDbContext context,
        IWebHostEnvironment environment)
    {
        _context = context;
        _environment = environment;
    }

    // GET: Gallery
    public async Task<IActionResult> Index()
    {
        var galleries = await _context.Galleries
            .Include(g => g.TouristSpot)
            .Include(g => g.Hotel)
            .Include(g => g.Restaurant)
            .Include(g => g.Resort)
            .Include(g => g.TourPackage)
            .ToListAsync();

        return View(galleries);
    }

    // GET: Gallery/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
            return NotFound();

        var gallery = await _context.Galleries
            .Include(g => g.TouristSpot)
            .Include(g => g.Hotel)
            .Include(g => g.Restaurant)
            .Include(g => g.Resort)
            .Include(g => g.TourPackage)
            .FirstOrDefaultAsync(g => g.Id == id);

        if (gallery == null)
            return NotFound();

        return View(gallery);
    }

    // GET: Gallery/Create
    public async Task<IActionResult> Create()
    {
        await LoadDropdowns();
        return View();
    }

    // POST: Gallery/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        Gallery gallery,
        IFormFile? ImageFile)
    {
        // Image required
        if (ImageFile == null || ImageFile.Length == 0)
        {
            ModelState.AddModelError(
                "ImagePath",
                "Please select an image."
            );
        }

        if (!ModelState.IsValid)
        {
            await LoadDropdowns();
            return View(gallery);
        }

        // Save image
        string folderPath = Path.Combine(
            _environment.WebRootPath,
            "images",
            "gallery"
        );

        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        string fileName =
            Guid.NewGuid().ToString()
            + Path.GetExtension(ImageFile!.FileName);

        string filePath = Path.Combine(
            folderPath,
            fileName
        );

        using (var stream = new FileStream(
            filePath,
            FileMode.Create))
        {
            await ImageFile.CopyToAsync(stream);
        }

        gallery.ImagePath =
            "/images/gallery/" + fileName;

        _context.Galleries.Add(gallery);

        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    // GET: Gallery/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
            return NotFound();

        var gallery = await _context.Galleries
            .FirstOrDefaultAsync(g => g.Id == id);

        if (gallery == null)
            return NotFound();

        await LoadDropdowns();

        return View(gallery);
    }

    // POST: Gallery/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        int id,
        Gallery gallery,
        IFormFile? ImageFile)
    {
        if (id != gallery.Id)
            return NotFound();

        var existingGallery =
            await _context.Galleries
                .AsNoTracking()
                .FirstOrDefaultAsync(g => g.Id == id);

        if (existingGallery == null)
            return NotFound();

        if (!ModelState.IsValid)
        {
            await LoadDropdowns();
            return View(gallery);
        }

        // New image selected
        if (ImageFile != null && ImageFile.Length > 0)
        {
            string folderPath = Path.Combine(
                _environment.WebRootPath,
                "images",
                "gallery"
            );

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            // Delete old image
            DeleteImage(existingGallery.ImagePath);

            string fileName =
                Guid.NewGuid().ToString()
                + Path.GetExtension(ImageFile.FileName);

            string filePath =
                Path.Combine(folderPath, fileName);

            using (var stream = new FileStream(
                filePath,
                FileMode.Create))
            {
                await ImageFile.CopyToAsync(stream);
            }

            gallery.ImagePath =
                "/images/gallery/" + fileName;
        }
        else
        {
            // Keep old image
            gallery.ImagePath =
                existingGallery.ImagePath;
        }

        _context.Galleries.Update(gallery);

        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    // GET: Gallery/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
            return NotFound();

        var gallery = await _context.Galleries
            .Include(g => g.TouristSpot)
            .Include(g => g.Hotel)
            .Include(g => g.Restaurant)
            .Include(g => g.Resort)
            .Include(g => g.TourPackage)
            .FirstOrDefaultAsync(g => g.Id == id);

        if (gallery == null)
            return NotFound();

        return View(gallery);
    }

    // POST: Gallery/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var gallery =
            await _context.Galleries.FindAsync(id);

        if (gallery == null)
            return NotFound();

        // Delete physical image
        DeleteImage(gallery.ImagePath);

        _context.Galleries.Remove(gallery);

        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    // Dropdown data
    private async Task LoadDropdowns()
    {
        ViewBag.TouristSpots =
            await _context.TouristSpots
                .OrderBy(x => x.Name)
                .ToListAsync();

        ViewBag.Hotels =
            await _context.Hotels
                .OrderBy(x => x.Name)
                .ToListAsync();

        ViewBag.Restaurants =
            await _context.Restaurants
                .OrderBy(x => x.Name)
                .ToListAsync();

        ViewBag.Resorts =
            await _context.Resorts
                .OrderBy(x => x.Name)
                .ToListAsync();

        ViewBag.TourPackages =
            await _context.TourPackages
                .OrderBy(x => x.PackageName)
                .ToListAsync();
    }

    // Delete physical image
    private void DeleteImage(string? imagePath)
    {
        if (string.IsNullOrEmpty(imagePath))
            return;

        string fullPath = Path.Combine(
            _environment.WebRootPath,
            imagePath.TrimStart('/')
                .Replace(
                    "/",
                    Path.DirectorySeparatorChar.ToString()
                )
        );

        if (System.IO.File.Exists(fullPath))
        {
            System.IO.File.Delete(fullPath);
        }
    }
}