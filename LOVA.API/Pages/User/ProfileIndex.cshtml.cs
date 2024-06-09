using LOVA.API.Models;
using LOVA.API.Services;
using LOVA.API.ViewModels.AddressList;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LOVA.API.Pages.User
{
    [Authorize(Roles = "User, Admin, Styrelse")]
    public class ProfileIndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly LovaDbContext _context;

        public ProfileIndexModel(UserManager<ApplicationUser> userManager, LovaDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }


        public string Username { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public IList<ProfileEditViewModel>  Profiles { get; set; }

        public async Task<IActionResult> OnGet()           
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();

        }

        private async Task LoadAsync(ApplicationUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            Profiles = await _context.PremiseContacts
                .Include(a => a.Premise)
                .ThenInclude(b => b.Well)
                .Where(a => a.Premise.Property == user.Property && a.IsDeleted == false)
                .Select(s => new ProfileEditViewModel
                {
                    Id = s.Id,
                    FirstName = s.FirstName,
                    LastName = s.LastName,
                    Email = s.Email,
                    MobileNumber = s.MobileNumber,
                    PhoneNumber = s.PhoneNumber,
                    PremiseId = s.PremiseId,
                    WellName = s.Premise.Well.WellName,
                    Address = s.Premise.Address,
                    Property = s.Premise.Property,
                    WantInfoEmail = s.WantInfoEmail,
                    WantInfoSMS = s.WantInfoSMS,
                    WantGrannsamverkanEmail = s.WantGrannsamverkanEmail,
                    IsActive = s.IsActive
                })
                .ToListAsync();


            if (Profiles.Count == 0)
            {
                Profiles = await _context.Premises
               .Include(b => b.Well)
               .Where(a => a.Property == user.Property)
               .Select(s => new ProfileEditViewModel
               {
                   Id = s.Id,
                   FirstName = "",
                   LastName = "",
                   Email = "",
                   MobileNumber = "",
                   PhoneNumber = "",
                   PremiseId = s.Id,
                   WellName = s.Well.WellName,
                   Address = s.Address,
                   Property = s.Property,
                   WantInfoEmail = true,
                   WantInfoSMS =true,
                   WantGrannsamverkanEmail = true,
                   IsActive = true
               })
               .ToListAsync();
            }

            // var wellObj = await _context.Premises.Include(a => a.Well).Where(a => a.Property == user.Property).FirstOrDefaultAsync();


            Username = userName;


        }
    }
}
