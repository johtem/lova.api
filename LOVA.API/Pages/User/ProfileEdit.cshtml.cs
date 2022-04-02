using LOVA.API.Models;
using LOVA.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace LOVA.API.Pages.User
{
    [Authorize(Roles = "User, Admin, Styrelse")]
    public class ProfileEditModel : PageModel
    {
        private readonly LovaDbContext _context;

        public ProfileEditModel(LovaDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public PremiseContact Contact { get; set; }

        public async Task<IActionResult> OnGetAsync(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Contact = await _context.PremiseContacts.FirstOrDefaultAsync(m => m.Id == id);

            if (Contact == null)
            {
                return NotFound();
            }
            return Page();
        }



        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Contact.UpdatedAt = System.DateTime.Now;

            _context.Attach(Contact).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ContactExists(Contact.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./ProfileIndex");
        }

        private bool ContactExists(long id)
        {
            return _context.PremiseContacts.Any(e => e.Id == id);
        }
    }
}
