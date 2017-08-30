using System.Collections.Generic;
using EMS.Core.Models.Mongo;

namespace EMS.Web.Website.Models
{
    public class SessionViewModel
    {
        public string UserId { get; set; }

        public string UserName { get; set; }

        public string SessionId { get; set; }

        public IEnumerable<CapturedKeyboardKeyMongoDocument> KeyboardKeys { get; set; }

        public IEnumerable<CapturedNetworkPacketMongoDocument> NetworkPackets { get; set; }

        public IEnumerable<CapturedCameraSnapshotMongoDocument> CameraSnapshots { get; set; }

        public IEnumerable<CapturedDisplaySnapshotMongoDocument> DisplaySnapshots { get; set; }

        public IEnumerable<CapturedForegroundProcessMongoDocument> ForegroundProcesses { get; set; }
    }
}