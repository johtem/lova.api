using LOVA.API.Services;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Azure.Cosmos.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LOVA.API.Hubs
{
    public class ActivationHub : Hub
    {
       
        public string GetServerTime()
        {
            return DateTime.UtcNow.ToString();
        }

        public async Task DrainActivity( string user, string message)
        {
            await Clients.All.SendAsync("DrainActivity",  user, message);
        }

        public async Task Drain(string drain)
        {
            // Create reference an existing table
            CloudTable table = await TableStorageCommon.CreateTableAsync("Drains");

            // Get existing data for address
            var drainExistingRow = await TableStorageUtils.RetrieveEntityUsingPointQueryAsync(table, drain.Substring(0,1), drain);

            DateTime dateNow = DateTime.Now;


            await Clients.Caller.SendAsync("Drain", drain, drainExistingRow, dateNow);
        }
    }
}
