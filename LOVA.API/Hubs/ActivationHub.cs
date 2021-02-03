using Microsoft.AspNetCore.SignalR;
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

        public async Task SendMessage( string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage",  user, message);
        }
    }
}
