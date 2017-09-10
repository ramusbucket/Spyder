using System.Threading;
using Easy.Common.Interfaces;
using EMS.Core.Models.DTOs;
using EMS.Core.Models.Mongo;
using EMS.Infrastructure.Statistics;
using EMS.Infrastructure.Stream;
using MongoDB.Driver;

namespace EMS.Web.MongoSavers.Models.Savers
{
    public class DisplaySnapshotsSaver : BaseMongoSaver<CapturedDisplaySnapshotMongoDocument, CapturedDisplaySnapshotDto>
    {
        public DisplaySnapshotsSaver(
            CancellationToken cToken, 
            IMongoCollection<CapturedDisplaySnapshotMongoDocument> outCollection,
            IStatisticsCollector statsCollector,
            IRestClient restClient) 
            : base(cToken, outCollection, Topics.DisplaySnapshots, statsCollector, restClient)
        {
        }

        protected override CapturedDisplaySnapshotMongoDocument FormatReceivedMessage(CapturedDisplaySnapshotDto message)
        {
            return new CapturedDisplaySnapshotMongoDocument
            {
                DisplaySnapshot = message.DisplaySnapshot,
                CreatedOn = message.CreatedOn,
                UserId = message.UserId,
                SessionId = message.SessionId,
                UserName = message.UserName
            };
        }
    }
}