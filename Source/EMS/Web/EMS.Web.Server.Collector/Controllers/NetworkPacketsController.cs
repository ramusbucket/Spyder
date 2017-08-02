using EMS.Core.Models;
using System.Collections.Generic;
using System.Web.Http;

namespace EMS.Web.Server.Collector.Controllers
{
    [RoutePrefix("api/NetworkPackets")]
    public class NetworkPacketsController : ApiController
    {
        [Route(nameof(PostNetworkPackets))]
        public IHttpActionResult PostNetworkPackets(IEnumerable<CapturedNetworkPacketDetails> model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }
           
            var response = new EmptyResponse
            {
                IsSuccessful = true,
                Message = $"Request handled successfully.",
            };

            return this.Ok(response);
        }
    }
}
