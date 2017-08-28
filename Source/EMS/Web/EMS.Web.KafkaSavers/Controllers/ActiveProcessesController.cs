using System.Collections.Generic;
using System.Web.Http;
using EMS.Core.Models;
using System.Threading.Tasks;
using EMS.Core.Models.DTOs;
using EMS.Infrastructure.Stream;

namespace EMS.Web.KafkaSavers.Controllers
{
    [RoutePrefix("api/ActiveProcesses")]
    public class ActiveProcessesController : BaseKafkaApiController
    {
        [Route(nameof(PostActiveProcesses))]
        public async Task<IHttpActionResult> PostActiveProcesses(IEnumerable<CapturedActiveProcessesDto> model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            await this.PublishToKafkaMultipleItems(model, Topics.ActiveProcesses);

            var response = new EmptyResponse
            {
                IsSuccessful = true,
                Message = $"Request handled successfully.",
            };

            return this.Ok(response);
        }
    }
}
