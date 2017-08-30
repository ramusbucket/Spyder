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
using EMS.Web.KafkaSavers.Models;

namespace EMS.Web.KafkaSavers.Controllers
{
    [System.Web.Http.Authorize]
    public class BaseKafkaApiController : ApiController
    {
        protected virtual async Task PublishToKafkaMultipleItems(IEnumerable<AuditableDto> data, string topicName)
        {
            var userId = this.User.Identity.GetUserId();
            var userName = this.User.Identity.GetUserName();

            foreach (var item in data)
            {
                item.UserId = userId;
                item.UserName = userName;
            }

            var key = TimeProvider.Current.UtcNow.Ticks.ToString();

            // TODO
            // Might result in Producer error while publishing one of the messages
            // Handle the error by logging it in elastic search
            var kafkaResponse = await KafkaClient.PublishMultiple(data, topicName, key);

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