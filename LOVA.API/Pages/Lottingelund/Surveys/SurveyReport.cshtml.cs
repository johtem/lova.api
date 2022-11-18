using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using LOVA.API.Models;
using LOVA.API.Services;
using Microsoft.AspNetCore.Authorization;

namespace LOVA.API.Pages.Lottingelund.Surveys
{
    [Authorize(Roles = "Admin, Styrelse")]  //Ej andra roller
    public class SurveyReportModel : PageModel
    {
        private readonly LOVA.API.Services.LovaDbContext _context;

        public SurveyReportModel(LOVA.API.Services.LovaDbContext context)
        {
            _context = context;
        }

        public IList<Survey> Survey { get;set; }

        public int NumberOfAnswer { get; set; }
        public int NumberOfAnswerQuestion1Alt1 { get; set; }
        
        public int NumberOfAnswerQuestion1Alt2 { get; set; }
        public int NumberOfAnswerQuestion1Alt3 { get; set; }
        public int NumberOfAnswerQuestion1Alt4 { get; set; }
        public int NumberOfAnswerQuestion1Alt5 { get; set; }
        public int NumberOfAnswerQuestion1Alt6 { get; set; }

        public int NumberOfAnswerQuestion3Alt1 { get; set; }
        public int NumberOfAnswerQuestion3Alt2 { get; set; }
        public int NumberOfAnswerQuestion3Alt3 { get; set; }

        public string TextOfAnswerQuestion1Alt1 { get; set; } = "Fri parkering med privat bolag";
        public string TextOfAnswerQuestion1Alt2 { get; set; } = "Fri parkering med Täby kommun";
        public string TextOfAnswerQuestion1Alt3 { get; set; } = "Enbart vissa sträckor med privat bolag";
        public string TextOfAnswerQuestion1Alt4 { get; set; } = "Enbart vissa sträckor med Täby kommun";
        public string TextOfAnswerQuestion1Alt5 { get; set; } = "Total parkeringsförbund med privat bolag";
        public string TextOfAnswerQuestion1Alt6 { get; set; } = "Total parkeringsförbund med Täby kommun";

        public string TextOfAnswerQuestion3Alt1 { get; set; } = "Införas på vissa av våra vägar med väg-gupp";
        public string TextOfAnswerQuestion3Alt2 { get; set; } = "Införas på vissa av våra vägar med chikaner";
        public string TextOfAnswerQuestion3Alt3 { get; set; } = "Inga åtgärder";

        public async Task OnGetAsync()
        {
            Survey = await _context.Surveys.Where(a => a.SurveyName == "Parkering och Väghinder").ToListAsync();
            NumberOfAnswer = Survey.Count;
            NumberOfAnswerQuestion1Alt1 = Survey.Where(a => a.Query1 == TextOfAnswerQuestion1Alt1).Count();
            NumberOfAnswerQuestion1Alt2 = Survey.Where(a => a.Query1 == TextOfAnswerQuestion1Alt2).Count();
            NumberOfAnswerQuestion1Alt3 = Survey.Where(a => a.Query1 == TextOfAnswerQuestion1Alt3).Count();

            NumberOfAnswerQuestion3Alt1 = Survey.Where(a => a.Query3 == TextOfAnswerQuestion3Alt1).Count();
            NumberOfAnswerQuestion3Alt2 = Survey.Where(a => a.Query3 == TextOfAnswerQuestion3Alt2).Count();
            NumberOfAnswerQuestion3Alt3 = Survey.Where(a => a.Query3 == TextOfAnswerQuestion3Alt3).Count();


        }
    }
}
