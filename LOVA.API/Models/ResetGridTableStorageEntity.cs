using Microsoft.Azure.Cosmos.Table;
using System;

namespace LOVA.API.Models
{
    public class ResetGridTableStorageEntity : TableEntity
    {

        public ResetGridTableStorageEntity()
        {
                
        }

        public ResetGridTableStorageEntity(string index, string node, DateTime resetDate)
        {
            PartitionKey = index;
            RowKey = node;
            ResetDate = resetDate;
        }


        public DateTime ResetDate { get; set; }
    }
}
