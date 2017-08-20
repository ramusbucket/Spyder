using EMS.Core.Models;
using EMS.Infrastructure.Stream;
using EMS.Web.Common.Mongo;
using MongoDB.Driver;
using System.Threading;

namespace EMS.Web.Worker.MongoSaver.Models
{
    public class CameraSnapshotsSaver : BaseMongoSaver<CapturedCameraSnapshot, CapturedCameraSnapshotDTO>
    {
        public CameraSnapshotsSaver(
             CancellationToken cToken,
             IMongoCollection<CapturedCameraSnapshot> mongoCollection) 
            : base(cToken, mongoCollection, Topics.CameraSnapshots)
        {
        }

        protected override CapturedCameraSnapshot FormatReceivedMessage(CapturedCameraSnapshotDTO message)
        {
            var item = new CapturedCameraSnapshot
            {
                UserId = message.UserId,
                CreatedOn = message.CreatedOn,
                CameraSnapshot = message.CameraSnapshot
            };

            return item;
        }
    }
}