using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LOVA.API.ViewModels.Components
{
    public class ActivityCountSeriesViewModel
    {
        public string Name { get; set; }
        public string Stack { get; set; }
        public IEnumerable<int> Data { get; set; }
    }
}
