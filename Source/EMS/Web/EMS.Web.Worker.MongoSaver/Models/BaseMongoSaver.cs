using Confluent.Kafka;
using Confluent.Kafka.Serialization;
using EMS.Infrastructure.Stream;
using EMS.Web.Common.Mongo;
using MongoDB.Driver;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EMS.Web.Worker.MongoSaver.Models
{
    public interface IMongoSaver
    {
        Task Execute();
    }

    public abstract class BaseMongoSaver<TOut, TIn> : IMongoSaver
        where TOut : class
        where TIn : class
    {
        private readonly string inputKafkaTopic;
        private readonly CancellationToken cToken;
        private MongoClient mongoClient;
        private IMongoDatabase mongoDatabase;
        private IMongoCollection<TOut> mongoCollection;
        private Consumer<string, string> consumer;

        public BaseMongoSaver(CancellationToken cToken, IMongoCollection<TOut> mongoCollection, string inputKafkaTopic)
        {
            this.cToken = cToken;
            this.mongoCollection = mongoCollection;
            this.inputKafkaTopic = inputKafkaTopic;
        }

        public async Task Execute()
        {
            var consumerConfig = new Dictionary<string, object>
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

            var keyDeserializer = new StringDeserializer(Encoding.UTF8);
            var valueDeserializer = new JsonDeserializer2();

            this.consumer = new Consumer<string, string>(
                consumerConfig,
                keyDeserializer,
                valueDeserializer);
            this.consumer.OnMessage += OnMessageReceived;
            this.consumer.Subscribe(this.inputKafkaTopic);

            while (!cToken.IsCancellationRequested)
            {
                this.consumer.Poll(TimeSpan.FromMilliseconds(100));
            }
        }

        protected virtual void OnMessageReceived(object sender, Message<string, string> kafkaMessage)
        {
            var message = JsonConvert.DeserializeObject<TIn>(kafkaMessage.Value);

            if (message == null)
            {
                // Log invalid message read with the message value and details
            }
            else
            {
                var mongoItem = this.FormatReceivedMessage(message);
                this.mongoCollection.InsertOneAsync(mongoItem).Wait();
                // notify WebAPI for db change (semi-push-notification)
            }
        }

        protected abstract TOut FormatReceivedMessage(TIn message);
    }
}