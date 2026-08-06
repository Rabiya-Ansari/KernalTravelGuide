using System.ComponentModel.DataAnnotations;

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
    }
}
