using System;
using System.Threading.Tasks;
using EMS.Infrastructure.Statistics;

namespace EMS.Web.MongoSavers.IntegrationTests
{
    class EmptyStatsCollector : IStatisticsCollector
    {
        public void Send<T>(T stats, string topic)
        {
            // Pass through
        }

        public async Task SendWithAck<T>(T stats, string topic)
        {
            // Pass through
        }

        public T Measure<T>(Func<T> action, string actionName, string topic)
        {
            return action.Invoke();
        }

        public void Measure(Action action, string actionName, string topic)
        {
            // Pass through
        }

        public async Task Measure(Func<Task> action, string actionName, string topic)
        {
            // Pass through
        }

        public Task<T> Measure<T>(Func<Task<T>> action, string actionName, string topic)
        {
            // Pass through
            return action.Invoke();
        }

        public async Task<T> MeasureWithAck<T>(Func<T> action, string actionName, string topic)
        {
            // Pass through
            return action.Invoke();
        }

        public async Task MeasureWithAck(Action action, string actionName, string topic)
        {
            // Pass through
        }

        public async Task MeasureWithAck(Func<Task> action, string actionName, string topic)
        {
            // Pass through
        }

        public Task<T> MeasureWithAck<T>(Func<Task<T>> action, string actionName, string topic)
        {
            return action.Invoke();
        }
    }
}
