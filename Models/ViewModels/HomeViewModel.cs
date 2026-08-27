using KernalTravelGuide.Models;

namespace KernalTravelGuide.Models.ViewModels
{
    // This view model provides the public Home page with real database data.
    public class HomeViewModel
    {
        // Latest active tourist spots shown on the Home page.
        public List<TouristSpot> TouristSpots { get; set; } = new();

        // Available hotels shown on the Home page.
        public List<Hotel> Hotels { get; set; } = new();

        // Available restaurants shown on the Home page.
        public List<Restaurant> Restaurants { get; set; } = new();

        // Available resorts shown on the Home page.
        public List<Resort> Resorts { get; set; } = new();

        // Available tour packages shown on the Home page.
        public List<TourPackage> TourPackages { get; set; } = new();

        // FIXED: Changed List<Review> to List<Feedback>
        public List<Feedback> Reviews { get; set; } = new();

        // Total number of tourist spots.
        public int TouristSpotCount { get; set; }

        // Total number of hotels.
        public int HotelCount { get; set; }

        // Total number of restaurants.
        public int RestaurantCount { get; set; }

        // Total number of resorts.
        public int ResortCount { get; set; }
    }
}