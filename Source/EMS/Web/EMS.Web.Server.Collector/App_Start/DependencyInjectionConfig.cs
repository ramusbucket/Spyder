using Confluent.Kafka;
using Confluent.Kafka.Serialization;
using EMS.Infrastructure.DependencyInjection;
using EMS.Infrastructure.DependencyInjection.Interfaces;
using EMS.Infrastructure.Stream;
using System.Collections.Generic;
using System.Text;

namespace EMS.Web.Server.Collector.App_Start
{
    public class DependencyInjectionConfig
    {
        public void RegisterDependencies()
        {
            var injector = UnityInjector.Instance;

            this.RegisterKafkaProducer(injector);
            this.RegisterKafkaConsumer(injector);
        }

        private void RegisterKafkaProducer(IInjector injector)
        {
            var kafkaBrokers = GetKafkaBrokers();
            var producerConfig = new Dictionary<string, object> { { "bootstrap.servers", kafkaBrokers } };

            var keySerializer = new StringSerializer(Encoding.UTF8);
            var valueSerializer = new JsonSerializer();

            var producer = new Producer<string, object>(
                producerConfig,
                keySerializer,
                valueSerializer);

            injector.RegisterInstance(producer);
        }

        private void RegisterKafkaConsumer(IInjector injector)
        {
            var kafkaBrokers = GetKafkaBrokers();
            var consumerConfig = new Dictionary<string, object>
            {
                { "group.id", "DefaultKafkaConsumer" },
                { "enable.auto.commit", false },
                { "auto.commit.interval.ms", 5000 },
                { "statistics.interval.ms", 60000 },
                { "bootstrap.servers", kafkaBrokers },
                { "default.topic.config", new Dictionary<string, object>()
                    {
                        { "auto.offset.reset", "smallest" }
                    }
                }
            };

            var keyDeserializer = new StringDeserializer(Encoding.UTF8);
            var valueDeserializer = new JsonDeserializer();

            var consumer = new Consumer<string, object>(
                consumerConfig,
                keyDeserializer,
                valueDeserializer);

            injector.RegisterInstance(consumer);
        }

        private string GetKafkaBrokers()
        {
            var kafkaBrokers = "localhost:9092";
            return kafkaBrokers;
        }

    }
}