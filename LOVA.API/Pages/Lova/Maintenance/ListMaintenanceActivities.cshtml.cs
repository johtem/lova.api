﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using LOVA.API.Models.Lova;
using LOVA.API.Services;
using LOVA.API.ViewModels.Lova;

namespace LOVA.API.Pages.Lova.Maintenance
{
    public class ListMaintenanceActivitiesModel : PageModel
    {
        private readonly LOVA.API.Services.LovaDbContext _context;

        public ListMaintenanceActivitiesModel(LOVA.API.Services.LovaDbContext context)
        {
            _context = context;
        }

        public IList<MaintenanceViewModel> Maintenance { get;set; }

        public async Task OnGetAsync()
        {
            Maintenance = await _context.Maintenances
                .Include(a => a.LatestMaintenances) 
                .Include(a => a.Association)
                .Include(a => a.MaintenanceGroup)
                .Select(a => new MaintenanceViewModel
                {
                    Id = a.Id,
                    LastMaintenance = a.LatestMaintenances.Max(d => d.LastMaintenance),
                    Association = a.Association.ShortName,
                    MaintenanceGroup = a.MaintenanceGroup.GroupName,
                    Name = a.Name,
                    RecurringFrequence = a.RecurringFrequence,
                    NextMaintenance = a.LatestMaintenances.Max(d => d.NextMaintenance)
                })
                .OrderBy(a => a.NextMaintenance)
                .ToListAsync();


            
        }
    }
}
