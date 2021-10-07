using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using LOVA.API.Models;
using LOVA.API.Services;

namespace LOVA.API.Pages.Lova.Activity
{
    public class CreateModel : PageModel
    {
        private readonly LOVA.API.Services.LovaDbContext _context;

        public CreateModel(LOVA.API.Services.LovaDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public LovaIssue LovaIssue { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            LovaIssue.CreatedAt = DateTime.Now;
            LovaIssue.UpdatedAt = DateTime.Now;


            _context.LovaIssues.Add(LovaIssue);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
