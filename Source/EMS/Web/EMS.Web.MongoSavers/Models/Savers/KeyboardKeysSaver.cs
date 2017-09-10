using System.Threading;
using Easy.Common.Interfaces;
using EMS.Core.Models.DTOs;
using EMS.Core.Models.Mongo;
using EMS.Infrastructure.Statistics;
using EMS.Infrastructure.Stream;
using MongoDB.Driver;

namespace EMS.Web.MongoSavers.Models.Savers
{
    public class KeyboardKeysSaver : BaseMongoSaver<CapturedKeyboardKeyMongoDocument, CapturedKeyDetailsDto>
    {
        public KeyboardKeysSaver(
            CancellationToken cToken, 
            IMongoCollection<CapturedKeyboardKeyMongoDocument> outCollection,
            IStatisticsCollector statsCollector,
            IRestClient restClient)
            : base(cToken, outCollection, Topics.CapturedKeyboardKeys, statsCollector, restClient)
        {
        }

        protected override CapturedKeyboardKeyMongoDocument FormatReceivedMessage(CapturedKeyDetailsDto message)
        {
            return new CapturedKeyboardKeyMongoDocument
            {
                KeyboardKey = message.KeyboardKey,
                CreatedOn = message.CreatedOn,
                UserId = message.UserId,
                SessionId = message.SessionId,
                UserName = message.UserName
            };
        }
    }
}