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

        public async Task OnGetAsync()
        {
            LovaIssue = await _context.LovaIssues.ToListAsync();
        }
    }
}
