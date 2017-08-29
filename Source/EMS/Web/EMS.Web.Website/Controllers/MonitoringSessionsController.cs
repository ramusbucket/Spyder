using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using EMS.Core.Models;
using EMS.Core.Models.Mongo;
using MongoDB.Driver;

namespace EMS.Web.Website.Controllers
{
    public class MonitoringSessionsController : Controller
    {
        private readonly IMongoCollection<MonitoringSessionMongoDocument> _monitoringSessionsCollection;
        private readonly IMongoCollection<CapturedActiveProcessesMongoDocument> _activeProcessesCollection;
        private readonly IMongoCollection<CapturedForegroundProcessMongoDocument> _foregroundProcessesCollection;
        private readonly IMongoCollection<CapturedNetworkPacketMongoDocument> _networkPacketsCollection;
        private readonly IMongoCollection<CapturedCameraSnapshotMongoDocument> _cameraSnapshotsCollection;
        private readonly IMongoCollection<CapturedKeyboardKeyMongoDocument> _keyboardKeysCollection;
        private readonly IMongoCollection<CapturedDisplaySnapshotMongoDocument> _displaySnapshotsCollection;

        public MonitoringSessionsController()
        {
            var mongoClient = new MongoClient("mongodb://localhost:27017");
            var mongoDb = mongoClient.GetDatabase("Spyder");

            _monitoringSessionsCollection =
                mongoDb.GetCollection<MonitoringSessionMongoDocument>(MongoCollections.MonitoringSessions);

            _activeProcessesCollection =
                mongoDb.GetCollection<CapturedActiveProcessesMongoDocument>(MongoCollections.ActiveProcesses);

            _foregroundProcessesCollection =
                mongoDb.GetCollection<CapturedForegroundProcessMongoDocument>(MongoCollections.ForegroundProcesses);

            _networkPacketsCollection =
                mongoDb.GetCollection<CapturedNetworkPacketMongoDocument>(MongoCollections.NetworkPackets);

            _cameraSnapshotsCollection =
                mongoDb.GetCollection<CapturedCameraSnapshotMongoDocument>(MongoCollections.CameraSnapshots);

            _keyboardKeysCollection =
                mongoDb.GetCollection<CapturedKeyboardKeyMongoDocument>(MongoCollections.CapturedKeyboardKeys);

            _displaySnapshotsCollection =
                mongoDb.GetCollection<CapturedDisplaySnapshotMongoDocument>(MongoCollections.DisplaySnapshots);
        }

        [HttpGet]
        public async Task<JsonResult> GetActiveSessions(int page = 1, int itemsPerPage = 12)
        {
            if (page < 1 || itemsPerPage < 1)
            {
                var message =
                    $"Parameters: \"{nameof(page)}\" and \"{nameof(itemsPerPage)}\" must not have a value lower than 1.";

                return Json(
                    new EmptyResponse(message, false), JsonRequestBehavior.AllowGet);
            }

            var activeSessionsCursor = await _monitoringSessionsCollection.FindAsync(
                x => x.IsActive,
                new FindOptions<MonitoringSessionMongoDocument, MonitoringSessionMongoDocument>()
                {
                    AllowPartialResults = true,
                    Limit = itemsPerPage,
                    Skip = (page - 1) * itemsPerPage,
                    Sort = new SortDefinitionBuilder<MonitoringSessionMongoDocument>().Descending(x => x.CreatedAt)
                });

            var activeSessions = await activeSessionsCursor.ToListAsync();

            return Json(
                new ResultResponse<List<MonitoringSessionMongoDocument>>(activeSessions), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<JsonResult> GetCameraSnapshots(string sessionId, int page = 1, int itemsPerPage = 3)
        {
            if (string.IsNullOrWhiteSpace(sessionId))
            {
                var message =
                    $"Parameter: \"{nameof(sessionId)}\" must not have a value of null, empty or whitespace.";

                return Json(
                    new EmptyResponse(message, false), JsonRequestBehavior.AllowGet);
            }

            var cameraSnapshotsCursor = await _cameraSnapshotsCollection.FindAsync(
                x => x.SessionId == sessionId,
                new FindOptions<CapturedCameraSnapshotMongoDocument, CapturedCameraSnapshotMongoDocument>()
                {
                    AllowPartialResults = true,
                    Limit = itemsPerPage,
                    Skip = (page - 1) * itemsPerPage,
                    Sort = new SortDefinitionBuilder<CapturedCameraSnapshotMongoDocument>().Descending(x => x.CreatedOn)
                });

            var cameraSnapshots = await cameraSnapshotsCursor.ToListAsync();

            return Json(
                new ResultResponse<List<CapturedCameraSnapshotMongoDocument>>(cameraSnapshots), JsonRequestBehavior.AllowGet);
        }
    }
}