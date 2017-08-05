using Confluent.Kafka.Serialization;
using Newtonsoft.Json;
using System.Text;

namespace EMS.Infrastructure.Stream
{
    public class JsonDeserializer : IDeserializer<object>
    {
        public object Deserialize(byte[] data)
        {
            return JsonConvert.DeserializeObject(Encoding.UTF8.GetString(data));
        }
    }
}
