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
    public class IndexModel : PageModel
    {
        private readonly LOVA.API.Services.LovaDbContext _context;

        public IndexModel(LOVA.API.Services.LovaDbContext context)
        {
            _context = context;
        }

        public IList<LovaIssue> LovaIssue { get;set; }

        [BindProperty(SupportsGet = true)]
        public bool ShowClosedActions { get; set; } = false;

        public async Task OnGetAsync()
        {
           
            var lovaIssue = _context.LovaIssues.OrderBy(a => a.UpdatedAt);
            if (!ShowClosedActions)
            {
                lovaIssue = (IOrderedQueryable<LovaIssue>)lovaIssue.Where(a => a.Status == Status.Påbörjad || a.Status == Status.Planerad).OrderBy(a => a.Status).ThenByDescending(a => a.UpdatedAt);
            }

            LovaIssue = await lovaIssue.ToListAsync();
        }
    }
}
