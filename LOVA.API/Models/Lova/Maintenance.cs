using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LOVA.API.Models.Lova
{


    public class Maintenance
    {
        public int Id { get; set; }

        public Association Association { get; set; }

        public int AssociationId { get; set; }

        [Display(Name = "Underhållsgrupp")]
        public MaintenanceGroup MaintenanceGroup { get; set; }

        public int MaintenanceGroupId { get; set; }

        [Display(Name = "Namn")]
        public string Name { get; set; }

        [Display(Name ="Återkommande")]
        public int RecurringFrequence { get; set; }

        public ICollection<LatestMaintenance> LatestMaintenances { get; set; }

    }
}
