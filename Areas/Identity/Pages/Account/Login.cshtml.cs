// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

using KernalTravelGuide.Data;

namespace KernalTravelGuide.Areas.Identity.Pages.Account;

public class LoginModel : PageModel
{
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILogger<LoginModel> _logger;

    public LoginModel(
        SignInManager<ApplicationUser> signInManager,
        UserManager<ApplicationUser> userManager,
        ILogger<LoginModel> logger)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _logger = logger;
    }

    [BindProperty]
    public InputModel Input { get; set; } = default!;

    public IList<AuthenticationScheme>? ExternalLogins { get; set; }

    public string? ReturnUrl { get; set; }

    [TempData]
    public string? ErrorMessage { get; set; }

    public class InputModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = default!;

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = default!;

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }


    // =========================================================
    // GET: Login
    // =========================================================
    public async Task<IActionResult> OnGetAsync(string? returnUrl = null)
    {
        /*
         * IMPORTANT:
         * If the user is already logged in,
         * do NOT show the Login page again.
         */

        if (User?.Identity?.IsAuthenticated == true)
        {
            var currentUser = await _userManager.GetUserAsync(User);

            if (currentUser != null)
            {
                // Admin → Admin Dashboard
                if (await _userManager.IsInRoleAsync(
                    currentUser,
                    "Admin"))
                {
                    return RedirectToAction(
                        "Index",
                        "Admin");
                }

                // Customer → Customer Dashboard
                if (await _userManager.IsInRoleAsync(
                    currentUser,
                    "Customer"))
                {
                    return RedirectToAction(
                        "Index",
                        "Dashboard");
                }

                /*
                 * If the account is authenticated but
                 * currently has no role, still don't send
                 * the user back to Login.
                 */
                return RedirectToAction(
                    "Index",
                    "Dashboard");
            }

            /*
             * Authentication cookie exists but user
             * cannot be found in database.
             * Sign out and allow normal login.
             */
            await _signInManager.SignOutAsync();
        }


        // Display previous error message if available.
        if (!string.IsNullOrEmpty(ErrorMessage))
        {
            ModelState.AddModelError(
                string.Empty,
                ErrorMessage);
        }

        returnUrl ??= Url.Content("~/");

        /*
         * Clear external authentication cookie
         * to ensure a clean login process.
         */
        await HttpContext.SignOutAsync(
            IdentityConstants.ExternalScheme);

        ExternalLogins =
            (await _signInManager
                .GetExternalAuthenticationSchemesAsync())
                .ToList();

        ReturnUrl = returnUrl;

        return Page();
    }


   
    // POST: Login
    
    public async Task<IActionResult> OnPostAsync(
        string? returnUrl = null)
    {
        returnUrl ??= Url.Content("~/");

        ExternalLogins =
            (await _signInManager
                .GetExternalAuthenticationSchemesAsync())
                .ToList();

        if (ModelState.IsValid)
        {
            /*
             * Authenticate the user using ASP.NET Core Identity.
             */
            var result =
                await _signInManager.PasswordSignInAsync(
                    Input.Email,
                    Input.Password,
                    Input.RememberMe,
                    lockoutOnFailure: false);


           
            // LOGIN SUCCESSFUL
           
            if (result.Succeeded)
            {
                _logger.LogInformation(
                    "User logged in.");

                /*
                 * Get the logged-in user's complete
                 * Identity record.
                 */
                var user =
                    await _userManager.FindByEmailAsync(
                        Input.Email);

                if (user == null)
                {
                    await _signInManager.SignOutAsync();

                    ModelState.AddModelError(
                        string.Empty,
                        "Unable to find the user account.");

                    return Page();
                }


               
                // ADMIN
                
                if (await _userManager.IsInRoleAsync(
                    user,
                    "Admin"))
                {
                    return RedirectToAction(
                        "Index",
                        "Admin");
                }


                
                // CUSTOMER
               
                if (await _userManager.IsInRoleAsync(
                    user,
                    "Customer"))
                {
                    return RedirectToAction(
                        "Index",
                        "Dashboard");
                }


               
                return RedirectToAction(
                    "Index",
                    "Dashboard");
            }


           
            // TWO FACTOR AUTHENTICATION
          
            if (result.RequiresTwoFactor)
            {
                return RedirectToPage(
                    "./LoginWith2fa",
                    new
                    {
                        ReturnUrl = returnUrl,
                        RememberMe = Input.RememberMe
                    });
            }


            // =================================================
            // ACCOUNT LOCKED
            // =================================================
            if (result.IsLockedOut)
            {
                _logger.LogWarning(
                    "User account locked out.");

                return RedirectToPage(
                    "./Lockout");
            }


            
            // INVALID LOGIN
            
            ModelState.AddModelError(
                string.Empty,
                "Invalid login attempt.");

            return Page();
        }


        // Validation failed.
        return Page();
    }
}