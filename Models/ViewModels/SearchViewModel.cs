using KernalTravelGuide.Models;

namespace KernalTravelGuide.Models.ViewModels
{
    public class SearchViewModel
    {
        // Search keyword
        public string? Keyword { get; set; }

        // Search category
        public string? Type { get; set; }

        // Location / City
        public int? CityId { get; set; }

        // Price range
        public double? MinPrice { get; set; }

        public double? MaxPrice { get; set; }

        // Quality / Rating
        public double? MinRating { get; set; }

        // Quantity
        // Hotels/Resorts = available rooms
        // Restaurants = seating capacity
        public int? MinQuantity { get; set; }

        // Availability
        public bool AvailableOnly { get; set; } = true;

        // Search Results
        public List<TouristSpot> TouristSpots { get; set; } = new();

        public List<Hotel> Hotels { get; set; } = new();

        public List<Restaurant> Restaurants { get; set; } = new();

        public List<Resort> Resorts { get; set; } = new();

        // Cities for location dropdown
        public List<City> Cities { get; set; } = new();
    }
}