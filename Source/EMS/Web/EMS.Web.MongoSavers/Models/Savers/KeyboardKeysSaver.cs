using System.Threading;
using EMS.Core.Models.DTOs;
using EMS.Core.Models.Mongo;
using EMS.Infrastructure.Stream;
using MongoDB.Driver;

namespace EMS.Web.MongoSavers.Models.Savers
{
    public class KeyboardKeysSaver : BaseMongoSaver<CapturedKeyboardKeyMongoDocument, CapturedKeyDetailsDto>
    {
        public KeyboardKeysSaver(
            CancellationToken cToken, 
            IMongoCollection<CapturedKeyboardKeyMongoDocument> outCollection)
            : base(cToken, outCollection, Topics.CapturedKeyboardKeys)
        {
        }

        protected override CapturedKeyboardKeyMongoDocument FormatReceivedMessage(CapturedKeyDetailsDto message)
        {
            return new CapturedKeyboardKeyMongoDocument
            {
                KeyboardKey = message.KeyboardKey,
                CreatedOn = message.CreatedOn,
                UserId = message.UserId,
                SessionId = message.SessionId
            };
        }
    }
}