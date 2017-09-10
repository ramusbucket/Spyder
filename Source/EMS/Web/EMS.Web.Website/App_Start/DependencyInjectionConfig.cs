using System.Threading;
using EMS.Core.Models.Mongo;
using EMS.Infrastructure.DependencyInjection;
using EMS.Infrastructure.DependencyInjection.Interfaces;
using MongoDB.Driver;

namespace EMS.Web.Website
{
    public class DependencyInjectionConfig
    {
        public void RegisterDependencies()
        {
            var injector = UnityInjector.Instance;

            this.RegisterCancellationToken(injector);
            this.RegisterMongoCollections(injector);
        }

        private void RegisterCancellationToken(IInjector injector)
        {
            injector.RegisterInstance<CancellationToken>(new CancellationToken());
        }

        private void RegisterMongoCollections(IInjector injector)
        {
            var mongoClient = new MongoClient("mongodb://localhost:27017");
            var mongoDatabase = mongoClient.GetDatabase("Spyder");

            var networkPacketsCollection = mongoDatabase.GetCollection<CapturedNetworkPacketMongoDocument>(MongoCollections.NetworkPackets);
            var keyboardKeysCollection = mongoDatabase.GetCollection<CapturedKeyboardKeyMongoDocument>(MongoCollections.CapturedKeyboardKeys);
            var cameraSnapshotsCollection = mongoDatabase.GetCollection<CapturedCameraSnapshotMongoDocument>(MongoCollections.CameraSnapshots);
            var activeProcessesCollection = mongoDatabase.GetCollection<CapturedActiveProcessesMongoDocument>(MongoCollections.ActiveProcesses);
            var displaySnapshotsCollection = mongoDatabase.GetCollection<CapturedDisplaySnapshotMongoDocument>(MongoCollections.DisplaySnapshots);
            var foregroundProcessesCollection = mongoDatabase.GetCollection<CapturedForegroundProcessMongoDocument>(MongoCollections.ForegroundProcesses);

            injector.RegisterInstance<IMongoClient>(mongoClient);
            injector.RegisterInstance<IMongoDatabase>(mongoDatabase);
            injector.RegisterInstance<IMongoCollection<CapturedNetworkPacketMongoDocument>>(networkPacketsCollection);
            injector.RegisterInstance<IMongoCollection<CapturedKeyboardKeyMongoDocument>>(keyboardKeysCollection);
            injector.RegisterInstance<IMongoCollection<CapturedCameraSnapshotMongoDocument>>(cameraSnapshotsCollection);
            injector.RegisterInstance<IMongoCollection<CapturedActiveProcessesMongoDocument>>(activeProcessesCollection);
            injector.RegisterInstance<IMongoCollection<CapturedDisplaySnapshotMongoDocument>>(displaySnapshotsCollection);
            injector.RegisterInstance<IMongoCollection<CapturedForegroundProcessMongoDocument>>(foregroundProcessesCollection);
        }
    }
}