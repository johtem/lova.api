using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LOVA.API.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Property { get; set; }

        public string Email2 { get; set; }

        public string ForeName { get; set; }        
        public string LastName { get; set; }

        public string ForeName2 { get; set; }
        public string LastName2 { get; set; }


        public string PostalCode { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public string Phone { get; set; }
        public string Phone2 { get; set; }







        public string FullName
        {
            get
            {
                if (string.IsNullOrEmpty(ForeName))
                {
                    return "";
                }
                else
                {
                    
                    return $"{ForeName} {LastName}";
                }
            }
            
        }




    }
}
