using System.Collections.Generic;
using Confluent.Kafka.Serialization;
using Newtonsoft.Json;
using System.Text;
using EMS.Infrastructure.Common.JsonConverters;

namespace EMS.Infrastructure.Stream
{
    public class JsonSerializerObjectToBytes : ISerializer<object>
    {
        private readonly JsonSerializerSettings _serializerSettings;

        public JsonSerializerObjectToBytes(JsonSerializerSettings serializerSettings)
        {
            _serializerSettings = serializerSettings;
        }

        public byte[] Serialize(object data)
        {
            return Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(data, _serializerSettings));
        }
    }
}
