
using LOVA.API.Services;
using LOVA.API.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MimeKit.Encodings;
using Org.BouncyCastle.Math.EC.Rfc7748;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LOVA.API.Pages.Shared.Components.LatestActivityWidget
{
    public class LatestActivityWidgetViewComponent : ViewComponent
    {
        private readonly LovaDbContext _context;

        public LatestActivityWidgetViewComponent(LovaDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(int dashboardItemSize)
        {

            IEnumerable<WellsDashboardViewModel> response = await GetLatestActivityAsync(dashboardItemSize);

            return View("Default", response);
        }


        private async Task<IEnumerable<WellsDashboardViewModel>> GetLatestActivityAsync(int dashboardItemSize)
        {
            IEnumerable<WellsDashboardViewModel> data = await _context.ActivityPerRows
                .Where(a => !a.Address.Contains("8"))
                .Where(a => !a.Address.Contains("7"))
                .GroupBy(x => x.Address, (x, y) => new WellsDashboardViewModel
                {
                    Address = x,
                    Date = y.Max(z => z.TimeUp)
                })
                .OrderBy(n => n.Date)
                .Take(dashboardItemSize)
                .ToListAsync();



            //IEnumerable<WellsDashboardViewModel>  NoActivities =  from n in _context.Activities
            //               where n.Active == true
            //               group n by n.Address into g
            //               select new WellsDashboardViewModel
            //               {
            //                   Address = g.Key,
            //                   Date = g.Max(t => t.Time)
            //               };


            //NoActivities = NoActivities.OrderBy(n => n.Date)
            //                        .Where(a => !EF.Functions.Like(a.Address, "%8"))
            //                        .Where(a => !EF.Functions.Like(a.Address, "%7"))
            //                        .Take(dashboardItemSize);
                                    

            return data;
        }
    }
}
