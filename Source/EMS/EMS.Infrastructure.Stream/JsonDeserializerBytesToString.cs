using Confluent.Kafka.Serialization;
using System.Text;

namespace EMS.Infrastructure.Stream
{
    public class JsonDeserializerBytesToString : IDeserializer<string>
    {
        public string Deserialize(byte[] data)
        {
            return Encoding.UTF8.GetString(data);
        }
    }
}
