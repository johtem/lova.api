using Kendo.Mvc.UI;
using LOVA.API.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LOVA.API.ViewModels
{
    public class RentalSchedularViewModel : ISchedulerEvent
    {
        public int RentalId { get; set; }

        public int RentalInventoryId { get; set; }
        public string Title { get; set; }

        private DateTime start;

        [Required]
        public DateTime Start
        {
            get
            {
                return start;
            }
            set
            {
                start = value.ToUniversalTime();
            }
        }

   


        private DateTime end;

        [Required]
        [DateGreaterThan(OtherField = "Start")]
        public DateTime End
        {
            get
            {
                return end;
            }
            set
            {
                end = value.ToUniversalTime();
            }
        }
        public string Description { get; set; }

        public string BackgroundColor { get; set; }
        public bool IsAllDay { get; set; }
        public string StartTimezone { get; set; }
        public string EndTimezone { get; set; }
        public string RecurrenceRule { get; set; }
        public string RecurrenceException { get; set; }
        public int? RecurrenceID { get; set; }

        public int NumberOf { get; set; }

        public string AspNetUserId { get; set; }


        public IEnumerable<int> Attendees { get; set; }



    }
}
