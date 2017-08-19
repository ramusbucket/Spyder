using System.Collections.Generic;
using System.Web.Http;
using EMS.Core.Models;
using EMS.Infrastructure.Stream;
using System.Threading.Tasks;

namespace EMS.Web.Server.Collector.Controllers
{
    [RoutePrefix("api/ForegroundProcess")]
    public class ForegroundProcessController : BaseKafkaApiController
    {
        [Route(nameof(PostForegroundProcess))]
        public async Task<IHttpActionResult> PostForegroundProcess(IEnumerable<CapturedForegroundProcessDTO> model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            await this.PublishToKafkaMultipleItems(model, Topics.ForegroundProcesses);

            var response = new EmptyResponse
            {
                IsSuccessful = true,
                Message = $"Request handled successfully.",
            };

            return this.Ok(response);
        }
    }
}
