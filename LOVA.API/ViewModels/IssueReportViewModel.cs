using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LOVA.API.ViewModels
{
    public class IssueReportViewModel
    {
        public long Id { get; set; }

        [Required(ErrorMessage = "Fältet är obligatoriskt")]
        public string WellName { get; set; }

        [Required(ErrorMessage = "Fältet är obligatoriskt")]
        public string ProblemDescription { get; set; }

        [Required(ErrorMessage = "Fältet är obligatoriskt")]
        public string SolutionDescription { get; set; }
        public string NewActivatorSerialNumber { get; set; }
        public string NewValveSerialNumber { get; set; }
        public string OldActivatorSerialNumber { get; set; }
        public string OldValveSerialNumber { get; set; }

        public string ImageName { get; set; }
        public bool IsChargeable { get; set; }

        public bool IsPhoto { get; set; }

        public bool IsLowVacuum { get; set; }

        public int MasterNode { get; set; }

        public int Alarm { get; set; }

        public string AspNetUserName { get; set; }

        public DateTime TimeForAlarm { get; set; }
        public DateTime ArrivalTime { get; set; }

        [Column(TypeName = "decimal(5, 2)")]
        public decimal TimeToRepair { get; set; }

        public DateTime CreatedAt { get; set; }

    }
}
