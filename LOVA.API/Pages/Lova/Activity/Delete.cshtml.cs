using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using LOVA.API.Models;
using LOVA.API.Services;

namespace LOVA.API.Pages.Lova.Activity
{
    public class DeleteModel : PageModel
    {
        private readonly LOVA.API.Services.LovaDbContext _context;

        public DeleteModel(LOVA.API.Services.LovaDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public LovaIssue LovaIssue { get; set; }

        public async Task<IActionResult> OnGetAsync(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            LovaIssue = await _context.LovaIssues.FirstOrDefaultAsync(m => m.Id == id);

            if (LovaIssue == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            LovaIssue = await _context.LovaIssues.FindAsync(id);

            if (LovaIssue != null)
            {
                _context.LovaIssues.Remove(LovaIssue);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
