using LOVA.API.Models;
using LOVA.API.Services;
using LOVA.API.ViewModels.Components;
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

        

        public async Task<IViewComponentResult> InvokeAsync(DateTime startDate, DateTime endDate, List<string> address)
        {

            ActivityPerRowWidgetViewModel response = new ActivityPerRowWidgetViewModel();

            response.ActivityPerRows = await GetActivitiesAsync(startDate, endDate, address);

            response.StartDate = startDate;
            response.EndDate = endDate;

            var apa = response.ActivityPerRows.Count();

            return View("Default", response);
        }

        private async Task<IEnumerable<ActivityPerRow>> GetActivitiesAsync(DateTime startDate, DateTime endDate, List<string> addresses)
        {
            return await _context.ActivityPerRows.Where(a => addresses.Contains(a.Address) && a.TimeUp >= startDate && a.TimeUp <= endDate).OrderByDescending(a => a.TimeUp).ToArrayAsync();
        }
    }
}
