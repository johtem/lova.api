using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LOVA.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LOVA.API.Pages.Administration.Role
{
    [Authorize(Roles = "Admin")]
    public class ListUsersModel : PageModel
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public ListUsersModel(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public IEnumerable<ApplicationUser> Users { get; set; } 

        public void OnGet()
        {
            Users = _userManager.Users;

        }
    }
}
