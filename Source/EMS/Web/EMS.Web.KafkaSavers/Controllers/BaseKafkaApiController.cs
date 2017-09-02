using System;
using EMS.Core.Models;
using EMS.Infrastructure.Common.Providers;
using EMS.Infrastructure.Stream;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using EMS.Core.Models.DTOs;
using EMS.Infrastructure.DependencyInjection;
using EMS.Infrastructure.Statistics;
using EMS.Web.KafkaSavers.Models;

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
            var userName = this.User.Identity.GetUserName();

            foreach (var item in data)
            {
                item.UserId = userId;
                item.UserName = userName;
            }

            // TODO
            // Might result in Producer error while publishing one of the messages
            // Handle the error by logging it in elastic search

            var kafkaResponse = await _statsCollector.MeasureWithAck(
                async () => await KafkaClient.PublishMultiple(data, topicName, null),
                nameof(KafkaClient.PublishMultiple));

            _statsCollector.Send(new { Topic = topicName });

            CollectorStatistics.Counters.AddOrUpdate(
                SplitPascalCase(this.GetType().Name),
                (k) => 1,
                (k, v) => v + 1);
        }

        private string SplitPascalCase(string text)
        {
            var result = new StringBuilder();

            foreach (var character in text)
            {
                if (char.IsUpper(character))
                {
                    result.Append($" {character}");
                }
                else
                {
                    result.Append(character);
                }
            }

            return result.ToString();
        }
    }
}