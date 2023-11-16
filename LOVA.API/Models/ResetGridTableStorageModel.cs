using Azure;
using Azure.Data.Tables;
using System;

namespace LOVA.API.Models
{
    public class ResetGridTableStorageModel : ITableEntity
    {

        public ResetGridTableStorageModel()
        {
                
        }

        public ResetGridTableStorageModel(string index, string node, DateTime resetDate)
        {
            PartitionKey = index;
            RowKey = node;
            ResetDate = resetDate;
        }

        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }

        public DateTime ResetDate { get; set; }
    }
}
