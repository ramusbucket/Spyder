using System.Collections.Generic;
using System.Web.Http;
using EMS.Core.Models;

namespace EMS.Web.Server.Collector.Controllers
{
    [RoutePrefix("api/ActiveProcesses")]
    public class ActiveProcessesController : BaseKafkaController
    {
        [Route(nameof(PostActiveProcesses))]
        public IHttpActionResult PostActiveProcesses(IEnumerable<CapturedActiveProcessesDTO> model)
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
