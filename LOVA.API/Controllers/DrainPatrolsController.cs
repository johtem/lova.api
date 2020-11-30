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

        public DrainPatrolsController(LovaDbContext context)
        {
            _context = context;
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
            //string wellName = string.Concat(drainPatrolViewModel.Master_node, drainPatrolViewModel.Address);

            //var well = await _context.Wells.Where(a => a.WellName == wellName).FirstOrDefaultAsync();

            //if (well == null )
            //{
            //    return NotFound();
            //}

            var insertData = new Activity
            {
               // WellId = well.Id,
                Master_node = drainPatrolViewModel.Master_node,
                Index = drainPatrolViewModel.Index,
                Address = drainPatrolViewModel.Address,
                Time = drainPatrolViewModel.Time,
                Active = drainPatrolViewModel.Active
            };



            _context.Activities.Add(insertData);
            await _context.SaveChangesAsync();

            return Ok(drainPatrolViewModel);
        }

    }
}
