using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LOVA.API.Models;
using LOVA.API.Services;
using LOVA.API.Filter;

namespace LOVA.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiKeyAuth]
    public class WellsController : ControllerBase
    {
        private readonly LovaDbContext _context;

        public WellsController(LovaDbContext context)
        {
            _context = context;
        }

        // GET: api/Wells
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Well>>> GetWells()
        {
            var data = await _context.Wells.ToListAsync();
            return data;
        }

        // GET: api/Wells/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Well>> GetWell(long id)
        {
            var well = await _context.Wells.FindAsync(id);

            if (well == null)
            {
                return NotFound();
            }

            return well;
        }

        // PUT: api/Wells/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutWell(long id, Well well)
        {
            if (id != well.Id)
            {
                return BadRequest();
            }

            _context.Entry(well).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WellExists(id))
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

        // POST: api/Wells
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<Well>> PostWell(Well well)
        {
            _context.Wells.Add(well);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetWell", new { id = well.Id }, well);
        }

        // DELETE: api/Wells/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Well>> DeleteWell(long id)
        {
            var well = await _context.Wells.FindAsync(id);
            if (well == null)
            {
                return NotFound();
            }

            _context.Wells.Remove(well);
            await _context.SaveChangesAsync();

            return well;
        }

        private bool WellExists(long id)
        {
            return _context.Wells.Any(e => e.Id == id);
        }
    }
}
