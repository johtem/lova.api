using Azure;
using Azure.Data.Tables;
using System;

namespace LOVA.API.Models
{
    public class DrainTableStorageModel : ITableEntity
    {

        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        // public DateTime? Timestamp { get; set; }
        public ETag ETag { get; set; }

        public DateTime TimeUp { get; set; }
        public DateTime TimeDown { get; set; }

        public bool IsActive { get; set; }

        public int HourlyCount { get; set; }

        public int DailyCount { get; set; }

        public int AverageActivity { get; set; }

        public int AverageRest { get; set; }

        public int NumberOfHouses { get; set;}


    }
}
