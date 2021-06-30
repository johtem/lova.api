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
using LOVA.API.Extensions;

namespace LOVA.API.Controllers
{
    [Route("api/[controller]")]
    [ApiExplorerSettings(GroupName = @"DrainPatrolActivities")]
    [ApiController]
    public class DrainPatrolActivitiesController : ControllerBase
    {
        private readonly LovaDbContext _context;

        public DrainPatrolActivitiesController(LovaDbContext context)
        {
            _context = context;
        }

        // GET: api/DrainPatrolActivities
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ActivityPerRow>>> GetActivityPerRows()
        {
            return await _context.ActivityPerRows.ToListAsync();
        }

        // GET: api/DrainPatrolActivities/5
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<ActivityPerDrainViewModel>>> GetActivityPerRow(string id)
        {
            var dateNow = DateTime.Now;
            int daysAgo = -2;
            
            var activityPerRow = await _context.ActivityPerRows.Where(a => a.Address == id)
                 .Where(a => a.TimeUp >= dateNow.AddDays(daysAgo) && a.TimeUp <= dateNow)
                 .Select(a => new ActivityPerDrainViewModel
                 {
                     TimeUp = a.TimeUp,
                     TimeDiffString = DateExtensions.SecondsToHHMMSS(Math.Abs((a.TimeDown - a.TimeUp).TotalSeconds))
                 })
                 .OrderByDescending(a => a.TimeUp)
                 .ToListAsync();

            if (activityPerRow == null)
            {
                return NotFound();
            }

            return activityPerRow;
        }

        // PUT: api/DrainPatrolActivities/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutActivityPerRow(long id, ActivityPerRow activityPerRow)
        {
            if (id != activityPerRow.Id)
            {
                return BadRequest();
            }

            _context.Entry(activityPerRow).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ActivityPerRowExists(id))
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

        // POST: api/DrainPatrolActivities
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ActivityPerRow>> PostActivityPerRow(ActivityPerRow activityPerRow)
        {
            _context.ActivityPerRows.Add(activityPerRow);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetActivityPerRow", new { id = activityPerRow.Id }, activityPerRow);
        }

        // DELETE: api/DrainPatrolActivities/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteActivityPerRow(long id)
        {
            var activityPerRow = await _context.ActivityPerRows.FindAsync(id);
            if (activityPerRow == null)
            {
                return NotFound();
            }

            _context.ActivityPerRows.Remove(activityPerRow);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ActivityPerRowExists(long id)
        {
            return _context.ActivityPerRows.Any(e => e.Id == id);
        }
    }
}
