using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
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
        public int LatestHour { get; set; }
        public int Latest3Hour { get; set; }
        public int Latest24Hour { get; set; }

        public DateTime DateNow { get; set; }
        public DateTime DateUtcNow { get; set; }

        public IEnumerable<DrainPatrol> Activities { get; set; }


        public async Task OnPost()
        {
            var wells = await _context.DrainPatrols.Where(a => a.Address == IssueReportViewModel.WellName).ToListAsync();

            // Change date to timezone Central Europe Standard Time
            DateUtcNow = DateTime.UtcNow;
            DateNow = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("Central Europe Standard Time"));

            // Latest activity
            LatestActivity = wells.Where(a => a.Active == true).OrderByDescending(a => a.Time).Take(1);

            // Number of activity last hour
            LatestHour = wells.Where(a => a.Time >= DateNow.AddHours(-1) && a.Active == true).Count();


            // Number of activity last 3 hour
            Latest3Hour = wells.Where(a => a.Time >= DateNow.AddHours(-3) && a.Active == true).Count();

            // Number of activity last 24 hour
            Latest24Hour = wells.Where(a => a.Time >= DateNow.AddHours(-24) && a.Active == true).Count();


            //
            Activities = await _context.DrainPatrols.Where(a => a.Address == IssueReportViewModel.WellName).OrderByDescending(a => a.Time).ToListAsync();
        }


    }
}
