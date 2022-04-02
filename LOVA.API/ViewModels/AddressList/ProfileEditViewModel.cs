using System.ComponentModel.DataAnnotations;

namespace LOVA.API.ViewModels.AddressList
{
    public class ProfileEditViewModel
    {
        public long Id { get; set; }

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

        [Display(Name = "Adress")]
        public string Address { get; set; }

        [Display(Name = "Intagsenhet")]
        public string WellName { get; set; }

        [Display(Name = "Fastighet")]
        public string Property { get; set; }

        [Display(Name="SMS utskick?")]
        public bool WantInfoSMS { get; set; }

        [Display(Name = "E-post utskick?")]
        public bool WantInfoEmail { get; set; }

        [Display(Name="Aktiv?")]
        public bool IsActive { get; set; }
    }
}
