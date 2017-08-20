using EMS.Core.Models;
using EMS.Infrastructure.Stream;
using EMS.Web.Common.Mongo;
using MongoDB.Driver;
using System.Threading;

namespace EMS.Web.Worker.MongoSaver.Models
{
    public class ActiveProcessesSaver : BaseMongoSaver<CapturedActiveProcesses, CapturedActiveProcessesDTO>
    {
        public ActiveProcessesSaver(
            CancellationToken cToken, 
            IMongoCollection<CapturedActiveProcesses> mongoCollection) 
            : base(cToken, mongoCollection, Topics.ActiveProcesses)
        {
        }

        protected override CapturedActiveProcesses FormatReceivedMessage(CapturedActiveProcessesDTO message)
        {
            return new CapturedActiveProcesses
            {
                ActiveProcesses = message.CapturedActiveProcesses,
                CreatedOn = message.CreatedOn,
                UserId = message.UserId
            };
        }
    }
}