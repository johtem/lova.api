using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LOVA.API.Models;
using LOVA.API.Services;
using LOVA.API.ViewModels;
using LOVA.API.Filter;
using System.ComponentModel;
using Microsoft.Azure.Cosmos;
using LOVA.API.Extensions;
using Microsoft.EntityFrameworkCore.Diagnostics;
using NPOI.SS.Formula.Functions;
using LOVA.API.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Azure.Data.Tables;

namespace LOVA.API.Controllers
{
    //[Route("api/[controller]")]
    [Route("api/activities")]
    [ApiExplorerSettings(GroupName = @"Activities")]
    [ApiController]
    [ApiKeyAuth]
    public class DrainPatrolsController : ControllerBase
    {
        private readonly LovaDbContext _context;
        private readonly IEmailService _mailService;
        private readonly IHubContext<ActivationHub> _hub;
        public IConfiguration _configuration { get; }

        private string storageUri { get; set; }
        private string accountName { get; set; }
        private string storageAccountKey { get; set; }

        public DrainPatrolsController(LovaDbContext context, IEmailService mailService, IHubContext<ActivationHub> hub, IConfiguration configuration)
        {
            _context = context;
            _mailService = mailService;
            _hub = hub;
            _configuration = configuration;

            storageUri = _configuration["TableStorage:StorageUrl"];
            accountName = _configuration["TableStorage:AccountName"];
            storageAccountKey = _configuration["TableStorage:StorageAccountKey"];
        }

        //// GET: api/DrainPatrols
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ActivityViewModel>>> GetDrainPatrols()
        {
            // var dateFrom = DateTime.Now.AddHours(MyConsts.HoursBackInTime);

            ActionResult<IEnumerable<ActivityViewModel>> data = await _context.Activities
                //.Where(a => a.Time > dateFrom)
                .Select(a => new ActivityViewModel
                {
                    Master_node = a.Master_node,
                    Index = a.Index,
                    Address = a.Address,
                    Time = a.Time,
                    Active = a.Active
                })
                .ToListAsync();

            return data;
        }

        // GET: api/activities/byMasterNode?masterNode=1
        [HttpGet("byMasterNode")]
        public async Task<ActionResult<IEnumerable<ActivityViewModel>>> GetDrainPatrols([FromQuery] int masterNode)
        {
            //var dateFrom = DateTime.Now.AddHours(MyConsts.HoursBackInTime);

            ActionResult<IEnumerable<ActivityViewModel>> data = await _context.Activities
                //.Where(a => a.Time > dateFrom && a.Master_node == masterNode)
                .Where(a => a.Master_node == masterNode)
                .Select(a => new ActivityViewModel
                {
                    Master_node = a.Master_node,
                    Index = a.Index,
                    Address = a.Address,
                    Time = a.Time,
                    Active = a.Active
                })
                .ToListAsync();

            return data;
        }

        // GET: api/DrainPatrols?masterNode=1&dateFrom=2020-09-12T12:30:45
        [HttpGet("byMasterNodeAndDateFrom")]
        public async Task<ActionResult<IEnumerable<ActivityViewModel>>> GetDrainPatrols(int masterNode, DateTime dateFrom)
        {

            ActionResult<IEnumerable<ActivityViewModel>> data = await _context.Activities
                .Where(a => a.Time > dateFrom && a.Master_node == masterNode)
                .Select(a => new ActivityViewModel
                {
                    Master_node = a.Master_node,
                    Index = a.Index,
                    Address = a.Address,
                    Time = a.Time,
                    Active = a.Active
                })
                .ToListAsync();

            return data;
        }


        // GET: api/DrainPatrols?dateFrom=2020-09-12T12:30:45
        [HttpGet("byDateFrom")]
        public async Task<ActionResult<IEnumerable<ActivityViewModel>>> GetDrainPatrols(DateTime dateFrom)
        {

            ActionResult<IEnumerable<ActivityViewModel>> data = await _context.Activities
                .Where(a => a.Time > dateFrom)
                .Select(a => new ActivityViewModel
                {
                    Master_node = a.Master_node,
                    Index = a.Index,
                    Address = a.Address,
                    Time = a.Time,
                    Active = a.Active
                })
                .ToListAsync();

            return data;
        }


