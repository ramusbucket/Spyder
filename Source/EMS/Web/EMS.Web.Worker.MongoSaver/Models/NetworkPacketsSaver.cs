using Confluent.Kafka;
using EMS.Core.Models;
using EMS.Infrastructure.Stream;
using EMS.Web.Common;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace EMS.Web.Worker.MongoSaver.Models
{
    public class NetworkPacketsSaver
    {
        private readonly CancellationToken cToken;
        private MongoClient mongoClient;
        private IMongoDatabase mongoDatabase;
        private IMongoCollection<CapturedNetworkPacketDetailsDTO> mongoCollection;

        public NetworkPacketsSaver(CancellationToken cToken)
        {
            this.cToken = cToken;
            this.mongoClient = new MongoClient("mongodb://localhost:27017");
            this.mongoDatabase = mongoClient.GetDatabase("Spyder");
            this.mongoCollection = mongoDatabase.GetCollection<CapturedNetworkPacketDetailsDTO>(MongoCollections.NetworkPackets);
        }

        public void Execute()
        {
            KafkaClient.Consumer.OnMessage += OnMessageReceived;
            KafkaClient.Consumer.Subscribe(Topics.NetworkPackets);

            while (!cToken.IsCancellationRequested)
            {
                KafkaClient.Consumer.Poll(TimeSpan.FromMilliseconds(100));
            }
        }

        private void OnMessageReceived(object sender, Message<string, string> kafkaMessage)
        {
            var message = JsonConvert.DeserializeObject<CapturedNetworkPacketDetailsDTO>(kafkaMessage.Value);
            
            if (message == null)
            {
                // Log invalid message read with the message value and details
            }
            else
            {
                this.mongoCollection.InsertOneAsync(message).Wait();

                // notify WebAPI for db change (semi-push-notification)
            }
        }
    }
}