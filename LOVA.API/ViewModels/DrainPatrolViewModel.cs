﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LOVA.API.ViewModels
{
    public class DrainPatrolViewModel
    {
        public string Slinga { get; set; }

        public string Address { get; set; }

        public DateTime Tid { get; set; }
        public bool Aktiv { get; set; }
    }
}
