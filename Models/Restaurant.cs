using System.ComponentModel.DataAnnotations;

namespace KernalTravelGuide.Models
{
    public class Restaurant
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        public int CityId { get; set; }

        public City? City { get; set; }

        [Range(1, 5)]
        public double Rating { get; set; }

        [Phone]
        public string? Phone { get; set; }

        public string? ImagePath { get; set; }
    }
}
