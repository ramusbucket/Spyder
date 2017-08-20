using EMS.Core.Models;
using EMS.Infrastructure.Stream;
using EMS.Web.Common.Mongo;
using MongoDB.Driver;
using System.Threading;

namespace EMS.Web.Worker.MongoSaver.Models
{
    public class DisplaySnapshotsSaver : BaseMongoSaver<CapturedDisplaySnapshot, CapturedDisplaySnapshotDTO>
    {
        public DisplaySnapshotsSaver(
            CancellationToken cToken, 
            IMongoCollection<CapturedDisplaySnapshot> mongoCollection, 
            string inputKafkaTopic) 
            : base(cToken, mongoCollection, Topics.DisplaySnapshots)
        {
        }

        protected override CapturedDisplaySnapshot FormatReceivedMessage(CapturedDisplaySnapshotDTO message)
        {
            return new CapturedDisplaySnapshot
            {
                DisplaySnapshot = message.DisplaySnapshot,
                CreatedOn = message.CreatedOn,
                UserId = message.UserId
            };
        }
    }
}