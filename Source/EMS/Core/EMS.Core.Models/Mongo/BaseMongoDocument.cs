using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace EMS.Core.Models.Mongo
{
    public class BaseMongoDocument
    {
        [BsonId]
        public ObjectId Id { get; set; }
    }
}
