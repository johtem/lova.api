using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Azure.Data.Tables;
using Azure;
using LOVA.API.Extensions;
using LOVA.API.Models;
using LOVA.API.Services;
using LOVA.API.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using NuGet.Protocol;

namespace LOVA.API.Pages.Lova
{
    [Authorize(Roles = "Lova, Admin, Styrelse, VA")]
    public class ActivationsModel : PageModel
    {

        public IEnumerable<DrainTableStorageEntity> qResult { get; set; }
        private readonly LovaDbContext _context;
        public IConfiguration _configuration { get; }

        private string storageUri { get; set; }
        private string accountName { get; set; }
        private string storageAccountKey { get; set; }

        public ActivationsModel(LovaDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;

            storageUri = _configuration["TableStorage:StorageUrl"];
            accountName = _configuration["TableStorage:AccountName"];
            storageAccountKey = _configuration["TableStorage:StorageAccountKey"];
        }

        public async Task OnGet()
        {

            // Construct a new <see cref="TableClient" /> using a <see cref="TableSharedKeyCredential" />.
            var tableDrainClient = new TableClient(
                new Uri(storageUri),
                "Drains",
                new TableSharedKeyCredential(accountName, storageAccountKey));

            // 
            var tableResetGridClient = new TableClient(
                new Uri(storageUri),
                "ResetGrid",
                new TableSharedKeyCredential(accountName, storageAccountKey));


            ResetGridTableStorageModel resetGridSlinga1 = await TableStorageUtils.RetrieveResetGridModelUsingPointQueryAsync(tableResetGridClient, "slinga", "1");
            ResetGridTableStorageModel resetGridSlinga2 = await TableStorageUtils.RetrieveResetGridModelUsingPointQueryAsync(tableResetGridClient, "slinga", "2");
            ResetGridTableStorageModel resetGridSlinga3 = await TableStorageUtils.RetrieveResetGridModelUsingPointQueryAsync(tableResetGridClient, "slinga", "3");

            // Create reference an existing table

            //CloudTable table = await TableStorageCommon.CreateTableAsync("Drains");
            
            tableDrainClient.CreateIfNotExists();

            

            ViewData["buttonTextSlinga1"] = $"Slinga 1 - Återställd {resetGridSlinga1.ResetDate}"; 
            ViewData["buttonTextSlinga2"] = $"Slinga 2 - Återställd {resetGridSlinga2.ResetDate}";
            ViewData["buttonTextSlinga3"] = $"Slinga 3 - Återställd {resetGridSlinga3.ResetDate}";

            //if (now > resetGridExistingRow.ResetDate.ToLocalTime() && now < resetGridExistingRow.ResetDate.ToLocalTime().AddHours(MyConsts.resetGridWaitTime))
            //{
            //    // qResult = table.ExecuteQuery(new TableQuery<DrainTableStorageEntity>()).Where(a => a.TimeUp.ToLocalTime() > resetGridExistingRow.ResetDate.ToLocalTime()).ToList();
            //    ViewData["buttonText"] = "Återställd " + resetGridExistingRow.ResetDate.ToLocalTime().ToString();
            //}

            //qResult = TableStorageUtils.GetAll(table);
            Pageable<DrainTableStorageModel> qResult1 = tableDrainClient.Query<DrainTableStorageModel>(maxPerPage: 500);

            // Filter only data from resetDate. Valid 2 hours.
            var now = DateTime.Now;

            ViewData["qResult"] = JsonConvert.SerializeObject(qResult1);


            var weekAgo = DateTime.Now.AddDays(-6);

            var data = qResult1.Where(a => a.TimeUp <= weekAgo);


            ViewData["noActivations"] = JsonConvert.SerializeObject(data);

            



        }




        /// <summary>
        ///     Get history for a specific well and present on page.
        /// </summary>
        /// <param name="drain"></param>
        /// <returns></returns>
        public async Task<PartialViewResult> OnGetDrainHistory(string drain)
        {

            //var numbers = await _context.Premises.Where(x => x.Well.WellName == drain).ToListAsync();
            //int n = numbers.Count();

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

            var tableDrainClient = new TableClient(
                new Uri(storageUri),
                "Drains",
                new TableSharedKeyCredential(accountName, storageAccountKey));

            // Create reference an existing table
            // CloudTable table = await TableStorageCommon.CreateTableAsync("Drains");
            string b = apa;

            string a = drain.Substring(0, 1);
            // Get existing data for a specific master_node and address
            var drainExistingRow = await TableStorageUtils.RetrieveDrainTableStorageModelUsingPointQueryAsync(tableDrainClient, a, drain);

            switch (apa)
            {
                case "AverageActivity":
                    drainExistingRow.AverageActivity = 0;
                    break;
                case "AverageRest":
                    drainExistingRow.AverageRest = 0;
                    break;
            }

            await TableStorageUtils.InsertOrMergeModelAsync(tableDrainClient, drainExistingRow);


            // return "deleted";
        }

