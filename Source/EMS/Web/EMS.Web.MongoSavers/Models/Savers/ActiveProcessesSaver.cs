using System.Threading;
using Easy.Common.Interfaces;
using EMS.Core.Models.DTOs;
using EMS.Core.Models.Mongo;
using EMS.Infrastructure.Statistics;
using EMS.Infrastructure.Stream;
using MongoDB.Driver;

namespace EMS.Web.MongoSavers.Models.Savers
{
    public class ActiveProcessesSaver : BaseMongoSaver<CapturedActiveProcessesMongoDocument, CapturedActiveProcessesDto>
    {
        public ActiveProcessesSaver(
            CancellationToken cToken, 
            IMongoCollection<CapturedActiveProcessesMongoDocument> outCollection,
            IStatisticsCollector statsCollector,
            IRestClient restClient) 
            : base(cToken, outCollection, Topics.ActiveProcesses, statsCollector, restClient)
        {
        }

        protected override CapturedActiveProcessesMongoDocument FormatReceivedMessage(CapturedActiveProcessesDto message)
        {
            return new CapturedActiveProcessesMongoDocument
            {
                ActiveProcesses = message.CapturedActiveProcesses,
                CreatedOn = message.CreatedOn,
                UserId = message.UserId,
                SessionId = message.SessionId
            };
        }
    }
}