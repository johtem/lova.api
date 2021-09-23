using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LOVA.API.Models;
using LOVA.API.Services;
using LOVA.API.ViewModels.Lottingelund;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace LOVA.API.Pages.Lottingelund
{
    public class SurveyModel : PageModel
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly LovaDbContext _context;

        public SurveyModel(UserManager<ApplicationUser> userManager, LovaDbContext context)
        {
            _userManager = userManager;
            _context = context;

            // var user = GetUser();
            
        }

        public async Task<ApplicationUser> GetUser()
        {
            return await _userManager.GetUserAsync(HttpContext.User);
        }


        [BindProperty]
        public SurveyViewModel SurveyViewModel { get; set; }

        public async Task<IActionResult> OnGet()
        {
            var user = await GetUser();

            var isAnswered = _context.Surveys.AnyAsync(a => a.UserName == user.UserName);

            if (!isAnswered.Result)
            {
               
                return RedirectToPage("SurveyAnswered");
            }
 

            // TODO: Add check if user already has answer to the survey

            var bepa = 1;

            return Page();

        }


        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }


            var apa = SurveyViewModel;

            return Page();
        }

    }
}
