using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;
using Confluent.Kafka.Serialization;
using Easy.Common.Interfaces;
using EMS.Core.Models.DTOs;
using EMS.Core.Models.Mongo;
using EMS.Infrastructure.Common.Enums;
using EMS.Infrastructure.Common.JsonConverters;
using EMS.Infrastructure.Stream;
using EMS.Web.MongoSavers.Models.Savers;
using MongoDB.Driver;
using Newtonsoft.Json;
using NUnit.Framework;
using EMS.Infrastructure.Statistics;
using Moq;

namespace EMS.Web.MongoSavers.IntegrationTests
{
    [TestFixture]
    public class KeyboardKeysSaverTests
    {
        private readonly Random random = new Random();

        private KeyboardKeysSaver saver;

        private Producer<string, object> kafkaProducer;

        private string mongoCollectionName;

        private IMongoClient mongoClient;

        private IMongoDatabase mongoDatabase;

        private IMongoCollection<CapturedKeyboardKeyMongoDocument> mongoCollection;

        private Mock<IStatisticsCollector> statsCollectorMock;

        private Mock<IRestClient> restClientMock;

        [SetUp]
        public void SetUp()
        {
            mongoClient = new MongoClient("mongodb://localhost:27017");
            mongoCollectionName = $"TestCollection{DateTime.UtcNow.Ticks}{this.random.Next(0, int.MaxValue)}";
            mongoDatabase = mongoClient.GetDatabase("SpyderIntegrationTests");
            mongoCollection = mongoDatabase.GetCollection<CapturedKeyboardKeyMongoDocument>(mongoCollectionName);
            kafkaProducer = CreateKafkaProducer();
            statsCollectorMock = new Mock<IStatisticsCollector>();
            restClientMock = new Mock<IRestClient>();

            saver = new KeyboardKeysSaver(
                CancellationToken.None,
                mongoCollection,
                statsCollectorMock.Object,
                restClientMock.Object);
        }

        [TearDown]
        public void TearDown()
        {
            mongoDatabase.DropCollection(mongoCollectionName);
            kafkaProducer.Dispose();
        }

        [TestCase(1)]
        [TestCase(5)]
        [TestCase(10)]
        [TestCase(100)]
        [TestCase(200)]
        [TestCase(10000)]
        public async Task StartPollingForMessages_ReadAndProcessMesages_SaveProcessedMessageToMongo(int messagesCount)
        {
            //// Start polling for messages
            //Task.Run(() => saver.Start());

            //// Insert messages in Kafka
            //var messages = new List<CapturedKeyDetailsDto>(messagesCount);
            //for (int i = 0; i < messagesCount; i++)
            //{
            //    var message = new CapturedKeyDetailsDto
            //    {
            //        UserId = "userId",
            //        CreatedOn = DateTime.UtcNow,
            //        KeyboardKey = KeyboardKey.A,
            //        SessionId = "sessionId",
            //        UserName = "userName"
            //    };

            //    messages.Add(message);

            //   var result = await kafkaProducer.ProduceAsync(Topics.CapturedKeyboardKeys, null, message);
            //}

            //// Wait for saver to process messages
            //await Task.Delay(2000);

            //// Assert all mesages are saved inside MongoDb
            //var messagesFromMongo = mongoCollection.Find(x => x.CreatedOn < DateTime.UtcNow).ToList();
            //Assert.AreEqual(messages.Count,messagesFromMongo.Count);
        }

        private Producer<string, object> CreateKafkaProducer()
        {
            var converters = new List<JsonConverter> { new ObjectIdConverter() };
            var serializerSettings = new JsonSerializerSettings { Converters = converters, Formatting = Formatting.Indented };
            var valueSerializer = new JsonSerializerObjectToBytes(serializerSettings);
            var producerConfig = new Dictionary<string, object>
            {
                { "bootstrap.servers", "localhost:9092" },
                { "batch.num.messages", "1" },
                { "queue.buffering.max.ms", "1" },
                { "socket.blocking.max.ms", "1" },

            };
            var keySerializer = new StringSerializer(Encoding.UTF8);
            return new Producer<string, object>(
                producerConfig,
                keySerializer,
                valueSerializer);
        }
    }
}
