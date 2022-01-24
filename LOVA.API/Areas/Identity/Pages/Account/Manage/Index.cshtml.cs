using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using LOVA.API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace LOVA.API.Areas.Identity.Pages.Account.Manage
{
    public partial class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly LOVA.API.Services.LovaDbContext _context;

        public IndexModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            LOVA.API.Services.LovaDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        public string Username { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }

            public string Property { get; set; }
            public string WellName { get; set; }

            public string ForeName { get; set; }
            public string LastName { get; set; }

            public string ForeName2 { get; set; }
            public string LastName2 { get; set; }

            public string Email { get; set; }
            public string Email2 { get; set; }

            public string Phone { get; set; }
            public string Phone2 { get; set; }
        }

        private async Task LoadAsync(ApplicationUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            var wellObj = await _context.Premises.Include(a => a.Well).Where(a => a.Property == user.Property).FirstOrDefaultAsync();
            

            Username = userName;

            Input = new InputModel
            {
                PhoneNumber = phoneNumber,
                WellName = wellObj.Well.WellName,
                Property = user.Property,
                ForeName = user.ForeName,
                LastName = user.LastName,
                Email = user.Email,
                ForeName2 = user.ForeName2,
                LastName2 = user.LastName2,
                Email2 = user.Email2,
                Phone2 = user.Phone2
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    var userId = await _userManager.GetUserIdAsync(user);
                    throw new InvalidOperationException($"Unexpected error occurred setting phone number for user with ID '{userId}'.");
                }
            }


            if (user.Property != Input.Property)
            {
                user.Property = Input.Property;

                await _userManager.UpdateAsync(user);
            }

            if (user.ForeName != Input.ForeName)
            {
                user.ForeName = Input.ForeName;

                await _userManager.UpdateAsync(user);
            }

            if (user.LastName != Input.LastName)
            {
                user.LastName = Input.LastName;

                await _userManager.UpdateAsync(user);
            }

            if (user.ForeName2 != Input.ForeName2)
            {
                user.ForeName2 = Input.ForeName2;

                await _userManager.UpdateAsync(user);
            }

            if (user.LastName2 != Input.LastName2)
            {
                user.LastName2 = Input.LastName2;

                await _userManager.UpdateAsync(user);
            }

            if (user.Email != Input.Email)
            {
                user.Email = Input.Email;

                await _userManager.UpdateAsync(user);
            }

            if (user.Email2 != Input.Email2)
            {
                user.Email2 = Input.Email2;

                await _userManager.UpdateAsync(user);
            }

            if (user.Phone2 != Input.Phone2)
            {
                user.Phone2 = Input.Phone2;

                await _userManager.UpdateAsync(user);
            }



            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Ditt konto är nu uppdaterat!";
            return RedirectToPage();
        }
    }
}
