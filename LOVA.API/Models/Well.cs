using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LOVA.API.Models
{
    public class Well
    {
        public long Id { get; set; }
        public string WellName { get; set; }
        public string ActivatorSerialNumber  { get; set; }

        public string ValveSerialNumber { get; set; }

        public bool IsActive { get; set; }

        public double? Longitude { get; set; }
        public double? Latitude { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public ICollection<Premise> Premises { get; set; }

        public ICollection<IssueReport> IssueReports { get; set; }

    }
}
