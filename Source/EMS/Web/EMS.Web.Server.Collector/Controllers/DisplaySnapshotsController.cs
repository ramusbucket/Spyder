using EMS.Core.Models;
using EMS.Infrastructure.Stream;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace EMS.Web.Server.Collector.Controllers
{
    [RoutePrefix("api/DisplaySnapshots")]
    public class DisplaySnapshotsController : BaseKafkaApiController
    {
        [Route(nameof(PostCapturedDisplaySnapshots))]
        public async Task<IHttpActionResult> PostCapturedDisplaySnapshots(IEnumerable<CapturedDisplaySnapshotDTO> model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            await this.PublishToKafkaMultipleItems(model, Topics.DisplaySnapshots);

            var response = new EmptyResponse
            {
                IsSuccessful = true,
                Message = $"Request {model} handled successfully.",
            };

            return this.Ok(response);
        }
    }
}
