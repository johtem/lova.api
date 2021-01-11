using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text.Json;
using System.Threading.Tasks;
using LOVA.API.Models;
using LOVA.API.Services;
using LOVA.API.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

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
        public List<string> Brunnar { get; set; } = new List<string>();

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
                Brunnar.Add(id);

                IEnumerable<ActivityPerRow> wells = await GetWellRowsAsync();

                DateNow = DateTime.Now;

                // Latest activity
                LatestActivity = wells.OrderByDescending(a => a.TimeUp).Take(1);

                // Number of activity last hour
                LatestHour = CountRows(wells, -1);

                // Number of activity last 3 hour
                Latest3Hour = CountRows(wells, -3);

                // Number of activity last 24 hour
                Latest24Hour = CountRows(wells, -24);


                PremisesPerWell = await GetPropertiesAsync();

                PremisesText = GetPremisesText();

            }

        }

        public IEnumerable<ActivityPerRow> LatestActivity { get; set; }
        public int LatestHour { get; set; }
        public int Latest3Hour { get; set; }
        public int Latest24Hour { get; set; }

        public DateTime DateNow { get; set; }


        public IEnumerable<PremisesPerWellViewModel> PremisesPerWell { get; set; }

        public string PremisesText { get; set; }


        public async Task OnPost()
        {

            IEnumerable<ActivityPerRow> wells = await GetWellRowsAsync();

            DateNow = DateTime.Now;
           

            // Latest activity
            LatestActivity = wells.OrderByDescending(a => a.TimeUp).Take(1);

            // Number of activity last hour
            LatestHour = CountRows(wells, -1);


            // Number of activity last 3 hour
            Latest3Hour = CountRows(wells, -3);

            // Number of activity last 24 hour
            Latest24Hour = CountRows(wells, -24);


            PremisesPerWell = await GetPropertiesAsync();

            PremisesText = GetPremisesText();


        }



        private async Task<IEnumerable<ActivityPerRow>> GetWellRowsAsync()
        {
            // return await _context.Activities.Where(a => Brunnar.Contains(a.Address) && a.Time >= endDate.AddDays(-2) && a.Time <= endDate).ToListAsync();
            return await _context.ActivityPerRows.Where(a => Brunnar.Contains(a.Address))
                 .Where(a => a.TimeUp >= endDate.AddDays(-2) && a.TimeUp <= endDate)
                 .ToListAsync();
        }

        private int CountRows(IEnumerable<ActivityPerRow> wells, int numberOfHours)
        {
            return wells.Where(a => a.TimeUp >= DateNow.AddHours(numberOfHours)).Count();
        }

        private async Task<IEnumerable<PremisesPerWellViewModel>> GetPropertiesAsync()
        {
            return await _context.Premises
                .Include(a => a.Well)
                //.Where(a => a.Well.WellName == WellName)
                .Where(a => Brunnar.Contains(a.Well.WellName))
                .Select(a => new PremisesPerWellViewModel
                { 
                    WellName = a.Well.WellName,
                    Property = a.Property,
                    Address = a.Address
                })
                .OrderBy(a => a.WellName)
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
