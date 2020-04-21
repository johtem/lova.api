using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LOVA.API.Models
{
    public class DrainPatrol
    {
        public long Id { get; set; }
        public string Slinga { get; set; }

        public string Address { get; set; }

        public DateTime Tid { get; set; }
        public bool Aktiv { get; set; }

        public long WellId { get; set; }
        public Well Well { get; set; }
    }
}
