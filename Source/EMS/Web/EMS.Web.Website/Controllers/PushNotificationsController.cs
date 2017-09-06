using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using EMS.Core.Models.Mongo;

namespace EMS.Web.Website.Controllers
{
    // TODO:
    // Authorize the route to precent unwanted situations
    [RoutePrefix("api/PushNotifications")]
    public class PushNotificationsController : ApiController
    {
        [HttpPost]
        [Route("Update")]
        public IHttpActionResult Update(object item)
        {
            if (item is CapturedActiveProcessesMongoDocument)
            {
                
            }
            if (item is CapturedForegroundProcessMongoDocument)
            {

            }
            if (item is CapturedCameraSnapshotMongoDocument)
            {

            }
            if (item is CapturedDisplaySnapshotMongoDocument)
            {

            }
            if (item is CapturedKeyboardKeyMongoDocument)
            {

            }
            if (item is CapturedNetworkPacketMongoDocument)
            {

            }

            return Ok();
        }
    }
}
