using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LOVA.API.Models;
using LOVA.API.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LOVA.API.Pages.Administration.Role
{
    public class EditUserModel : PageModel
    {

        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public EditUserModel(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        [BindProperty]
        public EditUserViewModel UserViewModel { get; set; }

        public async Task OnGet(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (User == null)
            {
                ViewData["ErrorMessage"] = $"User with Id = {id} cannot be found";

                RedirectToPage("NotFound");
            }

            // GetClaimsAsync retunrs the list of user Claims
            var userClaims = await _userManager.GetClaimsAsync(user);

            // GetRolesAsync returns the list of user Roles
            var userRoles = await _userManager.GetRolesAsync(user);

            UserViewModel = new EditUserViewModel
            {
                Id = user.Id,
                Email = user.Email,
                UserName = user.UserName,
                ForeName = user.ForeName,
                LastName = user.LastName,
                ForeName2 = user.ForeName2,
                LastName2 = user.LastName2,
                Property = user.Property,

                //City = user.City,
                Claims = userClaims.Select(c => c.Value).ToList(),
                Roles = userRoles
            };


        }


        public async Task<IActionResult> OnPostAsync(string id)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                ViewData["ErrorMessage"] = $"User with Id = {UserViewModel.Id} cannot be found";

                RedirectToPage("NotFound");
            }
            else
            {
                
                
                
                user.UserName = UserViewModel.UserName;
                user.Email = UserViewModel.Email;
                user.ForeName = UserViewModel.ForeName;
                user.LastName = UserViewModel.LastName;
                user.ForeName2 = UserViewModel.ForeName2;
                user.LastName2 = UserViewModel.LastName2;
                user.Property = UserViewModel.Property;
                
                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    return RedirectToPage("/Administration/Role/ListUsers");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return Page();
        }
    }
}
