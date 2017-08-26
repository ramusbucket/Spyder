
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using EMS.Web.Worker.MongoSaver.Controllers;
using EMS.Web.Worker.MongoSaver.Models;
using Newtonsoft.Json;

namespace EMS.Web.Worker.MongoSaver.Hubs
{
    public class ServiceStatisticsHub : Hub
    {
        public void PushServiceStatusToAllClients()
        {
            var statistics = HomeController.savers?.Where(x => x.Statistics != null).Select(x => x.Statistics).ToList();
            PushStatistics(statistics);
        }

        private void PushStatistics(List<ServiceStatistics> statistics)
        {
            var statsAsJson = JsonConvert.SerializeObject(statistics);
            Clients.All.pushStatistics(statsAsJson);
        }
    }

    public class ServicesStatistics
    {
        public List<ServiceStatistics> Value { get; set; }
    }
}