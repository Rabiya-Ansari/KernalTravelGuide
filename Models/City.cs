using System.ComponentModel.DataAnnotations;

namespace KernalTravelGuide.Models
{
    public class City
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        [RegularExpression(@"^[A-Za-z ]+$")]
        public string Name { get; set; } = string.Empty;

        public int CountryId { get; set; }

        public Country? Country { get; set; }
    }
}
