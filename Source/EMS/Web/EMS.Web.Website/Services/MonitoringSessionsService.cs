using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using EMS.Core.Models;
using EMS.Core.Models.Mongo;
using MongoDB.Driver;

namespace EMS.Web.Website.Services
{
    public class MonitoringSessionsService : IMonitoringSessionsService
    {

        private readonly IMongoCollection<MonitoringSessionMongoDocument> _monitoringSessionsCollection;
        private readonly IMongoCollection<CapturedActiveProcessesMongoDocument> _activeProcessesCollection;
        private readonly IMongoCollection<CapturedForegroundProcessMongoDocument> _foregroundProcessesCollection;
        private readonly IMongoCollection<CapturedNetworkPacketMongoDocument> _networkPacketsCollection;
        private readonly IMongoCollection<CapturedCameraSnapshotMongoDocument> _cameraSnapshotsCollection;
        private readonly IMongoCollection<CapturedKeyboardKeyMongoDocument> _keyboardKeysCollection;
        private readonly IMongoCollection<CapturedDisplaySnapshotMongoDocument> _displaySnapshotsCollection;

        public MonitoringSessionsService()
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

        public async Task<IEnumerable<MonitoringSessionMongoDocument>> GetActiveSessions(
            int page = 1, 
            int itemsPerPage = 12)
        {
            if (page < 1)
            {
                var message = $"Parameter: \"{nameof(page)}\" must not have a value lower than 1.";
                throw new ArgumentOutOfRangeException(nameof(page),message);
            }

            if (itemsPerPage < 1)
            {
                var message = $"Parameter: \"{nameof(itemsPerPage)}\" must not have a value lower than 1.";
                throw new ArgumentOutOfRangeException(nameof(itemsPerPage), message);
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

            return await activeSessionsCursor.ToListAsync();
        }

        public async Task<IEnumerable<CapturedCameraSnapshotMongoDocument>> GetCameraSnapshots(
            string sessionId, 
            int page = 1, 
            int itemsPerPage = 3)
        {
            if (page < 1)
            {
                var message = $"Parameter: \"{nameof(page)}\" must not have a value lower than 1.";
                throw new ArgumentOutOfRangeException(nameof(page), message);
            }

            if (itemsPerPage < 1)
            {
                var message = $"Parameter: \"{nameof(itemsPerPage)}\" must not have a value lower than 1.";
                throw new ArgumentOutOfRangeException(nameof(itemsPerPage), message);
            }

            if (string.IsNullOrWhiteSpace(sessionId))
            {
                var message = $"Parameter: \"{nameof(sessionId)}\" must not be a null, empty or whitespace value.";
                throw new ArgumentOutOfRangeException(nameof(sessionId), message);
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

            return await cameraSnapshotsCursor.ToListAsync();
        }

        public async Task<IEnumerable<CapturedDisplaySnapshotMongoDocument>> GetDisplaySnapshots(
            string sessionId,
            int page = 1,
            int itemsPerPage = 3)
        {
            if (page < 1)
            {
                var message = $"Parameter: \"{nameof(page)}\" must not have a value lower than 1.";
                throw new ArgumentOutOfRangeException(nameof(page), message);
            }

            if (itemsPerPage < 1)
            {
                var message = $"Parameter: \"{nameof(itemsPerPage)}\" must not have a value lower than 1.";
                throw new ArgumentOutOfRangeException(nameof(itemsPerPage), message);
            }

            if (string.IsNullOrWhiteSpace(sessionId))
            {
                var message = $"Parameter: \"{nameof(sessionId)}\" must not be a null, empty or whitespace value.";
                throw new ArgumentOutOfRangeException(nameof(sessionId), message);
            }

            var displaySnapshotsCursor = await _displaySnapshotsCollection.FindAsync(
                x => x.SessionId == sessionId,
                new FindOptions<CapturedDisplaySnapshotMongoDocument, CapturedDisplaySnapshotMongoDocument>()
                {
                    AllowPartialResults = true,
                    Limit = itemsPerPage,
                    Skip = (page - 1) * itemsPerPage,
                    Sort = new SortDefinitionBuilder<CapturedDisplaySnapshotMongoDocument>().Descending(x => x.CreatedOn)
                });

            return await displaySnapshotsCursor.ToListAsync();
        }

        public async Task<IEnumerable<CapturedKeyboardKeyMongoDocument>> GetKeyboardKeys(
            string sessionId,
            int page = 1,
            int itemsPerPage = 3)
        {
            if (page < 1)
            {
                var message = $"Parameter: \"{nameof(page)}\" must not have a value lower than 1.";
                throw new ArgumentOutOfRangeException(nameof(page), message);
            }

            if (itemsPerPage < 1)
            {
                var message = $"Parameter: \"{nameof(itemsPerPage)}\" must not have a value lower than 1.";
                throw new ArgumentOutOfRangeException(nameof(itemsPerPage), message);
            }

            if (string.IsNullOrWhiteSpace(sessionId))
            {
                var message = $"Parameter: \"{nameof(sessionId)}\" must not be a null, empty or whitespace value.";
                throw new ArgumentOutOfRangeException(nameof(sessionId), message);
            }

            var keyboardKeysCursor = await _keyboardKeysCollection.FindAsync(
                x => x.SessionId == sessionId,
                new FindOptions<CapturedKeyboardKeyMongoDocument, CapturedKeyboardKeyMongoDocument>()
                {
                    AllowPartialResults = true,
                    Limit = itemsPerPage,
                    Skip = (page - 1) * itemsPerPage,
                    Sort = new SortDefinitionBuilder<CapturedKeyboardKeyMongoDocument>().Descending(x => x.CreatedOn)
                });

            return await keyboardKeysCursor.ToListAsync();
        }

        public async Task<IEnumerable<CapturedNetworkPacketMongoDocument>> GetNetworkPackets(
            string sessionId,
            int page = 1,
            int itemsPerPage = 3)
        {
            if (page < 1)
            {
                var message = $"Parameter: \"{nameof(page)}\" must not have a value lower than 1.";
                throw new ArgumentOutOfRangeException(nameof(page), message);
            }

            if (itemsPerPage < 1)
            {
                var message = $"Parameter: \"{nameof(itemsPerPage)}\" must not have a value lower than 1.";
                throw new ArgumentOutOfRangeException(nameof(itemsPerPage), message);
            }

            if (string.IsNullOrWhiteSpace(sessionId))
            {
                var message = $"Parameter: \"{nameof(sessionId)}\" must not be a null, empty or whitespace value.";
                throw new ArgumentOutOfRangeException(nameof(sessionId), message);
            }

            var networkPacketsCursor = await _networkPacketsCollection.FindAsync(
                x => x.SessionId == sessionId,
                new FindOptions<CapturedNetworkPacketMongoDocument, CapturedNetworkPacketMongoDocument>()
                {
                    AllowPartialResults = true,
                    Limit = itemsPerPage,
                    Skip = (page - 1) * itemsPerPage,
                    Sort = new SortDefinitionBuilder<CapturedNetworkPacketMongoDocument>().Descending(x => x.CreatedOn)
                });

            return await networkPacketsCursor.ToListAsync();
        }

        public async Task<IEnumerable<CapturedForegroundProcessMongoDocument>> GetForegroundProcesses(
            string sessionId,
            int page = 1,
            int itemsPerPage = 3)
        {
            if (page < 1)
            {
                var message = $"Parameter: \"{nameof(page)}\" must not have a value lower than 1.";
                throw new ArgumentOutOfRangeException(nameof(page), message);
            }

            if (itemsPerPage < 1)
            {
                var message = $"Parameter: \"{nameof(itemsPerPage)}\" must not have a value lower than 1.";
                throw new ArgumentOutOfRangeException(nameof(itemsPerPage), message);
            }

            if (string.IsNullOrWhiteSpace(sessionId))
            {
                var message = $"Parameter: \"{nameof(sessionId)}\" must not be a null, empty or whitespace value.";
                throw new ArgumentOutOfRangeException(nameof(sessionId), message);
            }

            var foregroundProcessesCursor = await _foregroundProcessesCollection.FindAsync(
                x => x.SessionId == sessionId,
                new FindOptions<CapturedForegroundProcessMongoDocument, CapturedForegroundProcessMongoDocument>()
                {
                    AllowPartialResults = true,
                    Limit = itemsPerPage,
                    Skip = (page - 1) * itemsPerPage,
                    Sort = new SortDefinitionBuilder<CapturedForegroundProcessMongoDocument>().Descending(x => x.CreatedOn)
                });

            return await foregroundProcessesCursor.ToListAsync();
        }

        public async Task<IEnumerable<CapturedActiveProcessesMongoDocument>> GetActiveProcesses(
            string sessionId,
            int page = 1,
            int itemsPerPage = 3)
        {
            if (page < 1)
            {
                var message = $"Parameter: \"{nameof(page)}\" must not have a value lower than 1.";
                throw new ArgumentOutOfRangeException(nameof(page), message);
            }

            if (itemsPerPage < 1)
            {
                var message = $"Parameter: \"{nameof(itemsPerPage)}\" must not have a value lower than 1.";
                throw new ArgumentOutOfRangeException(nameof(itemsPerPage), message);
            }

            if (string.IsNullOrWhiteSpace(sessionId))
            {
                var message = $"Parameter: \"{nameof(sessionId)}\" must not be a null, empty or whitespace value.";
                throw new ArgumentOutOfRangeException(nameof(sessionId), message);
            }

            var networkPacketsCursor = await _activeProcessesCollection.FindAsync(
                x => x.SessionId == sessionId,
                new FindOptions<CapturedActiveProcessesMongoDocument, CapturedActiveProcessesMongoDocument>()
                {
                    AllowPartialResults = true,
                    Limit = itemsPerPage,
                    Skip = (page - 1) * itemsPerPage,
                    Sort = new SortDefinitionBuilder<CapturedActiveProcessesMongoDocument>().Descending(x => x.CreatedOn)
                });

            return await networkPacketsCursor.ToListAsync();
        }
    }

    public interface IMonitoringSessionsService
    {

    }
}