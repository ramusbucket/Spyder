using EMS.Core.Models;
using EMS.Infrastructure.Stream;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using EMS.Core.Models.DTOs;

namespace EMS.Web.KafkaSavers.Controllers
{
    [RoutePrefix("api/NetworkPackets")]
    public class NetworkPacketsController : BaseKafkaApiController
    {
        [Route(nameof(PostNetworkPackets))]
        public async Task<IHttpActionResult> PostNetworkPackets(IEnumerable<CapturedNetworkPacketDetailsDto> model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            await this.PublishToKafkaMultipleItems(model, Topics.NetworkPackets);

            var response = new EmptyResponse
            {
                IsSuccessful = true,
                Message = $"Request handled successfully.",
            };

            return this.Ok(response);
        }
    }
}
