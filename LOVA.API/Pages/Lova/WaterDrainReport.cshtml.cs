using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using LOVA.API.Services;
using LOVA.API.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace LOVA.API.Pages.Lova
{
    [Authorize(Policy = "RequireLovaRole")]
    public class WaterDrainReportModel : PageModel
    {

        private readonly LovaDbContext _context;

        public WaterDrainReportModel(LovaDbContext context)
        {
            _context = context;
        }

        public void OnGet()
        {

        }

        public async Task<JsonResult> OnPostRead([DataSourceRequest] DataSourceRequest request)
        {
            var readData = await _context.IssueReports
                .Include(a => a.Well)
                .Select(a => new IssueReportViewModel
                        {
                            Id = a.Id, 
                            WellName = a.Well.WellName,
                            CreatedAt = a.CreatedAt,
                            ProblemDescription = a.ProblemDescription,
                            SolutionDescription = a.SolutionDescription,
                            NewActivatorSerialNumber = a.NewActivatorSerialNumber,
                            NewValveSerialNumber = a.NewValveSerialNumber,
                            IsChargeable = a.IsChargeable,
                            IsPhoto = a.IsPhoto
                        }
                    )
                .OrderByDescending(a => a.CreatedAt)
                .ToListAsync();

            return new JsonResult(readData.ToDataSourceResult(request));

        }
    }
}