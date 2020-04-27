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

        
    }
}
