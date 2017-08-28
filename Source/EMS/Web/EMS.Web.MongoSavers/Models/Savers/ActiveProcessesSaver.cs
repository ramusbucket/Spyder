using System.Threading;
using EMS.Core.Models.DTOs;
using EMS.Core.Models.Mongo;
using EMS.Infrastructure.Stream;
using MongoDB.Driver;

namespace EMS.Web.MongoSavers.Models.Savers
{
    public class ActiveProcessesSaver : BaseMongoSaver<CapturedActiveProcessesMongoDocument, CapturedActiveProcessesDto>
    {
        public ActiveProcessesSaver(
            CancellationToken cToken, 
            IMongoCollection<CapturedActiveProcessesMongoDocument> outCollection) 
            : base(cToken, outCollection, Topics.ActiveProcesses)
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