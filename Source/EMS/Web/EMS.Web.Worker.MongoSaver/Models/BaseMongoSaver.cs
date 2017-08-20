using Confluent.Kafka;
using EMS.Infrastructure.Stream;
using MongoDB.Driver;
using Newtonsoft.Json;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace EMS.Web.Worker.MongoSaver.Models
{
    public interface IMongoSaver
    {
        Task Execute();
    }

    public abstract class BaseMongoSaver<TOut,TIn> : IMongoSaver
        where TOut : class
        where TIn : class
    {
        private readonly string inputKafkaTopic;
        private readonly CancellationToken cToken;
        private MongoClient mongoClient;
        private IMongoDatabase mongoDatabase;
        private IMongoCollection<TOut> mongoCollection;

        public BaseMongoSaver(CancellationToken cToken, IMongoCollection<TOut> mongoCollection, string inputKafkaTopic)
        {
            this.cToken = cToken;
            this.mongoCollection = mongoCollection;
            this.inputKafkaTopic = inputKafkaTopic;
        }

        public async Task Execute()
        {
            KafkaClient.Consumer.OnMessage += OnMessageReceived;
            KafkaClient.Consumer.Subscribe(this.inputKafkaTopic);

            while (!cToken.IsCancellationRequested)
            {
                KafkaClient.Consumer.Poll(TimeSpan.FromMilliseconds(100));
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