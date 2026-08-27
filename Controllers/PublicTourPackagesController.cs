using KernalTravelGuide.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KernalTravelGuide.Controllers
{
    public class PublicTourPackagesController : Controller
    {
        private readonly AppDbContext _context;

        public PublicTourPackagesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: PublicTourPackages/Index
        public async Task<IActionResult> Index()
        {
            var packages = await _context.TourPackages
                .AsNoTracking()
                .ToListAsync();

            return View(packages ?? new List<TourPackage>());
        }

        // GET: PublicTourPackages/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var package = await _context.TourPackages
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id);

            if (package == null)
                return NotFound();

            return View(package);
        }

        // GET: PublicTourPackages/BookNow/5
        // Is action par [Authorize] lagaya gaya hai taake bina login ke access na ho
        [Authorize]
        public IActionResult BookNow(int tourPackageId)
        {
            // Login hone ke baad direct MyBookings Controller par redirect kar dega
            return RedirectToAction("Create", "MyBookings", new { tourPackageId = tourPackageId });
        }
    }
}