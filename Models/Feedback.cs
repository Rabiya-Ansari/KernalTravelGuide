using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KernalTravelGuide.Models
{
    public class Feedback
    {

        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        [RegularExpression(@"^[A-Za-z ]+$")]
        public string CustomerName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [Range(1, 5)]
        public int Rating { get; set; }

        [Required]
        [StringLength(1000)]
        public string Comments { get; set; } = string.Empty;

        public DateTime FeedbackDate { get; set; } = DateTime.Now;

        public int? HotelId { get; set; }
        public int? ResortId { get; set; }
        public int? RestaurantId { get; set; }
        public int? TouristSpotId { get; set; }
        public int? TourPackageId { get; set; }


        [ForeignKey(nameof(HotelId))]
        public Hotel? Hotel { get; set; }

        [ForeignKey(nameof(ResortId))]
        public Resort? Resort { get; set; }

        [ForeignKey(nameof(RestaurantId))]
        public Restaurant? Restaurant { get; set; }

        [ForeignKey(nameof(TouristSpotId))]
        public TouristSpot? TouristSpot { get; set; }

        [ForeignKey(nameof(TourPackageId))]
        public TourPackage? TourPackage { get; set; }
    }
}