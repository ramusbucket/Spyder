using EMS.Core.Models;
using EMS.Infrastructure.Stream;
using EMS.Web.Common.Mongo;
using MongoDB.Driver;
using System.Threading;

namespace EMS.Web.Worker.MongoSaver.Models
{
    public class ForegroundProcessesSaver : BaseMongoSaver<CapturedForegroundProcess, CapturedForegroundProcessDTO>
    {
        public ForegroundProcessesSaver(
            CancellationToken cToken, 
            IMongoCollection<CapturedForegroundProcess> mongoCollection) 
            : base(cToken, mongoCollection, Topics.ForegroundProcesses)
        {
        }

        protected override CapturedForegroundProcess FormatReceivedMessage(CapturedForegroundProcessDTO message)
        {
            return new CapturedForegroundProcess
            {
                ForegroundProcess = message.CapturedForegroundProcess,
                CreatedOn = message.CreatedOn,
                UserId = message.UserId
            };
        }
    }
}