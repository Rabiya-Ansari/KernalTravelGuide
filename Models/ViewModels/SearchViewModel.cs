using KernalTravelGuide.Models;

namespace KernalTravelGuide.Models.ViewModels
{
    public class SearchViewModel
    {
        public string? Keyword { get; set; }

        public int? CityId { get; set; }

        public double? MinPrice { get; set; }

        public double? MaxPrice { get; set; }

        public List<TouristSpot> TouristSpots { get; set; } = new();

        public List<Hotel> Hotels { get; set; } = new();

        public List<Restaurant> Restaurants { get; set; } = new();

        public List<Resort> Resorts { get; set; } = new();

        public List<City> Cities { get; set; } = new();
    }
}