using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LOVA.API.Models.Lova
{
    public enum MaintenanceGroup
    {
        Avloppstank,
        Vakuumsystem,
        Intagsenheter,
        Styrskåp,
        Ventilkammare,
        Pumphus
    }


    public class Maintenance
    {
        public int Id { get; set; }

        [Display(Name = "Underhållsgrupp")]
        public MaintenanceGroup MaintenanceGroup { get; set; }

        [Display(Name = "Namn")]
        public string Name { get; set; }

        [Display(Name ="Återkommande")]
        public int RecurringFrequence { get; set; }

        public ICollection<LatestMaintenance> LatestMaintenances { get; set; }

    }
}
