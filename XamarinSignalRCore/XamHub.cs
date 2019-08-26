using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XamarinSignalRClient;

namespace XamarinSignalRCore
{
    public class XamHub : Hub
    {
        public async Task SendTo(string packet, string connectionId)
        {
            var message = new XamHubMessage
            {
                Payload = packet,
                SenderId = Context.ConnectionId,
            };
            await base.Clients.User(connectionId).SendAsync("OnMessage", message);
        }

        public async Task SendOthers(string packet)
        {
            var message = new XamHubMessage
            {
                Payload = packet,
                SenderId = Context.ConnectionId,
            };
            await base.Clients.AllExcept(base.Context.ConnectionId).SendAsync("OnMessage", message);
        }

        public async Task SendAll(string packet)
        {
            var message = new XamHubMessage
            {
                Payload = packet,
                SenderId = Context.ConnectionId,
            };
            await base.Clients.All.SendAsync("OnMessage", message);
        }
    }
}
