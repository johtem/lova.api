using LOVA.API.Models;

namespace LOVA.API.ViewModels.Admin
{
    public class MailSubscriptionViewModel
    {
        public string? Email { get; set; }
        public string MailType { get; set; }

        public bool IsScription { get; set; }

        public long MailSubscriptionId { get; set; }
        public long MailTypeId { get; set; }

    }
}
