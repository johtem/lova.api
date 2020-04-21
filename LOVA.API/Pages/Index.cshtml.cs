using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LOVA.API.Pages
{
    public class IndexModel : PageModel
    {

        public IndexModel(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            UserMgr = userManager;
            SignInMgr = signInManager;
        }

        public UserManager<IdentityUser> UserMgr { get; }
        public SignInManager<IdentityUser> SignInMgr { get; }

        public void OnGet()
        {

        }
    }
}