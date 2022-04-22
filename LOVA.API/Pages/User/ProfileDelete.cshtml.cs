using LOVA.API.Models;
using LOVA.API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace LOVA.API.Pages.User
{
    public class ProfileDeleteModel : PageModel
    {
        private readonly LovaDbContext _context;

        public ProfileDeleteModel(LovaDbContext context)
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

        public async Task<IActionResult> OnPostAsync(long? id)
        {

            var member = await _context.PremiseContacts.FindAsync(id);

            if (member != null)
            {
                member.IsDeleted = true;

                _context.Attach(member).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }



            return RedirectToPage("./ProfileIndex");
        }
    }
}
