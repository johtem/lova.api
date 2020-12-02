using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LOVA.API.Models
{
    public class Notification
    {
        public int Id { get; set; }

        [Display(Name = "Titel")]
        public string Title { get; set; }

        [Display(Name = "Meddelande")]
        public string Message { get; set; }

        [Display(Name = "Datum från")]
        public DateTime FromDate { get; set; }

        [Display(Name = "Datum till")]
        public DateTime ToDate { get; set; }
    }
}
