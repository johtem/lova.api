﻿using LOVA.API.Models;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LOVA.API.Services
{
    public class TableStorageUtils
    {
  
        
        //  <QueryData>
        public static async Task<DrainTableStorageEntity> RetrieveEntityUsingPointQueryAsync(CloudTable table, string partitionKey, string rowKey)
        {
            try
            {
                TableOperation retrieveOperation = TableOperation.Retrieve<DrainTableStorageEntity>(partitionKey, rowKey);
                TableResult result = await table.ExecuteAsync(retrieveOperation);
                DrainTableStorageEntity customer = result.Result as DrainTableStorageEntity;
                if (customer != null)
                {
                    Console.WriteLine("\t{0}\t{1}\t{2}\t{3}", customer.PartitionKey, customer.RowKey, customer.TimeUp, customer.TimeDown);
                }

                if (result.RequestCharge.HasValue)
                {
                    Console.WriteLine("Request Charge of Retrieve Operation: " + result.RequestCharge);
                }

                return customer;
            }
            catch (StorageException e)
            {
                Console.WriteLine(e.Message);
                Console.ReadLine();
                throw;
            }
        }


        public static async Task<ResetGridTableStorageEntity> RetrieveResetGridEntityUsingPointQueryAsync(CloudTable table, string partitionKey, string rowKey)
        {
            try
            {
                TableOperation retrieveOperation = TableOperation.Retrieve<ResetGridTableStorageEntity>(partitionKey, rowKey);
                TableResult result = await table.ExecuteAsync(retrieveOperation);
                ResetGridTableStorageEntity customer = result.Result as ResetGridTableStorageEntity;
                if (customer != null)
                {
                    Console.WriteLine("\t{0}\t{1}\t{2}", customer.PartitionKey, customer.RowKey, customer.ResetDate);
                }

                if (result.RequestCharge.HasValue)
                {
                    Console.WriteLine("Request Charge of Retrieve Operation: " + result.RequestCharge);
                }

                return customer;
            }
            catch (StorageException e)
            {
                Console.WriteLine(e.Message);
                Console.ReadLine();
                throw;
            }
        }


        public static IEnumerable<DrainTableStorageEntity> GetAll(CloudTable table, DateTime resetDate) 
        {

            var queryResult = table.ExecuteQuery(new TableQuery<DrainTableStorageEntity>()).ToList();

            // Filter only data from resetDate. Valid 2 hours.
            var now = DateTime.Now;

            if (now > resetDate && now < resetDate.AddHours(MyConsts.resetGridWaitTime))
            {
                queryResult = table.ExecuteQuery(new TableQuery<DrainTableStorageEntity>()).Where(a => a.TimeUp.ToLocalTime() > resetDate).ToList();
            }



            return queryResult;
        }


        //  <InsertItem>
        public static async Task<DrainTableStorageEntity> InsertOrMergeEntityAsync(CloudTable table, DrainTableStorageEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }

            try
            {
                // Create the InsertOrReplace table operation
                TableOperation insertOrMergeOperation = TableOperation.InsertOrMerge(entity);

                // Execute the operation.
                TableResult result = await table.ExecuteAsync(insertOrMergeOperation);
                DrainTableStorageEntity insertedCustomer = result.Result as DrainTableStorageEntity;

                if (result.RequestCharge.HasValue)
                {
                    Console.WriteLine("Request Charge of InsertOrMerge Operation: " + result.RequestCharge);
                }

                return insertedCustomer;
            }
            catch (StorageException e)
            {
                Console.WriteLine(e.Message);
                Console.ReadLine();
                throw;
            }
        }
        //  </InsertItem>


        public static async Task<ResetGridTableStorageEntity> InsertOrMergeResetGridEntityAsync(CloudTable table, ResetGridTableStorageEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }

            try
            {
                // Create the InsertOrReplace table operation
                TableOperation insertOrMergeOperation = TableOperation.InsertOrMerge(entity);

                // Execute the operation.
                TableResult result = await table.ExecuteAsync(insertOrMergeOperation);
                ResetGridTableStorageEntity insertedCustomer = result.Result as ResetGridTableStorageEntity;

                if (result.RequestCharge.HasValue)
                {
                    Console.WriteLine("Request Charge of InsertOrMerge Operation: " + result.RequestCharge);
                }

                return insertedCustomer;
            }
            catch (StorageException e)
            {
                Console.WriteLine(e.Message);
                Console.ReadLine();
                throw;
            }
        }


        //  <DeleteItem>
        public static async Task DeleteEntityAsync(CloudTable table, DrainTableStorageEntity deleteEntity)
        {
            try
            {
                if (deleteEntity == null)
                {
                    throw new ArgumentNullException("deleteEntity");
                }

                TableOperation deleteOperation = TableOperation.Delete(deleteEntity);
                TableResult result = await table.ExecuteAsync(deleteOperation);

                if (result.RequestCharge.HasValue)
                {
                    Console.WriteLine("Request Charge of Delete Operation: " + result.RequestCharge);
                }

            }
            catch (StorageException e)
            {
                Console.WriteLine(e.Message);
                Console.ReadLine();
                throw;
            }
        }
        //  </DeleteItem>

        /// <summary>
        /// Check if given connection string is for Azure Table storage or Azure CosmosDB Table.
        /// </summary>
        /// <returns>true if azure cosmosdb table</returns>
        public static bool IsAzureCosmosdbTable()
        {
             return true;
        }
    }
}
