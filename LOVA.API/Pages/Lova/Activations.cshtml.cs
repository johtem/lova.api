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

            //qResult = TableStorageUtils.GetAll(table);
            Pageable<DrainTableStorageModel> qResult1 = tableDrainClient.Query<DrainTableStorageModel>(maxPerPage: 500);

            // Filter only data from resetDate. Valid 2 hours.
            var now = DateTime.Now;

            ViewData["buttonTextSlinga1"] = $"Slinga 1 - Återställd {resetGridSlinga1.ResetDate.ToLocalTime()}"; 
            ViewData["buttonTextSlinga2"] = $"Slinga 2 - Återställd {resetGridSlinga2.ResetDate.ToLocalTime()}";
            ViewData["buttonTextSlinga3"] = $"Slinga 3 - Återställd {resetGridSlinga3.ResetDate.ToLocalTime()}";

            //if (now > resetGridExistingRow.ResetDate.ToLocalTime() && now < resetGridExistingRow.ResetDate.ToLocalTime().AddHours(MyConsts.resetGridWaitTime))
            //{
            //    // qResult = table.ExecuteQuery(new TableQuery<DrainTableStorageEntity>()).Where(a => a.TimeUp.ToLocalTime() > resetGridExistingRow.ResetDate.ToLocalTime()).ToList();
            //    ViewData["buttonText"] = "Återställd " + resetGridExistingRow.ResetDate.ToLocalTime().ToString();
            //}

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

            var now = DateTime.Now;
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

                IEnumerable<DrainTableStorageModel> dResult = tableDrainClient.Query<DrainTableStorageModel>(ent => ent.PartitionKey == slinga); //ExecuteQuery(new TableQuery<DrainTableStorageModel>()).Where(a => a.PartitionKey == slinga).ToList();

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
            
            string aa = _configuration["TableStorage:StorageUrl"];
            
           

        }

        private DateTime convertToLocalTimeZone(DateTime time)
        {
            // Define Timezone
            TimeZoneInfo Sweden_Standard_Time = TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time");
            return TimeZoneInfo.ConvertTime(time, Sweden_Standard_Time);
        }
    }
}
