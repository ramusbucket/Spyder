using EMS.Web.Worker.MongoSaver.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace EMS.Web.Worker.MongoSaver.Controllers
{
    public class HomeController : Controller
    {
        private static CancellationToken token = new CancellationToken();
        private static NetworkPacketsSaver saver;

        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }

        public ActionResult RunNetworkPacketsSaver()
        {
            saver = new NetworkPacketsSaver(token);
            saver.Execute();

            return this.View();
        }
    }
}
