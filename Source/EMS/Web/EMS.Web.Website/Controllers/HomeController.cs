using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MongoDB.Driver;
using EMS.Core.Models.Mongo;

namespace EMS.Web.Website.Controllers
{
    public class HomeController : Controller
    {
        public HomeController()
        {
            var mongoClient = new MongoClient("mongodb://localhost:27017");
            var mongoDb = mongoClient.GetDatabase("Spyder");

            var sessionsCollection =
                mongoDb.GetCollection<MonitoringSessionMongoDocument>(MongoCollections.MonitoringSessions);

            var activeProcessesCollection =
                mongoDb.GetCollection<CapturedActiveProcessesMongoDocument>(MongoCollections.ActiveProcesses);

            var foregroundProcessesCollection =
                mongoDb.GetCollection<CapturedForegroundProcessMongoDocument>(MongoCollections.ForegroundProcesses);

            var networkPacketsCollection =
                mongoDb.GetCollection<CapturedNetworkPacketMongoDocument>(MongoCollections.NetworkPackets);

            var cameraSnapshotsCollection =
                mongoDb.GetCollection<MonitoringSessionMongoDocument>(MongoCollections.CameraSnapshots);

            var keyboardKeysCollection =
                mongoDb.GetCollection<MonitoringSessionMongoDocument>(MongoCollections.CapturedKeyboardKeys);

            var displaySnapshotsCollection =
                mongoDb.GetCollection<MonitoringSessionMongoDocument>(MongoCollections.DisplaySnapshots);

        }
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }

        public JsonResult GetActiveSessions()
        {
            
        }
    }
}
