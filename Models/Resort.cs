using System.ComponentModel.DataAnnotations;

namespace KernalTravelGuide.Models
{
    public class Resort
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        public int CityId { get; set; }

        public City? City { get; set; }

        [Range(1000, 200000)]
        public double Price { get; set; }

        [Range(1, 5)]
        public int Rating { get; set; }

        public bool Availability { get; set; }

        public string? ImagePath { get; set; }
    }
}
