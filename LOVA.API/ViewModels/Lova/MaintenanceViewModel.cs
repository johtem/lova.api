using LOVA.API.Models.Lova;
using System;
using System.ComponentModel.DataAnnotations;

namespace LOVA.API.ViewModels.Lova
{
    public class MaintenanceViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Underhållsgrupp")]
        public MaintenanceGroup MaintenanceGroup { get; set; }

        [Display(Name = "Namn")]
        public string Name { get; set; }

        [Display(Name = "Intervall (månad)")]
        public int RecurringFrequence { get; set; }

        [DataType(DataType.Date)]
        [Display(Name="Senaste")]
        public DateTime LastMaintenance { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Nästa")]
        public DateTime NextMaintenance 
        { 
            get 
            {
                return LastMaintenance.AddMonths(RecurringFrequence);              
            } 

        }
    }
}
