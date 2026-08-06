using System.ComponentModel.DataAnnotations;

namespace KernalTravelGuide.Models
{
    public class TourPackage
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string PackageName { get; set; } = string.Empty;

        [Required]
        [Range(1, 30)]
        public int DurationDays { get; set; }

        [Range(1000, 500000)]
        public double Price { get; set; }

        [StringLength(1000)]
        public string Description { get; set; } = string.Empty;

        public string? ImagePath { get; set; }

        public bool IsAvailable { get; set; }
    }
}
