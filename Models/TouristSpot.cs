using System.ComponentModel.DataAnnotations;

namespace KernalTravelGuide.Models
{
    public class TouristSpot
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(500)]
        public string Description { get; set; } = string.Empty;

        [Required]
        public int CityId { get; set; }

        public City? City { get; set; }

        [Range(0, 50000)]
        public double EntryFee { get; set; }

        [Url]
        public string? MapUrl { get; set; }

        public string? ImagePath { get; set; }

        public bool IsActive { get; set; }
    }
}
