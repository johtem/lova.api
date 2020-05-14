using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LOVA.API.Models;
using LOVA.API.Services;
using LOVA.API.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace LOVA.API.Pages.Lova
{
    [Authorize(Policy = "RequireLovaRole")]
    public class AddNewActivityModel : PageModel
    {
        private readonly LovaDbContext _context;

        public AddNewActivityModel(LovaDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public IssueReportViewModel IssueReportViewModel { get; set; }
        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }


          var well = await _context.Wells.Where(a => a.WellName == IssueReportViewModel.WellName).FirstOrDefaultAsync();

            if (well == null)
            {

            }

            IssueReport insertData = new IssueReport
            {
                WellId = well.Id,
                ProblemDescription = IssueReportViewModel.ProblemDescription,
                SolutionDescription = IssueReportViewModel.SolutionDescription,
                NewActivatorSerialNumber = IssueReportViewModel.NewActivatorSerialNumber,
                NewValveSerialNumber = IssueReportViewModel.NewValveSerialNumber,
                OldActivatorSerialNumber = well.ActivatorSerialNumber,
                OldValveSerialNumber = well.ValveSerialNumber,
                IsChargeable = IssueReportViewModel.IsChargeable,
                IsPhoto = IssueReportViewModel.IsPhoto,
                IsLowVacuum = IssueReportViewModel.IsLowVacuum,
                MasterNode = IssueReportViewModel.MasterNode + 1,
                Alarm = IssueReportViewModel.Alarm + 1,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            
            well.ActivatorSerialNumber = string.IsNullOrEmpty(IssueReportViewModel.NewActivatorSerialNumber) ? well.ActivatorSerialNumber : IssueReportViewModel.NewActivatorSerialNumber;
            well.ValveSerialNumber = string.IsNullOrEmpty(IssueReportViewModel.NewValveSerialNumber) ? well.ValveSerialNumber : IssueReportViewModel.NewValveSerialNumber;
            well.UpdatedAt = DateTime.UtcNow;

            await _context.AddAsync(well);
            _context.Entry(well).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            _context.IssueReports.Add(insertData);
            await _context.SaveChangesAsync();

            return  RedirectToPage("WaterDrainReport");
        }

        public async Task<JsonResult> OnGetWell(string text)
        {
           var wells = _context.Wells;

            if (!string.IsNullOrEmpty(text))
            {
                var apa = wells.Where(p => p.WellName.StartsWith(text));

                return new JsonResult(await apa.ToListAsync());
            }
            

            return new JsonResult(await wells.ToListAsync());
        }
    }
}