using System.ComponentModel.DataAnnotations;

namespace KernalTravelGuide.Models
{
    public class Country
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        [RegularExpression(@"^[A-Za-z ]+$")]
        public string Name { get; set; } = string.Empty;

        public ICollection<City>? Cities { get; set; }
    }
}
