using System.ComponentModel.DataAnnotations;

namespace KernalTravelGuide.Models
{
    public class Hotel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        public int CityId { get; set; }

        public City? City { get; set; }

        [Range(500, 100000)]
        public double PricePerNight { get; set; }

        [Range(1, 5)]
        public int StarRating { get; set; }

        [Range(0, 1000)]
        public int AvailableRooms { get; set; }

        [Phone]
        public string? ContactNo { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        [Url]
        public string? Website { get; set; }

        public string? ImagePath { get; set; }

        public bool Availability { get; set; }
    }
}