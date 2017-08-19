using Confluent.Kafka;
using EMS.Infrastructure.Common.Providers;
using EMS.Infrastructure.Stream;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace EMS.Web.Server.Collector.Controllers
{
    public class BaseKafkaApiController :ApiController
    {
        protected virtual async Task<Message<string,object>[]> PublishToKafkaMultipleItems(IEnumerable<object> data, string topicName)
        {
            var userId = this.User.Identity.GetUserId();
            var key = TimeProvider.Current.UtcNow.Ticks.ToString();

            // TODO
            // Might result in Producer error while publishing one of the messages
            // Handle the error by logging it in elastic search
            return await KafkaClient.PublishMultiple(data, topicName, key);
        }
    }
}