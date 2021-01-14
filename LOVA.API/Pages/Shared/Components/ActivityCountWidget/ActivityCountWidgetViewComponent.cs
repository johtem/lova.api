using LOVA.API.Extensions;
using LOVA.API.Models;
using LOVA.API.Services;
using LOVA.API.ViewModels.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LOVA.API.Pages.Shared.Components.ActivityCountWidget
{
    public class ActivityCountWidgetViewComponent : ViewComponent
    {
        private readonly LovaDbContext _context;

        public ActivityCountWidgetViewComponent(LovaDbContext context)
        {
            _context = context;
        }


        public async Task<IViewComponentResult> InvokeAsync(DateTime startDate, DateTime endDate, List<string> address)
        {

            //ActivityCountWidgetViewModel response = new ActivityCountWidgetViewModel();

            // var countData = await GetActivityCountAsync(startDate, endDate, address);

            var listOfFullHours = DateExtensions.ListOfFullHours(endDate).ToArray();

            var model = new ActivityCountWidgetViewModel();

            model.Categories = listOfFullHours;


            foreach (var a in address)
            {
                var countData = await GetActivityCountAsync(startDate, endDate, a);

                int[] series = new int[24];

                int x = 0;

                foreach (var h in listOfFullHours)
                {
                    var dataHourly = countData.FirstOrDefault(a => a.Hourly.ToString("yyyy-MM-dd HH:mm") == h);

                    if (dataHourly != null)
                    {
                        series[x] = dataHourly.CountActivity;
                    }
                    else
                    {
                        series[x] = 0;
                    }

                    x += 1;
                }


                model.Series.Add(new ActivityCountSeriesViewModel()
                {
                    Name = a,
                    Stack = a,
                    Data =  series //new int[] { 1, 2, 3 }
                });

            }


            return View("Default", model);
        }

        private async Task<IEnumerable<ActivityCount>> GetActivityCountAsync(DateTime startDate, DateTime endDate, string address)
        {
            return await _context.ActivityCounts
                .Where(a => a.Hourly >= endDate.AddDays(-1) && a.Hourly <= endDate)
                .Where(a => a.Address == address)
                .OrderBy(a => a.Hourly)
                .ToListAsync();

        }


    }
}
