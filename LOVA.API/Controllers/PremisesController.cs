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
    [ApiController]
    public class PremisesController : ControllerBase
    {
        private readonly LovaDbContext _context;

        public PremisesController(LovaDbContext context)
        {
            _context = context;
        }

        // GET: api/Premises
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Premise>>> GetPremises()
        {
            var data = await _context.Premises
                .Include(a => a.Well)
                .ToListAsync();

            return data;
        }

        // GET: api/Premises/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Premise>> GetPremise(long id)
        {
            var premise = await _context.Premises.FindAsync(id);

            if (premise == null)
            {
                return NotFound();
            }

            return premise;
        }

        // PUT: api/Premises/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPremise(long id, Premise premise)
        {
            if (id != premise.Id)
            {
                return BadRequest();
            }

            _context.Entry(premise).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PremiseExists(id))
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

        // POST: api/Premises
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<Premise>> PostPremise(Premise premise)
        {
            _context.Premises.Add(premise);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPremise", new { id = premise.Id }, premise);
        }

        // DELETE: api/Premises/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Premise>> DeletePremise(long id)
        {
            var premise = await _context.Premises.FindAsync(id);
            if (premise == null)
            {
                return NotFound();
            }

            _context.Premises.Remove(premise);
            await _context.SaveChangesAsync();

            return premise;
        }

        private bool PremiseExists(long id)
        {
            return _context.Premises.Any(e => e.Id == id);
        }
    }
}
