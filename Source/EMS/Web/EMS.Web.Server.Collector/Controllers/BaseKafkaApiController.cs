using EMS.Core.Models;
using EMS.Infrastructure.Common.Providers;
using EMS.Infrastructure.Stream;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Web.Http;

namespace EMS.Web.Server.Collector.Controllers
{
    public class BaseKafkaApiController : ApiController
    {
        protected virtual async Task PublishToKafkaMultipleItems(IEnumerable<Auditable> data, string topicName)
        {
            var userId = this.User.Identity.GetUserId();
            foreach(var item in data)
            {
                item.UserId = userId;
            }

            var key = TimeProvider.Current.UtcNow.Ticks.ToString();

            // TODO
            // Might result in Producer error while publishing one of the messages
            // Handle the error by logging it in elastic search
            var kafkaResponse = await KafkaClient.PublishMultiple(data, topicName, key);

            var directoryPath = $@"D:\Dev\Spyder\Source\EMS\Web\EMS.Web.Server.Collector\App_Data\" + topicName;
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            File.WriteAllText(
              $@"{directoryPath}\{TimeProvider.Current.UtcNow.ToFileTimeUtc()}.txt",
              $"Publish in topic: {topicName} for User: {this.User.Identity.GetUserName()} {Environment.NewLine}{JsonConvert.SerializeObject(kafkaResponse)}");
        }
    }
}