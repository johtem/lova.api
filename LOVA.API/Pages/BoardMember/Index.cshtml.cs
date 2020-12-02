using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using LOVA.API.Models;
using LOVA.API.Services;

namespace LOVA.API.Pages.BoardMember
{
    public class IndexModel : PageModel
    {
        private readonly LOVA.API.Services.LovaDbContext _context;

        public IndexModel(LOVA.API.Services.LovaDbContext context)
        {
            _context = context;
        }

        public IList<Notification> Notification { get;set; }

        public async Task OnGetAsync()
        {
            Notification = await _context.Notifications.ToListAsync();
        }
    }
}
