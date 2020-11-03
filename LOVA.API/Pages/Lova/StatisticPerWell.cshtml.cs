using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LOVA.API.Models;
using LOVA.API.Services;
using LOVA.API.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace LOVA.API.Pages.Lova
{
    public class StatisticPerWellModel : PageModel
    {

        private readonly LovaDbContext _context;

        public StatisticPerWellModel(LovaDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Well> Wells { get; set; }


        [BindProperty]
        public IssueReportViewModel IssueReportViewModel { get; set; }

        public void OnGet()
        {
            
        }

        public IEnumerable<DrainPatrol> LatestActivity { get; set; }
        public IEnumerable<WellsDashboardViewModel> LatestHour { get; set; }
        public IEnumerable<WellsDashboardViewModel> Latest3Hour { get; set; }


        public async Task OnPost()
        {
            var wells = await _context.DrainPatrols.Where(a => a.Address == IssueReportViewModel.WellName).ToListAsync();

            // Latest activity
            LatestActivity = wells.OrderByDescending(a => a.Time).Take(1);

            // Number of activity last hour
            LatestHour = wells.Where(a => a.Time >= DateTime.Now.AddHours(-1))
                          .GroupBy(x => new { x.Time.Date, x.Address })
                          .OrderByDescending(g => g.Count())
                          .Select(x => new WellsDashboardViewModel
                          {
                              Count = x.Count(),
                              Date = x.Key.Date,
                              Address = x.Key.Address
                          })
                          .ToList()
                          .Take(1);

            // Number of activity last 24 hour
            Latest3Hour = wells.Where(a => a.Time >= DateTime.Now.AddHours(-3))
                          .GroupBy(x => new { x.Time.Date, x.Address })
                          .OrderByDescending(g => g.Count())
                          .Select(x => new WellsDashboardViewModel
                          {
                              Count = x.Count(),
                              Date = x.Key.Date,
                              Address = x.Key.Address
                          })
                          .ToList()
                          .Take(1);

            //
        }


    }
}
