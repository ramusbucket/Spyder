﻿using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Confluent.Kafka;

namespace EMS.Infrastructure.Statistics
{
    public class KafkaStatisticsCollector : IStatisticsCollector
    {
        private readonly Producer<string, object> _producer;
        private readonly string _applicationName;
        private readonly string _serverName;
        private readonly string _topic;

        public KafkaStatisticsCollector(Producer<string, object> producer, string topic, string applicationName, string serverName)
        {
            _producer = producer;
            _applicationName = applicationName;
            _serverName = serverName;
            _topic = topic;
        }

        public void Send<T>(T stats)
        {
            _producer.ProduceAsync(_topic, null, stats);
        }

        public async Task SendWithAck<T>(T stats)
        {
            var message = await _producer.ProduceAsync(_topic, null, stats);

            if (message == null)
            {
                throw new Exception("Something fatal happened with the Confluent Kafka driver while trying to ProdyceAsync() a message.");
            }

            if (message.Error != null && message.Error.HasError)
            {
                throw new Exception(message.Error.Reason);
            }
        }

        public T Measure<T>(Func<T> action, string actionName)
        {
            var stopwatch = Stopwatch.StartNew();
            var result = action.Invoke();
            stopwatch.Stop();

            var statistics = new ActionExecutionMetrics
            {
                ActionName = actionName,
                ServerName = _serverName,
                ApplicationName = _applicationName,
                ActionExecutionTime = (long)stopwatch.Elapsed.TotalMilliseconds
            };

            Send(statistics);

            return result;
        }

        public void Measure(Action action, string actionName)
        {
            var stopwatch = Stopwatch.StartNew();
            action.Invoke();
            stopwatch.Stop();

            var statistics = new ActionExecutionMetrics
            {
                ActionName = actionName,
                ServerName = _serverName,
                ApplicationName = _applicationName,
                ActionExecutionTime = (long)stopwatch.Elapsed.TotalMilliseconds
            };

            Send(statistics);
        }

        public async Task Measure(Func<Task> action, string actionName = null)
        {
            var stopwatch = Stopwatch.StartNew();
            await action.Invoke();
            stopwatch.Stop();

            var statistics = new ActionExecutionMetrics
            {
                ActionName = actionName,
                ServerName = _serverName,
                ApplicationName = _applicationName,
                ActionExecutionTime = (long)stopwatch.Elapsed.TotalMilliseconds
            };

            Send(statistics);
        }

        public async Task<T> Measure<T>(Func<Task<T>> action, string actionName)
        {
            var stopwatch = Stopwatch.StartNew();
            var result = await action.Invoke();

            stopwatch.Stop();

            var statistics = new ActionExecutionMetrics
            {
                ActionName = actionName,
                ServerName = _serverName,
                ApplicationName = _applicationName,
                ActionExecutionTime = (long)stopwatch.Elapsed.TotalMilliseconds
            };

            Send(statistics);

            return result;
        }

        public async Task<T> MeasureWithAck<T>(Func<T> action, string actionName)
        {
            var stopwatch = Stopwatch.StartNew();
            var result = action.Invoke();
            stopwatch.Stop();

            var statistics = new ActionExecutionMetrics
            {
                ActionName = actionName,
                ServerName = _serverName,
                ApplicationName = _applicationName,
                ActionExecutionTime = (long)stopwatch.Elapsed.TotalMilliseconds
            };

            await SendWithAck(statistics);

            return result;
        }

        public async Task MeasureWithAck(Action action, string actionName)
        {
            var stopwatch = Stopwatch.StartNew();
            action.Invoke();
            stopwatch.Stop();

            var statistics = new ActionExecutionMetrics
            {
                ActionName = actionName,
                ServerName = _serverName,
                ApplicationName = _applicationName,
                ActionExecutionTime = (long)stopwatch.Elapsed.TotalMilliseconds
            };

            await SendWithAck(statistics);
        }

        public async Task MeasureWithAck(Func<Task> action, string actionName = null)
        {
            var stopwatch = Stopwatch.StartNew();
            await action.Invoke();
            stopwatch.Stop();

            var statistics = new ActionExecutionMetrics
            {
                ActionName = actionName,
                ServerName = _serverName,
                ApplicationName = _applicationName,
                ActionExecutionTime = (long)stopwatch.Elapsed.TotalMilliseconds
            };

            await SendWithAck(statistics);
        }

        public async Task<T> MeasureWithAck<T>(Func<Task<T>> action, string actionName)
        {
            var stopwatch = Stopwatch.StartNew();
            var result = await action.Invoke();

            stopwatch.Stop();

            var statistics = new ActionExecutionMetrics
            {
                ActionName = actionName,
                ServerName = _serverName,
                ApplicationName = _applicationName,
                ActionExecutionTime = (long)stopwatch.Elapsed.TotalMilliseconds
            };

            await SendWithAck(statistics);

            return result;
        }
    }
}
