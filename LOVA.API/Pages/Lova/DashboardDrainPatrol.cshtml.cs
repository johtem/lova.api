using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;
using Kendo.Mvc.Extensions;
using LOVA.API.Models;
using LOVA.API.Services;
using LOVA.API.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace LOVA.API.Pages.Lova
{
    [Authorize(Roles = "Lova, Admin, Styrelse, VA")]
    public class DashboardDrainPatrolModel : PageModel
    {
        private readonly LovaDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public DashboardDrainPatrolModel(LovaDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IEnumerable<WellsDashboardViewModel> NumberOfActivities { get; set; }
        public IEnumerable<WellsDashboardViewModel> NumberOfActivitiesFull { get; set; }

        public IEnumerable<DrainPatrolAlarm> Alarms { get; set; }

        public int TotalNumberOfActivitiesLast24H { get; set; }
        public int TotalNumberOfDrainingLast24H { get; set; }

        public int DashboardItemSize { get; set; } = MyConsts.DashboardItemSize;


        public async Task OnGet()
        {
            var usr = await _userManager.GetUserAsync(HttpContext.User);

            var user = await _userManager.FindByIdAsync(usr.Id);

            

            var totalNumberOfActivitiesLast24H = _context.ActivityPerRows.Where(a => a.TimeUp >= DateTime.Now.AddDays(-1));

            TotalNumberOfActivitiesLast24H = totalNumberOfActivitiesLast24H.Count();

            TotalNumberOfDrainingLast24H = totalNumberOfActivitiesLast24H.Where(a => a.IsGroupAddress == false).Count();

            var allNumberOfActivities = await _context.ActivityPerRows  
                          .Where(a => a.TimeUp >= DateTime.Now.AddDays(-6))
                          .GroupBy(x => new { x.TimeUp.Date, x.Address })
                          .OrderByDescending(g => g.Count())
                          .Select(x => new WellsDashboardViewModel
                          {
                              Count = x.Count(),
                              Date = x.Key.Date,
                              Address = x.Key.Address,
                          })
                          .ToListAsync();


            //NumberOfActivities = allNumberOfActivities.Where(a => !EF.Functions.Like(a.Address, "%7")).Take(MyConsts.DashboardItemSize);
            NumberOfActivities = allNumberOfActivities.Where(a => !a.Address.Contains("7") && !a.Address.Contains("8")).Take(MyConsts.DashboardItemSize);

            NumberOfActivitiesFull = allNumberOfActivities.Where(a => a.Address.Contains("8")).Take(MyConsts.DashboardItemSize);

            Alarms = await _context.DrainPatrolAlarms.OrderByDescending(a => a.TimeStamp).Take(MyConsts.DashboardItemSize).ToListAsync();


            

           BackgroundJob.Enqueue<IEmailService>(x => x.SendEmailAsync(new MailRequest { ToEmail = "johan@tempelman.nu", Subject = "L�va Dashboard", Body = $"{user.FullName} is looking at the L�va dashboard" }));
             
        }
    }
}
