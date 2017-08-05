using Confluent.Kafka.Serialization;
using Newtonsoft.Json;
using System.Text;

namespace EMS.Infrastructure.Stream
{

    public class JsonSerializer : ISerializer<object>
    {
        public byte[] Serialize(object data)
        {
            return Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(data, Formatting.Indented));
        }
    }
}
