namespace KernalTravelGuide.Models.ViewModels
{
    // View model used to display users with their Identity role.
    public class UserListItemViewModel
    {
        // Identity user ID.
        public string Id { get; set; } = string.Empty;

        // User's full name.
        public string FullName { get; set; } = string.Empty;

        // User email.
        public string? Email { get; set; }

        // User phone.
        public string? PhoneNumber { get; set; }

        // Identity username.
        public string? UserName { get; set; }

        // Assigned Identity role.
        public string Role { get; set; } = "Customer";
    }
}
