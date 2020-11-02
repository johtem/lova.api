using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LOVA.API.Models;
using LOVA.API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LOVA.API.Pages.Lova
{
    public class DashboardDrainPatrolModel : PageModel
    {
        private readonly LovaDbContext _context;

        public DashboardDrainPatrolModel(LovaDbContext context)
        {
            _context = context;
        }

        public IEnumerable<WellsNoActivity> NoActivities { get; set; }

        public void OnGet()
        {
            NoActivities = from n in _context.DrainPatrols
                    group n by n.Address into g   
                    select new WellsNoActivity
                    {
                        Address = g.Key, 
                        Date = g.Max(t => t.Time) 
                    };

            NoActivities = NoActivities.OrderBy(n => n.Date).Take(5);

        }
    }

    public class WellsNoActivity
    {
        public string Address { get; set; }
        public DateTime Date { get; set; }
    }
}
