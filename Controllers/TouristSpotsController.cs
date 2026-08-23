using KernalTravelGuide.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

[Authorize(Roles = "Admin")]
public class TouristSpotsController : Controller
{
    private readonly AppDbContext _context;
    private readonly IWebHostEnvironment _environment;

    public TouristSpotsController(
        AppDbContext context,
        IWebHostEnvironment environment)
    {
        _context = context;
        _environment = environment;
    }

    // GET: TouristSpots
    public async Task<IActionResult> Index()
    {
        var touristSpots = await _context.TouristSpots
            .Include(t => t.City)
            .ToListAsync();

        return View(touristSpots);
    }

    // GET: TouristSpots/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
            return NotFound();

        var touristSpot = await _context.TouristSpots
            .Include(t => t.City)
            .FirstOrDefaultAsync(t => t.Id == id);

        if (touristSpot == null)
            return NotFound();

        return View(touristSpot);
    }

    // GET: TouristSpots/Create
    public IActionResult Create()
    {
        ViewBag.CityId = new SelectList(
            _context.Cities,
            "Id",
            "Name"
        );

        return View();
    }

    // POST: TouristSpots/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        [Bind("Id,Name,Description,CityId,EntryFee,MapUrl,IsActive")]
        TouristSpot touristSpot,
        IFormFile? ImageFile)
    {
        if (ModelState.IsValid)
        {
            if (ImageFile != null && ImageFile.Length > 0)
            {
                string uploadsFolder = Path.Combine(
                    _environment.WebRootPath,
                    "uploads",
                    "touristspots"
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

                using (var stream =
                       new FileStream(filePath, FileMode.Create))
                {
                    await ImageFile.CopyToAsync(stream);
                }

                touristSpot.ImagePath =
                    "/uploads/touristspots/" + fileName;
            }

            _context.TouristSpots.Add(touristSpot);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        ViewBag.CityId = new SelectList(
            _context.Cities,
            "Id",
            "Name",
            touristSpot.CityId
        );

        return View(touristSpot);
    }

    // GET: TouristSpots/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
            return NotFound();

        var touristSpot =
            await _context.TouristSpots.FindAsync(id);

        if (touristSpot == null)
            return NotFound();

        ViewBag.CityId = new SelectList(
            _context.Cities,
            "Id",
            "Name",
            touristSpot.CityId
        );

        return View(touristSpot);
    }

    // POST: TouristSpots/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        int id,
        [Bind("Id,Name,Description,CityId,EntryFee,MapUrl,IsActive")]
        TouristSpot touristSpot,
        IFormFile? ImageFile)
    {
        if (id != touristSpot.Id)
            return NotFound();

        var existingSpot =
            await _context.TouristSpots
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.Id == id);

        if (existingSpot == null)
            return NotFound();

        if (ModelState.IsValid)
        {
            // New image selected
            if (ImageFile != null && ImageFile.Length > 0)
            {
                string uploadsFolder = Path.Combine(
                    _environment.WebRootPath,
                    "uploads",
                    "touristspots"
                );

                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                // Delete old image
                if (!string.IsNullOrEmpty(
                    existingSpot.ImagePath))
                {
                    string oldImagePath = Path.Combine(
                        _environment.WebRootPath,
                        existingSpot.ImagePath
                            .TrimStart('/')
                            .Replace(
                                "/",
                                Path.DirectorySeparatorChar.ToString()
                            )
                    );

                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                string extension =
                    Path.GetExtension(ImageFile.FileName);

                string fileName =
                    Guid.NewGuid().ToString() + extension;

                string filePath =
                    Path.Combine(uploadsFolder, fileName);

                using (var stream =
                       new FileStream(filePath, FileMode.Create))
                {
                    await ImageFile.CopyToAsync(stream);
                }

                touristSpot.ImagePath =
                    "/uploads/touristspots/" + fileName;
            }
            else
            {
                // Keep old image
                touristSpot.ImagePath =
                    existingSpot.ImagePath;
            }

            _context.Update(touristSpot);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        ViewBag.CityId = new SelectList(
            _context.Cities,
            "Id",
            "Name",
            touristSpot.CityId
        );

        return View(touristSpot);
    }

    // GET: TouristSpots/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
            return NotFound();

        var touristSpot = await _context.TouristSpots
            .Include(t => t.City)
            .FirstOrDefaultAsync(t => t.Id == id);

        if (touristSpot == null)
            return NotFound();

        return View(touristSpot);
    }

    // POST: TouristSpots/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var touristSpot =
            await _context.TouristSpots.FindAsync(id);

        if (touristSpot == null)
            return NotFound();

        // Delete image from wwwroot
        if (!string.IsNullOrEmpty(touristSpot.ImagePath))
        {
            string imagePath = Path.Combine(
                _environment.WebRootPath,
                touristSpot.ImagePath
                    .TrimStart('/')
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

        _context.TouristSpots.Remove(touristSpot);

        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    private bool TouristSpotExists(int id)
    {
        return _context.TouristSpots
            .Any(e => e.Id == id);
    }
}