using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using LOVA.API.Models;
using LOVA.API.Services;
using LOVA.API.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace LOVA.API.Pages.Lova
{
    [Authorize(Roles = "Lova, Admin, Styrelse")]
    public class WaterDrainReportModel : PageModel
    {

        private readonly LovaDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public WaterDrainReportModel(LovaDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public void OnGet()
        {

        }

        public async Task<JsonResult> OnPostRead([DataSourceRequest] DataSourceRequest request)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

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
                            TimeForAlarm = a.TimeForAlarm,
                            TimeToRepair = a.TimeToRepair,
                            ArrivalTime = a.ArrivalTime,
                            ImageName = a.Photo,
                            IsChargeable = a.IsChargeable,
                            IsPhoto = a.IsPhoto,
                            AspNetUserName = user.UserName
                        }
                    )
                .OrderByDescending(a => a.CreatedAt)
                .ToListAsync();

            return new JsonResult(readData.ToDataSourceResult(request));

        }
    }
}