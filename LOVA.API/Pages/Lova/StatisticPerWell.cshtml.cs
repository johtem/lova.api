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
        public string WellName { get; set; }

        [BindProperty]
        public DateTime startDate { get; set; }

        [BindProperty]
        public DateTime endDate { get; set; }


        public async Task OnGet(string id)
        {
            startDate = DateTime.Now.AddHours(-3);
            endDate = DateTime.Now;

            if (id != null)
            {

                WellName = id;

                IEnumerable<DrainPatrol> wells = await GetWellRowsAsync();

                // Change date to timezone Central Europe Standard Time
                DateNow = DateTime.Now;
                // DateNow = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("Central Europe Standard Time"));

                // Latest activity
                LatestActivity = wells.Where(a => a.Active == true).OrderByDescending(a => a.Time).Take(1);

                // Number of activity last hour
                LatestHour = wells.Where(a => a.Time >= DateNow.AddHours(-1) && a.Active == true).Count();


                // Number of activity last 3 hour
                Latest3Hour = wells.Where(a => a.Time >= DateNow.AddHours(-3) && a.Active == true).Count();

                // Number of activity last 24 hour
                Latest24Hour = wells.Where(a => a.Time >= DateNow.AddHours(-24) && a.Active == true).Count();


                PremisesPerWell = await GetPropertiesAsync();

                PremisesText = GetPremisesText();


                //
                Activities = await GetActivitiesAsync();
            }
        }

        public IEnumerable<DrainPatrol> LatestActivity { get; set; }
        public int LatestHour { get; set; }
        public int Latest3Hour { get; set; }
        public int Latest24Hour { get; set; }

        public DateTime DateNow { get; set; }
        public DateTime DateUtcNow { get; set; }

        public IEnumerable<DrainPatrol> Activities { get; set; }

        public IEnumerable<PremisesPerWellViewModel> PremisesPerWell { get; set; }

        public string PremisesText { get; set; }


        public async Task OnPost()
        {
            // var wells = await _context.DrainPatrols.Where(a => a.Address == IssueReportViewModel.WellName).ToListAsync();
            IEnumerable<DrainPatrol> wells = await GetWellRowsAsync();

            // Change date to timezone Central Europe Standard Time
            DateNow = DateTime.Now;
            //DateNow = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("Central Europe Standard Time"));

            // Latest activity
            LatestActivity = wells.Where(a => a.Active == true).OrderByDescending(a => a.Time).Take(1);

            // Number of activity last hour
            LatestHour = wells.Where(a => a.Time >= DateNow.AddHours(-1) && a.Active == true).Count();


            // Number of activity last 3 hour
            Latest3Hour = wells.Where(a => a.Time >= DateNow.AddHours(-3) && a.Active == true).Count();

            // Number of activity last 24 hour
            Latest24Hour = wells.Where(a => a.Time >= DateNow.AddHours(-24) && a.Active == true).Count();


            PremisesPerWell = await GetPropertiesAsync();

            PremisesText = GetPremisesText();




            //
            Activities = await GetActivitiesAsync();
        }


        private async Task<IEnumerable<DrainPatrol>> GetActivitiesAsync()
        {
            return await _context.DrainPatrols.Where(a => a.Address == WellName && a.Time >= startDate && a.Time <= endDate).OrderByDescending(a => a.Time).ToListAsync();
        }

        private async Task<IEnumerable<DrainPatrol>> GetWellRowsAsync()
        {
            return await _context.DrainPatrols.Where(a => a.Address == WellName ).ToListAsync();
        }

        private async Task<IEnumerable<PremisesPerWellViewModel>> GetPropertiesAsync()
        {
            return await _context.Premises
                .Include(a => a.Well)
                .Where(a => a.Well.WellName == WellName)
                .Select(a => new PremisesPerWellViewModel
                { 
                    WellName = a.Well.WellName,
                    Property = a.Property,
                    Address = a.Address
                })
                .ToListAsync();


        }

        private string GetPremisesText()
        {
            switch (PremisesPerWell.Count())
            {
                case 0:
                    return "Ingen data inlagd";
                case 1:
                    return "Ansluten fastighet";

                default:
                    return "Anslutna fastigheter";


            }


        }


    }
}
