using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LOVA.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LOVA.API.Pages.Lottingelund
{
    [Authorize(Roles = "User, Admin, Styrelse")]
    public class BoardModel : PageModel
    {

        public LOVA.API.Models.BoardMember Ordf { get; set; }
        public LOVA.API.Models.BoardMember Kansli { get; set; }
        public LOVA.API.Models.BoardMember Kassor { get; set; }
        public LOVA.API.Models.BoardMember Vag { get; set; }
        public LOVA.API.Models.BoardMember VA { get; set; }
        public LOVA.API.Models.BoardMember Tomt { get; set; }


        public void OnGet()
        {
            Ordf = MyConsts.Ordf;
            Kansli = MyConsts.Kansli;
            Kassor = MyConsts.Kassor;
            Vag = MyConsts.Vag;
            VA = MyConsts.VA;
            Tomt = MyConsts.Tomt;
                
        }
    }
}
