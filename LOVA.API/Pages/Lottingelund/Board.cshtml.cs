using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LOVA.API.Pages.Lottingelund
{
    [Authorize(Roles = "User, Admin, Styrelse")]
    public class BoardModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
