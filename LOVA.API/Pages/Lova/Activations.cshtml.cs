using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using LOVA.API.Extensions;
using LOVA.API.Models;
using LOVA.API.Services;
using LOVA.API.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace LOVA.API.Pages.Lova
{
    [Authorize(Roles = "Lova, Admin, Styrelse, VA")]
    public class ActivationsModel : PageModel
    {

        public IEnumerable<DrainTableStorageEntity> qResult { get; set; }
        private readonly LovaDbContext _context;

        public ActivationsModel(LovaDbContext context)
        {
            _context = context;
        }

        public async Task OnGet()
        {
            // Create reference an existing table
            CloudTable table = await TableStorageCommon.CreateTableAsync("Drains");
            
            qResult = TableStorageUtils.GetAll(table);

            ViewData["qResult"] = JsonConvert.SerializeObject(qResult);


            var weekAgo = DateTime.Now.AddDays(-6);

            var data = qResult.Where(a => a.TimeUp <= weekAgo);


            ViewData["noActivations"] = JsonConvert.SerializeObject(data);
        }




        /// <summary>
        ///     Get history for a specific well and present on page.
        /// </summary>
        /// <param name="drain"></param>
        /// <returns></returns>
        public async Task<PartialViewResult> OnGetDrainHistory(string drain)
        {

            var dateNow = DateTime.Now;
            int daysAgo = -1;

            switch (drain)
            {
                case "2O2":
                    daysAgo = -7;
                    break;
                case "2O1":
                    daysAgo = -7;
                    break;
                default:
                    break;
            }


            var drainHistory = await _context.ActivityPerRows.Where(a => a.Address == drain)
                 .Where(a => a.TimeUp >= dateNow.AddDays(daysAgo) && a.TimeUp <= dateNow)
                 .Select(a => new ActivityPerDrainViewModel
                 {
                     TimeUp = a.TimeUp,
                     TimeDiffString = DateExtensions.SecondsToHHMMSS(Math.Abs((a.TimeDown - a.TimeUp).TotalSeconds))
                 })
                 .OrderByDescending(a => a.TimeUp)
                 .ToListAsync();

            return Partial("_DrainHistory", drainHistory);
        }
    }



}
