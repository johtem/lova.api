using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure.Storage.Blobs.Models;
using LOVA.API.Models;
using LOVA.API.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace LOVA.API.Pages.Administration.Role
{
    public class EditUserInRoleModel : PageModel
    {

        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public EditUserInRoleModel(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        [BindProperty]
        public List<UserRoleViewModel> userRoleVM { get; set; } = new List<UserRoleViewModel>();

        public async Task<IActionResult> OnGetAsync(string id)
        {
            ViewData["roleId"] = id;

            var role = await _roleManager.FindByIdAsync(id);

            if (role == null)
            {
                ViewData["ErrorMessage"] = $"Role with Id = {id} cannot be found";
                RedirectToPage("NotFound");
            }

            var users = await _userManager.Users.ToListAsync();

            foreach (var user in users)
            {
                var userRoleViewModel = new UserRoleViewModel
                {
                    UserId = user.Id,
                    UserName = user.UserName,

                };

                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    userRoleViewModel.IsSelected = true;
                }
                else
                {
                    userRoleViewModel.IsSelected = false;
                }

                userRoleVM.Add(userRoleViewModel);


            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {

            var apa = userRoleVM;

            var role = await _roleManager.FindByIdAsync(id);

            if (role == null)
            {
                ViewData["ErrorMessage"] = $"Role with ID = {id} cannot be found";
                RedirectToPage("/Administration/Role/NotFound");
            }

            for (int i = 0; i < userRoleVM.Count; i++)
            {
               var user = await _userManager.FindByIdAsync(userRoleVM[i].UserId);

                IdentityResult result = null;

                if (userRoleVM[i].IsSelected && !(await _userManager.IsInRoleAsync(user, role.Name)))
                {
                    result = await _userManager.AddToRoleAsync(user, role.Name);
                }
                else if (!userRoleVM[i].IsSelected && await _userManager.IsInRoleAsync(user, role.Name))
                {
                    result = await _userManager.RemoveFromRoleAsync(user, role.Name);
                }
                else
                {
                    continue;
                }

                if (result.Succeeded)
                {
                    if (i < (userRoleVM.Count - 1))
                    {
                        continue;
                    }
                    else
                    {
                        return RedirectToPage("/Administration/Role/EditRole", new { id = id });
                    }
                }
            }

            return RedirectToPage("/Administration/Role/EditRole", new { id = id });
            
        }
    }
}