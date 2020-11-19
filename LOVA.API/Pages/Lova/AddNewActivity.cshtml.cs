using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using LOVA.API.Models;
using LOVA.API.Services;
using LOVA.API.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace LOVA.API.Pages.Lova
{
    [Authorize(Roles = "Lova, Admin, Styrelse, VA")]
    public class AddNewActivityModel : PageModel
    {
        private readonly LovaDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IBlobService _blobService;


        public AddNewActivityModel(LovaDbContext context, UserManager<ApplicationUser> userManager, IBlobService blobService)
        {
            _context = context;
            _userManager = userManager;
            _blobService = blobService;
        }

        [BindProperty]
        public IssueReportViewModel IssueReportViewModel { get; set; }


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

            var user = await _userManager.GetUserAsync(HttpContext.User);

            var fileName = "";

            if (IssueReportViewModel.File !=null)
            {

                var fileContent = Microsoft.Net.Http.Headers.ContentDispositionHeaderValue.Parse(IssueReportViewModel.File.ContentDisposition);
                fileName = Path.GetFileName(fileContent.FileName.ToString().Trim('"'));
                var filePath = Path.GetTempFileName();

                string result = await UploadFile(IssueReportViewModel.File, filePath);
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
                Photo = fileName,
                Alarm = IssueReportViewModel.Alarm + 1,
                TimeForAlarm = IssueReportViewModel.TimeForAlarm,
                ArrivalTime = IssueReportViewModel.ArrivalTime,
                TimeToRepair = IssueReportViewModel.TimeToRepair,
                AspNetUserId = user.Id,
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


        public async Task<string> UploadFile(IFormFile file, string filePath)
        {

            try
            {
                await _blobService.UploadFileBlobAsync(file, MyConsts.lovaPhotos);
            }
            catch (Exception)
            {

                return "ERROR";
            }


            return "OK";

            

        }
    }
}