
using KernalTravelGuide.Data;
using KernalTravelGuide.Models.Enums;
using System.Security.Claims;
using KernalTravelGuide.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

[Authorize(Roles = "Admin")]
public class HotelsController : Controller
{
    private readonly AppDbContext _context;
    private readonly IWebHostEnvironment _environment;

    public HotelsController(
        AppDbContext context,
        IWebHostEnvironment environment)
    {
        _context = context;
        _environment = environment;
    }

    // GET: Hotels
    public async Task<IActionResult> Index()
    {
        var hotels = await _context.Hotels
            .Include(h => h.City)
            .ToListAsync();

        return View(hotels);
    }

    // GET: Hotels/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
            return NotFound();

        var hotel = await _context.Hotels
            .Include(h => h.City)
            .FirstOrDefaultAsync(h => h.Id == id);

        if (hotel == null)
            return NotFound();

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
        [Bind("Id,Name,CityId,PricePerNight,StarRating,ContactNo,Email,Website,Availability")]
        Hotel hotel,
        IFormFile? ImageFile)
    {
        if (ModelState.IsValid)
        {
            // Image upload
            if (ImageFile != null && ImageFile.Length > 0)
            {
                string uploadsFolder = Path.Combine(
                    _environment.WebRootPath,
                    "uploads",
                    "hotels"
                );

                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                string extension = Path.GetExtension(ImageFile.FileName);

                string fileName = Guid.NewGuid().ToString() + extension;

                string filePath = Path.Combine(
                    uploadsFolder,
                    fileName
                );

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await ImageFile.CopyToAsync(stream);
                }

                hotel.ImagePath = "/uploads/hotels/" + fileName;
            }

            _context.Hotels.Add(hotel);

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
            return NotFound();

        var hotel = await _context.Hotels.FindAsync(id);

        if (hotel == null)
            return NotFound();

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
        int id,
        [Bind("Id,Name,CityId,PricePerNight,StarRating,ContactNo,Email,Website,Availability")]
        Hotel hotel,
        IFormFile? ImageFile)
    {
        if (id != hotel.Id)
            return NotFound();

        var existingHotel = await _context.Hotels
            .AsNoTracking()
            .FirstOrDefaultAsync(h => h.Id == id);

        if (existingHotel == null)
            return NotFound();

        if (ModelState.IsValid)
        {
            // New image selected
            if (ImageFile != null && ImageFile.Length > 0)
            {
                string uploadsFolder = Path.Combine(
                    _environment.WebRootPath,
                    "uploads",
                    "hotels"
                );

                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                // Delete old image
                if (!string.IsNullOrEmpty(existingHotel.ImagePath))
                {
                    string oldImagePath = Path.Combine(
                        _environment.WebRootPath,
                        existingHotel.ImagePath.TrimStart('/')
                            .Replace("/", Path.DirectorySeparatorChar.ToString())
                    );

                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                string extension = Path.GetExtension(ImageFile.FileName);

                string fileName = Guid.NewGuid().ToString() + extension;

                string filePath = Path.Combine(
                    uploadsFolder,
                    fileName
                );

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await ImageFile.CopyToAsync(stream);
                }

                hotel.ImagePath = "/uploads/hotels/" + fileName;
            }
            else
            {
                // Keep old image
                hotel.ImagePath = existingHotel.ImagePath;
            }

            _context.Update(hotel);

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

    // GET: Hotels/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
            return NotFound();

        var hotel = await _context.Hotels
            .Include(h => h.City)
            .FirstOrDefaultAsync(h => h.Id == id);

        if (hotel == null)
            return NotFound();

        return View(hotel);
    }

    // POST: Hotels/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var hotel = await _context.Hotels.FindAsync(id);

        if (hotel == null)
            return NotFound();

        // Delete image from wwwroot
        if (!string.IsNullOrEmpty(hotel.ImagePath))
        {
            string imagePath = Path.Combine(
                _environment.WebRootPath,
                hotel.ImagePath.TrimStart('/')
                    .Replace("/", Path.DirectorySeparatorChar.ToString())
            );

            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }
        }

        _context.Hotels.Remove(hotel);

        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    private bool HotelExists(int id)
    {
        return _context.Hotels.Any(e => e.Id == id);
    }
}