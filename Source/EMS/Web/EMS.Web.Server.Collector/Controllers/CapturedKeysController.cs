using EMS.Core.Models;
using EMS.Infrastructure.Common.Providers;
using EMS.Infrastructure.Stream;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace EMS.Web.Server.Collector.Controllers
{
    [RoutePrefix("api/CapturedKeys")]
    public class CapturedKeysController : BaseKafkaApiController
    {
        [Route(nameof(PostCapturedKeys))]
        public async Task<IHttpActionResult> PostCapturedKeys(IEnumerable<CapturedKeyDetailsDTO> model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            await this.PublishToKafkaMultipleItems(model, Topics.CapturedKeyboardKeys);

            var response = new EmptyResponse
            {
                IsSuccessful = true,
                Message = $"Request handled successfully.",
            };

            return this.Ok(response);
        }
    }
}
