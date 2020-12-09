using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LOVA.API.Models;
using LOVA.API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace LOVA.API.Pages.Lova
{
    public class FixActivityPerRowTableModel : PageModel
    {
        private readonly LovaDbContext _context;

        public FixActivityPerRowTableModel(LovaDbContext context)
        {
            _context = context;
        }

        public async Task OnGet()
        {
            var response = await  _context.ActivityPerRows.Where(a => a.Id >= 9500 && a.Id < 9929).ToListAsync();

            response.ForEach(m => m.TimeUp = m.TimeUp.AddHours(1));

            // _context.SaveChanges();


        }
    }
}
