using EMS.Core.Models;
using EMS.Infrastructure.Common.Providers;
using EMS.Infrastructure.Stream;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using EMS.Web.Server.Collector.Models;

namespace EMS.Web.Server.Collector.Controllers
{
    [System.Web.Http.Authorize]
    public class BaseKafkaApiController : ApiController
    {
        protected virtual async Task PublishToKafkaMultipleItems(IEnumerable<BaseDTO> data, string topicName)
        {
            var userId = this.User.Identity.GetUserId();
            foreach (var item in data)
            {
                item.UserId = userId;
            }

            var key = TimeProvider.Current.UtcNow.Ticks.ToString();

            // TODO
            // Might result in Producer error while publishing one of the messages
            // Handle the error by logging it in elastic search
            var kafkaResponse = await KafkaClient.PublishMultiple(data, topicName, key);

            CollectorStatistics.Counters.AddOrUpdate(
                this.GetType().Name,
                (k) => 1,
                (k, v) => v + 1);
        }
    }
}