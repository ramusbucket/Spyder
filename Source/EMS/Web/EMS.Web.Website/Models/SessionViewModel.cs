using System.Collections.Generic;
using EMS.Core.Models.Mongo;

namespace EMS.Web.Website.Models
{
    public class SessionViewModel
    {
        public IEnumerable<CapturedKeyboardKeyMongoDocument> KeyboardKeys { get; set; }

        public IEnumerable<CapturedNetworkPacketMongoDocument> NetworkPackets { get; set; }

        public IEnumerable<CapturedCameraSnapshotMongoDocument> CameraSnapshots { get; set; }

        public IEnumerable<CapturedDisplaySnapshotMongoDocument> DisplaySnapshots { get; set; }

        public IEnumerable<CapturedForegroundProcessMongoDocument> ForegroundProcesses { get; set; }
    }
}