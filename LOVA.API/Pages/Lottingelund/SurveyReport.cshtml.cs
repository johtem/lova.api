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

namespace LOVA.API.Pages.Lottingelund
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

        public int NumberOfAnswerQuestion2Alt1 { get; set; }
        public int NumberOfAnswerQuestion2Alt2 { get; set; }
        public int NumberOfAnswerQuestion2Alt3 { get; set; }

        public string TextOfAnswerQuestion1Alt1 { get; set; } = "VA - engångsbelopp";
        public string TextOfAnswerQuestion1Alt2 { get; set; } = "VA - kvartalsvis";
        public string TextOfAnswerQuestion1Alt3 { get; set; } = "VA - ombilda till samfällighet";

        public string TextOfAnswerQuestion2Alt1 { get; set; } = "Väg - engångsbelopp";
        public string TextOfAnswerQuestion2Alt2 { get; set; } = "Väg - kvartalsvis";
        public string TextOfAnswerQuestion2Alt3 { get; set; } = "Väg - exernt lån";

        public async Task OnGetAsync()
        {
            Survey = await _context.Surveys.Where(a => a.SurveyName == "Investeringar").ToListAsync();
            NumberOfAnswer = Survey.Count;
            NumberOfAnswerQuestion1Alt1 = Survey.Where(a => a.Query1 == TextOfAnswerQuestion1Alt1).Count();
            NumberOfAnswerQuestion1Alt2 = Survey.Where(a => a.Query1 == TextOfAnswerQuestion1Alt2).Count();
            NumberOfAnswerQuestion1Alt3 = Survey.Where(a => a.Query1 == TextOfAnswerQuestion1Alt3).Count();

            NumberOfAnswerQuestion2Alt1 = Survey.Where(a => a.Query2 == TextOfAnswerQuestion2Alt1).Count();
            NumberOfAnswerQuestion2Alt2 = Survey.Where(a => a.Query2 == TextOfAnswerQuestion2Alt2).Count();
            NumberOfAnswerQuestion2Alt3 = Survey.Where(a => a.Query2 == TextOfAnswerQuestion2Alt3).Count();


        }
    }
}
