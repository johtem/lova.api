using System;
using System.ComponentModel.DataAnnotations;

namespace LOVA.API.Models
{
    public class SmsMessage
    {
        public string To { get; set; }
        public string From { get; set; }
        public string Message { get; set; }

        [Display(Name = "Välj")]
        public string ListType { get; set; }


        [Display(Name = "VA Slinga 1")]
        public bool IsNode1 { get; set; }

        [Display(Name = "VA Slinga 2")]
        public bool IsNode2 { get; set; }

        [Display(Name = "VA Slinga 3")]
        public bool IsNode3 { get; set; }


    }
}
