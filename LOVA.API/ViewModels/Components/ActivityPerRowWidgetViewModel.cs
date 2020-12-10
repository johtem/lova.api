using LOVA.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LOVA.API.ViewModels.Components
{
    public class ActivityPerRowWidgetViewModel
    {
        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public IEnumerable<ActivityPerRow> ActivityPerRows { get; set; }

    }
}
