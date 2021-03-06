﻿using Confluent.Kafka;
using EMS.Infrastructure.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMS.Infrastructure.Stream
{
    public static class KafkaClient
    {
        private static Lazy<Producer<string, object>> producerWithPartitionKey;
        private static Lazy<Consumer<string, string>> consumerWithPartitionKey;

        static KafkaClient()
        {
            producerWithPartitionKey =
                new Lazy<Producer<string, object>>(
                    () => UnityInjector.Instance.Resolve<Producer<string, object>>());

            consumerWithPartitionKey =
                new Lazy<Consumer<string, string>>(
                    () => UnityInjector.Instance.Resolve<Consumer<string, string>>());
        }

        public static Producer<string, object> Producer
        {
            get
            {
                return producerWithPartitionKey.Value;
            }
        }

        public static Consumer<string, string> Consumer
        {
            get
            {
                return consumerWithPartitionKey.Value;
            }
        }

        public static async Task<Message<string, object>> PublishSingle(
            object item,
            string topicName,
            string key = null)
        {
            return await Producer.ProduceAsync(topicName, key, item);
        }

        public static async Task<Message<string, object>[]> PublishMultiple(
            IEnumerable<object> items,
            string topicName,
            string key = null)
        {
            var tasks = new List<Task<Message<string, object>>>(items.Count());

            foreach (var item in items)
            {
                tasks.Add(Producer.ProduceAsync(topicName, key, item));
            }

            return await Task.WhenAll(tasks);
        }
    }
}
