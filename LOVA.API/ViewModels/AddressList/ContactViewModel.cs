using System;
using System.ComponentModel.DataAnnotations;

namespace LOVA.API.ViewModels.AddressList
{
    public class ContactViewModel
    {
        public long PremiseId { get; set; }

        [Display(Name = "Förnamn")]
        public string FirstName { get; set; }

        [Display(Name = "Efternamn")]
        public string LastName { get; set; }

        [Display(Name = "Epost")]
        public string Email { get; set; }

        [Display(Name = "Mobil")]
        public string MobileNumber { get; set; }

        [Display(Name = "Hemtelefon")]
        public string PhoneNumber { get; set; }

        [Display(Name = "SMS utskick?")]
        public bool WantInfoSMS { get; set; }

        [Display(Name = "E-post utskick?")]
        public bool WantInfoEmail { get; set; }


    }
}
