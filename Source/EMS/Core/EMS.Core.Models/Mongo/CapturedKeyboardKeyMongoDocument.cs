using EMS.Infrastructure.Common.Enums;
using MongoDB.Bson.Serialization.Attributes;

namespace EMS.Core.Models.Mongo
{
    public class CapturedKeyboardKeyMongoDocument : AuditableMongoDocument
    {
        public KeyboardKey KeyboardKey { get; set; }

        [BsonIgnore]
        public string KeyboardKeyName => this.KeyboardKey.ToString();
    }
}
