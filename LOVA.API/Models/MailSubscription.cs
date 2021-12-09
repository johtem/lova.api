using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LOVA.API.Models
{
    public class MailSubscription
    {
        public long Id { get; set; }

        public string Name { get; set; }
        public string Email { get; set; }

        public bool IsScription { get; set; }

        public long MailTypeId { get; set; }
        public MailType MailType { get; set; }
    }
}
