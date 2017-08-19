using System.Collections.Generic;
using System.Web.Http;
using EMS.Core.Models;
using System.Threading.Tasks;
using EMS.Infrastructure.Stream;

namespace EMS.Web.Server.Collector.Controllers
{
    [RoutePrefix("api/CameraSnapshots")]
    public class CameraSnapshotsController : BaseKafkaApiController
    {
        [Route(nameof(PostCameraSnapshots))]
        public async Task<IHttpActionResult> PostCameraSnapshots(IEnumerable<CapturedCameraSnapshotDTO> model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            await this.PublishToKafkaMultipleItems(model, Topics.CameraSnapshots);

            var response = new EmptyResponse
            {
                IsSuccessful = true,
                Message = $"Request handled successfully.",
            };

            return this.Ok(response);
        }
    }
}
