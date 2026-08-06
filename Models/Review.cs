using KernalTravelGuide.Data;
using System.ComponentModel.DataAnnotations;

namespace KernalTravelGuide.Models
{
    public class Review
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;

        public ApplicationUser? User { get; set; }

        [Required]
        [Range(1, 5)]
        public int Rating { get; set; }

        [Required]
        [StringLength(500)]
        public string Comment { get; set; } = string.Empty;

        public DateTime ReviewDate { get; set; } = DateTime.Now;

        public int? TouristSpotId { get; set; }
        public TouristSpot? TouristSpot { get; set; }

        public int? HotelId { get; set; }
        public Hotel? Hotel { get; set; }

        public int? RestaurantId { get; set; }
        public Restaurant? Restaurant { get; set; }

        public int? ResortId { get; set; }
        public Resort? Resort { get; set; }
    }
}
