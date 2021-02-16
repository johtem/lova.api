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
using Microsoft.Azure.Cosmos.Table;
using LOVA.API.Extensions;
using Microsoft.EntityFrameworkCore.Diagnostics;
using NPOI.SS.Formula.Functions;
using LOVA.API.Hubs;
using Microsoft.AspNetCore.SignalR;

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

        public DrainPatrolsController(LovaDbContext context, IEmailService mailService, IHubContext<ActivationHub> hub)
        {
            _context = context;
            _mailService = mailService;
            _hub = hub;
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

            // Create reference to an existing table
            CloudTable table = await TableStorageCommon.CreateTableAsync("Drains");


            DrainTableStorageEntity drainExistingRow = new DrainTableStorageEntity();

            // Get existing data for a specific master_node and address
            drainExistingRow = await TableStorageUtils.RetrieveEntityUsingPointQueryAsync(table, drainPatrolViewModel.Master_node.ToString(), drainPatrolViewModel.Address);

            if (drainExistingRow == null)
            {
                drainExistingRow = new DrainTableStorageEntity
                {
                    PartitionKey = drainPatrolViewModel.Master_node.ToString(),
                    RowKey = drainPatrolViewModel.Address,
                    TimeDown = drainPatrolViewModel.Time,
                    TimeUp = drainPatrolViewModel.Time.AddHours(-1),
                    IsActive = drainPatrolViewModel.Active,
                    HourlyCount = 0
                };

            }

            // Create a new/update record for Azure Table Storage
            DrainTableStorageEntity drain = new DrainTableStorageEntity(drainPatrolViewModel.Master_node.ToString(), drainPatrolViewModel.Address);

            // Check if address is actice
            if (drainPatrolViewModel.Active)
            {
                // Store data in Azure nosql table if Active == true
                drain.TimeUp = drainPatrolViewModel.Time;
                drain.TimeDown = drainExistingRow.TimeDown;
                
                
                drain.IsActive = drainPatrolViewModel.Active;



                // Add hourly counter if within same hour otherwise save count to Azure SQL table AcitvityCounts    
                if (DateExtensions.NewHour(drainPatrolViewModel.Time, drainExistingRow.TimeUp.AddHours(1)))
                {
                    // New hour reset counter to one
                    drain.HourlyCount = 1;

                    if (DateExtensions.IsNewDay(drainPatrolViewModel.Time, drainExistingRow.TimeUp.AddHours(1)))
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
                        Hourly = DateExtensions.RemoveMinutesAndSeconds(drainExistingRow.TimeUp.AddHours(1)),
                        AverageCount = averageCount
                    };


                    drain.AverageActivity = (averageCount + drainExistingRow.HourlyCount) / 2;



                    _context.ActivityCounts.Add(ac);
                    await _context.SaveChangesAsync();


                    // if latest hour is greater than average send out warning email

                    if (drainExistingRow.HourlyCount > averageCount)
                    {
                        // await SendEmailMoreThanAverage(ac);
                        if (averageCount > 2)
                        {
                            await SendEmail(ac);
                        }
                    }
                }
                else
                {
                    // withing the same hour add one to existing sum.
                    drain.HourlyCount = drainExistingRow.HourlyCount + 1;
                    drain.DailyCount = drainExistingRow.DailyCount + 1;
                    //drain.TimeDown = drainPatrolViewModel.Time;

                }

                // End hourly counter


                // Save updated to the Azure nosql table 
                await TableStorageUtils.InsertOrMergeEntityAsync(table, drain);

            }
            else
            {
                // Get data from Azure table and store data in one row on ActivityPerRow

                // drain = await TableStorageUtils.RetrieveEntityUsingPointQueryAsync(table, drainPatrolViewModel.Master_node.ToString(), drainPatrolViewModel.Address);
                drain.TimeUp = drainExistingRow.TimeUp;
                drain.TimeDown = drainPatrolViewModel.Time;
                drain.IsActive = drainPatrolViewModel.Active;

                await TableStorageUtils.InsertOrMergeEntityAsync(table, drain);

                bool isGroup = false;

                if (drain.RowKey.Contains("7") || drain.RowKey.Contains("8"))
                {
                    isGroup = true;
                }

                var perRowData = new ActivityPerRow
                {
                    Address = drain.RowKey,
                    TimeUp = drain.TimeUp.AddHours(1),
                    TimeDown = drain.TimeDown,
                    TimeDiff = (drain.TimeDown - drain.TimeUp.AddHours(1)).TotalMilliseconds,
                    IsGroupAddress = isGroup
                };


                _context.ActivityPerRows.Add(perRowData);
                await _context.SaveChangesAsync();


            }


            // Save activity in Azure SQL to table Activities
            _context.Activities.Add(insertData);
            await _context.SaveChangesAsync();

            return Ok(drainPatrolViewModel);
        }

        private async Task SendEmail(ActivityCount ac)
        {
            MailRequest request = new MailRequest();

            request.ToEmail = "johan@tempelman.nu";
            request.Subject = ac.Address;
            request.Body = $"{ac.Address} har aktiverats mer än genomsnittet. {ac.CountActivity} gånger denna timme {ac.Hourly}\n\r Mvh Löva";


            await _mailService.SendEmailAsync(request);
        }


        private async Task SendEmailMoreThanAverage(ActivityCount ac)
        {
            try
            {
                await _mailService.SendToManyActivitiesEmailAsync(ac);
            }
            catch (Exception ex)
            {

            }

        }


    }
}
