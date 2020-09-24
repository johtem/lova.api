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
using Org.BouncyCastle.Math.EC.Rfc7748;

namespace LOVA.API.Controllers
{
    [Route("api/[controller]")]
    [ApiExplorerSettings(GroupName = @"IssueReports")]
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
                    ImageName = a.Photo,                   
                    IsChargeable = a.IsChargeable,
                    TimeForAlarm = a.TimeForAlarm,
                    TimeToRepair = a.TimeToRepair,
                    ArrivalTime = a.ArrivalTime,
                    CreatedAt = a.CreatedAt
                })
                .OrderByDescending(a => a.CreatedAt)
                .ToListAsync();

            return data;
        }

        [HttpGet("CasePerWell")]
        public async Task<ActionResult<IEnumerable<WellCountViewModel>>> GetWells(string fromDate)
        {

            DateTime fromD = Convert.ToDateTime(fromDate);



            ActionResult<IEnumerable<WellCountViewModel>> data =      
                              await (from p in _context.IssueReports
                                     where p.CreatedAt >= fromD
                                     group p by p.Well.WellName into g
                                     orderby g.Count() descending
                                     select new WellCountViewModel
                                      {
                                          WellName = g.Key,
                                          Case = g.Count()
                                      }).Take(5).ToListAsync();


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
                Photo = issueReport.ImageName,
                OldActivatorSerialNumber = well.ActivatorSerialNumber,
                OldValveSerialNumber = well.ValveSerialNumber,
                TimeForAlarm = issueReport.TimeForAlarm,
                ArrivalTime = issueReport.ArrivalTime,
                TimeToRepair = issueReport.TimeToRepair,
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

        

        private bool IssueReportExists(long id)
        {
            return _context.IssueReports.Any(e => e.Id == id);
        }
    }
}
