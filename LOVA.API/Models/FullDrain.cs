using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LOVA.API.Models
{
    public class FullDrain
    {
        public int Id { get; set; }
        public string Address { get; set; }

        public DateTime DateFulldrain { get; set; }

        public DateTime MailSent { get; set; }

        public bool IsUpdated { get; set; }
    }
}
