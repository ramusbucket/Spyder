using EMS.Core.Models;
using EMS.Infrastructure.Stream;
using EMS.Web.Common.Mongo;
using MongoDB.Driver;
using System.Threading;
using System;

namespace EMS.Web.Worker.MongoSaver.Models
{
    public class KeyboardKeysSaver : BaseMongoSaver<CapturedKeyboardKey, CapturedKeyDetailsDTO>
    {
        public KeyboardKeysSaver(CancellationToken cToken, IMongoCollection<CapturedKeyboardKey> mongoCollection)
            : base(cToken, mongoCollection, Topics.CapturedKeyboardKeys)
        {
        }

        protected override CapturedKeyboardKey FormatReceivedMessage(CapturedKeyDetailsDTO message)
        {
            return new CapturedKeyboardKey
            {
                KeyboardKey = message.KeyboardKey,
                CreatedOn = message.CreatedOn,
                UserId = message.UserId
            };
        }
    }
}