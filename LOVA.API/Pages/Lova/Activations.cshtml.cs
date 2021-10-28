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
            

            // Get reset time from ResetGrid
            CloudTable tableResetGrid = await TableStorageCommon.CreateTableAsync("ResetGrid");

            ResetGridTableStorageEntity resetGridExistingRow = new ResetGridTableStorageEntity();

            resetGridExistingRow = await TableStorageUtils.RetrieveResetGridEntityUsingPointQueryAsync(tableResetGrid, "one", "two");

            // Get rows newer than that date

            qResult = TableStorageUtils.GetAll(table);

            // Filter only data from resetDate. Valid 2 hours.
            var now = DateTime.Now;
            ViewData["buttonText"] = "Återställ grid i 2h";
            if (now > resetGridExistingRow.ResetDate.ToLocalTime() && now < resetGridExistingRow.ResetDate.ToLocalTime().AddHours(MyConsts.resetGridWaitTime))
            {
                qResult = table.ExecuteQuery(new TableQuery<DrainTableStorageEntity>()).Where(a => a.TimeUp.ToLocalTime() > resetGridExistingRow.ResetDate.ToLocalTime()).ToList();
                ViewData["buttonText"] = resetGridExistingRow.ResetDate.ToLocalTime().ToString();
            }

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
                    daysAgo = -14;
                    break;
                case "2O1":
                    daysAgo = -14;
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


        public async Task OnGetRemoveColumn(string drain, string apa)
        {
            // Create reference an existing table
            CloudTable table = await TableStorageCommon.CreateTableAsync("Drains");
            string b = apa;

            string a = drain.Substring(0, 1);
            // Get existing data for a specific master_node and address
            var drainExistingRow = await TableStorageUtils.RetrieveEntityUsingPointQueryAsync(table, a, drain);

            switch (apa)
            {
                case "AverageActivity":
                    drainExistingRow.AverageActivity = 0;
                    break;
                case "AverageRest":
                    drainExistingRow.AverageRest = 0;
                    break;
            }

            await TableStorageUtils.InsertOrMergeEntityAsync(table, drainExistingRow);


            // return "deleted";
        }

        public async Task OnGetResetGrids()
        {

            // Create reference to an existing table
            CloudTable table = await TableStorageCommon.CreateTableAsync("ResetGrid");

            ResetGridTableStorageEntity resetGridExistingRow = new ResetGridTableStorageEntity();

            // Get existing data for a specific master_node and address
            resetGridExistingRow = await TableStorageUtils.RetrieveResetGridEntityUsingPointQueryAsync(table, "one", "two");

            // Create a new/update record for Azure Table Storage
            ResetGridTableStorageEntity drain = new ResetGridTableStorageEntity("one", "two", DateTime.Now);

            await TableStorageUtils.InsertOrMergeResetGridEntityAsync(table, drain);

        }
    }
}
