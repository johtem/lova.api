using LOVA.API.Models;
using LOVA.API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LOVA.API.Pages.Shared.Components.ActivityPerRowWidget
{
    public class ActivityPerRowWidgetViewComponent : ViewComponent
    {
        private readonly LovaDbContext _context;

        public ActivityPerRowWidgetViewComponent(LovaDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(DateTime startDate, DateTime endDate, string address)
        {

            IEnumerable<ActivityPerRow> response = await GetActivitiesAsync(startDate, endDate, address);

            return View("Default", response);
        }

        private async Task<IEnumerable<ActivityPerRow>> GetActivitiesAsync(DateTime startDate, DateTime endDate, string address)
        {
            return await _context.ActivityPerRows.Where(a => a.Address == address && a.TimeUp >= startDate && a.TimeUp <= endDate).OrderByDescending(a => a.TimeUp).ToArrayAsync();
        }
    }
}
