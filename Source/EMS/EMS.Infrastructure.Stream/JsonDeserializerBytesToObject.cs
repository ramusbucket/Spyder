using Confluent.Kafka.Serialization;
using Newtonsoft.Json;
using System.Text;

namespace EMS.Infrastructure.Stream
{
    public class JsonDeserializerBytesToObject : IDeserializer<object>
    {
        private readonly JsonSerializerSettings _settings;

        public JsonDeserializerBytesToObject(JsonSerializerSettings settings)
        {
            _settings = settings;
        }

        public object Deserialize(byte[] data)
        {
            return JsonConvert.DeserializeObject(Encoding.UTF8.GetString(data), _settings);
        }
    }
}
