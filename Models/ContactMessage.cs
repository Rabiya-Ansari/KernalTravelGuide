using System.ComponentModel.DataAnnotations;

namespace KernalTravelGuide.Models
{
    public class ContactMessage
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        [RegularExpression(@"^[A-Za-z ]+$")]
        public string Name { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [RegularExpression(@"^03\d{9}$",
            ErrorMessage = "Enter valid Pakistani mobile number.")]
        public string Phone { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Subject { get; set; } = string.Empty;

        [Required]
        [StringLength(1000)]
        public string Message { get; set; } = string.Empty;

        public DateTime SentOn { get; set; } = DateTime.Now;
    }
}
