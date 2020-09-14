using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LOVA.API.Models
{
    public class DrainPatrol
    {
        public long Id { get; set; }
        public int Master_node { get; set; }

        public int Index { get; set; }

        public string Address { get; set; }

        public DateTime Time { get; set; }
        public bool Active { get; set; }

    }
}
