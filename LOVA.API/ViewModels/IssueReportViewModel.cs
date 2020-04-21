using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LOVA.API.ViewModels
{
    public class IssueReportViewModel
    {
        public string WellName { get; set; }
        public string ProblemDescription { get; set; }

        public string SolutionDescription { get; set; }
        public string NewActivatorSerialNumber { get; set; }
        public string NewValveSerialNumber { get; set; }
        public string OldActivatorSerialNumber { get; set; }
        public string OldValveSerialNumber { get; set; }
        public bool IsChargeable { get; set; }

        public DateTime CreatedAt { get; set; }

    }
}
