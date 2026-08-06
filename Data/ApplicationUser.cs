using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace KernalTravelGuide.Data
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [StringLength(50)]
        [RegularExpression(@"^[A-Za-z ]+$",
            ErrorMessage = "Only alphabets are allowed.")]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        [RegularExpression(@"^[A-Za-z ]+$")]
        public string LastName { get; set; } = string.Empty;

        [StringLength(250)]
        public string? Address { get; set; }

        public DateTime CreatedOn { get; set; } = DateTime.Now;
    }
}