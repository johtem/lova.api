using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LOVA.API.ViewModels
{
    public class EditUserViewModel
    {
        public EditUserViewModel()
        {
            Claims = new List<string>();
            Roles = new List<string>();
        }

        public string Id { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public string City { get; set; }

        
        public string Property { get; set; }

        public string Email2 { get; set; }

        [Required]
        public string ForeName { get; set; }

        [Required]
        public string LastName { get; set; }

        public string ForeName2 { get; set; }
        public string LastName2 { get; set; }

        public List<string> Claims { get; set; }

        public IList<string> Roles { get; set; }
    }
}
