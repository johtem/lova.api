using System;

namespace LOVA.API.ViewModels
{
    public class WellsDashboardViewModel
    {
        public string Address { get; set; }
        public DateTime Date { get; set; }

        public int Count { get; set; }

        public bool IsGroupAddress { get; set; }
    }
}
