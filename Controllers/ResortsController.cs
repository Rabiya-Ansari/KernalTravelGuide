using KernalTravelGuide.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

[Authorize(Roles = "Admin")]
public class ResortsController : Controller
{
    private readonly AppDbContext _context;
    private readonly IWebHostEnvironment _environment;

    public ResortsController(
        AppDbContext context,
        IWebHostEnvironment environment)
    {
        _context = context;
        _environment = environment;
    }

    // GET: Resorts
    public async Task<IActionResult> Index()
    {
        var resorts = await _context.Resorts
            .Include(r => r.City)
            .ToListAsync();

        return View(resorts);
    }

    // GET: Resorts/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
            return NotFound();

        var resort = await _context.Resorts
            .Include(r => r.City)
            .FirstOrDefaultAsync(r => r.Id == id);

        if (resort == null)
            return NotFound();

        return View(resort);
    }

    // GET: Resorts/Create
    public IActionResult Create()
    {
        ViewBag.CityId = new SelectList(
            _context.Cities,
            "Id",
            "Name"
        );

        return View();
    }

    // POST: Resorts/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        [Bind("Id,Name,CityId,Price,Rating,Availability")]
        Resort resort,
        IFormFile? ImageFile)
    {
        if (ModelState.IsValid)
        {
            // Upload image
            if (ImageFile != null && ImageFile.Length > 0)
            {
                string folder = Path.Combine(
                    _environment.WebRootPath,
                    "uploads",
                    "resorts"
                );

                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }

                string extension =
                    Path.GetExtension(ImageFile.FileName);

                string fileName =
                    Guid.NewGuid().ToString() + extension;

                string filePath =
                    Path.Combine(folder, fileName);

                using (var stream = new FileStream(
                    filePath,
                    FileMode.Create))
                {
                    await ImageFile.CopyToAsync(stream);
                }

                resort.ImagePath =
                    "/uploads/resorts/" + fileName;
            }

            _context.Resorts.Add(resort);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        ViewBag.CityId = new SelectList(
            _context.Cities,
            "Id",
            "Name",
            resort.CityId
        );

        return View(resort);
    }

    // GET: Resorts/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
            return NotFound();

        var resort = await _context.Resorts.FindAsync(id);

        if (resort == null)
            return NotFound();

        ViewBag.CityId = new SelectList(
            _context.Cities,
            "Id",
            "Name",
            resort.CityId
        );

        return View(resort);
    }

    // POST: Resorts/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        int id,
        [Bind("Id,Name,CityId,Price,Rating,Availability,ImagePath")]
        Resort resort,
        IFormFile? ImageFile)
    {
        if (id != resort.Id)
            return NotFound();

        if (ModelState.IsValid)
        {
            var existingResort =
                await _context.Resorts
                    .AsNoTracking()
                    .FirstOrDefaultAsync(r => r.Id == id);

            if (existingResort == null)
                return NotFound();

            // Keep old image
            resort.ImagePath = existingResort.ImagePath;

            // If new image selected
            if (ImageFile != null && ImageFile.Length > 0)
            {
                string folder = Path.Combine(
                    _environment.WebRootPath,
                    "uploads",
                    "resorts"
                );

                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }

                // Delete old image
                if (!string.IsNullOrEmpty(existingResort.ImagePath))
                {
                    string oldImagePath =
                        Path.Combine(
                            _environment.WebRootPath,
                            existingResort.ImagePath
                                .TrimStart('/')
                                .Replace('/', Path.DirectorySeparatorChar)
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
                    Path.Combine(folder, fileName);

                using (var stream = new FileStream(
                    filePath,
                    FileMode.Create))
                {
                    await ImageFile.CopyToAsync(stream);
                }

                resort.ImagePath =
                    "/uploads/resorts/" + fileName;
            }

            _context.Resorts.Update(resort);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        ViewBag.CityId = new SelectList(
            _context.Cities,
            "Id",
            "Name",
            resort.CityId
        );

        return View(resort);
    }

    // GET: Resorts/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
            return NotFound();

        var resort = await _context.Resorts
            .Include(r => r.City)
            .FirstOrDefaultAsync(r => r.Id == id);

        if (resort == null)
            return NotFound();

        return View(resort);
    }

    // POST: Resorts/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var resort = await _context.Resorts.FindAsync(id);

        if (resort == null)
            return NotFound();

        // Delete image from wwwroot
        if (!string.IsNullOrEmpty(resort.ImagePath))
        {
            string imagePath =
                Path.Combine(
                    _environment.WebRootPath,
                    resort.ImagePath
                        .TrimStart('/')
                        .Replace(
                            '/',
                            Path.DirectorySeparatorChar
                        )
                );

            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }
        }

        _context.Resorts.Remove(resort);

        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }
}