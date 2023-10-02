using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using LOVA.API.Extensions;
using LOVA.API.Models;
using LOVA.API.Services;
using LOVA.API.ViewModels;
using LOVA.API.Models;
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

            ResetGridTableStorageEntity resetGridExistingSlinga1 = new ResetGridTableStorageEntity();
            ResetGridTableStorageEntity resetGridExistingSlinga2 = new ResetGridTableStorageEntity();
            ResetGridTableStorageEntity resetGridExistingSlinga3 = new ResetGridTableStorageEntity();

            resetGridExistingSlinga1 = await TableStorageUtils.RetrieveResetGridEntityUsingPointQueryAsync(tableResetGrid, "slinga", "1");
            resetGridExistingSlinga2 = await TableStorageUtils.RetrieveResetGridEntityUsingPointQueryAsync(tableResetGrid, "slinga", "2");
            resetGridExistingSlinga3 = await TableStorageUtils.RetrieveResetGridEntityUsingPointQueryAsync(tableResetGrid, "slinga", "3");

            // Get rows newer than that date

            qResult = TableStorageUtils.GetAll(table);

            // Filter only data from resetDate. Valid 2 hours.
            var now = DateTime.Now;

            ViewData["buttonTextSlinga1"] = $"Slinga 1 - Återställd {resetGridExistingSlinga1.ResetDate.ToLocalTime()}"; 
            ViewData["buttonTextSlinga2"] = $"Slinga 2 - Återställd {resetGridExistingSlinga2.ResetDate.ToLocalTime()}";
            ViewData["buttonTextSlinga3"] = $"Slinga 3 - Återställd {resetGridExistingSlinga3.ResetDate.ToLocalTime()}";

            //if (now > resetGridExistingRow.ResetDate.ToLocalTime() && now < resetGridExistingRow.ResetDate.ToLocalTime().AddHours(MyConsts.resetGridWaitTime))
            //{
            //    // qResult = table.ExecuteQuery(new TableQuery<DrainTableStorageEntity>()).Where(a => a.TimeUp.ToLocalTime() > resetGridExistingRow.ResetDate.ToLocalTime()).ToList();
            //    ViewData["buttonText"] = "Återställd " + resetGridExistingRow.ResetDate.ToLocalTime().ToString();
            //}

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

        public async Task OnGetResetGrids(string slinga)
        {

            // Create reference to an existing table
            CloudTable table = await TableStorageCommon.CreateTableAsync("ResetGrid");

            ResetGridTableStorageEntity resetGridExistingRow = new ResetGridTableStorageEntity();

            // Get existing data for a specific master_node and address
            resetGridExistingRow = await TableStorageUtils.RetrieveResetGridEntityUsingPointQueryAsync(table, "slinga", slinga);

            // Create a new/update record for Azure Table Storage

            var now = DateTime.Now;
            //if (now > resetGridExistingRow.ResetDate.ToLocalTime() && now < resetGridExistingRow.ResetDate.ToLocalTime().AddHours(MyConsts.resetGridWaitTime))
            //{
            //    now = now.AddDays(-1);
            //}
            //else
            //{

            //}


            ResetGridTableStorageEntity drain = new ResetGridTableStorageEntity("slinga", slinga, now);

            await TableStorageUtils.InsertOrMergeResetGridEntityAsync(table, drain);

            await DeleteRows(slinga);

        }

        public async Task DeleteRows(string slinga)
        {
            try
            {
                // Create reference to an existing table
                CloudTable table = await TableStorageCommon.CreateTableAsync("Drains");

                IEnumerable<DrainTableStorageEntity> dResult = table.ExecuteQuery(new TableQuery<DrainTableStorageEntity>()).Where(a => a.PartitionKey == slinga).ToList();

                foreach (DrainTableStorageEntity d in dResult)
                {
                    var oper = TableOperation.Delete(d);
                    table.Execute(oper);
                }
            }
            catch
            {

            }

        }



        public async Task OnGetTestKnapp()
        {
            int master_node = 1;
            string address = "1O7";

            var drainPatrolViewModel = new LOVA.API.ViewModels.ActivityViewModel
            {
                Master_node = master_node,
                Index = 0,
                Address = address,
                Time = DateTime.UtcNow,
                Active = false
            };

            // Create reference to an existing table
            CloudTable table = await TableStorageCommon.CreateTableAsync("Drains");

            // DrainTableStorageEntity drainExistingRow = new DrainTableStorageEntity();

            // Get existing data for a specific master_node and address
            DrainTableStorageEntity drainExistingRow = await TableStorageUtils.RetrieveEntityUsingPointQueryAsync(table, master_node.ToString(), address);

            if (drainExistingRow == null)
            {
                drainExistingRow = new DrainTableStorageEntity(master_node.ToString(), address);
                drainExistingRow.Timestamp = DateTime.UtcNow;
                drainExistingRow.TimeUp = DateTime.UtcNow.AddSeconds(-5);
                drainExistingRow.TimeDown = DateTime.UtcNow.AddSeconds(-4);
                drainExistingRow.IsActive = false;
                drainExistingRow.AverageActivity = 0;
                drainExistingRow.AverageRest = 0;
                drainExistingRow.DailyCount = 0;
                drainExistingRow.HourlyCount = 0;

                await TableStorageUtils.InsertOrMergeEntityAsync(table, drainExistingRow);


            }
            var apa = drainExistingRow;


            // Create a new/update record for Azure Table Storage
            DrainTableStorageEntity drain = new DrainTableStorageEntity(drainPatrolViewModel.Master_node.ToString(), drainPatrolViewModel.Address);

            // Check if address is actice
            if (drainPatrolViewModel.Active)
            {
                // Store data in Azure nosql table if Active == true
                drain.TimeUp = drainPatrolViewModel.Time.ToLocalTime();
                drain.TimeDown = drainExistingRow.TimeDown.ToLocalTime();
                drain.IsActive = drainPatrolViewModel.Active;
                drain.AverageActivity = drainExistingRow.AverageActivity;


                // Adjust the average rest time
                var diff = (drainPatrolViewModel.Time - drainExistingRow.TimeDown).TotalSeconds;
                if (drainExistingRow.AverageRest == 0)
                {
                    drain.AverageRest = (int)diff;
                }
                else
                {
                    drain.AverageRest = (int)((drainExistingRow.AverageRest + diff) / 2);
                }

                // Add hourly counter if within same hour otherwise save count to Azure SQL table AcitvityCounts    
                if (DateExtensions.NewHour(drainPatrolViewModel.Time.ToLocalTime(), drainExistingRow.TimeUp.ToLocalTime()))
                {
                    // New hour reset counter to one
                    drain.HourlyCount = 1;

                    if (DateExtensions.IsNewDay(drainPatrolViewModel.Time.ToLocalTime(), drainExistingRow.TimeUp.ToLocalTime()))
                    {
                        drain.DailyCount = 1;
                    }
                    else
                    {
                        drain.DailyCount = drain.DailyCount + 1;
                    }

                    var averageCount = drainExistingRow.AverageActivity;

                    // Save counter
                    ActivityCount ac = new ActivityCount
                    {
                        Address = drainExistingRow.RowKey,
                        CountActivity = drainExistingRow.HourlyCount,
                        Hourly = DateExtensions.RemoveMinutesAndSeconds(convertToLocalTimeZone(drainExistingRow.TimeUp)),
                        AverageCount = averageCount
                    };


                    drain.AverageActivity = (averageCount + drainExistingRow.HourlyCount) / 2;

                    await TableStorageUtils.InsertOrMergeEntityAsync(table, drain);

                    _context.ActivityCounts.Add(ac);
                    await _context.SaveChangesAsync();

                }
                else
                {
                    // withing the same hour add one to existing sum.
                    drain.HourlyCount = drainExistingRow.HourlyCount + 1;
                    drain.DailyCount = drainExistingRow.DailyCount + 1;



                    // Save updated to the Azure nosql table 
                    await TableStorageUtils.InsertOrMergeEntityAsync(table, drain);

                }
            }
            else
            {
                // Get data from Azure table and store data in one row on ActivityPerRow

                // drain = await TableStorageUtils.RetrieveEntityUsingPointQueryAsync(table, drainPatrolViewModel.Master_node.ToString(), drainPatrolViewModel.Address);
                drain.TimeUp = drainExistingRow.TimeUp.ToLocalTime();
                drain.TimeDown = drainPatrolViewModel.Time.ToLocalTime();
                drain.IsActive = drainPatrolViewModel.Active;
                drain.HourlyCount = drainExistingRow.HourlyCount;
                drain.AverageRest = drainExistingRow.AverageRest;

                var diff = (drain.TimeDown - drain.TimeUp).TotalSeconds;
                if (drainExistingRow.AverageActivity == 0)
                {
                    drain.AverageActivity = (int)diff;
                }
                else
                {
                    drain.AverageActivity = (int)((drainExistingRow.AverageActivity + diff) / 2);
                }

                await TableStorageUtils.InsertOrMergeEntityAsync(table, drain);

                bool isGroup = false;

                if (drain.RowKey.Contains("7") || drain.RowKey.Contains("8"))
                {
                    isGroup = true;
                }

                var perRowData = new ActivityPerRow
                {
                    Address = drain.RowKey,
                    TimeUp = drain.TimeUp,
                    TimeDown = drain.TimeDown,
                    TimeDiff = (drain.TimeDown - drain.TimeUp).TotalMilliseconds,
                    //(drain.TimeDown - drain.TimeUp.AddHours(1)).TotalMilliseconds,
                    IsGroupAddress = isGroup
                };


                _context.ActivityPerRows.Add(perRowData);
                await _context.SaveChangesAsync();

            }

        }

        private DateTime convertToLocalTimeZone(DateTime time)
        {
            // Define Timezone
            TimeZoneInfo Sweden_Standard_Time = TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time");
            return TimeZoneInfo.ConvertTime(time, Sweden_Standard_Time);
        }
    }
}
