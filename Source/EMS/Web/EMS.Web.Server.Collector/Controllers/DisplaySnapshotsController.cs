using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using EMS.Core.Models;

namespace EMS.Web.Server.Collector.Controllers
{
    [RoutePrefix("api/DisplaySnapshots")]
    public class DisplaySnapshotsController : BaseKafkaController
    {
        [Route(nameof(PostCapturedDisplaySnapshots))]
        public IHttpActionResult PostCapturedDisplaySnapshots(IEnumerable<CapturedDisplaySnapshotDTO> model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var response = new EmptyResponse
            {
                IsSuccessful = true,
                Message = $"Request {model} handled successfully.",
            };

            return this.Ok(response);
        }
    }
}
