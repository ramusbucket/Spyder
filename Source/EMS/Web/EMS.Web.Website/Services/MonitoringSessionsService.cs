using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EMS.Core.Models.Mongo;
using EMS.Web.Website.Models;
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
            var mongoDb = new MongoClient("mongodb://localhost:27017")
                .GetDatabase("Spyder");

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

        public async Task<IEnumerable<SessionViewModel>> GetActiveSessionsDetails(
            int page = 1,
            int itemsPerPage = 12)
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

            var sessionsIds = (await GetActiveSessions(page, itemsPerPage)).Select(x => x.Id.SessionId);
            var result = sessionsIds.Select(async x => new SessionViewModel
            {
                KeyboardKeys = await GetKeyboardKeys(x),
                NetworkPackets = await GetNetworkPackets(x),
                CameraSnapshots = await GetCameraSnapshots(x),
                DisplaySnapshots = await GetDisplaySnapshots(x),
                ForegroundProcesses = await GetForegroundProcesses(x)
            });

            var realResult = new List<SessionViewModel>();
            foreach (var r in result)
            {
                var x = await r;
                realResult.Add(x);
            }

            return realResult;
        }

        public async Task<IEnumerable<MonitoringSessionMongoDocument>> GetActiveSessions(
            int page = 1,
            int itemsPerPage = 12)
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

            var activeSessionsCursor = await _monitoringSessionsCollection.FindAsync(
                x => x.IsActive,
                new FindOptions<MonitoringSessionMongoDocument, MonitoringSessionMongoDocument>()
                {
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
            return await GetPageFromCollection<CapturedCameraSnapshotMongoDocument>(
                _cameraSnapshotsCollection,
                sessionId,
                page,
                itemsPerPage);
        }

        public async Task<IEnumerable<CapturedDisplaySnapshotMongoDocument>> GetDisplaySnapshots(
            string sessionId,
            int page = 1,
            int itemsPerPage = 3)
        {
            return await GetPageFromCollection<CapturedDisplaySnapshotMongoDocument>(
                _displaySnapshotsCollection,
                sessionId,
                page,
                itemsPerPage);
        }

        public async Task<IEnumerable<CapturedKeyboardKeyMongoDocument>> GetKeyboardKeys(
            string sessionId,
            int page = 1,
            int itemsPerPage = 3)
        {
            return await GetPageFromCollection<CapturedKeyboardKeyMongoDocument>(
                _keyboardKeysCollection,
                sessionId,
                page,
                itemsPerPage);
        }

        public async Task<IEnumerable<CapturedNetworkPacketMongoDocument>> GetNetworkPackets(
            string sessionId,
            int page = 1,
            int itemsPerPage = 3)
        {
            return await GetPageFromCollection<CapturedNetworkPacketMongoDocument>(
                _networkPacketsCollection,
                sessionId,
                page,
                itemsPerPage);
        }

        public async Task<IEnumerable<CapturedForegroundProcessMongoDocument>> GetForegroundProcesses(
            string sessionId,
            int page = 1,
            int itemsPerPage = 3)
        {
            return await GetPageFromCollection<CapturedForegroundProcessMongoDocument>(
                _foregroundProcessesCollection,
                sessionId,
                page,
                itemsPerPage);
        }

        public async Task<IEnumerable<CapturedActiveProcessesMongoDocument>> GetActiveProcesses(
            string sessionId,
            int page = 1,
            int itemsPerPage = 3)
        {
            return await GetPageFromCollection<CapturedActiveProcessesMongoDocument>(
                _activeProcessesCollection,
                sessionId,
                page,
                itemsPerPage);
        }

        private async Task<IEnumerable<T>> GetPageFromCollection<T>(
            IMongoCollection<T> mongoCollection,
            string sessionId,
            int page = 1,
            int itemsPerPage = 3)
            where T : AuditableMongoDocument
        {
            if (page < 1)
            {
                var message = $"Argument {nameof(page)} must not have a value lower than 1.";
                throw new ArgumentOutOfRangeException(nameof(page), message);
            }

            if (itemsPerPage < 1)
            {
                var message = $"Argument {nameof(itemsPerPage)} must not have a value lower than 1.";
                throw new ArgumentOutOfRangeException(nameof(itemsPerPage), message);
            }

            if (string.IsNullOrWhiteSpace(sessionId))
            {
                var message = $"Argument {nameof(sessionId)} must not be a null, empty or whitespace value.";
                throw new ArgumentOutOfRangeException(nameof(sessionId), message);
            }

            var pagedItemsCursor = await mongoCollection.FindAsync(
                x => x.SessionId == sessionId,
                new FindOptions<T, T>()
                {
                    AllowPartialResults = true,
                    Limit = itemsPerPage,
                    Skip = (page - 1) * itemsPerPage,
                    Sort = new SortDefinitionBuilder<T>().Descending(x => x.CreatedOn)
                });

            return await pagedItemsCursor.ToListAsync();
        }
    }

    public interface IMonitoringSessionsService
    {
        Task<IEnumerable<SessionViewModel>> GetActiveSessionsDetails(
            int page = 1,
            int itemsPerPage = 12);

        Task<IEnumerable<MonitoringSessionMongoDocument>> GetActiveSessions(
            int page = 1,
            int itemsPerPage = 12);

        Task<IEnumerable<CapturedCameraSnapshotMongoDocument>> GetCameraSnapshots(
            string sessionId,
            int page = 1,
            int itemsPerPage = 3);

        Task<IEnumerable<CapturedDisplaySnapshotMongoDocument>> GetDisplaySnapshots(
            string sessionId,
            int page = 1,
            int itemsPerPage = 3);

        Task<IEnumerable<CapturedKeyboardKeyMongoDocument>> GetKeyboardKeys(
            string sessionId,
            int page = 1,
            int itemsPerPage = 3);

        Task<IEnumerable<CapturedNetworkPacketMongoDocument>> GetNetworkPackets(
            string sessionId,
            int page = 1,
            int itemsPerPage = 3);

        Task<IEnumerable<CapturedForegroundProcessMongoDocument>> GetForegroundProcesses(
            string sessionId,
            int page = 1,
            int itemsPerPage = 3);

        Task<IEnumerable<CapturedActiveProcessesMongoDocument>> GetActiveProcesses(
            string sessionId,
            int page = 1,
            int itemsPerPage = 3);

    }
}