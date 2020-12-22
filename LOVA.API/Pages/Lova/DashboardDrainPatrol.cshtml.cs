using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kendo.Mvc.Extensions;
using LOVA.API.Models;
using LOVA.API.Services;
using LOVA.API.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace LOVA.API.Pages.Lova
{
    [Authorize(Roles = "Lova, Admin, Styrelse, VA")]
    public class DashboardDrainPatrolModel : PageModel
    {
        private readonly LovaDbContext _context;

        public DashboardDrainPatrolModel(LovaDbContext context)
        {
            _context = context;
        }

        public IEnumerable<WellsDashboardViewModel> NumberOfActivities { get; set; }
        public IEnumerable<WellsDashboardViewModel> NumberOfActivitiesFull { get; set; }

        public IEnumerable<DrainPatrolAlarm> Alarms { get; set; }

        public int TotalNumberOfActivitiesLast24H { get; set; }
        public int TotalNumberOfDrainingLast24H { get; set; }

        public int DashboardItemSize { get; set; } = MyConsts.DashboardItemSize;


        public async Task OnGet()
        {


            var totalNumberOfActivitiesLast24H = _context.ActivityPerRows.Where(a => a.TimeUp >= DateTime.Now.AddDays(-1));

           // TotalNumberOfActivitiesLast24H = totalNumberOfActivitiesLast24H.Count();

            TotalNumberOfDrainingLast24H = totalNumberOfActivitiesLast24H.Where(a => !EF.Functions.Like(a.Address, "%7") && !EF.Functions.Like(a.Address, "%8")).Count();

            var allNumberOfActivities = await _context.Activities
                          .Where(a => a.Active == true)
                          .GroupBy(x => new { x.Time.Date, x.Address })
                          .OrderByDescending(g => g.Count())
                          .Select(x => new WellsDashboardViewModel
                          {
                              Count = x.Count(),
                              Date = x.Key.Date,
                              Address = x.Key.Address
                          })
                          .ToListAsync();


            NumberOfActivities = allNumberOfActivities.Where(a => !EF.Functions.Like(a.Address, "%7")).Take(MyConsts.DashboardItemSize);

            NumberOfActivitiesFull = allNumberOfActivities.Where(a => EF.Functions.Like(a.Address, "%8")).Take(MyConsts.DashboardItemSize);

            Alarms = await _context.DrainPatrolAlarms.OrderByDescending(a => a.TimeStamp).Take(MyConsts.DashboardItemSize).ToListAsync();
        }
    }
}
