using LOVA.API.Models;
using LOVA.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace LOVA.API.Pages.User
{
    [Authorize(Roles = "User, Admin, Styrelse")]
    public class ProfileNewModel : PageModel
    {
        private readonly LovaDbContext _context;

        public ProfileNewModel(LovaDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public PremiseContact Contact { get; set; } = new PremiseContact();

        public void OnGet(long id)
        {
            Contact.PremiseId = id;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Contact.IsActive = true;
            Contact.IsDeleted = false;
            Contact.CreatedAt = System.DateTime.Now;
            Contact.UpdatedAt = System.DateTime.Now;

            _context.PremiseContacts.Add(Contact);
            await _context.SaveChangesAsync();

            return RedirectToPage("./ProfileIndex");
        }
    }
}
