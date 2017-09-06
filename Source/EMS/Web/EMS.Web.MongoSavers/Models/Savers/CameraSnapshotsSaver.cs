using System.Threading;
using Easy.Common.Interfaces;
using EMS.Core.Models.DTOs;
using EMS.Core.Models.Mongo;
using EMS.Infrastructure.Statistics;
using EMS.Infrastructure.Stream;
using MongoDB.Driver;

namespace EMS.Web.MongoSavers.Models.Savers
{
    public class CameraSnapshotsSaver : BaseMongoSaver<CapturedCameraSnapshotMongoDocument, CapturedCameraSnapshotDto>
    {
        public CameraSnapshotsSaver(
            CancellationToken cToken,
            IMongoCollection<CapturedCameraSnapshotMongoDocument> outCollection,
            IStatisticsCollector statsCollector,
            IRestClient restClient)
            : base(cToken, outCollection, Topics.CameraSnapshots, statsCollector, restClient)
        {
        }

        protected override CapturedCameraSnapshotMongoDocument FormatReceivedMessage(CapturedCameraSnapshotDto message)
        {
            var item = new CapturedCameraSnapshotMongoDocument
            {
                UserId = message.UserId,
                CreatedOn = message.CreatedOn,
                CameraSnapshot = message.CameraSnapshot,
                SessionId = message.SessionId
            };

            return item;
        }
    }
}