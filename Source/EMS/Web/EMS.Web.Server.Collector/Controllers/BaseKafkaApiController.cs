using EMS.Core.Models;
using EMS.Infrastructure.Common.Providers;
using EMS.Infrastructure.Stream;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Web.Hosting;
using System.Web.Http;

namespace EMS.Web.Server.Collector.Controllers
{
    [Authorize]
    public class BaseKafkaApiController : ApiController
    {
        private readonly string RootApplicationDirectory = HostingEnvironment.ApplicationPhysicalPath;

        protected virtual async Task PublishToKafkaMultipleItems(IEnumerable<BaseDTO> data, string topicName)
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

            var directoryPath = $@"{RootApplicationDirectory}\App_Data\" + topicName;
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            File.WriteAllText($@"{directoryPath}\{TimeProvider.Current.UtcNow.ToFileTimeUtc()}.txt","OK");
        }
    }
}