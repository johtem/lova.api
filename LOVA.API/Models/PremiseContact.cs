using System;
using System.ComponentModel.DataAnnotations;

namespace LOVA.API.Models
{
    public class PremiseContact
    {
        public long Id { get; set; }

        public long PremiseId { get; set; }
        public Premise Premise { get; set; }

        
        public string FirstName { get; set; }

        
        public string LastName { get; set; }

        
        public string Email { get; set; }

       
        public string MobileNumber { get; set; }

       
        public string PhoneNumber { get; set; }

        public bool IsActive { get; set; }

        public bool WantInfoSMS { get; set; }

        public bool WantInfoEmail { get; set; }


        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

    }
}
