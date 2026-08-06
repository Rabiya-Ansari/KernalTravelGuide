using KernalTravelGuide.Data;
using Microsoft.AspNetCore.Identity;

namespace KernalTravelGuide.Models
{
    public static class SeedAdmin
    {
        public static async Task SeedAdminAsync(UserManager<ApplicationUser> userManager)
        {
            string email = "admin@karnel.com";

            if (await userManager.FindByEmailAsync(email) == null)
            {
                var user = new ApplicationUser
                {
                    FirstName = "System",
                    LastName = "Admin",
                    UserName = email,
                    Email = email,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(user, "Admin@123");

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "Admin");
                }
            }
        }
    }
}