        // GET: api/DrainPatrols/byMasterNodeAndAddressAndDateFrom?masterNode=1&dateFrom=2020-09-12T12:30:45
        [HttpGet("byMasterNodeAndAddressAndDateFrom")]
        public async Task<ActionResult<IEnumerable<ActivityViewModel>>> GetDrainPatrols(int masterNode, string address, DateTime dateFrom)
        {

            ActionResult<IEnumerable<ActivityViewModel>> data = await _context.Activities
                .Where(a => a.Time > dateFrom && a.Master_node == masterNode && a.Address == address)
                .Select(a => new ActivityViewModel
                {
                    Master_node = a.Master_node,
                    Index = a.Index,
                    Address = a.Address,
                    Time = a.Time,
                    Active = a.Active
                })
                .ToListAsync();

            return data;
        }





        // POST: api/DrainPatrols
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Activity>> PostDrainPatrol(ActivityViewModel drainPatrolViewModel)
        {

            var insertData = new Activity
            {
                Master_node = drainPatrolViewModel.Master_node,
                Index = drainPatrolViewModel.Index,
                Address = drainPatrolViewModel.Address,
                Time = drainPatrolViewModel.Time,
                Active = drainPatrolViewModel.Active
            };

            // SignalR to update page with above record
            await _hub.Clients.All.SendAsync("DrainActivity", insertData.Address, insertData);


            // Save data in Azure Table Storage to flatten out the data.
            await SaveToTableStorage(drainPatrolViewModel);


            // Save activity in Azure SQL to table Activities
            // _context.Activities.Add(insertData);
            // await _context.SaveChangesAsync();

            // Email if A or B-Alarm
            switch (insertData.Address)
            {
                case "2O1":
                    await SendEmailAlarm(insertData, "A-larm");
                    break;
                case "2O2":
                    await SendEmailAlarm(insertData, "B-larm");
                    break;
                case "1B8":
                    await SendEmailWellFull(insertData);
                    break;

                default:
                    //if (insertData.Address.StartsWith("3") && insertData.Address.EndsWith("8"))
                    //{
                    //    await SendEmailWellFull(insertData);
                    //}
                    break;
            }

            return Ok(drainPatrolViewModel);
        }

