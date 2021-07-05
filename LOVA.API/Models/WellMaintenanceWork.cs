using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LOVA.API.Models
{
    public class WellMaintenanceWork
    {
        public long Id { get; set; }
        public long WellId { get; set; }
        public Well Well { get; set; }

        public bool IsPhoto { get; set; }
        public string Photo { get; set; }

        public string WorkBy { get; set; }

        public string WorkDescription { get; set; }

        public DateTime CreatedAt { get; set; }

    }
}
