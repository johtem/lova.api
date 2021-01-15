using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LOVA.API.Models
{
    public class ActivityCount
    {
        public long Id { get; set; }

        [Required]
        public string Address { get; set; }

        public DateTime Hourly { get; set; }

        public int CountActivity { get; set; }

        public int AverageCount { get; set; }
    }
}
