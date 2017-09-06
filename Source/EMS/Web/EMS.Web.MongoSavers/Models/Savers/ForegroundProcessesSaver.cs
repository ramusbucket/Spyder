using System.Threading;
using EMS.Core.Models.DTOs;
using EMS.Core.Models.Mongo;
using EMS.Infrastructure.Statistics;
using EMS.Infrastructure.Stream;
using MongoDB.Driver;

namespace EMS.Web.MongoSavers.Models.Savers
{
    public class ForegroundProcessesSaver : BaseMongoSaver<CapturedForegroundProcessMongoDocument, CapturedForegroundProcessDto>
    {
        public ForegroundProcessesSaver(
            CancellationToken cToken, 
            IMongoCollection<CapturedForegroundProcessMongoDocument> outCollection,
            IStatisticsCollector statsCollector) 
            : base(cToken, outCollection, Topics.ForegroundProcesses, statsCollector)
        {
        }

        protected override CapturedForegroundProcessMongoDocument FormatReceivedMessage(CapturedForegroundProcessDto message)
        {
            return new CapturedForegroundProcessMongoDocument
            {
                ForegroundProcess = message.CapturedForegroundProcess,
                CreatedOn = message.CreatedOn,
                UserId = message.UserId,
                SessionId = message.SessionId
            };
        }
    }
}