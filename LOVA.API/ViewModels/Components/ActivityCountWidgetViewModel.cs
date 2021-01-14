using LOVA.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LOVA.API.ViewModels.Components
{
    public class ActivityCountWidgetViewModel
    {
        public ActivityCountWidgetViewModel()
        {
            Series = new List<ActivityCountSeriesViewModel>();
            
        }

      

        public List<ActivityCountSeriesViewModel> Series { get; set; }

        public string[] Categories { get; set; }
    }
}
