using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
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

        public string AspNetUserId { get; set; }

        public DateTime TimeForAlarm { get; set; }
        public DateTime ArrivalTime { get; set; }

        [Column(TypeName = "decimal(5, 2)")]
        public decimal TimeToRepair { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

    }
}
