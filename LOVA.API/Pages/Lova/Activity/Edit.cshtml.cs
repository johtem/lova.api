using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LOVA.API.Models;
using LOVA.API.Services;

namespace LOVA.API.Pages.Lova.Activity
{
    public class EditModel : PageModel
    {
        private readonly LOVA.API.Services.LovaDbContext _context;

        public EditModel(LOVA.API.Services.LovaDbContext context)
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

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            LovaIssue.UpdatedAt = DateTime.Now;

            _context.Attach(LovaIssue).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LovaIssueExists(LovaIssue.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool LovaIssueExists(long id)
        {
            return _context.LovaIssues.Any(e => e.Id == id);
        }
    }
}
