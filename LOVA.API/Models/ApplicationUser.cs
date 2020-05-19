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

    }
}