        private async Task SaveToTableStorage(ActivityViewModel drainPatrolViewModel)
        {

            var tableDrainClient = new TableClient(
                new Uri(storageUri),
                "Drains",
                new TableSharedKeyCredential(accountName, storageAccountKey));

            // Get existing data for a specific master_node and address
            DrainTableStorageModel drainExistingRow = await TableStorageUtils.RetrieveDrainTableStorageModelUsingPointQueryAsync(tableDrainClient, drainPatrolViewModel.Master_node.ToString(), drainPatrolViewModel.Address);

           
            // Verify if address in memory table
            if (drainExistingRow == null)
            {
                var recordsNumbers = await _context.Premises.Where(x => x.Well.WellName == drainPatrolViewModel.Address).ToListAsync();

                int numbers = recordsNumbers.Count();

                // await SendEmailTest(numbers, drainPatrolViewModel.Address);


                drainExistingRow = new DrainTableStorageModel
                {
                    PartitionKey = drainPatrolViewModel.Master_node.ToString(),
                    RowKey = drainPatrolViewModel.Address,
                    Timestamp = DateTime.UtcNow,
                    TimeUp = DateTime.UtcNow.AddMinutes(-5),
                    TimeDown = DateTime.UtcNow.AddMinutes(-4),
                    IsActive = false,
                    AverageActivity = 0,
                    AverageRest = 0,
                    DailyCount = 0,
                    HourlyCount = 0,
                    NumberOfHouses = numbers
                };

                var result = await TableStorageUtils.InsertOrMergeModelAsync(tableDrainClient, drainExistingRow);
            }

            // Create a new/update record for Azure Table Storage
            DrainTableStorageModel drain = new DrainTableStorageModel();
            drain.PartitionKey = drainPatrolViewModel.Master_node.ToString();
            drain.RowKey = drainPatrolViewModel.Address;
            drain.NumberOfHouses = drainExistingRow.NumberOfHouses;

            // Check if address is actice
            if (drainPatrolViewModel.Active)
            {
                // Store data in Azure nosql table if Active == true
                drain.TimeUp = DateTime.SpecifyKind(drainPatrolViewModel.Time, DateTimeKind.Utc);
                drain.TimeDown = DateTime.SpecifyKind(drainExistingRow.TimeDown, DateTimeKind.Utc);
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
                        drain.DailyCount = drainExistingRow.DailyCount + 1;
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
                drain.DailyCount = drainExistingRow.DailyCount;
                drain.AverageRest = drainExistingRow.AverageRest;
                drain.NumberOfHouses = drainExistingRow.NumberOfHouses;

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
                    TimeUp = DateTime.SpecifyKind(drain.TimeUp, DateTimeKind.Utc),
                    TimeDown = DateTime.SpecifyKind(drain.TimeDown, DateTimeKind.Utc),
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

        private async Task SendEmail(ActivityCount ac)
        {
            MailRequest request = new MailRequest();

            request.ToEmail = "johan@tempelman.nu";
            request.Subject = ac.Address;
            request.Body = $"{ac.Address} har aktiverats mer än genomsnittet. {ac.CountActivity} gånger denna timme {ac.Hourly}\n\r Mvh Löva";


            await _mailService.SendEmailAsync(request);
        }

        private async Task SendEmailTest(int ac, string well)
        {
            MailRequest request = new MailRequest();

            request.ToEmail = "johan@tempelman.nu";
            request.Subject = $"{well} - {ac.ToString()}";
            request.Body = $"{ac} st fastigheter";


            await _mailService.SendEmailAsync(request);
        }

        private async Task SendEmailAlarm(Activity ac, string alarmType)
        {

            if (ac.Active)
            {
                MailRequest request = new MailRequest();

                request.ToEmail = ""; // Will be added in EmailService.cs


                request.Subject = alarmType;
                request.Body = $"{alarmType} har aktiverats tid: {ac.Time.ToLocalTime().ToShortTimeString()} \n\r Mvh Löva";


                await _mailService.SendAlarmEmailAsync(request, "Alarm");
            }
            else
            {
                MailRequest request = new MailRequest();

                request.ToEmail = ""; // Will be added in EmailService.cs


                request.Subject = alarmType;
                request.Body = $"{alarmType} har de-aktiverats tid: {ac.Time.ToLocalTime().ToShortTimeString()} \n\r Mvh Löva";


                await _mailService.SendAlarmEmailAsync(request, "Alarm");
            }

        }

        private async Task SendEmailWellFull(Activity ac)
        {
            MailRequest request = new MailRequest();
            var emailType = "DrainFull";

            if (ac.Active)
            {

                request.ToEmail = ""; // Will be added in EmailService.cs


                request.Subject = $"Intagsenhet full - {ac.Address}";
                request.Body = $"Intagsenhet {ac.Address} är full. \n\r Mvh Löva";


                await _mailService.SendAlarmEmailAsync(request, emailType);
            }
            else
            {
                

                request.ToEmail = ""; // Will be added in EmailService.cs


                request.Subject = $"Intagsenhet tömd - {ac.Address}";
                request.Body = $"Intagsenhet {ac.Address} är nu tömd. \n\r Mvh Löva";


                await _mailService.SendAlarmEmailAsync(request, emailType);
            }

        }


        private async Task SendEmailMoreThanAverage(ActivityCount ac)
        {
            try
            {
                await _mailService.SendToManyActivitiesEmailAsync(ac);
            }
            catch
            {

            }

        }


    }
}
