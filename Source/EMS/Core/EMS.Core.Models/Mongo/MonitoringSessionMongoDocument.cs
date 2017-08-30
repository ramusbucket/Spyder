using MongoDB.Bson.Serialization.Attributes;

namespace EMS.Core.Models.Mongo
{
    public class MonitoringSessionMongoDocument : AuditableMongoDocument
    {
        [BsonElement]
        public bool IsActive { get; set; }
    }
}
