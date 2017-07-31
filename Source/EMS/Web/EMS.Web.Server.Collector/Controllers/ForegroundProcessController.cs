using System.Collections.Generic;
using System.Web.Http;
using EMS.Core.Models;

namespace EMS.Web.Server.Collector.Controllers
{
    [RoutePrefix("api/ForegroundProcess")]
    public class ForegroundProcessController : ApiController
    {
        [Route(nameof(PostForegroundProcess))]
        public IHttpActionResult PostForegroundProcess(IEnumerable<CapturedForegroundProcessDetails> model)
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
