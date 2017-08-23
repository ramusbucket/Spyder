using Confluent.Kafka;
using Confluent.Kafka.Serialization;
using EMS.Infrastructure.DependencyInjection;
using EMS.Infrastructure.DependencyInjection.Interfaces;
using EMS.Infrastructure.Stream;
using EMS.Web.Common.Mongo;
using EMS.Web.Worker.MongoSaver.Models;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace EMS.Web.Worker.MongoSaver.App_Start
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
        }

        private void RegisterCancellationToken(IInjector injector)
        {
            injector.RegisterInstance<CancellationToken>(new CancellationToken());
        }

        private void RegisterMongoCollections(IInjector injector)
        {
            var mongoClient = new MongoClient("mongodb://localhost:27017");
            var mongoDatabase = mongoClient.GetDatabase("Spyder");

            var networkPacketsCollection = mongoDatabase.GetCollection<CapturedNetworkPacket>(MongoCollections.NetworkPackets);
            var keyboardKeysCollection = mongoDatabase.GetCollection<CapturedKeyboardKey>(MongoCollections.CapturedKeyboardKeys);
            var cameraSnapshotsCollection = mongoDatabase.GetCollection<CapturedCameraSnapshot>(MongoCollections.CameraSnapshots);
            var activeProcessesCollection = mongoDatabase.GetCollection<CapturedActiveProcesses>(MongoCollections.ActiveProcesses);
            var displaySnapshotsCollection = mongoDatabase.GetCollection<CapturedDisplaySnapshot>(MongoCollections.DisplaySnapshots);
            var foregroundProcessesCollection = mongoDatabase.GetCollection<CapturedForegroundProcess>(MongoCollections.ForegroundProcesses);

            injector.RegisterInstance<IMongoClient>(mongoClient);
            injector.RegisterInstance<IMongoDatabase>(mongoDatabase);
            injector.RegisterInstance<IMongoCollection<CapturedNetworkPacket>>(networkPacketsCollection);
            injector.RegisterInstance<IMongoCollection<CapturedKeyboardKey>>(keyboardKeysCollection);
            injector.RegisterInstance<IMongoCollection<CapturedCameraSnapshot>>(cameraSnapshotsCollection);
            injector.RegisterInstance<IMongoCollection<CapturedActiveProcesses>>(activeProcessesCollection);
            injector.RegisterInstance<IMongoCollection<CapturedDisplaySnapshot>>(displaySnapshotsCollection);
            injector.RegisterInstance<IMongoCollection<CapturedForegroundProcess>>(foregroundProcessesCollection);
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