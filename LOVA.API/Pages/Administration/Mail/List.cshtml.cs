using LOVA.API.Models;
using LOVA.API.Services;
using LOVA.API.ViewModels.Admin;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LOVA.API.Pages.Administration.Mail
{
    public class ListModel : PageModel
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly LovaDbContext _context;

        public ListModel(UserManager<ApplicationUser> userManager, LovaDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        [BindProperty]
        public IList<MailSubscriptionViewModel> MailSubscriptions { get; set; }

        public async Task OnGetAsync()
        {
            var user = await GetUser();

            MailSubscriptions = new List<MailSubscriptionViewModel>();

            var s = from mailTypes in _context.MailTypes
                    join mailSubscriptions in _context.MailSubscriptions on mailTypes.Id equals mailSubscriptions.MailTypeId into grouping
                    from mailSubscriptions in grouping.DefaultIfEmpty()
                    where mailSubscriptions.Email == user.UserName || mailSubscriptions.Email == null
                    select new { mailTypes, mailSubscriptions };



            foreach(var item in s)
            {

                var apa = new MailSubscriptionViewModel
                {
                    MailSubscriptionId = item.mailSubscriptions == null ? 0 : item.mailSubscriptions.Id,
                    MailTypeId = item.mailTypes.Id,
                    Email = item.mailSubscriptions == null ? "" : item.mailSubscriptions.Email,
                    MailType = item.mailTypes.Type,
                    IsScription = item.mailSubscriptions == null ? false : item.mailSubscriptions.IsScription,
                };
                
               MailSubscriptions.Add(apa);

            }

 
        }


        public async Task<ApplicationUser> GetUser()
        {
            return await _userManager.GetUserAsync(HttpContext.User);
        }

        public async Task<IActionResult> OnPostAsync()
        {

            var user = await GetUser();

            foreach (var item in MailSubscriptions)
            {
                if (item.MailSubscriptionId == 0)
                {

                    var ms = new MailSubscription
                    {
                        Email = user.UserName,
                        MailTypeId = item.MailTypeId,
                        Name = ""
                    };

                    await _context.AddAsync(ms);

                    await _context.SaveChangesAsync();

                }
                else
                {
                    var ms = new MailSubscription
                    {
                        Id = item.MailSubscriptionId,
                        Email = user.UserName,
                        MailTypeId = item.MailTypeId,
                        Name = "",
                        IsScription = item.IsScription
                    };

                    _context.Attach(ms).State = EntityState.Modified;

                    await _context.SaveChangesAsync();


                }
            };

            return RedirectToPage();
        }
    }
}
