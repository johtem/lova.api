
using Azure;
using Microsoft.Azure.Cosmos.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LOVA.API.Models
{
    public class DrainTableStorageEntity : TableEntity
    {
        public DrainTableStorageEntity()
        {
                
        }

        public DrainTableStorageEntity(string masterNode, string address)
        {
            PartitionKey = masterNode;
            RowKey = address;
        }


        public DateTime TimeUp { get; set; }
        public DateTime TimeDown { get; set; }

        public bool IsActive { get; set; }

        public int HourlyCount { get; set; }

        public int DailyCount { get; set; }

        public int AverageActivity { get; set; }

        public int AverageRest { get; set; }

        public int NumberOfHouses { get; set; }



    }
}
