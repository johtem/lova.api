using LOVA.API.Extensions;
using System;
using System.ComponentModel.DataAnnotations;

namespace LOVA.API.Models
{
    public class PremiseContact
    {
        public long Id { get; set; }

        public long PremiseId { get; set; }
        public Premise Premise { get; set; }

        [Required(ErrorMessage = "Kan inte vara tomt.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Kan inte vara tomt.")]
        public string LastName { get; set; }

        [RequiredIfTrue(nameof(WantInfoEmail), ErrorMessage = "Kan inte skicka e-post utan e-postadress.")]
        public string Email { get; set; }

        [RequiredIfTrue(nameof(WantInfoSMS), ErrorMessage = "Kan inte skicka sms utan mobiltelefonnummer.")]
        public string MobileNumber { get; set; }

       
        public string PhoneNumber { get; set; }

        public bool IsActive { get; set; }

        public bool IsDeleted { get; set; }

        public bool WantInfoSMS { get; set; }

        public bool WantInfoEmail { get; set; }

        public bool WantGrannsamverkanEmail { get; set; }


        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

    }
}
