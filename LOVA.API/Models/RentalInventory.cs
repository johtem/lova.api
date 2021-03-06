﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LOVA.API.Models
{
    public class RentalInventory
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public int NumberOf { get; set; }

        public string GroupItems { get; set; }

        public string BackgroundColor { get; set; }
        public ICollection<RentalReservation> RentalReservations { get; set; }
    }
}
