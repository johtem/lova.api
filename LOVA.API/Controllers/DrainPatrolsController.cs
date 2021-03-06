﻿using System;
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

            await SaveToTableStorage(drainPatrolViewModel);

           

            // Save activity in Azure SQL to table Activities
            _context.Activities.Add(insertData);
            await _context.SaveChangesAsync();

            // Email if A or B-Alarm
            switch (insertData.Address)
            {
                case "2O1":
                    await SendEmailAlarm(insertData, "A-larm");
                    break;
                case "2O2":
                    await SendEmailAlarm(insertData, "B-larm");
                    break;
                default:
                    break;
            }

            return Ok(drainPatrolViewModel);
        }

        private async Task SaveToTableStorage(ActivityViewModel drainPatrolViewModel)
        {
            
            // Create reference to an existing table
            CloudTable table = await TableStorageCommon.CreateTableAsync("Drains");

            DrainTableStorageEntity drainExistingRow = new DrainTableStorageEntity();

            // Get existing data for a specific master_node and address
            drainExistingRow = await TableStorageUtils.RetrieveEntityUsingPointQueryAsync(table, drainPatrolViewModel.Master_node.ToString(), drainPatrolViewModel.Address);


            // Create a new/update record for Azure Table Storage
            DrainTableStorageEntity drain = new DrainTableStorageEntity(drainPatrolViewModel.Master_node.ToString(), drainPatrolViewModel.Address);

            // Check if address is actice
            if (drainPatrolViewModel.Active)
            {
                // Store data in Azure nosql table if Active == true
                drain.TimeUp = drainPatrolViewModel.Time;
                drain.TimeDown = drainExistingRow.TimeDown;
                drain.IsActive = drainPatrolViewModel.Active;
                drain.AverageActivity = drainExistingRow.AverageActivity;

                var diff = (drainPatrolViewModel.Time - convertToLocalTimeZone(drainExistingRow.TimeDown)).TotalSeconds;
                if (drainExistingRow.AverageRest == 0)
                {
                    drain.AverageRest = (int)diff;
                }
                else
                {
                    drain.AverageRest = (int)((drainExistingRow.AverageRest + diff) / 2);
                }

                // Add hourly counter if within same hour otherwise save count to Azure SQL table AcitvityCounts    
                if (DateExtensions.NewHour(drainPatrolViewModel.Time, convertToLocalTimeZone(drainExistingRow.TimeUp)))
                {
                    // New hour reset counter to one
                    drain.HourlyCount = 1;

                    if (DateExtensions.IsNewDay(drainPatrolViewModel.Time, convertToLocalTimeZone(drainExistingRow.TimeUp)))
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
                drain.TimeUp = drainExistingRow.TimeUp;
                drain.TimeDown = drainPatrolViewModel.Time;
                drain.IsActive = drainPatrolViewModel.Active;
                drain.HourlyCount = drainExistingRow.HourlyCount;
                drain.AverageRest = drainExistingRow.AverageRest;

                var diff = (drain.TimeDown - convertToLocalTimeZone(drain.TimeUp)).TotalSeconds;
                if (drainExistingRow.AverageActivity == 0)
                {
                    drain.AverageActivity = (int)diff;
                }else
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
                    TimeUp = convertToLocalTimeZone(drain.TimeUp),
                    TimeDown = drain.TimeDown,
                    TimeDiff = (drain.TimeDown - convertToLocalTimeZone(drain.TimeUp)).TotalMilliseconds,
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

        private async Task SendEmailAlarm(Activity ac, string alarmType)
        {
            
            if (ac.Active)
            {
                MailRequest request = new MailRequest();

                request.ToEmail = ""; // Will be added in EmailService.cs


                request.Subject = alarmType;
                request.Body = $"{alarmType} har aktiverats tid: {ac.Time.ToShortTimeString()} \n\r Mvh Löva";


                await _mailService.SendAlarmEmailAsync(request);
            }
            
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
