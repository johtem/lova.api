using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LOVA.API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LOVA.API.Pages
{
    public class IndexModel : PageModel
    {

        public IndexModel(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            UserMgr = userManager;
            SignInMgr = signInManager;
        }

        public UserManager<ApplicationUser> UserMgr { get; }
        public SignInManager<ApplicationUser> SignInMgr { get; }

        public void OnGet()
        {

        }
    }
}