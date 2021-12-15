using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LOVA.API.Models;
using LOVA.API.Services;
using LOVA.API.ViewModels.Lottingelund;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace LOVA.API.Pages.Lottingelund
{
    [Authorize(Roles = "Admin, Styrelse, User")]
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

            var isAnswered = _context.SurveyAnswereds.AnyAsync(a => a.UserName == user.UserName && a.SurveyName == "Investeringar");

            if (isAnswered.Result)
            {
                return RedirectToPage("SurveyAnswered");
            }



            return Page();

        }


        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await GetUser();


            // Check if logged in user tries to save more than one survey
            var isAnswered = _context.SurveyAnswereds.AnyAsync(a => a.UserName == user.UserName && a.SurveyName == "Investeringar");

            if (isAnswered.Result)
            {
                return RedirectToPage("SurveyAnswered");
            }


            SurveyAnswered sa = new SurveyAnswered
            {
                UserName = user.UserName,
                SurveyName = "Investeringar"
            };

            _context.SurveyAnswereds.Add(sa);
            await _context.SaveChangesAsync();

            Survey survey = new Survey
            {
                SurveyName = "Investeringar",
                Query1 = SurveyViewModel.Query1,
                Query2 = SurveyViewModel.Query2,
                Query3 = SurveyViewModel.Query3,
                //Query4 = SurveyViewModel.Query4,
                //Query5 = SurveyViewModel.Query5,
            };

            _context.Surveys.Add(survey);
            await _context.SaveChangesAsync();

            return RedirectToPage("SurveyThanks");
        }

    }
}
