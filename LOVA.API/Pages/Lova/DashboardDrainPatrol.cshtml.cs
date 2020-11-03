using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kendo.Mvc.Extensions;
using LOVA.API.Models;
using LOVA.API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace LOVA.API.Pages.Lova
{
    public class DashboardDrainPatrolModel : PageModel
    {
        private readonly LovaDbContext _context;

        public DashboardDrainPatrolModel(LovaDbContext context)
        {
            _context = context;
        }

        public IEnumerable<WellsDashboardViewModel> NoActivities { get; set; }
        public IEnumerable<WellsDashboardViewModel> NumberOfActivities { get; set; }

        public void OnGet()
        {
            NoActivities = from n in _context.DrainPatrols
                           group n by n.Address into g
                           select new WellsDashboardViewModel
                           {
                               Address = g.Key,
                               Date = g.Max(t => t.Time)
                           };

            NoActivities = NoActivities.OrderBy(n => n.Date).Take(5);




            NumberOfActivities = _context.DrainPatrols
                          .GroupBy(x => new { x.Time.Date, x.Address })
                          .OrderByDescending(g => g.Count())
                          .Select(x => new WellsDashboardViewModel
                          {
                              Count = x.Count(),
                              Date = x.Key.Date,
                              Address = x.Key.Address
                          })
                          .ToList()
                          .Take(5);

        }
    }
}
