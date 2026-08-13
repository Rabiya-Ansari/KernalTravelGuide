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

    // ================= INDEX =================

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


    // ================= DETAILS =================

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


    // ================= CREATE GET =================

    public async Task<IActionResult> Create()
    {
        await LoadDropdowns();

        return View();
    }


    // ================= CREATE POST =================

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        Gallery gallery,
        IFormFile ImageFile)
    {
        // Image required
        if (ImageFile == null || ImageFile.Length == 0)
        {
            ModelState.AddModelError(
                "ImageFile",
                "Please select an image.");
        }

        if (ModelState.IsValid)
        {
            // Upload folder
            string uploadsFolder = Path.Combine(
                _environment.WebRootPath,
                "uploads",
                "gallery"
            );

            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            // Unique file name
            string extension =
                Path.GetExtension(ImageFile.FileName);

            string fileName =
                Guid.NewGuid().ToString() + extension;

            string filePath =
                Path.Combine(uploadsFolder, fileName);

            // Save image
            using (var stream = new FileStream(
                filePath,
                FileMode.Create))
            {
                await ImageFile.CopyToAsync(stream);
            }

            // Database path
            gallery.ImagePath =
                "/uploads/gallery/" + fileName;

            _context.Galleries.Add(gallery);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        await LoadDropdowns();

        return View(gallery);
    }


    // ================= EDIT GET =================

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
            return NotFound();

        var gallery =
            await _context.Galleries.FindAsync(id);

        if (gallery == null)
            return NotFound();

        await LoadDropdowns();

        return View(gallery);
    }


    // ================= EDIT POST =================

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

        // Keep old image
        gallery.ImagePath = existingGallery.ImagePath;

        // If new image selected
        if (ImageFile != null && ImageFile.Length > 0)
        {
            string uploadsFolder = Path.Combine(
                _environment.WebRootPath,
                "uploads",
                "gallery"
            );

            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            string extension =
                Path.GetExtension(ImageFile.FileName);

            string fileName =
                Guid.NewGuid().ToString() + extension;

            string filePath =
                Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(
                filePath,
                FileMode.Create))
            {
                await ImageFile.CopyToAsync(stream);
            }

            // Delete old image
            if (!string.IsNullOrEmpty(existingGallery.ImagePath))
            {
                string oldImagePath =
                    Path.Combine(
                        _environment.WebRootPath,
                        existingGallery.ImagePath.TrimStart('/')
                    );

                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }
            }

            gallery.ImagePath =
                "/uploads/gallery/" + fileName;
        }

        if (ModelState.IsValid)
        {
            _context.Update(gallery);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        await LoadDropdowns();

        return View(gallery);
    }


    // ================= DELETE GET =================

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


    // ================= DELETE POST =================

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var gallery =
            await _context.Galleries.FindAsync(id);

        if (gallery == null)
            return NotFound();

        // Delete physical image
        if (!string.IsNullOrEmpty(gallery.ImagePath))
        {
            string imagePath =
                Path.Combine(
                    _environment.WebRootPath,
                    gallery.ImagePath.TrimStart('/')
                );

            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }
        }

        _context.Galleries.Remove(gallery);

        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }


    // ================= DROPDOWNS =================

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
}