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

namespace LOVA.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IssueReportsController : ControllerBase
    {
        private readonly LovaDbContext _context;

        public IssueReportsController(LovaDbContext context)
        {
            _context = context;
        }

        // GET: api/IssueReports
        [HttpGet]
        public async Task<ActionResult<IEnumerable<IssueReportViewModel>>> GetIssueReports()
        {

            ActionResult<IEnumerable<IssueReportViewModel>>  data = await _context.IssueReports
                .Include(a => a.Well)
                .Select(a => new IssueReportViewModel
                {
                    WellName = a.Well.WellName,
                    ProblemDescription = a.ProblemDescription,
                    SolutionDescription = a.SolutionDescription,
                    NewActivatorSerialNumber = a.NewActivatorSerialNumber,
                    NewValveSerialNumber = a.NewValveSerialNumber,
                    IsChargeable = a.IsChargeable,
                    CreatedAt = a.CreatedAt
                })
                .ToListAsync();

            return data;
        }

        // GET: api/IssueReports/5
        [HttpGet("{id}")]
        public async Task<ActionResult<IssueReport>> GetIssueReport(long id)
        {
            var issueReport = await _context.IssueReports.FindAsync(id);

            if (issueReport == null)
            {
                return NotFound();
            }

            return issueReport;
        }

        // PUT: api/IssueReports/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutIssueReport(long id, IssueReport issueReport)
        {
            if (id != issueReport.Id)
            {
                return BadRequest();
            }

            _context.Entry(issueReport).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!IssueReportExists(id))
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

        // POST: api/IssueReports
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<IssueReport>> PostIssueReport(IssueReportViewModel issueReport)
        {
            var well = await _context.Wells.Where(a => a.WellName == issueReport.WellName).FirstOrDefaultAsync();

            var insertData = new IssueReport
            {
                WellId = well.Id,
                ProblemDescription = issueReport.ProblemDescription,
                SolutionDescription = issueReport.SolutionDescription,
                NewActivatorSerialNumber = issueReport.NewActivatorSerialNumber,
                NewValveSerialNumber = issueReport.NewValveSerialNumber,
                IsChargeable = issueReport.IsChargeable,
                OldActivatorSerialNumber = well.ActivatorSerialNumber,
                OldValveSerialNumber = well.ValveSerialNumber,
                CreatedAt = issueReport.CreatedAt,
                UpdatedAt = issueReport.CreatedAt
            };

            well.ActivatorSerialNumber = string.IsNullOrEmpty(issueReport.NewActivatorSerialNumber) ? well.ActivatorSerialNumber : issueReport.NewActivatorSerialNumber;
            well.ValveSerialNumber = string.IsNullOrEmpty(issueReport.NewValveSerialNumber) ? well.ValveSerialNumber : issueReport.NewValveSerialNumber;
            well.UpdatedAt = DateTime.UtcNow;

            await _context.AddAsync(well);
            _context.Entry(well).State = EntityState.Modified;
            await _context.SaveChangesAsync();
               
            _context.IssueReports.Add(insertData);
            await _context.SaveChangesAsync();

            // return CreatedAtAction("GetIssueReport", new { id = issueReport.Id }, issueReport);
            return Ok(issueReport);
        }

        // DELETE: api/IssueReports/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<IssueReport>> DeleteIssueReport(long id)
        {
            var issueReport = await _context.IssueReports.FindAsync(id);
            if (issueReport == null)
            {
                return NotFound();
            }

            _context.IssueReports.Remove(issueReport);
            await _context.SaveChangesAsync();

            return issueReport;
        }

        private bool IssueReportExists(long id)
        {
            return _context.IssueReports.Any(e => e.Id == id);
        }
    }
}
