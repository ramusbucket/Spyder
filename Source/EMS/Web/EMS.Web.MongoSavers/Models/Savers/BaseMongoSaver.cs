using System;
using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;
using EMS.Core.Models.Mongo;
using EMS.Infrastructure.Common.Providers;
using EMS.Web.MongoSavers.App_Start;
using MongoDB.Driver;
using Newtonsoft.Json;

namespace EMS.Web.MongoSavers.Models.Savers
{
    public abstract class BaseMongoSaver<TOut, TIn> : IMongoSaver
        where TOut : AuditableMongoDocument
        where TIn : class
    {
        private readonly CancellationToken _cToken;

        private readonly string _kafkaConsumerTopic;

        private readonly Consumer<string, string> _kafkaConsumer;

        private readonly IMongoCollection<TOut> _outCollection;

        private readonly IMongoCollection<MonitoringSessionMongoDocument> _sessionsCollection;

        protected BaseMongoSaver(CancellationToken cToken, IMongoCollection<TOut> outCollection, string kafkaConsumerTopic)
        {
            _cToken = cToken;
            _outCollection = outCollection;
            _sessionsCollection = outCollection.Database.GetCollection<MonitoringSessionMongoDocument>(MongoCollections.MonitoringSessions);
            _kafkaConsumerTopic = kafkaConsumerTopic;
            _kafkaConsumer = DependencyInjectionConfig.GetKafkaConsumerInstance();
            Statistics = new ServiceStatistics();
        }

        public ServiceStatistics Statistics { get; private set; }

        public async Task Start()
        {
            Statistics.StartDate = TimeProvider.Current.UtcNow;

            _kafkaConsumer.OnMessage += OnMessageReceived;
            _kafkaConsumer.Subscribe(_kafkaConsumerTopic);

            while (!_cToken.IsCancellationRequested)
            {
                _kafkaConsumer.Poll(TimeSpan.FromMilliseconds(100));
                Statistics.LastPollDate = TimeProvider.Current.UtcNow;
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

            Statistics.LastReceivedMessageDate = TimeProvider.Current.UtcNow;

            var mongoItem = FormatReceivedMessage(message);

            var session = new MonitoringSessionMongoDocument
            {
                UserId = mongoItem.UserId,
                SessionId = mongoItem.SessionId,
                UserName = mongoItem.UserName,
                CreatedOn = TimeProvider.Current.UtcNow,
                IsActive = true
            };

            var updateResult = _sessionsCollection.FindOneAndUpdate(
                new FilterDefinitionBuilder<MonitoringSessionMongoDocument>()
                    .Where(x => x.SessionId == session.SessionId && x.UserId == session.UserId),
                new UpdateDefinitionBuilder<MonitoringSessionMongoDocument>()
                    .Set(x => x.UserId,session.UserId)
                    .Set(x=> x.SessionId, session.SessionId)
                    .Set(x=> x.CreatedOn, session.CreatedOn)
                    .Set(x=> x.IsActive, true),
                new FindOneAndUpdateOptions<MonitoringSessionMongoDocument, MonitoringSessionMongoDocument>()
                {
                    IsUpsert = true
                });
            _outCollection.InsertOne(mongoItem);
            // notify WebAPI for db change (semi-push-notification)

            Statistics.LastProcessedItemDate = TimeProvider.Current.UtcNow;
            Statistics.ProcessedItemsCount++;
        }

        protected abstract TOut FormatReceivedMessage(TIn message);
    }
}
