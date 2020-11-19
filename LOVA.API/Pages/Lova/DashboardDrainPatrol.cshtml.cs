using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kendo.Mvc.Extensions;
using LOVA.API.Models;
using LOVA.API.Services;
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

        public IEnumerable<WellsDashboardViewModel> NoActivities { get; set; }
        public IEnumerable<WellsDashboardViewModel> NumberOfActivities { get; set; }
        public IEnumerable<WellsDashboardViewModel> NumberOfActivitiesFull { get; set; }

        public int TotalNumberOfActivitiesLast24H { get; set; }


        public void OnGet()
        {
            NoActivities = from n in _context.DrainPatrols
                           where n.Active == true
                           group n by n.Address into g
                           select new WellsDashboardViewModel
                           {
                               Address = g.Key,
                               Date = g.Max(t => t.Time)
                           };

            NoActivities = NoActivities.OrderBy(n => n.Date).Take(5);


            TotalNumberOfActivitiesLast24H = _context.DrainPatrols.Where(a => a.Active == true && a.Time >= DateTime.Now.AddDays(-1)).Count();


            var allNumberOfActivities = _context.DrainPatrols
                          .Where(a => a.Active == true)
                          .GroupBy(x => new { x.Time.Date, x.Address })
                          .OrderByDescending(g => g.Count())
                          .Select(x => new WellsDashboardViewModel
                          {
                              Count = x.Count(),
                              Date = x.Key.Date,
                              Address = x.Key.Address
                          })
                          .ToList();


            NumberOfActivities = allNumberOfActivities.Where(a => !EF.Functions.Like(a.Address, "%7")).Take(5);

            NumberOfActivitiesFull = allNumberOfActivities.Where(a => EF.Functions.Like(a.Address, "%8")).Take(5);
        }
    }
}
