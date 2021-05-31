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
    [ApiExplorerSettings(GroupName = @"Wells")]
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
            var data = await _context.Wells.Take(10).ToListAsync();
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

        // GET: 
        [HttpGet("byMasterNode")]
        public async Task<ActionResult<IEnumerable<Well>>> GetWells([FromQuery]  int masterNode)
        {
            var data = await _context.Wells.Where(a => a.MasterNode == masterNode).ToListAsync();

            return data;
        }


        // PUT: api/Premises/5
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

        private bool WellExists(long id)
        {
            return _context.Wells.Any(e => e.Id == id);
        }


    }
}
