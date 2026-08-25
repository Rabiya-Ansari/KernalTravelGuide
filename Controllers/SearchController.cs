using KernalTravelGuide.Data;
using KernalTravelGuide.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KernalTravelGuide.Controllers
{
    public class SearchController : Controller
    {
        private readonly AppDbContext _context;

        public SearchController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index(
            string? keyword,
            int? cityId,
            double? minPrice,
            double? maxPrice)
        {
            var touristSpots = _context.TouristSpots
                .Include(x => x.City)
                .Where(x => x.IsActive)
                .AsQueryable();

            var hotels = _context.Hotels
                .Include(x => x.City)
                .Where(x => x.Availability)
                .AsQueryable();

            var restaurants = _context.Restaurants
                .Include(x => x.City)
                .AsQueryable();

            var resorts = _context.Resorts
                .Include(x => x.City)
                .Where(x => x.Availability)
                .AsQueryable();


            // Keyword
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                keyword = keyword.Trim();

                touristSpots = touristSpots.Where(x =>
                    x.Name.Contains(keyword));

                hotels = hotels.Where(x =>
                    x.Name.Contains(keyword));

                restaurants = restaurants.Where(x =>
                    x.Name.Contains(keyword));

                resorts = resorts.Where(x =>
                    x.Name.Contains(keyword));
            }


            // City
            if (cityId.HasValue)
            {
                touristSpots = touristSpots
                    .Where(x => x.CityId == cityId.Value);

                hotels = hotels
                    .Where(x => x.CityId == cityId.Value);

                restaurants = restaurants
                    .Where(x => x.CityId == cityId.Value);

                resorts = resorts
                    .Where(x => x.CityId == cityId.Value);
            }


            // Tourist Spot Entry Fee
            if (minPrice.HasValue)
            {
                touristSpots = touristSpots
                    .Where(x => x.EntryFee >= minPrice.Value);

                hotels = hotels
                    .Where(x => x.PricePerNight >= minPrice.Value);

                resorts = resorts
                    .Where(x => x.Price >= minPrice.Value);
            }

            if (maxPrice.HasValue)
            {
                touristSpots = touristSpots
                    .Where(x => x.EntryFee <= maxPrice.Value);

                hotels = hotels
                    .Where(x => x.PricePerNight <= maxPrice.Value);

                resorts = resorts
                    .Where(x => x.Price <= maxPrice.Value);
            }


            var model = new SearchViewModel
            {
                Keyword = keyword,
                CityId = cityId,
                MinPrice = minPrice,
                MaxPrice = maxPrice,

                TouristSpots = await touristSpots.ToListAsync(),
                Hotels = await hotels.ToListAsync(),
                Restaurants = await restaurants.ToListAsync(),
                Resorts = await resorts.ToListAsync(),

                Cities = await _context.Cities
                    .OrderBy(x => x.Name)
                    .ToListAsync()
            };

            return View(model);
        }
    }
}