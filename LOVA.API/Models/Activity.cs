﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LOVA.API.Models
{
    public class Activity
    {
        public long Id { get; set; }
        public int Master_node { get; set; }

        public int Index { get; set; }

        [StringLength(10)]
        public string Address { get; set; }

        public DateTime Time { get; set; }
        public bool Active { get; set; }

    }
}
