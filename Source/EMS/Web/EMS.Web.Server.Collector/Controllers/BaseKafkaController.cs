using Confluent.Kafka;
using EMS.Infrastructure.DependencyInjection;
using System.Web.Http;

namespace EMS.Web.Server.Collector.Controllers
{
    public class BaseKafkaController : ApiController
    {
        private Producer<string, object> producer =
            UnityInjector.Instance.Resolve<Producer<string, object>>();
        private Consumer<string, object> consumer =
            UnityInjector.Instance.Resolve<Consumer<string, object>>();

        public BaseKafkaController()
        {
        }

        public Producer<string, object> Producer
        {
            get
            {
                return producer;
            }
        }

        public Consumer<string, object> Consumer
        {
            get
            {
                return consumer;
            }
        }
    }
}
