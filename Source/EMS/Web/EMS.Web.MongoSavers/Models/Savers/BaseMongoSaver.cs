using System;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;
using Easy.Common;
using Easy.Common.Interfaces;
using EMS.Core.Models.Mongo;
using EMS.Infrastructure.Common.Providers;
using EMS.Infrastructure.Statistics;
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

        private readonly IStatisticsCollector _statsCollector;

        private readonly IRestClient _restClient;

        protected BaseMongoSaver(
            CancellationToken cToken,
            IMongoCollection<TOut> outCollection,
            string kafkaConsumerTopic,
            IStatisticsCollector statsCollector,
            IRestClient restClient)
        {
            _cToken = cToken;
            _outCollection = outCollection;
            _sessionsCollection = outCollection.Database.GetCollection<MonitoringSessionMongoDocument>(MongoCollections.MonitoringSessions);
            _kafkaConsumerTopic = kafkaConsumerTopic;
            _kafkaConsumer = DependencyInjectionConfig.GetKafkaConsumerInstance();
            _statsCollector = statsCollector;
            _restClient = restClient;
            Statistics = new ServiceStatistics();
        }

        public ServiceStatistics Statistics { get; private set; }

        public async Task Start()
        {
            _kafkaConsumer.OnMessage += OnMessageReceived;
            _kafkaConsumer.Subscribe(_kafkaConsumerTopic);

            while (!_cToken.IsCancellationRequested)
            {
                _statsCollector.Measure(
                    () => _kafkaConsumer.Poll(TimeSpan.FromMilliseconds(100)), 
                    nameof(_kafkaConsumer.Poll),
                    "metrics");
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

            var mongoItem = FormatReceivedMessage(message);

            var session = new MonitoringSessionMongoDocument
            {
                UserId = mongoItem.UserId,
                SessionId = mongoItem.SessionId,
                UserName = mongoItem.UserName,
                CreatedOn = TimeProvider.Current.UtcNow,
                IsActive = true
            };

            var updateResult = _statsCollector.Measure(
                () => _sessionsCollection.FindOneAndUpdate(
                    new FilterDefinitionBuilder<MonitoringSessionMongoDocument>()
                        .Where(x => x.SessionId == session.SessionId && x.UserId == session.UserId),
                    new UpdateDefinitionBuilder<MonitoringSessionMongoDocument>()
                        .Set(x => x.UserId, session.UserId)
                        .Set(x => x.SessionId, session.SessionId)
                        .Set(x => x.CreatedOn, session.CreatedOn)
                        .Set(x => x.IsActive, true),
                    new FindOneAndUpdateOptions<MonitoringSessionMongoDocument, MonitoringSessionMongoDocument>()
                    {
                        IsUpsert = true
                    }),
                nameof(_sessionsCollection.FindOneAndUpdate),
                "metrics");

            _statsCollector.Measure(
                () => _outCollection.InsertOne(mongoItem),
                nameof(_outCollection.InsertOne),
                "metrics");

            // notify WebAPI for db change (semi-push-notification)
            // keep in mind that during high loads, you might kill the users browsers and the website
            var request = new HttpRequestMessage(HttpMethod.Post,
                new Uri("http://localhost:60101/api/PushNotifications/Update"));
            request.Content = new JSONContent(JsonConvert.SerializeObject(mongoItem), Encoding.UTF8);
            var repsonse = _restClient.SendAsync(request).Result;
        }

        protected abstract TOut FormatReceivedMessage(TIn message);
    }
}
