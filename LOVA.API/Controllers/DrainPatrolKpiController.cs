using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LOVA.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LOVA.API.Controllers
{
    [Route("api/[controller]")]
    [Route("api/kpis")]
    [ApiController]
    public class DrainPatrolKpiController : ControllerBase
    {
        private readonly LovaDbContext _context;

        public DrainPatrolKpiController(LovaDbContext context)
        {
            _context = context;
        }
    }
}
