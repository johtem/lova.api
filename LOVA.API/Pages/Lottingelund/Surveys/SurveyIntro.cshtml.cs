using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LOVA.API.Pages.Lottingelund.Surveys
{
    [Authorize(Roles = "Admin, Styrelse, User")]
    public class SurveyIntroModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
