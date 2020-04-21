using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LOVA.API.Services;
using LOVA.API.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace LOVA.API.Pages.Lova
{
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

        public void OnPost()
        {
            var apa = IssueReportViewModel.ProblemDescription;
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