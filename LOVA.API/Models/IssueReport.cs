using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LOVA.API.Models
{
    public class IssueReport
    {
        public long Id { get; set; }
        public long WellId { get; set; }
        public Well Well { get; set; }

        public string ProblemDescription { get; set; }

        public string SolutionDescription { get; set; }
        public string NewActivatorSerialNumber { get; set; }
        public string NewValveSerialNumber { get; set; }
        public string OldActivatorSerialNumber { get; set; }
        public string OldValveSerialNumber { get; set; }
        public bool IsChargeable { get; set; }

        public bool IsPhoto { get; set; }

        public string Photo { get; set; }
        public bool IsLowVacuum { get; set; }

        public int MasterNode { get; set; }

        public int Alarm { get; set; }
        

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

    }
}
