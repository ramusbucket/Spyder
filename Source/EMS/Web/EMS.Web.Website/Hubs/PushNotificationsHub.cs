using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace EMS.Web.Website.Hubs
{
    public class PushNotificationsHub : Hub
    {
        public void Hello()
        {
            Clients.All.hello();
        }
    }
}