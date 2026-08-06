using System.ComponentModel.DataAnnotations;

namespace KernalTravelGuide.Models
{
    public class TravelInformation
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string TransportName { get; set; } = string.Empty;

        [Required]
        public int FromCityId { get; set; }

        [Required]
        public int ToCityId { get; set; }

        public City? FromCity { get; set; }

        public City? ToCity { get; set; }

        [Range(100, 50000)]
        public double Fare { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }
    }
}
