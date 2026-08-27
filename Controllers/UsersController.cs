using KernalTravelGuide.Data;
using KernalTravelGuide.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace KernalTravelGuide.Controllers
{
   
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

       
        public UsersController(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
            var users = _userManager.Users
                .OrderBy(u => u.FirstName)
                .ThenBy(u => u.LastName)
                .ToList();

            var model = new List<UserListItemViewModel>();

            foreach (var user in users)
            {
               
                var roles = await _userManager.GetRolesAsync(user);

                model.Add(new UserListItemViewModel
                {
                    Id = user.Id,
                    FullName = $"{user.FirstName} {user.LastName}".Trim(),
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    UserName = user.UserName,
                    Role = roles.FirstOrDefault() ?? "No Role"
                });
            }

            return View(model);
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return NotFound();

            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
                return NotFound();

            // Get the user's current role.
            var roles = await _userManager.GetRolesAsync(user);

            ViewBag.Role = roles.FirstOrDefault() ?? "No Role";

            return View(user);
        }

        // GET: Users/Edit/5
       
        public async Task<IActionResult> Edit(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return NotFound();

            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
                return NotFound();

            var roles = await _userManager.GetRolesAsync(user);

            ViewBag.Roles = await GetAvailableRolesAsync();

            ViewBag.IsCurrentUser =
                user.Id == _userManager.GetUserId(User);

            var model = new UserEditViewModel
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email ?? string.Empty,
                PhoneNumber = user.PhoneNumber,
                Address = user.Address,
                Role = roles.FirstOrDefault() ?? "Customer"
            };

            return View(model);
        }

        // POST: Users/Edit/5
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UserEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Roles = await GetAvailableRolesAsync();
                ViewBag.IsCurrentUser =
                    model.Id == _userManager.GetUserId(User);

                return View(model);
            }

            var user = await _userManager.FindByIdAsync(model.Id);

            if (user == null)
                return NotFound();

            var existingUser =
                await _userManager.FindByEmailAsync(model.Email.Trim());

            if (existingUser != null && existingUser.Id != user.Id)
            {
                ModelState.AddModelError(
                    nameof(model.Email),
                    "This email address is already registered.");

                ViewBag.Roles = await GetAvailableRolesAsync();
                ViewBag.IsCurrentUser =
                    user.Id == _userManager.GetUserId(User);

                return View(model);
            }

            
            user.FirstName = model.FirstName.Trim();
            user.LastName = model.LastName.Trim();

            user.PhoneNumber =
                string.IsNullOrWhiteSpace(model.PhoneNumber)
                    ? null
                    : model.PhoneNumber.Trim();

            user.Address =
                string.IsNullOrWhiteSpace(model.Address)
                    ? null
                    : model.Address.Trim();

           
            var emailResult =
                await _userManager.SetEmailAsync(
                    user,
                    model.Email.Trim());

            if (!emailResult.Succeeded)
            {
                AddIdentityErrors(emailResult);

                ViewBag.Roles = await GetAvailableRolesAsync();
                ViewBag.IsCurrentUser =
                    user.Id == _userManager.GetUserId(User);

                return View(model);
            }

            // Keep username synchronized with email.
            var usernameResult =
                await _userManager.SetUserNameAsync(
                    user,
                    model.Email.Trim());

            if (!usernameResult.Succeeded)
            {
                AddIdentityErrors(usernameResult);

                ViewBag.Roles = await GetAvailableRolesAsync();
                ViewBag.IsCurrentUser =
                    user.Id == _userManager.GetUserId(User);

                return View(model);
            }

            
            var updateResult =
                await _userManager.UpdateAsync(user);

            if (!updateResult.Succeeded)
            {
                AddIdentityErrors(updateResult);

                ViewBag.Roles = await GetAvailableRolesAsync();
                ViewBag.IsCurrentUser =
                    user.Id == _userManager.GetUserId(User);

                return View(model);
            }

            var currentUserId =
                _userManager.GetUserId(User);

            
            if (user.Id != currentUserId)
            {
                
                if (!await _roleManager.RoleExistsAsync(model.Role))
                {
                    ModelState.AddModelError(
                        nameof(model.Role),
                        "Selected role does not exist.");

                    ViewBag.Roles =
                        await GetAvailableRolesAsync();

                    ViewBag.IsCurrentUser = false;

                    return View(model);
                }

                var currentRoles =
                    await _userManager.GetRolesAsync(user);

                
                if (currentRoles.Count > 0)
                {
                    var removeResult =
                        await _userManager.RemoveFromRolesAsync(
                            user,
                            currentRoles);

                    if (!removeResult.Succeeded)
                    {
                        AddIdentityErrors(removeResult);

                        ViewBag.Roles =
                            await GetAvailableRolesAsync();

                        ViewBag.IsCurrentUser = false;

                        return View(model);
                    }
                }

                
                var addRoleResult =
                    await _userManager.AddToRoleAsync(
                        user,
                        model.Role);

                if (!addRoleResult.Succeeded)
                {
                    AddIdentityErrors(addRoleResult);

                    ViewBag.Roles =
                        await GetAvailableRolesAsync();

                    ViewBag.IsCurrentUser = false;

                    return View(model);
                }
            }

            TempData["Success"] =
                "User updated successfully.";

            return RedirectToAction(nameof(Index));
        }

        // GET: Users/Delete/5
       
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return NotFound();

            var user =
                await _userManager.FindByIdAsync(id);

            if (user == null)
                return NotFound();

            var roles =
                await _userManager.GetRolesAsync(user);

            ViewBag.Role =
                roles.FirstOrDefault() ?? "No Role";

            return View(user);
        }

        // POST: Users/Delete/5
        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return NotFound();

            var user =
                await _userManager.FindByIdAsync(id);

            if (user == null)
                return NotFound();

            
            if (user.Id == _userManager.GetUserId(User))
            {
                TempData["Error"] =
                    "You cannot delete your own account.";

                return RedirectToAction(nameof(Index));
            }

            var result =
                await _userManager.DeleteAsync(user);

            if (!result.Succeeded)
            {
                AddIdentityErrors(result);

                var roles =
                    await _userManager.GetRolesAsync(user);

                ViewBag.Role =
                    roles.FirstOrDefault() ?? "No Role";

                return View("Delete", user);
            }

            TempData["Success"] =
                "User deleted successfully.";

            return RedirectToAction(nameof(Index));
        }

        // Return the roles created by SeedRoles.
        private async Task<List<string>> GetAvailableRolesAsync()
        {
            var roles = new List<string>();

            if (await _roleManager.RoleExistsAsync("Admin"))
                roles.Add("Admin");

            if (await _roleManager.RoleExistsAsync("Customer"))
                roles.Add("Customer");

            return roles;
        }

        private void AddIdentityErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(
                    string.Empty,
                    error.Description);
            }
        }
    }
}