using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LOVA.API.Pages.Administration.Role
{
    [Authorize(Policy = "RequireAdminRole")]
    public class ListRolesModel : PageModel
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public ListRolesModel(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        [BindProperty]
        public IEnumerable<IdentityRole> roles { get; set; }

        public void OnGet()
        {
            roles = _roleManager.Roles;
        }
    }
}