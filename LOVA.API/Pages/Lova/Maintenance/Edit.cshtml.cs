using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LOVA.API.Models.Lova;
using LOVA.API.Services;

namespace LOVA.API.Pages.Lova.Maintenance
{
    public class EditModel : PageModel
    {
        private readonly LOVA.API.Services.LovaDbContext _context;

        public EditModel(LOVA.API.Services.LovaDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public LatestMaintenance LatestMaintenance { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            LatestMaintenance = await _context.LatestMaintenances
                .Include(l => l.Maintenance)
                .Where(m => m.MaintenanceId == id)
                .OrderByDescending(m => m.LastMaintenance)
                .FirstOrDefaultAsync();

            if (LatestMaintenance == null)
            {
                return NotFound();
            }
           
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            //_context.Attach(LatestMaintenance).State = EntityState.Modified;

            var latestMain = new LatestMaintenance
            {
                LastMaintenance = LatestMaintenance.LastMaintenance,
                NextMaintenance = LatestMaintenance.LastMaintenance.AddMonths(LatestMaintenance.Maintenance.RecurringFrequence),
                MaintenanceId = LatestMaintenance.MaintenanceId
            };

            _context.Add(latestMain);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LatestMaintenanceExists(LatestMaintenance.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./ListMaintenanceActivities");
        }

        private bool LatestMaintenanceExists(int id)
        {
            return _context.LatestMaintenances.Any(e => e.Id == id);
        }
    }
}
