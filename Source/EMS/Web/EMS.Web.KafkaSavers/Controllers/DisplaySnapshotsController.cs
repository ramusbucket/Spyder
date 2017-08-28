using EMS.Core.Models;
using EMS.Infrastructure.Stream;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using EMS.Core.Models.DTOs;

namespace EMS.Web.KafkaSavers.Controllers
{
    [RoutePrefix("api/DisplaySnapshots")]
    public class DisplaySnapshotsController : BaseKafkaApiController
    {
        [Route(nameof(PostCapturedDisplaySnapshots))]
        public async Task<IHttpActionResult> PostCapturedDisplaySnapshots(IEnumerable<CapturedDisplaySnapshotDto> model)
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
