﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LOVA.API.ViewModels
{
    public class DrainPatrolAlarmViewModel
    {
        public int Master_node { get; set; }

        public int Index { get; set; }

        public string Address { get; set; }

        public DateTime TimeStamp { get; set; }

        public string AlarmType { get; set; }
        public int Amount { get; set; }
        public int Limit { get; set; }
    }
}
