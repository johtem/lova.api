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

        public DrainPatrolsController(LovaDbContext context, IEmailService mailService)
        {
            _context = context;
            _mailService = mailService;
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
        public async Task<ActionResult<IEnumerable<ActivityViewModel>>> GetDrainPatrols([FromQuery]int masterNode)
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
                .Where(a =>  a.Time > dateFrom)
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

            // Save data temporary in Table Storage to flatten out the data.

            // Create reference an existing table
            CloudTable table = await TableStorageCommon.CreateTableAsync("Drains");

            
            // Get existing data for address
            var drainExistingRow = await TableStorageUtils.RetrieveEntityUsingPointQueryAsync(table, drainPatrolViewModel.Master_node.ToString(), drainPatrolViewModel.Address);


            DrainTableStorageEntity drain = new DrainTableStorageEntity(drainPatrolViewModel.Master_node.ToString(), drainPatrolViewModel.Address);

            // 
            if (drainPatrolViewModel.Active)
            {
                // Store data in Azure nosql table if Active == true
                drain.TimeUp = drainPatrolViewModel.Time;
                drain.TimeDown = drainPatrolViewModel.Time;


                // Add hourly counter if within same hour otherwise save count to Azure SQL table AcitvityCounts    
                if (DateExtensions.NewHour(drainPatrolViewModel.Time, drainExistingRow.TimeUp.AddHours(1)))
                {
                    // New hour reset counter to one
                    drain.HourlyCount = 1;


                    var averageCount = drainExistingRow.AverageActivity == 0 ? 0 : drainExistingRow.AverageActivity;

                    // Save counter
                    ActivityCount ac = new ActivityCount
                    {
                        Address = drainExistingRow.RowKey,
                        CountActivity = drainExistingRow.HourlyCount,
                        Hourly = DateExtensions.RemoveMinutesAndSeconds(drainExistingRow.TimeUp.AddHours(1)),
                        AverageCount = averageCount
                    };


                    

                    drain.AverageActivity = (averageCount + drainExistingRow.HourlyCount) / 2;

                    await TableStorageUtils.InsertOrMergeEntityAsync(table, drain);

                    _context.ActivityCounts.Add(ac);
                    await _context.SaveChangesAsync();

                    if (drainExistingRow.HourlyCount > averageCount)
                    {
                        // await SendEmailMoreThanAverage(ac);
                        await SendEmail(ac);
                    }

                    await SendEmailMoreThanAverage(ac);

                }
                else
                {
                    // withing the same hour add one to existing sum.
                    drain.HourlyCount = drainExistingRow.HourlyCount + 1;

                    await TableStorageUtils.InsertOrMergeEntityAsync(table, drain);

                }

                // End hourly counter


                // Save updated to the Azure nosql table 
                

            }
            else
            {
                // Get data from Azure table and store data in one row on ActivityPerRow

                drain = await TableStorageUtils.RetrieveEntityUsingPointQueryAsync(table, drainPatrolViewModel.Master_node.ToString(), drainPatrolViewModel.Address);
                drain.TimeDown = drainPatrolViewModel.Time;               

                var perRowData = new ActivityPerRow
                {
                    Address = drain.RowKey,
                    TimeUp = drain.TimeUp.AddHours(1),
                    TimeDown = drain.TimeDown,
                    TimeDiff = (drain.TimeDown - drain.TimeUp.AddHours(1)).TotalMilliseconds
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
