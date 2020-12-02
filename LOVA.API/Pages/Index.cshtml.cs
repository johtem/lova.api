using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LOVA.API.Models;
using LOVA.API.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace LOVA.API.Pages
{
    public class IndexModel : PageModel
    {

        private readonly LovaDbContext _context;

        public IndexModel(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, LovaDbContext context)
        {
            UserMgr = userManager;
            SignInMgr = signInManager;
            _context = context;
        }

        public UserManager<ApplicationUser> UserMgr { get; }
        public SignInManager<ApplicationUser> SignInMgr { get; }

        public IEnumerable<Notification> Notifications { get; set; }

        public async Task OnGetAsync()
        {
            DateTime now = DateTime.Now;

            Notifications = await _context.Notifications.Where(a => a.FromDate <= now && now <= a.ToDate).ToListAsync();


        }
    }
}