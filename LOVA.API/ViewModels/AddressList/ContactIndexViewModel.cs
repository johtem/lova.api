using System;
using System.ComponentModel.DataAnnotations;

namespace LOVA.API.ViewModels.AddressList
{
    public class ContactIndexViewModel
    {
        public long PremiseContactId { get; set; }

        [Display(Name = "Adress")]
        public string Address { get; set; }

        [Display(Name = "Intagsenhet")]
        public string WellName { get; set; }

        [Display(Name = "Fastighet")]
        public string Property { get; set; }

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

        public bool IsActive { get; set; }

        public DateTime SortDate { get; set; }

        public string FullName
        {
            get
            {
                if (string.IsNullOrEmpty(FirstName))
                {
                    return "";
                }
                else
                {

                    return $"{FirstName} {LastName}";
                }
            }

        }

    }
}
