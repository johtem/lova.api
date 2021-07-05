using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LOVA.API.Models;
using LOVA.API.Services;

namespace LOVA.API.Controllers
{
    [Route("api/[controller]")]
    [ApiExplorerSettings(GroupName = @"WellMaintenanceWork")]
    [ApiController]
    public class WellMaintenanceWorksController : ControllerBase
    {
        private readonly LovaDbContext _context;

        public WellMaintenanceWorksController(LovaDbContext context)
        {
            _context = context;
        }

        // GET: api/WellMaintenanceWorks
        [HttpGet]
        public async Task<ActionResult<IEnumerable<WellMaintenanceWork>>> GetWellMaintenanceWorks()
        {
            return await _context.WellMaintenanceWorks.ToListAsync();
        }

        // GET: api/WellMaintenanceWorks/5
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<WellMaintenanceWork>>> GetWellMaintenanceWork(long wellId)
        {
            var wellMaintenanceWorks = await _context.WellMaintenanceWorks.Where(a => a.WellId == wellId).ToListAsync();

            if (wellMaintenanceWorks == null)
            {
                return NotFound();
            }

            return wellMaintenanceWorks;
        }

        // PUT: api/WellMaintenanceWorks/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutWellMaintenanceWork(long id, WellMaintenanceWork wellMaintenanceWork)
        {
            if (id != wellMaintenanceWork.Id)
            {
                return BadRequest();
            }

            _context.Entry(wellMaintenanceWork).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WellMaintenanceWorkExists(id))
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

        // POST: api/WellMaintenanceWorks
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<WellMaintenanceWork>> PostWellMaintenanceWork(WellMaintenanceWork wellMaintenanceWork)
        {
            _context.WellMaintenanceWorks.Add(wellMaintenanceWork);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetWellMaintenanceWork", new { id = wellMaintenanceWork.Id }, wellMaintenanceWork);
        }

        // DELETE: api/WellMaintenanceWorks/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWellMaintenanceWork(long id)
        {
            var wellMaintenanceWork = await _context.WellMaintenanceWorks.FindAsync(id);
            if (wellMaintenanceWork == null)
            {
                return NotFound();
            }

            _context.WellMaintenanceWorks.Remove(wellMaintenanceWork);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool WellMaintenanceWorkExists(long id)
        {
            return _context.WellMaintenanceWorks.Any(e => e.Id == id);
        }
    }
}
