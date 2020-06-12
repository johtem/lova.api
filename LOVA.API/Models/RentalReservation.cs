using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LOVA.API.Models
{
    public class RentalReservation
    {
        public int Id { get; set; }
        public int RentalInventoryId { get; set; }

        public RentalInventory RentalInventory { get; set; }

        public int NumberOf { get; set; }

        public string AspNetUserId { get; set; }

        public string Description { get; set; }

        public int RentalPaymentId { get; set; }
        public DateTime PickupDate { get; set; }
        public DateTime ReturnDate { get; set; }
    }
}
