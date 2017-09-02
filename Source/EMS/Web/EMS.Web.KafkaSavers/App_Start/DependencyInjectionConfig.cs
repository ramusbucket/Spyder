using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Confluent.Kafka;
using Confluent.Kafka.Serialization;
using EMS.Infrastructure.DependencyInjection;
using EMS.Infrastructure.DependencyInjection.Interfaces;
using EMS.Infrastructure.Statistics;
using EMS.Infrastructure.Stream;

namespace EMS.Web.KafkaSavers
{
    public class DependencyInjectionConfig
    {
        public void RegisterDependencies()
        {
            var injector = UnityInjector.Instance;

            this.RegisterKafkaProducer(injector);
            this.RegisterKafkaConsumer(injector);
            this.RegisterStatsCollector(injector);
        }

        private void RegisterStatsCollector(IInjector injector)
        {
            var producer = injector.Resolve<Producer<string, object>>();
            var applicationName = Assembly.GetExecutingAssembly().FullName;
            var topic = "Metrics";
            var serverName = "Home";

            injector.RegisterInstance<IStatisticsCollector>(
                new KafkaStatisticsCollector(producer, topic, applicationName, serverName));
        }

        private void RegisterKafkaProducer(IInjector injector)
        {
            var kafkaBrokers = GetKafkaBrokers();
            var producerConfig = new Dictionary<string, object>
            {
                { "bootstrap.servers", kafkaBrokers },
                { "socket.blocking.max.ms", 1 },
                { "linger.ms", 0}
            };

            var keySerializer = new StringSerializer(Encoding.UTF8);
            var valueSerializer = new JsonSerializerObjectToBytes();

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
            var valueDeserializer = new JsonDeserializerBytesToObject();

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