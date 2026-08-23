using KernalTravelGuide.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Authorize(Roles = "Admin")]
public class TourPackagesController : Controller
{
    private readonly AppDbContext _context;
    private readonly IWebHostEnvironment _environment;

    public TourPackagesController(
        AppDbContext context,
        IWebHostEnvironment environment)
    {
        _context = context;
        _environment = environment;
    }

    // GET: TourPackages
    public async Task<IActionResult> Index()
    {
        var tourPackages = await _context.TourPackages
            .ToListAsync();

        return View(tourPackages);
    }

    // GET: TourPackages/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
            return NotFound();

        var tourPackage = await _context.TourPackages
            .FirstOrDefaultAsync(x => x.Id == id);

        if (tourPackage == null)
            return NotFound();

        return View(tourPackage);
    }

    // GET: TourPackages/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: TourPackages/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        [Bind("Id,PackageName,DurationDays,Price,Description,IsAvailable")]
        TourPackage tourPackage,
        IFormFile? ImageFile)
    {
        if (ModelState.IsValid)
        {
            // Upload image
            if (ImageFile != null && ImageFile.Length > 0)
            {
                string folderPath = Path.Combine(
                    _environment.WebRootPath,
                    "images",
                    "tourpackages"
                );

                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                string fileName =
                    Guid.NewGuid().ToString()
                    + Path.GetExtension(ImageFile.FileName);

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

                tourPackage.ImagePath =
                    "/images/tourpackages/" + fileName;
            }

            _context.TourPackages.Add(tourPackage);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        return View(tourPackage);
    }

    // GET: TourPackages/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
            return NotFound();

        var tourPackage =
            await _context.TourPackages.FindAsync(id);

        if (tourPackage == null)
            return NotFound();

        return View(tourPackage);
    }

    // POST: TourPackages/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        int id,
        [Bind("Id,PackageName,DurationDays,Price,Description,IsAvailable")]
        TourPackage tourPackage,
        IFormFile? ImageFile)
    {
        if (id != tourPackage.Id)
            return NotFound();

        if (ModelState.IsValid)
        {
            var existingPackage =
                await _context.TourPackages
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == id);

            if (existingPackage == null)
                return NotFound();

            // If new image selected
            if (ImageFile != null && ImageFile.Length > 0)
            {
                string folderPath = Path.Combine(
                    _environment.WebRootPath,
                    "images",
                    "tourpackages"
                );

                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                // Delete old image
                if (!string.IsNullOrEmpty(existingPackage.ImagePath))
                {
                    string oldImagePath =
                        Path.Combine(
                            _environment.WebRootPath,
                            existingPackage.ImagePath.TrimStart('/')
                                .Replace("/", Path.DirectorySeparatorChar.ToString())
                        );

                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

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

                tourPackage.ImagePath =
                    "/images/tourpackages/" + fileName;
            }
            else
            {
                // Keep old image
                tourPackage.ImagePath =
                    existingPackage.ImagePath;
            }

            try
            {
                _context.Update(tourPackage);

                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TourPackageExists(tourPackage.Id))
                    return NotFound();

                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        return View(tourPackage);
    }

    // GET: TourPackages/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
            return NotFound();

        var tourPackage =
            await _context.TourPackages
                .FirstOrDefaultAsync(x => x.Id == id);

        if (tourPackage == null)
            return NotFound();

        return View(tourPackage);
    }

    // POST: TourPackages/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var tourPackage =
            await _context.TourPackages.FindAsync(id);

        if (tourPackage == null)
            return NotFound();

        // Delete image from wwwroot
        if (!string.IsNullOrEmpty(tourPackage.ImagePath))
        {
            string imagePath =
                Path.Combine(
                    _environment.WebRootPath,
                    tourPackage.ImagePath.TrimStart('/')
                        .Replace(
                            "/",
                            Path.DirectorySeparatorChar.ToString()
                        )
                );

            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }
        }

        _context.TourPackages.Remove(tourPackage);

        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    private bool TourPackageExists(int id)
    {
        return _context.TourPackages
            .Any(x => x.Id == id);
    }
}