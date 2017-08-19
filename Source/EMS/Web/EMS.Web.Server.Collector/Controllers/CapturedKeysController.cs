using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using EMS.Core.Models;
using Microsoft.AspNet.Identity;
using EMS.Infrastructure.Stream;
using System.Threading.Tasks;

namespace EMS.Web.Server.Collector.Controllers
{
    [RoutePrefix("api/CapturedKeys")]
    public class CapturedKeysController : BaseKafkaController
    {
        [Route(nameof(PostCapturedKeys))]
        public async Task<IHttpActionResult> PostCapturedKeys(IEnumerable<CapturedKeyDetailsDTO> model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var userId = this.User.Identity.GetUserId();

            try
            {
                foreach(var key in model)
                {
                    var resultMessage = await this.Producer.ProduceAsync(Topics.CapturedKeyboardKeys, "key", key);
                }
            }
            catch (Exception e)
            {

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
