using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using EMS.Core.Models;

namespace EMS.Web.Server.Collector.Controllers
{
    [RoutePrefix("api/CapturedKeys")]
    public class CapturedKeysController : ApiController
    {
        [Route(nameof(PostCapturedKeys))]
        public IHttpActionResult PostCapturedKeys(IEnumerable<CapturedKeyDetails> model)
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
