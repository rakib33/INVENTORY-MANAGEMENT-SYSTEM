using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;



namespace BookStore.Hubs
{
    public class CustomerHub : Hub
    {
        [HubMethodName("BroadcastData")]
        public static void BroadcastData()
        {
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<CustomerHub>();
            context.Clients.All.updatedData();
        }

    }
}