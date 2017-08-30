using System;
using MongoDB.Bson.Serialization.Attributes;

namespace EMS.Core.Models.Mongo
{
    public class AuditableMongoDocument : BaseMongoDocument
    {
        [BsonElement]
        public string UserId { get; set; }

        [BsonElement]
        public string UserName { get; set; }

        [BsonElement]
        public string SessionId { get; set; }

        [BsonElement]
        public DateTime CreatedOn { get; set; }
    }
}
