using System.ComponentModel.DataAnnotations;

namespace KernalTravelGuide.Models
{
    public class Gallery
    {
        public int Id { get; set; }

        [Required]
        [StringLength(250)]
        public string ImagePath { get; set; } = string.Empty;

        [StringLength(150)]
        public string? Caption { get; set; }

        public int? TouristSpotId { get; set; }
        public TouristSpot? TouristSpot { get; set; }

        public int? HotelId { get; set; }
        public Hotel? Hotel { get; set; }

        public int? RestaurantId { get; set; }
        public Restaurant? Restaurant { get; set; }

        public int? ResortId { get; set; }
        public Resort? Resort { get; set; }

        public int? TourPackageId { get; set; }
        public TourPackage? TourPackage { get; set; }
    }
}
