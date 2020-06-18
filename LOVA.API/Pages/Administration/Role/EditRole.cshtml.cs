using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using LOVA.API.Models;
using LOVA.API.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace LOVA.API.Pages.Administration.Role
{
    
    public class EditRoleModel : PageModel
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public EditRoleModel(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        [BindProperty]
        public EditRoleViewModel editModel { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);

            if (role == null)
            {
                ViewData["ErrorMessage"] = $"Role with Id = {id} cannot be found";
                
                RedirectToPage("NotFound");
            }

            editModel = new EditRoleViewModel
            {
                Id = role.Id,
                RoleName = role.Name
            };

            var users = await _userManager.Users.ToListAsync();

            foreach (var user in users)
            {
                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    editModel.Users.Add(user.UserName);
                }
            }

            return Page();
            
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var role = await _roleManager.FindByIdAsync(editModel.Id);

            if (role == null)
            {
                ViewData["ErrorMessage"] = $"Role with Id = {editModel.Id} cannot be found";

                RedirectToPage("NotFound");
            }
            else
            {
                role.Name = editModel.RoleName;
                var result = await _roleManager.UpdateAsync(role);

                if (result.Succeeded)
                {
                    RedirectToPage("ListRoles");
                }

                foreach(var error in result.Errors )
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return Page();
        }
    }
}