        public async Task OnGetResetGrids(string slinga)
        {

            var tableResetGridClient = new TableClient(
                new Uri(storageUri),
                "ResetGrid",
                new TableSharedKeyCredential(accountName, storageAccountKey));

            // Create reference to an existing table
            //CloudTable table = await TableStorageCommon.CreateTableAsync("ResetGrid");

            ResetGridTableStorageModel resetGridExistingRow = new ResetGridTableStorageModel();

            // Get existing data for a specific master_node and address
            resetGridExistingRow = await TableStorageUtils.RetrieveResetGridModelUsingPointQueryAsync(tableResetGridClient, "slinga", slinga);

            // Create a new/update record for Azure Table Storage

            var n = DateTime.Now;
            var now = DateTime.SpecifyKind(n, DateTimeKind.Utc);
            //if (now > resetGridExistingRow.ResetDate.ToLocalTime() && now < resetGridExistingRow.ResetDate.ToLocalTime().AddHours(MyConsts.resetGridWaitTime))
            //{
            //    now = now.AddDays(-1);
            //}
            //else
            //{

            //}


            ResetGridTableStorageModel drain = new ResetGridTableStorageModel("slinga", slinga, now);

            await TableStorageUtils.InsertOrMergeResetGridModelAsync(tableResetGridClient, drain);

            await DeleteRows(slinga);

        }

        public async Task DeleteRows(string slinga)
        {
            try
            {
                // Create reference to an existing table
               // TableClient table = await TableStorageCommon.CreateTableAsync("Drains");

                var tableDrainClient = new TableClient(
                new Uri(storageUri),
                "Drains",
                new TableSharedKeyCredential(accountName, storageAccountKey));

                IEnumerable<DrainTableStorageModel> dResult = tableDrainClient.Query<DrainTableStorageModel>(filter: $"PartitionKey eq '{slinga}'"); //ExecuteQuery(new TableQuery<DrainTableStorageModel>()).Where(a => a.PartitionKey == slinga).ToList();

                foreach (DrainTableStorageModel d in dResult)
                {
                   // var oper = tableDrainClient.Delete(d);
                    tableDrainClient.DeleteEntity(d.PartitionKey, d.RowKey);
                }
            }
            catch
            {

            }

        }



        public async Task OnGetTestKnapp()
        {
            ActivityViewModel drainPatrolViewModel = new ActivityViewModel();
            drainPatrolViewModel.Address = "2G2";
            drainPatrolViewModel.Active = false;
            drainPatrolViewModel.Master_node = 2;
            DateTime saveNow = DateTime.Now;
            drainPatrolViewModel.Time = DateTime.SpecifyKind(saveNow, DateTimeKind.Utc);

            string rowKey = drainPatrolViewModel.Address;

            var tableDrainClient = new TableClient(
                new Uri(storageUri),
                "Drains",
                new TableSharedKeyCredential(accountName, storageAccountKey));

            // Get existing data for a specific master_node and address
            DrainTableStorageModel drainExistingRow = await TableStorageUtils.RetrieveDrainTableStorageModelUsingPointQueryAsync(tableDrainClient, "2", rowKey);

            // Verify if address in memory table
            if (drainExistingRow == null)
            {
                drainExistingRow = new DrainTableStorageModel();
                drainExistingRow.PartitionKey = "2";
                drainExistingRow.RowKey = rowKey;
                drainExistingRow.Timestamp = DateTime.UtcNow;
                drainExistingRow.TimeUp = DateTime.UtcNow.AddMinutes(-5);
                drainExistingRow.TimeDown = DateTime.UtcNow.AddMinutes(-4);
                drainExistingRow.IsActive = false;
                drainExistingRow.AverageActivity = 0;
                drainExistingRow.AverageRest = 0;
                drainExistingRow.DailyCount = 0;
                drainExistingRow.HourlyCount = 0;

                await TableStorageUtils.InsertOrMergeModelAsync(tableDrainClient, drainExistingRow);
            }

            // Create a new/update record for Azure Table Storage
            DrainTableStorageModel drain = new DrainTableStorageModel();
            drain.PartitionKey = drainPatrolViewModel.Master_node.ToString();
            drain.RowKey = drainPatrolViewModel.Address;
            // drain.Timestamp = DateTime.SpecifyKind(saveNow, DateTimeKind.Utc);

            // Check if address is actice
            if (drainPatrolViewModel.Active)
            {
                // Store data in Azure nosql table if Active == true
                drain.TimeUp = DateTime.SpecifyKind(drainPatrolViewModel.Time, DateTimeKind.Utc);
                drain.TimeDown = DateTime.SpecifyKind(drainExistingRow.TimeDown, DateTimeKind.Utc);
                //drain.TimeDown = drainExistingRow.TimeDown;
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

                    await TableStorageUtils.InsertOrMergeModelAsync(tableDrainClient, drain);

                    _context.ActivityCounts.Add(ac);
                    await _context.SaveChangesAsync();

                }
                else
                {
                    // withing the same hour add one to existing sum.
                    drain.HourlyCount = drainExistingRow.HourlyCount + 1;
                    drain.DailyCount = drainExistingRow.DailyCount + 1;



                    // Save updated to the Azure nosql table 
                    await TableStorageUtils.InsertOrMergeModelAsync(tableDrainClient, drain);

                }
            }
            else
            {
                // Get data from Azure table and store data in one row on ActivityPerRow

                // drain = await TableStorageUtils.RetrieveEntityUsingPointQueryAsync(table, drainPatrolViewModel.Master_node.ToString(), drainPatrolViewModel.Address);
                drain.TimeUp = DateTime.SpecifyKind(drainExistingRow.TimeUp, DateTimeKind.Utc);
                drain.TimeDown = DateTime.SpecifyKind(drainPatrolViewModel.Time, DateTimeKind.Utc);
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

                await TableStorageUtils.InsertOrMergeModelAsync(tableDrainClient, drain);

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
