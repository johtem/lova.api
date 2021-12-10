using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LOVA.API.Models
{
    public class MailType
    {
        public long Id { get; set; }
        public string Type { get; set; }

        public string Description { get; set; }

        public ICollection<MailSubscription> MailSubscriptions { get; set; }

    }
}
