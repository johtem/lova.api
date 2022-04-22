using LOVA.API.Models;
using LOVA.API.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LOVA.API.Pages.BoardMember.Members
{
    public class MemberLoginResetModel : PageModel
    {
        private readonly LOVA.API.Services.LovaDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public MemberLoginResetModel(LovaDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IEnumerable<ApplicationUser> Users { get; set; }

        [BindProperty]
        public string LoginNameId { get; set; }

        [TempData]
        public string StatusMessage { get; set; }



        public IActionResult OnGet()
        {
            StatusMessage = "";

            Users = _userManager.Users;

            ViewData["Users"] = new SelectList(Users.OrderBy(a => a.UserName), "Id", "UserName");
            return Page();
        }


        public async Task<IActionResult> OnPost()
        {

            var user = await _userManager.Users.Where(a => a.Id == LoginNameId).FirstOrDefaultAsync();

            var property = await _context.Premises.Where(a => a.Property == user.UserName).FirstOrDefaultAsync();

            var first = property.Address[..4];
            var split = property.Address.Split(" ");

            string newPassword;



            if (user.UserName.Contains("@"))
            {
                newPassword = "Pass#123";
            }
            else
            {
                if (split[1].Length == 1)
                {
                    newPassword = first + "0" + split[1];
                }
                else
                {
                    newPassword = first + split[1];
                }
            }



            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            var addPasswordResult = await _userManager.ResetPasswordAsync(user, code, newPassword);


            if (!addPasswordResult.Succeeded)
            {
                foreach (var error in addPasswordResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return Page();
            }


            StatusMessage = $"Lösenordet är återställt för {property.Property}";


            Users = _userManager.Users;

            ViewData["Users"] = new SelectList(Users.OrderBy(a => a.UserName), "Id", "UserName");
            return Page();
        }


    }
}
