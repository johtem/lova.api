using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using LOVA.API.Models.Lova;
using LOVA.API.Services;
using LOVA.API.ViewModels.Lova;
using Microsoft.EntityFrameworkCore;

namespace LOVA.API.Pages.Lova.Maintenance
{
    public class CreateModel : PageModel
    {
        private readonly LOVA.API.Services.LovaDbContext _context;

        public CreateModel(LOVA.API.Services.LovaDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGet()
        {
            var apa = await _context.Associations.ToListAsync();
            ViewData["AssociationList"] = new SelectList(apa, "Id", "ShortName");

            
            
            return Page();
        }


        public async Task<JsonResult> OngetGetMaintenanceGroups(int associationId)
        {
            var groups = await _context.MaintenanceGroups.Where(a => a.AssociationId == associationId).ToListAsync();

            return new JsonResult(groups); 
        }

        [BindProperty]
        public MaintenanceViewModel Maintenance { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }



            var main = new LOVA.API.Models.Lova.Maintenance
            {
                RecurringFrequence = Maintenance.RecurringFrequence,
                Name = Maintenance.Name,
                MaintenanceGroupId = Maintenance.MaintenanceGroupId,
                AssociationId = Maintenance.AssociationId
            };


            _context.Maintenances.Add(main);
            await _context.SaveChangesAsync();

            var latestmain = new LatestMaintenance
            {
                MaintenanceId = main.Id,
                LastMaintenance = Maintenance.LastMaintenance,
                NextMaintenance = Maintenance.LastMaintenance.AddMonths(Maintenance.RecurringFrequence)
            };

            _context.LatestMaintenances.Add(latestmain);
            await _context.SaveChangesAsync();



            return RedirectToPage("./ListMaintenanceActivities");
        }
    }
}
