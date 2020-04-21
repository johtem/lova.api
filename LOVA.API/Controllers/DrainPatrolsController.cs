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
    [Route("api/[controller]")]
    [ApiController]
    [ApiKeyAuth]
    public class DrainPatrolsController : ControllerBase
    {
        private readonly LovaDbContext _context;

        public DrainPatrolsController(LovaDbContext context)
        {
            _context = context;
        }

        // GET: api/DrainPatrols
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DrainPatrolViewModel>>> GetDrainPatrols()
        {
            ActionResult<IEnumerable<DrainPatrolViewModel>> data = await _context.DrainPatrols
                .Include(a => a.Well)
                .Select(a => new DrainPatrolViewModel
                {
                    Slinga = a.Slinga,
                    Address = a.Address,
                    Tid = a.Tid,
                    Aktiv = a.Aktiv
                })
                .ToListAsync();


            return data;
        }

        // GET: api/DrainPatrols/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DrainPatrol>> GetDrainPatrol(long id)
        {
            var drainPatrol = await _context.DrainPatrols.FindAsync(id);

            if (drainPatrol == null)
            {
                return NotFound();
            }

            return drainPatrol;
        }

        // PUT: api/DrainPatrols/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDrainPatrol(long id, DrainPatrol drainPatrol)
        {
            if (id != drainPatrol.Id)
            {
                return BadRequest();
            }

            _context.Entry(drainPatrol).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DrainPatrolExists(id))
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

        // POST: api/DrainPatrols
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<DrainPatrol>> PostDrainPatrol(DrainPatrolViewModel drainPatrolViewModel)
        {
            string wellName = string.Concat(drainPatrolViewModel.Slinga, drainPatrolViewModel.Address);

            var well = await _context.Wells.FirstOrDefaultAsync();

            if (well == null )
            {
                return NotFound();
            }

            var insertData = new DrainPatrol
            {
                WellId = well.Id,
                Slinga = drainPatrolViewModel.Slinga,
                Address = drainPatrolViewModel.Address,
                Tid = drainPatrolViewModel.Tid,
                Aktiv = drainPatrolViewModel.Aktiv,

            };



            _context.DrainPatrols.Add(insertData);
            await _context.SaveChangesAsync();

            return Ok(drainPatrolViewModel);
        }

        // DELETE: api/DrainPatrols/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<DrainPatrol>> DeleteDrainPatrol(long id)
        {
            var drainPatrol = await _context.DrainPatrols.FindAsync(id);
            if (drainPatrol == null)
            {
                return NotFound();
            }

            _context.DrainPatrols.Remove(drainPatrol);
            await _context.SaveChangesAsync();

            return drainPatrol;
        }

        private bool DrainPatrolExists(long id)
        {
            return _context.DrainPatrols.Any(e => e.Id == id);
        }
    }
}
