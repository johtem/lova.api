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

namespace LOVA.API.Controllers
{
    [Route("api/alarms")]
    [ApiController]
    [ApiExplorerSettings(GroupName = @"Alarms")]
    [ApiKeyAuth]
    public class DrainPatrolAlarmsController : ControllerBase
    {
        private readonly LovaDbContext _context;

        public DrainPatrolAlarmsController(LovaDbContext context)
        {
            _context = context;
        }

        // GET: api/DrainPatrolAlarms
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DrainPatrolAlarmViewModel>>> GetDrainPatrolAlarms()
        {
            var dateFrom = DateTime.Now.AddHours(MyConsts.HoursBackInTime);

            ActionResult<IEnumerable<DrainPatrolAlarmViewModel>> data = await _context.DrainPatrolAlarms
                
                // .Where(a => a.TimeStamp > dateFrom)
                .OrderByDescending(a => a.Id)
                .Take(10)
                .Select(a => new DrainPatrolAlarmViewModel
                {
                    Master_node = a.Master_node,
                    Index = a.Index,
                    Address = a.Address,
                    TimeStamp = a.TimeStamp,
                    AlarmType = a.AlarmType,
                    Amount = a.Amount,
                    Limit = a.Limit
                })
                .ToListAsync();


            return data;


        }

        // GET: api/DrainPatrolAlarms/5
        [HttpGet("{id}")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ActionResult<DrainPatrolAlarm>> GetDrainPatrolAlarm(long id)
        {
            var drainPatrolAlarm = await _context.DrainPatrolAlarms.FindAsync(id);

            if (drainPatrolAlarm == null)
            {
                return NotFound();
            }

            return drainPatrolAlarm;
        }

        // PUT: api/DrainPatrolAlarms/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> PutDrainPatrolAlarm(long id, DrainPatrolAlarm drainPatrolAlarm)
        {
            if (id != drainPatrolAlarm.Id)
            {
                return BadRequest();
            }

            _context.Entry(drainPatrolAlarm).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DrainPatrolAlarmExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/DrainPatrolAlarms
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<DrainPatrolAlarm>> PostDrainPatrolAlarm(DrainPatrolAlarmViewModel drainPatrolAlarmVM)
        {


            var insertData = new DrainPatrolAlarm
            {

                Master_node = drainPatrolAlarmVM.Master_node,
                Index = drainPatrolAlarmVM.Index,
                Address = drainPatrolAlarmVM.Address,
                TimeStamp = drainPatrolAlarmVM.TimeStamp,
                AlarmType = drainPatrolAlarmVM.AlarmType,
                Amount = drainPatrolAlarmVM.Amount,
                Limit = drainPatrolAlarmVM.Limit
            };



            _context.DrainPatrolAlarms.Add(insertData);
            await _context.SaveChangesAsync();

            return Ok(drainPatrolAlarmVM);
        }

        // DELETE: api/DrainPatrolAlarms/5
        [HttpDelete("{id}")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ActionResult<DrainPatrolAlarm>> DeleteDrainPatrolAlarm(long id)
        {
            var drainPatrolAlarm = await _context.DrainPatrolAlarms.FindAsync(id);
            if (drainPatrolAlarm == null)
            {
                return NotFound();
            }

            _context.DrainPatrolAlarms.Remove(drainPatrolAlarm);
            await _context.SaveChangesAsync();

            return drainPatrolAlarm;
        }

        private bool DrainPatrolAlarmExists(long id)
        {
            return _context.DrainPatrolAlarms.Any(e => e.Id == id);
        }
    }
}
