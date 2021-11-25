using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LOVA.API.Pages.Lottingelund
{
    [Authorize(Roles = "Admin, Styrelse")]
    public class SurveyIntroModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
