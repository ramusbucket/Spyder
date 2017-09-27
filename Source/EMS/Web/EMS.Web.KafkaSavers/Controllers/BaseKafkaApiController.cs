using EMS.Infrastructure.Stream;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using EMS.Core.Models.DTOs;
using EMS.Infrastructure.DependencyInjection;
using EMS.Infrastructure.Statistics;

namespace EMS.Web.KafkaSavers.Controllers
{
    [System.Web.Http.Authorize]
    public class BaseKafkaApiController : ApiController
    {
        private readonly IStatisticsCollector _statsCollector;

        public BaseKafkaApiController()
        {
            _statsCollector = UnityInjector.Instance.Resolve<IStatisticsCollector>();
        }

        protected virtual async Task PublishToKafkaMultipleItems(IEnumerable<AuditableDto> data, string topicName)
        {
            var userId = this.User.Identity.GetUserId();
            var userName = this.User.Identity.Name;

            // TODO
            // Might result in Producer error while publishing one of the messages
            // Handle the error by logging it in elastic search
            foreach (var item in data)
            {
                item.UserId = userId;
                item.UserName = userName;

                var kafkaProducerResponse =
                    await _statsCollector.MeasureWithAck(
                        async () => await KafkaClient.PublishSingle(item, topicName),
                        nameof(KafkaClient.PublishSingle), 
                        "metrics");

                if (kafkaProducerResponse.Error.HasError)
                {
                    await _statsCollector.SendWithAck(
                        new
                        {
                            Error = kafkaProducerResponse.Error.Reason,
                            Topic = topicName,
                            UserId = userId,
                            Item = item
                        },
                        "logs");
                }

                await _statsCollector.SendWithAck(
                    new
                    {
                        KafkaResponse = kafkaProducerResponse,
                        UserId = userId,
                        Item = item
                    },
                    "logs");
            }

            _statsCollector.Send(
                new
                {
                    Topic = topicName,
                    ApplicationName = "EMS.Web.KafkaSavers",
                    ServerName = "Localhost"
                },
                "metrics");
        }
    }
}