using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LOVA.API.Models
{

    public enum Status
    {
        Påbörjad,
        Slutförd,
        Parkerad
    }

    public class LovaIssue
    {
        
        public long Id { get; set; }

        public Status Status { get; set; }

        public string Action { get; set; }

        public string Response { get; set; }
        public string OwnedBy { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
