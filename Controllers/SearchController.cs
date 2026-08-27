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
            string? type,
            int? cityId,
            double? minPrice,
            double? maxPrice,
            double? minRating,
            int? minQuantity,
            bool availableOnly = true)
        {
            // -----------------------------------------
            // Base Queries
            // -----------------------------------------

            var touristSpots = _context.TouristSpots
                .Include(x => x.City)
                .Where(x => x.IsActive)
                .AsQueryable();

            var hotels = _context.Hotels
                .Include(x => x.City)
                .AsQueryable();

            var restaurants = _context.Restaurants
                .Include(x => x.City)
                .AsQueryable();

            var resorts = _context.Resorts
                .Include(x => x.City)
                .AsQueryable();


            // -----------------------------------------
            // Keyword
            // -----------------------------------------

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


            // -----------------------------------------
            // Category
            // -----------------------------------------

            if (!string.IsNullOrWhiteSpace(type))
            {
                type = type.Trim();

                switch (type.ToLower())
                {
                    case "touristspot":
                        hotels = hotels.Where(x => false);
                        restaurants = restaurants.Where(x => false);
                        resorts = resorts.Where(x => false);
                        break;

                    case "hotel":
                        touristSpots = touristSpots.Where(x => false);
                        restaurants = restaurants.Where(x => false);
                        resorts = resorts.Where(x => false);
                        break;

                    case "restaurant":
                        touristSpots = touristSpots.Where(x => false);
                        hotels = hotels.Where(x => false);
                        resorts = resorts.Where(x => false);
                        break;

                    case "resort":
                        touristSpots = touristSpots.Where(x => false);
                        hotels = hotels.Where(x => false);
                        restaurants = restaurants.Where(x => false);
                        break;
                }
            }


            // -----------------------------------------
            // Location / City
            // -----------------------------------------

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


            // -----------------------------------------
            // Minimum Price
            // -----------------------------------------

            if (minPrice.HasValue)
            {
                touristSpots = touristSpots
                    .Where(x => x.EntryFee >= minPrice.Value);

                hotels = hotels
                    .Where(x => x.PricePerNight >= minPrice.Value);

                restaurants = restaurants
                    .Where(x => x.AveragePrice >= minPrice.Value);

                resorts = resorts
                    .Where(x => x.Price >= minPrice.Value);
            }


            // -----------------------------------------
            // Maximum Price
            // -----------------------------------------

            if (maxPrice.HasValue)
            {
                touristSpots = touristSpots
                    .Where(x => x.EntryFee <= maxPrice.Value);

                hotels = hotels
                    .Where(x => x.PricePerNight <= maxPrice.Value);

                restaurants = restaurants
                    .Where(x => x.AveragePrice <= maxPrice.Value);

                resorts = resorts
                    .Where(x => x.Price <= maxPrice.Value);
            }


            // -----------------------------------------
            // Quality / Rating
            // -----------------------------------------

            if (minRating.HasValue)
            {
                hotels = hotels
                    .Where(x => x.StarRating >= minRating.Value);

                restaurants = restaurants
                    .Where(x => x.Rating >= minRating.Value);

                resorts = resorts
                    .Where(x => x.Rating >= minRating.Value);
            }


            // -----------------------------------------
            // Quantity
            //
            // Hotel    = AvailableRooms
            // Restaurant = Capacity
            // Resort   = AvailableRooms
            // -----------------------------------------

            if (minQuantity.HasValue)
            {
                hotels = hotels
                    .Where(x => x.AvailableRooms >= minQuantity.Value);

                restaurants = restaurants
                    .Where(x => x.Capacity >= minQuantity.Value);

                resorts = resorts
                    .Where(x => x.AvailableRooms >= minQuantity.Value);
            }


            // -----------------------------------------
            // Availability
            // -----------------------------------------

            if (availableOnly)
            {
                touristSpots = touristSpots
                    .Where(x => x.IsActive);

                hotels = hotels
                    .Where(x => x.Availability);

                restaurants = restaurants
                    .Where(x => x.Availability);

                resorts = resorts
                    .Where(x => x.Availability);
            }


            // -----------------------------------------
            // Search ViewModel
            // -----------------------------------------

            var model = new SearchViewModel
            {
                Keyword = keyword,
                Type = type,
                CityId = cityId,

                MinPrice = minPrice,
                MaxPrice = maxPrice,

                MinRating = minRating,
                MinQuantity = minQuantity,

                AvailableOnly = availableOnly,

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


        // -----------------------------------------
        // Advanced Search Page
        // -----------------------------------------

        [HttpGet]
        public async Task<IActionResult> Advanced()
        {
            var model = new SearchViewModel
            {
                Cities = await _context.Cities
                    .OrderBy(x => x.Name)
                    .ToListAsync()
            };

            return View(model);
        }
    }
}