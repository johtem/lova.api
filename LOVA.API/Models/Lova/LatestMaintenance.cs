using System;
using System.ComponentModel.DataAnnotations;

namespace LOVA.API.Models.Lova
{
    public class LatestMaintenance
    {
        public int Id { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Senaste")]
        public DateTime LastMaintenance { get; set; }

        [DataType(DataType.Date)]
        public DateTime NextMaintenance { get; set; }

        public Maintenance Maintenance { get; set; }

        public int MaintenanceId { get; set; }
    }
}
