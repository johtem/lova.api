using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LOVA.API.Models
{
    public class ActivityPerRow
    {
        public long Id { get; set; }

        public string Address { get; set; }

        public bool IsGroupAddress { get; set; }

        public DateTime TimeUp { get; set; }
        public DateTime TimeDown { get; set; }

        public double TimeDiff { get; set; }

    }
}
