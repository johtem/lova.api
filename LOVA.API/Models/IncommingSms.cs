using System;

namespace LOVA.API.Models
{
    public class IncommingSms
    {
        public int Id { get; set; }
        public string To { get; set; }
        public string From { get; set; }
        public string Message { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
