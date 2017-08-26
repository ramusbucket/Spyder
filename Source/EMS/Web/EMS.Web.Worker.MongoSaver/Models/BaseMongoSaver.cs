using Confluent.Kafka;
using EMS.Infrastructure.Common.Providers;
using EMS.Web.Worker.MongoSaver.App_Start;
using MongoDB.Driver;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EMS.Web.Worker.MongoSaver.Controllers;
using EMS.Web.Worker.MongoSaver.Hubs;
using Microsoft.AspNet.SignalR;

namespace EMS.Web.Worker.MongoSaver.Models
{
    public interface IMongoSaver
    {
        Task Start();

        ServiceStatistics Statistics { get; }
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
            this.Statistics = new ServiceStatistics();
        }

        public ServiceStatistics Statistics { get; private set; }

        public async Task Start()
        {
            // Extract to base worker in a proper place
            this.Statistics.StartDate = TimeProvider.Current.UtcNow;

            this.consumer = DependencyInjectionConfig.GetKafkaConsumerInstance();
            this.consumer.OnMessage += OnMessageReceived;
            this.consumer.Subscribe(this.inputKafkaTopic);

            while (!cToken.IsCancellationRequested)
            {
                this.consumer.Poll(TimeSpan.FromMilliseconds(100));
                this.Statistics.LastPollDate = TimeProvider.Current.UtcNow;
            }
        }

        protected virtual void OnMessageReceived(object sender, Message<string, string> kafkaMessage)
        {
            if (kafkaMessage.Value == null)
            {
                // Log invalid message read with the message details
                return;
            }

            var message = JsonConvert.DeserializeObject<TIn>(kafkaMessage.Value);
            if (message == null)
            {
                // Log invalid message read with the message value and details
                return;
            }

            this.Statistics.LastReceivedMessageDate = TimeProvider.Current.UtcNow;

            var mongoItem = this.FormatReceivedMessage(message);
            this.mongoCollection.InsertOneAsync(mongoItem).Wait();
            // notify WebAPI for db change (semi-push-notification)

            this.Statistics.LastProcessedItemDate = TimeProvider.Current.UtcNow;
            this.Statistics.ProcessedItemsCount++;

            PushStatisticsToAllClients();
        }

        protected abstract TOut FormatReceivedMessage(TIn message);

        private void PushStatisticsToAllClients()
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<ServiceStatisticsHub>();
            var statistics = HomeController.savers?
                .Where(x => x.Statistics != null)
                .Select(x => new
                {
                    Saver = x.GetType().Name,
                    Stats = x.Statistics
                })
                .ToList();

            var statsAsJson = JsonConvert.SerializeObject(statistics);
            context.Clients.All.pushStatistics(statsAsJson);
        }
    }
}