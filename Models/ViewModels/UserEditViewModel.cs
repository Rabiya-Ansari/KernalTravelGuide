using System.ComponentModel.DataAnnotations;

namespace KernalTravelGuide.Models.ViewModels
{
    // View model used by Admin to edit a user's profile and role.
    public class UserEditViewModel
    {
        // Identity user ID.
        [Required]
        public string Id { get; set; } = string.Empty;

        // User first name.
        [Required]
        [StringLength(50)]
        [RegularExpression(
            @"^[A-Za-z ]+$",
            ErrorMessage = "Only alphabets and spaces are allowed.")]
        public string FirstName { get; set; } = string.Empty;

        // User last name.
        [Required]
        [StringLength(50)]
        [RegularExpression(
            @"^[A-Za-z ]+$",
            ErrorMessage = "Only alphabets and spaces are allowed.")]
        public string LastName { get; set; } = string.Empty;

        // User email.
        [Required]
        [EmailAddress]
        [StringLength(256)]
        public string Email { get; set; } = string.Empty;

        // Optional phone number.
        [Phone]
        [StringLength(20)]
        public string? PhoneNumber { get; set; }

        // Optional address.
        [StringLength(250)]
        public string? Address { get; set; }

        // Admin can assign Admin or Customer.
        [Required]
        public string Role { get; set; } = "Customer";
    }
}