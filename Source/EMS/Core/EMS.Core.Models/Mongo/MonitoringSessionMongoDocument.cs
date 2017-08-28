using System;
using MongoDB.Bson.Serialization.Attributes;

namespace EMS.Core.Models.Mongo
{
    public class MonitoringSessionMongoDocument
    {
        [BsonId]
        public MonitoringSessionId Id { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedAt { get; set; }
    }

    public class MonitoringSessionId
    {
        [BsonElement]
        public string UserId { get; set; }

        [BsonElement]
        public string SessionId { get; set; }
    }
}
